
using System.Collections.Generic;

namespace ET.Demo.ALoginCenter
{
    [ComponentOf(typeof(Scene))]
    public class LoginInfiRecordComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<long, int> AccountLoginInfoDict = new Dictionary<long, int>();
    }
}
