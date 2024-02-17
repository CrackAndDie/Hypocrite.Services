using Hypocrite.Core.Container.Extensions;
using Hypocrite.Core.Container.Interfaces;
using Hypocrite.Core.Container.Registration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hypocrite.Core.Container
{
    public class LightContainer : ILightContainer
    {
        public IList<IContainerRegistration> Registrations { get; set; } = new List<IContainerRegistration>();

        public IInstanceCreator InstanceCreator { get; set; } = new DefaultInstanceCreator();

        public void SetInstanceCreator(IInstanceCreator creator)
        {
            InstanceCreator = creator;
        }

        public bool IsRegistered(Type type)
        {
            return IsRegistered(type, out IContainerRegistration _);
        }

        public bool IsRegistered(Type type, out IContainerRegistration registration)
        {
            return IsRegistered(type, string.Empty, out registration);
        }

        public bool IsRegistered(Type type, string name)
        {
            return IsRegistered(type, name, out IContainerRegistration _);
        }

        public bool IsRegistered(Type type, string name, out IContainerRegistration registration)
        {
            var regs = string.IsNullOrWhiteSpace(name) ? Registrations : Registrations.Where(x => x.Name == name);
            registration = regs.FirstOrDefault(x => x.RegisteredType == type);
            return registration != null;
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
            AddOrReplace(registration);
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
                Name = name,
            };
            AddOrReplace(registration);
            return this;
        }

        public ILightContainer RegisterType(Type registeredType, Type mappedToType, bool isSingleton = false)
        {
            return RegisterType(registeredType, mappedToType, string.Empty, isSingleton);
        }

        public ILightContainer RegisterType(Type registeredType, Type mappedToType, string name, bool isSingleton = false)
        {
            var parametrizedConstructor = mappedToType.GetNormalConstructor();

            var registration = new ContainerRegistration()
            {
                RegisteredType = registeredType,
                MappedToType = mappedToType,
                Instance = null,
                RegistrationType = isSingleton ? RegistrationType.Instance : RegistrationType.Type,
                InjectionMembers = parametrizedConstructor.GetParameters(),
                Name = name,
            };
            AddOrReplace(registration);
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
            if (IsRegistered(type, name, out IContainerRegistration registration))
            {
                switch (registration.RegistrationType)
                {
                    case RegistrationType.Type:
                    case RegistrationType.Instance:
                        {
                            return registration.GetInstance(this, withInjections);
                        }
                    case RegistrationType.Func:
                        {
                            return registration.GetFunc();
                        }
                }
            }

            // try to resolve with empty name
            if (!string.IsNullOrWhiteSpace(name))
            {
                return Resolve(type, string.Empty, withInjections);
            }

            // check for primitives
            if (type.IsPrimitive || type == typeof(string))
            {
                return type.GetDefaultValue();
            }

            // try to create it by my own
            if (type.IsClass)
            {
                var parametrizedConstructor = type.GetNormalConstructor();
                var tempRegistration = new ContainerRegistration()
                {
                    RegisteredType = type,
                    MappedToType = type,
                    Instance = null,
                    RegistrationType = RegistrationType.Type,
                    InjectionMembers = parametrizedConstructor.GetParameters(),
                };
                return tempRegistration.GetInstance(this);
            }
            return null;
        }

        public void ResolveInjections(object instance)
        {
            if (InstanceCreator.RequiresInjections(instance))
            {
                InstanceCreator.ResolveInjections(instance, this);
            }
        }

        public void Dispose()
        {
            
        }

        private int IndexOfReg(IContainerRegistration registration)
        {
            if (IsRegistered(registration.RegisteredType, registration.Name))
            {
                int index = Registrations.Select(x => (x.RegisteredType, x.Name)).ToList().IndexOf((registration.RegisteredType, registration.Name));
                return index;
            }
            return -1;
        }

        private void AddOrReplace(IContainerRegistration registration)
        {
            int ind;
            if ((ind = IndexOfReg(registration)) != -1)
            {
                Registrations[ind] = registration;
            }
            else
            {
                Registrations.Add(registration);
            }
        }
    }
}
