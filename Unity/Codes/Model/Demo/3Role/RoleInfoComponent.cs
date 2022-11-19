
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType(typeof(RoleInfo))]
    public class RoleInfoComponent:Entity,IAwake,IDestroy
    {
        public List<RoleInfo> roleinfolist = new List<RoleInfo>();
        public long CurrentRoleID = 0;
    }
}
