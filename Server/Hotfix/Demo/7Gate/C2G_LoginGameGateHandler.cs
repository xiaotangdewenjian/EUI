
using System;
using ET.Demo;
using static System.Formats.Asn1.AsnWriter;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    [FriendClass(typeof(SessionStateComponent))]
    [FriendClass(typeof(PlayerComponent))]
    [FriendClass(typeof(Player))]
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

            #region 通知登录中心记录一下
            StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.LoginCenterConfig;
            L2G_AddLoginRecord l2ARoleLogin = (L2G_AddLoginRecord)await MessageHelper.CallActor(loginCenterConfig.InstanceId,
                                                                                new G2L_AddLoginRecord() { AccountId = request.Account, ServerId = session.DomainScene().Zone });
            #endregion

            #region 改变一下一个组件的状态
            SessionStateComponent SessionStateComponent = session.GetComponent<SessionStateComponent>();
            if (SessionStateComponent == null)
            {
                SessionStateComponent = session.AddComponent<SessionStateComponent>();
            }
            SessionStateComponent.State = SessionState.Normal;
            #endregion


            //Player对应着AccountID和UnityID，可能这就是一种映射吧
            Player player = session.DomainScene().GetComponent<PlayerComponent>().Get(request.Account);

            if(player == null)
            {
                player = session.DomainScene().GetComponent<PlayerComponent>().AddChildWithId<Player, long, long>(request.Account, request.Account, request.RoleId);
                player.PlayerState = PlayerState.Gate;
                player.Session = session;//player能拿到session实体
                session.DomainScene().GetComponent<PlayerComponent>().idPlayers.Add(player.AccountID,player);
                //注意下面的是给session添加MailBoxComponent
                session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
            }

            session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;//156
            session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;//session能拿到player实体
            session.GetComponent<SessionPlayerComponent>().AccountID = player.AccountID;
            player.Session = session;
            reply();

        }
    }
}
