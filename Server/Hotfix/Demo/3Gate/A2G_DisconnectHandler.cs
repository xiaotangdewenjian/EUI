
using System;

namespace ET
{
    //这个scene就是gate网关
    public class A2G_DisconnectHandler:AMActorRpcHandler<Scene, L2G_DisconnectGateUnit,G2L_DisconnectGateUnit>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnit request, G2L_DisconnectGateUnit response, Action reply)
        {
            Player player = scene.GetComponent<PlayerComponent>().Get(request.AccountID);//尝试拿到gate上对应的玩家
            if(player == null)
            {
                reply();
                response.Error = ErrorCode.ERR_Success;
                return;
            }

            scene.GetComponent<PlayerComponent>().Remove(request.AccountID);
            player.Dispose();
            response.Error = ErrorCode.ERR_Wrong;
            await ETTask.CompletedTask;
            
            reply();
        }
    }
}
