using Hypocrite.Core.Container.Common;
using Hypocrite.Core.Container.Interfaces;
using System;
using System.Reflection;

namespace Hypocrite.Core.Container.Registration
{
    public class ContainerRegistration : IContainerRegistration
    {
        public Type RegisteredType { get; set; }

        public Type MappedToType { get; set; }

        public object Instance { get; set; }
        
        public RegistrationType RegistrationType { get; set; }

        public MemberInjectionInfo MemberInjectionInfo { get; private set; } = new MemberInjectionInfo();
        public ConstructorInjectionInfo ConstructorInjectionInfo { get; private set; } = new ConstructorInjectionInfo();

        /// <summary>
        /// Returns null because all the checks should be inside <see cref="QuickSet{TValue}"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Always true</returns>
        public bool Equals(IContainerRegistration other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Reg: {RegisteredType}, Map: {MappedToType}, Type: {RegistrationType}";
        }
    }
}
