using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hypocrite.Core.Container.Extensions
{
    internal static class TypeExtensions
    {
        internal static ConstructorInfo GetNormalConstructor(this Type mappedToType)
        {
            var parametrizedConstructor = mappedToType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(x => x.GetParameters().Length > 0);
            // check if it exists
            parametrizedConstructor = parametrizedConstructor ?? mappedToType.GetTypeInfo().DeclaredConstructors.FirstOrDefault();
            return parametrizedConstructor;
        }

        internal static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }
    }
}
