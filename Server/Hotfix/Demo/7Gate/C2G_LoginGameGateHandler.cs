
using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    public class C2G_LoginGameGateHandler : AMRpcHandler<C2G_LoginGameGate, G2C_LoginGameGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGameGate request, G2C_LoginGameGate response, Action reply)
        {
            #region 先检查gate令牌
            string checktoken = session.DomainScene().GetComponent<GateSessionKeyComponent>().Get(request.Account);
            if (checktoken == null || checktoken != request.Key)
            {
                response.Error = ErrorCode.ERR_Token;
                reply();
                await TimerComponent.Instance.WaitAsync(1000);
                session.Dispose();
                return;
            }
            #endregion

            session.RemoveComponent<SessionAcceptTimeoutComponent>();//学习a2clogin
            //这里有与登录中心服有关的操作我就不写了

            //Player对应着AccountID和UnityID，可能这就是一种映射吧
            Player player = session.DomainScene().GetComponent<PlayerComponent>().Get(request.RoleId);

            if(player == null)
            {
                player = session.DomainScene().GetComponent<PlayerComponent>().AddChildWithId<Player, long, long>(request.RoleId, request.Account, request.RoleId);
                player.PlayerState = PlayerState.Gate;
                session.DomainScene().GetComponent<PlayerComponent>().Add(player);
                //注意下面的是给session添加MailBoxComponent
                session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
            }

            session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;//156
            session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;//session能拿到player实体
            player.sessioninstanceid = session.InstanceId;//player能拿到session实体


        }
    }
}
