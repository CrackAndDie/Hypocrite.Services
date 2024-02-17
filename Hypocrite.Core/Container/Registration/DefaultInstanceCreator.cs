using Hypocrite.Core.Container.Extensions;
using Hypocrite.Core.Container.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hypocrite.Core.Container.Registration
{
    public class DefaultInstanceCreator : IInstanceCreator
    {
        public object CreateInstance(IContainerRegistration registration, ILightContainer container, bool withInjections = true)
        {
            var constructor = registration.MappedToType.GetNormalConstructor();

            List<object> constructorParams = new List<object>();
            // injecting constructor parameters
            if (registration.InjectionMembers != null)
            {
                foreach (var param in registration.InjectionMembers)
                {
                    var paramInstance = container.Resolve(param.ParameterType);
                    // if parameter has default value then it should be applied
                    if (paramInstance == param.ParameterType.GetDefaultValue() && param.DefaultValue != DBNull.Value)
                    {
                        paramInstance = param.DefaultValue;
                    }
                    constructorParams.Add(paramInstance);
                }
            }

            var instance = constructor?.Invoke(constructorParams.ToArray());
            if (registration.RegistrationType == RegistrationType.Type)
            {
                // save the instance as cache
                registration.Instance = instance;
            }

            if (withInjections && RequiresInjections(instance))
                ResolveInjections(instance, container);

            if (registration.RegistrationType == RegistrationType.Type)
            {
                // remove the cached instance
                registration.Instance = null;
            }
            return instance;
        }

        public void ResolveInjections(object instance, ILightContainer container, Type type = null)
        {
            type = type ?? instance.GetType();
            // fields
            foreach (var f in type.GetTypeInfo().DeclaredFields)
            {
                var attrs = f.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    // recursion problem solver
                    if (f.GetValue(instance) != null)
                        continue;
                    var dep = container.Resolve(f.FieldType, false);
                    f.SetValue(instance, dep);
                    container.ResolveInjections(dep);
                }
            }
            // properties
            foreach (var p in type.GetTypeInfo().DeclaredProperties)
            {
                var attrs = p.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    // recursion problem solver
                    if (p.GetValue(instance) != null)
                        continue;
                    var dep = container.Resolve(p.PropertyType, false);
                    p.SetValue(instance, dep, null);
                    container.ResolveInjections(dep);
                }
            }

            if (type.BaseType != null)
            {
                ResolveInjections(instance, container, type.BaseType);
            }
        }

        public bool RequiresInjections(object instance, Type type = null)
        {
            type = type ?? instance.GetType();
            // fields
            foreach (var f in type.GetTypeInfo().DeclaredFields)
            {
                var attrs = f.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    if (f.GetValue(instance) == null)
                        return true;
                }
            }
            // properties
            foreach (var p in type.GetTypeInfo().DeclaredProperties)
            {
                var attrs = p.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    if (p.GetValue(instance) == null)
                        return true;
                }
            }

            if (type.BaseType != null)
            {
                return RequiresInjections(instance, type.BaseType);
            }

            return false;
        }
    }
}
