using Hypocrite.Core.Container.Common;
using Hypocrite.Core.Container.Extensions;
using Hypocrite.Core.Container.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Hypocrite.Core.Container.Registration
{
    public class DefaultInstanceCreator : IInstanceCreator
    {
        public object CreateInstance(IContainerRegistration registration, ILightContainer container, bool withInjections = true)
        {
            // creating instance depending on constructor info
            object instance = null;
            if (registration.ConstructorInjectionInfo.DefaultConstructorDelegate != null)
            {
                instance = registration.ConstructorInjectionInfo.DefaultConstructorDelegate();
            }
            else if (registration.ConstructorInjectionInfo.DefaultConstructorInfo != null)
            {
                List<object> constructorParams = new List<object>();
                foreach (var param in registration.ConstructorInjectionInfo.InjectionMembers)
                {
                    var paramInstance = container.Resolve(param.ParameterType);
                    // if parameter has default value then it should be applied
                    if (paramInstance == param.ParameterType.GetDefaultValue() && param.DefaultValue != DBNull.Value)
                    {
                        paramInstance = param.DefaultValue;
                    }
                    constructorParams.Add(paramInstance);
                }
                instance = registration.ConstructorInjectionInfo.DefaultConstructorInfo.Invoke(constructorParams.ToArray());
            }
            
            if (registration.RegistrationType == RegistrationType.Type)
            {
                // save the instance as cache
                registration.Instance = instance;
            }

            if (withInjections)
                ResolveInjections(instance, container, registration.MemberInjectionInfo);

            if (registration.RegistrationType == RegistrationType.Type)
            {
                // remove the cached instance
                registration.Instance = null;
            }
            return instance;
        }

        public void ResolveInjections(object instance, ILightContainer container, MemberInjectionInfo injectionInfo)
        {
            // fields
            foreach (var f in injectionInfo.InjectionFields)
            {
                // recursion problem solver
                if (f.GetValue(instance) != null)
                    continue;
                var dep = container.Resolve(f.FieldType, string.Empty, false, out IContainerRegistration reg);
                f.SetValue(instance, dep);
                container.ResolveInjections(dep, reg?.MemberInjectionInfo);
            }
            // properties
            foreach (var p in injectionInfo.InjectionProperties)
            {
                // recursion problem solver
                if (p.GetValue(instance) != null)
                    continue;
                var dep = container.Resolve(p.PropertyType, string.Empty, false, out IContainerRegistration reg);
                p.SetValue(instance, dep, null);
                container.ResolveInjections(dep, reg?.MemberInjectionInfo);
            }
        }

        public bool RequiresInjections(object instance, MemberInjectionInfo injectionInfo)
        {
            // fields
            foreach (var f in injectionInfo.InjectionFields)
            {
                if (f.GetValue(instance) == null)
                    return true;
            }
            // properties
            foreach (var p in injectionInfo.InjectionProperties)
            {
                if (p.GetValue(instance) == null)
                    return true;
            }
            return false;
        }
    }
}
