using System;
using System.Reflection;

namespace Hypocrite.Core.Container.Common
{
    public class ConstructorInjectionInfo
    {
        public Func<object> DefaultConstructorDelegate { get; set; }
        public ConstructorInfo DefaultConstructorInfo { get; set; }
        public ParameterInfo[] InjectionMembers { get; set; }
    }
}
