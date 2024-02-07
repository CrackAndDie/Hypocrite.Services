using System;
using System.Collections.Generic;
using System.Text;

namespace Hypocrite.Core.Container.Interfaces
{
    public interface IRequireInjection
    {
        void OnInjectionsReady();
        void OnResolveReady();
    }
}
