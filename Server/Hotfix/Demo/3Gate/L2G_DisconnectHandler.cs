
using System;

namespace ET
{
    [FriendClass(typeof(Player))]
    //这个scene就是gate网关
    public class L2G_DisconnectHandler:AMActorRpcHandler<Scene, L2G_DisconnectGateUnit,G2L_DisconnectGateUnit>
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

            scene.GetComponent<GateSessionKeyComponent>().Remove(request.AccountID);
            Session gsession = player.Session;
            if(gsession != null && !gsession.IsDisposed)
            {
                gsession.Send(new A2C_Disconnect() { });
                await TimerComponent.Instance.WaitAsync(1000);
                gsession.Dispose();
            }
            player.Session.Dispose();
            player.AddComponent<PlayerOfflineOutTimeComponent>();//10秒后将玩家踢下线
            reply();
        }
    }
}
