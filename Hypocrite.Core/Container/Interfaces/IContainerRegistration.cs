using Hypocrite.Core.Container.Common;
using Hypocrite.Core.Container.Registration;
using System;
using System.Reflection;

namespace Hypocrite.Core.Container.Interfaces
{
    public interface IContainerRegistration : IEquatable<IContainerRegistration>
    {
        Type RegisteredType { get; }
        Type MappedToType { get; }
        object Instance { get; set; }

        RegistrationType RegistrationType { get; }

        MemberInjectionInfo MemberInjectionInfo { get; }
        ConstructorInjectionInfo ConstructorInjectionInfo { get; }
    }
}
