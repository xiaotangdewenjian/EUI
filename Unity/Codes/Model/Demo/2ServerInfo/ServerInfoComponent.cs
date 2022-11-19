
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType(typeof(ServerInfo))]
    public class ServerInfoComponent:Entity,IAwake,IDestroy
    {
        public List<ServerInfo> serverinfolist = new List<ServerInfo>();
        public int CurrentServerID = 0;
    }
}
