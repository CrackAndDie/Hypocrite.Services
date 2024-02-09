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

        public string Name { get; set; }

        public ParameterInfo[] InjectionMembers { get; set; }

        public RegistrationType RegistrationType { get; set; }

        public override string ToString()
        {
            return $"Reg: {RegisteredType}, Map: {MappedToType}, Type: {RegistrationType}";
        }
    }
}
