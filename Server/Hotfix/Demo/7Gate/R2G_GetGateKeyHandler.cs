
using System;

namespace ET
{
    public class R2G_GetGateKeyHandler : AMActorRpcHandler<Scene, R2G_GetLoginGateKey, G2R_GetLoginGateKey>
    {
        protected override async ETTask Run(Scene scene, R2G_GetLoginGateKey request, G2R_GetLoginGateKey response, Action reply)
        {
            string key = RandomHelper.RandInt64().ToString();
            scene.GetComponent<GateSessionKeyComponent>().Remove(request.AccountId);
            scene.GetComponent<GateSessionKeyComponent>().Add(request.AccountId, key);//这玩意就是用来当作tokencomponent的
            response.GateSessionKey = key;
            reply();
            await ETTask.CompletedTask;
        }
    }
}
