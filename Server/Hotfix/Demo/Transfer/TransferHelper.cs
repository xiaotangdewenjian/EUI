namespace ET
{
    public static class TransferHelper
    {
        public static async ETTask Transfer(Unit unit, long ActorInstanceId, string sceneName)
        {
            // 通知客户端开始切场景
            MessageHelper.SendToClient(unit, new M2C_StartSceneChange() { SceneInstanceId = ActorInstanceId, SceneName = sceneName });
            
            M2M_UnitTransferRequest m2mrequest = new M2M_UnitTransferRequest();

            #region 把unit身上的每一个组件保存到消息体中
            m2mrequest.Unit = unit;
            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is ITransfer)
                {
                    m2mrequest.Entitys.Add(entity);
                }
            }
            #endregion

            // 删除Mailbox,让发给Unit的ActorLocation消息重发
            unit.RemoveComponent<MailBoxComponent>();

            // location加锁
            long oldInstanceId = unit.InstanceId;//随机的
            await LocationProxyComponent.Instance.Lock(unit.Id, unit.InstanceId);//这里就是城市间转移

            M2M_UnitTransferResponse response = await ActorMessageSenderComponent.Instance.Call(ActorInstanceId, m2mrequest) as M2M_UnitTransferResponse;
            
            await LocationProxyComponent.Instance.UnLock(unit.Id, oldInstanceId, response.NewInstanceId);
            unit.Dispose();
        }
    }
}