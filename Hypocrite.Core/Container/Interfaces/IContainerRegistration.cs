using Hypocrite.Core.Container.Registration;
using System;
using System.Reflection;

namespace Hypocrite.Core.Container.Interfaces
{
    public interface IContainerRegistration
    {
        Type RegisteredType { get; }
        Type MappedToType { get; }
        object Instance { get; set; }

        string Name { get; set; }

        ParameterInfo[] InjectionMembers { get; set; }
        RegistrationType RegistrationType { get; }
    }
}
