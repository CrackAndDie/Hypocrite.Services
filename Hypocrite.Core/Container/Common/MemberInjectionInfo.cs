using System.Collections.Generic;
using System.Reflection;

namespace Hypocrite.Core.Container.Common
{
    public class MemberInjectionInfo
    {
        public IList<FieldInfo> InjectionFields { get; private set; } = new List<FieldInfo>();
        public IList<PropertyInfo> InjectionProperties { get; private set; } = new List<PropertyInfo>();
        public bool IsInjected { get; set; } = false;
        public bool RequireInjection { get; private set; } // when the lists are not empty

        public void CalcInjectionReq()
        {
            RequireInjection = InjectionFields.Count > 0 || InjectionProperties.Count > 0;
        }
    }
}
