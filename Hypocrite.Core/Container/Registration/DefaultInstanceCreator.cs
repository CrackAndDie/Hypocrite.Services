using Hypocrite.Core.Container.Common;
using Hypocrite.Core.Container.Extensions;
using Hypocrite.Core.Container.Interfaces;
using System;
using System.Collections.Generic;

namespace Hypocrite.Core.Container.Registration
{
    public class DefaultInstanceCreator : IInstanceCreator
    {
        private ILightContainer _lightContainer;

        public DefaultInstanceCreator(ILightContainer lightContainer)
        {
            _lightContainer = lightContainer;
        }

        public object CreateInstance(IContainerRegistration registration, bool withInjections)
        {
            object instance = CreatePureInstance(registration);

            if (withInjections && registration.MemberInjectionInfo.RequireInjection)
            {
				ResolveInjections(instance, registration.MemberInjectionInfo);
                registration.MemberInjectionInfo.IsInjected = true;
			}

            return instance;
        }

        public void ResolveInjections(object instance, MemberInjectionInfo injectionInfo)
        {
            // there are if statements because they are more efficient on empty lists
            if (injectionInfo?.InjectionFields.Count > 0)
            {
                // fields
                foreach (var f in injectionInfo?.InjectionFields)
                {
                    // recursion problem solver
                    if (f.GetValue(instance) != null)
                        continue;
                    var dep = _lightContainer.Resolve(f.FieldType, string.Empty, false, out IContainerRegistration reg);
                    f.SetValue(instance, dep);
                    if (reg == null)
                        continue;
                    if (reg.MemberInjectionInfo.RequireInjection && !reg.MemberInjectionInfo.IsInjected)
                    {
                        ResolveInjections(dep, reg.MemberInjectionInfo);
                        reg.RegistrationPolicy.OnInjectionDone();
                    }
                }
            }
            if (injectionInfo?.InjectionProperties.Count > 0)
            {
                // properties
                foreach (var p in injectionInfo?.InjectionProperties)
                {
                    // recursion problem solver
                    if (p.GetValue(instance) != null)
                        continue;
                    var dep = _lightContainer.Resolve(p.PropertyType, string.Empty, false, out IContainerRegistration reg);
                    p.SetValue(instance, dep, null);
                    if (reg == null)
                        continue;
                    if (reg.MemberInjectionInfo.RequireInjection && !reg.MemberInjectionInfo.IsInjected)
                    {
                        ResolveInjections(dep, reg?.MemberInjectionInfo);
                        reg.RegistrationPolicy.OnInjectionDone();
                    }
                }
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

        public object CreatePureInstance(IContainerRegistration registration)
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
                    var paramInstance = _lightContainer.Resolve(param.ParameterType);
                    // if parameter has default value then it should be applied
                    if (paramInstance == param.ParameterType.GetDefaultValue() && param.DefaultValue != DBNull.Value)
                    {
                        paramInstance = param.DefaultValue;
                    }
                    constructorParams.Add(paramInstance);
                }
				instance = registration.ConstructorInjectionInfo.DefaultConstructorInfo.Invoke(constructorParams.ToArray());
			}
            return instance;
        }
    }
}
