using Hypocrite.Core.Container.Common;
using Hypocrite.Core.Container.Extensions;
using Hypocrite.Core.Container.InstancePolicy;
using Hypocrite.Core.Container.Interfaces;
using Hypocrite.Core.Container.Registration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Hypocrite.Core.Container
{
    public class LightContainer : ILightContainer
    {
        public QuickSet<IContainerRegistration> Registrations { get; set; } = new QuickSet<IContainerRegistration>();

        public IInstanceCreator InstanceCreator { get; private set; }

        public LightContainer()
        {
            InstanceCreator = new DefaultInstanceCreator(this);
        }

        public bool IsRegistered(Type type)
        {
            return IsRegistered(type, "");
        }

        public bool IsRegistered(Type type, string name)
        {
            return Registrations.Get(type.GetHashCode(), name) != null;
        }

        public bool IsRegistered(Type type, string name, out IContainerRegistration registration)
        {
            registration = Registrations.Get(type.GetHashCode(), name)?.Value;
            return registration != null;
        }

        public IContainerRegistration GetRegistration(Type type, string name)
        {
            return Registrations.Get(type.GetHashCode(), name)?.Value;
        }

        public ILightContainer RegisterFactory(Type type, Func<ILightContainer, Type, object> factory)
        {
            var registration = new ContainerRegistration()
            {
                RegisteredType = type,
                MappedToType = factory.GetType(),
                Instance = factory,
                RegistrationType = RegistrationType.Func,
            };
            AddOrReplace(registration, string.Empty);
            SetUpRegistration(registration);
            return this;
        }

        public ILightContainer RegisterInstance(Type type, object instance)
        {
            return RegisterInstance(type, instance, string.Empty);
        }

        public ILightContainer RegisterInstance(Type type, object instance, string name)
        {
            var registration = new ContainerRegistration()
            {
                RegisteredType = type,
                MappedToType = instance.GetType(),
                Instance = instance,
                RegistrationType = RegistrationType.Instance,
            };
            AddOrReplace(registration, name);
            SetUpRegistration(registration);
            return this;
        }

        public ILightContainer RegisterType(Type registeredType, Type mappedToType, bool isSingleton = false)
        {
            return RegisterType(registeredType, mappedToType, string.Empty, isSingleton);
        }

        public ILightContainer RegisterType(Type registeredType, Type mappedToType, string name, bool isSingleton = false)
        {
            var registration = new ContainerRegistration()
            {
                RegisteredType = registeredType,
                MappedToType = mappedToType,
                Instance = null,
                RegistrationType = isSingleton ? RegistrationType.Instance : RegistrationType.Type,
            };
            AddOrReplace(registration, name);
            SetUpRegistration(registration);
            return this;
        }

        public object Resolve(Type type)
        {
            return Resolve(type, true);
        }

        public object Resolve(Type type, bool withInjections)
        {
            return Resolve(type, string.Empty, withInjections);
        }

        public object Resolve(Type type, string name, bool withInjections)
        {
            return Resolve(type, name, withInjections, out IContainerRegistration _);
        }

        public object Resolve(Type type, string name, bool withInjections, out IContainerRegistration outRegistration)
        {
            if (IsRegistered(type, name, out outRegistration))
            {
                return outRegistration.RegistrationPolicy.CreateInstance(withInjections);
            }

            // try to resolve with empty name
            if (!string.IsNullOrWhiteSpace(name))
            {
                return Resolve(type, string.Empty, withInjections, out outRegistration);
            }

            // settings to null because it was not registered
            outRegistration = null;

            // check for primitives
            if (type.IsPrimitive || type == typeof(string))
            {
                return type.GetDefaultValue();
            }

            // try to create it by my own
            if (type.IsClass)
            {
                RegisterType(type, type, name, false);
                return Resolve(type, name, true);
            }
            return null;
        }

        public void ResolveInjections(object instance)
        {
            ResolveInjections(instance, null);
        }

        public void ResolveInjections(object instance, MemberInjectionInfo injectionInfo)
        {
            if (injectionInfo == null)
            {
                injectionInfo = new MemberInjectionInfo();
                InternalGenerateInjectionInfo(instance, injectionInfo);
            }
            InstanceCreator.ResolveInjections(instance, injectionInfo);
        }

        public bool RequiresInjections(object instance)
        {
            return RequiresInjections(instance, null);
        }

        public bool RequiresInjections(object instance, MemberInjectionInfo injectionInfo)
        {
            if (injectionInfo == null)
            {
                injectionInfo = new MemberInjectionInfo();
                InternalGenerateInjectionInfo(instance, injectionInfo);
            }
            return InstanceCreator.RequiresInjections(instance, injectionInfo);
        }

        public void Dispose()
        {
            
        }

        private void AddOrReplace(IContainerRegistration registration, string name)
        {
            Registrations.AddOrReplace(registration.RegisteredType.GetHashCode(), name, registration);
        }

        private void SetUpRegistration(IContainerRegistration registration)
        {
            // setting up constructor data
            if (registration.Instance != null && registration.RegistrationType == RegistrationType.Instance)
            {
                InternalGenerateInjectionInfo(registration.Instance, registration.MemberInjectionInfo);
                registration.MemberInjectionInfo.CalcInjectionReq();
            }
            else if (registration.RegistrationType == RegistrationType.Instance || registration.RegistrationType == RegistrationType.Type)
            {
                InternalGenerateInjectionInfo(null, registration.MemberInjectionInfo, registration.MappedToType);
                registration.MemberInjectionInfo.CalcInjectionReq();

                var ctor = registration.MappedToType.GetNormalConstructor();
                registration.ConstructorInjectionInfo.InjectionMembers = ctor.GetParameters();

                // prepare constructor info
                if (registration.ConstructorInjectionInfo.InjectionMembers.Length > 0)
                {
                    registration.ConstructorInjectionInfo.DefaultConstructorInfo = ctor;
                }
                else
                {
                    NewExpression newExp = Expression.New(ctor);
                    Expression<Func<object>> lambda = Expression.Lambda<Func<object>>(newExp);
                    registration.ConstructorInjectionInfo.DefaultConstructorDelegate = lambda.Compile();
                }
            }

            // setting up policy
            switch (registration.RegistrationType)
            {
                case RegistrationType.Instance:
                    registration.RegistrationPolicy = new SingletonPolicy(this, registration);
                    break;
                case RegistrationType.Type:
                    registration.RegistrationPolicy = new TypePolicy(this, registration);
                    break;
                case RegistrationType.Func:
                    registration.RegistrationPolicy = new FuncPolicy(this, registration);
                    break;
            }
        }

        private void InternalGenerateInjectionInfo(object instance, MemberInjectionInfo injectionInfo, Type type = null)
        {
            type = type ?? instance.GetType();
            // fields
            foreach (var f in type.GetTypeInfo().DeclaredFields)
            {
                var attrs = f.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    injectionInfo.InjectionFields.Add(f);
                }
            }
            // properties
            foreach (var p in type.GetTypeInfo().DeclaredProperties)
            {
                var attrs = p.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    injectionInfo.InjectionProperties.Add(p);
                }
            }

            if (type.BaseType != null)
            {
                InternalGenerateInjectionInfo(instance, injectionInfo, type.BaseType);
            }
        }
    }
}
