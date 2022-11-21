﻿namespace ET
{
    public static class DisconnectHelper
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            long instanceId = self.InstanceId;

            await TimerComponent.Instance.WaitAsync(1000);

            if (self.InstanceId != instanceId)
            {
                return;
            }
            self.Dispose();
            
        }
        
        public static async ETTask KickPlayer(Player player,bool isException = false)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }

            switch (player.PlayerState)
            {
                case PlayerState.Disconnect:
                    break;
                case PlayerState.Gate:
                    break;
                case PlayerState.Game:
                    //通知游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                    var m2GRequestExitGame = (M2G_RequestExitGame)await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestExitGame());

                    //通知移除账号角色登录信息
                    long LoginCenterConfigSceneId = StartSceneConfigCategory.Instance.LoginCenterConfig.InstanceId;
                    var L2G_RemoveLoginRecord = (L2G_RemoveLoginRecord)await MessageHelper.CallActor(LoginCenterConfigSceneId, new G2L_RemoveLoginRecord()
                    {
                        AccountId = player.AccountID,
                        ServerId = player.DomainZone()
                    });
                    break;
            }

            //player的domainscene还是gatescene
            player.PlayerState = PlayerState.Disconnect;
            player.DomainScene().GetComponent<PlayerComponent>()?.Remove(player.AccountID);
            player?.Dispose();
            await TimerComponent.Instance.WaitAsync(300);
        }
        
    }
}