﻿
using ET.Demo;
using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    [FriendClass(typeof(GateMapComponent))]
    [FriendClass(typeof(SessionPlayerComponent))]
    [FriendClass(typeof(SessionStateComponent))]
    public class C2G_MyEnterGameHandler : AMRpcHandler<C2G_MyEnterGame, G2C_MyEnterGame>
    {
        protected override async ETTask Run(Session session, C2G_MyEnterGame request, G2C_MyEnterGame response, Action reply)
        {


            Player player = Game.EventSystem.Get(session.GetComponent<SessionPlayerComponent>().PlayerInstanceId) as Player;
           
            #region 二次登录
            if(player.PlayerState == PlayerState.Game)
            {
                IActorResponse reqEnter = await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestEnterGameState());
                if (reqEnter.Error == ErrorCode.ERR_Success)
                {
                    reply();
                    return;
                }
                response.Error = ErrorCode.ERR_Wrong;//没找到unit的话会走这步
                await DisconnectHelper.KickPlayer(player, true);
                reply();
                session?.Disconnect().Coroutine();
            }
            #endregion


            #region 正常进入

            GateMapComponent gatemapcomponent = player.AddComponent<GateMapComponent>();
            gatemapcomponent.Scene = await SceneFactory.Create(gatemapcomponent, "GateMap",SceneType.Map);

            

            Unit unit = UnitFactory.Create(gatemapcomponent.Scene, player.Id,UnitType.Player);
            unit.AddComponent<UnitGateComponent, long>(session.InstanceId);

            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(),"Map1");
            await TransferHelper.Transfer(unit, startSceneConfig.InstanceId, startSceneConfig.Name);

            player.UnitId = unit.Id; //15622//之前是roleid
            response.MyId = unit.Id; //15622

            response.Error = ErrorCode.ERR_Success;
            reply();
            #endregion

            #region player和session改变状态为game
            SessionStateComponent SessionStateComponent = session.GetComponent<SessionStateComponent>();
            if (SessionStateComponent == null)
            {
                SessionStateComponent = session.AddComponent<SessionStateComponent>();
            }
            SessionStateComponent.State = SessionState.Game;

            player.PlayerState = PlayerState.Game;
            #endregion


        }
    }
}
