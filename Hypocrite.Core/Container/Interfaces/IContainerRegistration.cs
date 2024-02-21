using Hypocrite.Core.Container.Common;
using Hypocrite.Core.Container.InstancePolicy;
using Hypocrite.Core.Container.Registration;
using System;

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

        BasePolicy RegistrationPolicy { get; set; }
    }
}
