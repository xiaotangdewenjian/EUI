
using System.Collections.Generic;

namespace ET
{
    public class ServerInfoComponent:Entity,IAwake,IDestroy
    {
        public List<ServerInfo> serverinfolist = new List<ServerInfo>();
    }
}
