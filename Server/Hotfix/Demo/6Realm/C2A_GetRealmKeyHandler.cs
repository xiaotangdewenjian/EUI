
using System;

namespace ET
{
    public class C2A_GetRealmKeyHandler : AMRpcHandler<C2A_GetRealmKey, A2C_GetRealmKey>
    {
        protected override async ETTask Run(Session session, C2A_GetRealmKey request, A2C_GetRealmKey response, Action reply)
        {
            #region 先检查令牌
            string checktoken = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountID);
            if (checktoken == null || checktoken != request.Token)
            {
                response.Error = ErrorCode.ERR_Token;
                reply();
                await TimerComponent.Instance.WaitAsync(1000);
                session.Dispose();
                return;
            }
            #endregion

            #region 服务器内部拿到realmkey的地址
            StartSceneConfig realmStartSceneConfig = RealmGateAddressHelper.GetRealm(request.ServerID);
            #endregion

            R2A_GetRealmKey r2agetrealmkey = (R2A_GetRealmKey)await MessageHelper.CallActor(realmStartSceneConfig.InstanceId, new A2R_GetRealmKey() { AccountId = request.AccountID });
            if (r2agetrealmkey.Error != ErrorCode.ERR_Success)
            {
                response.Error = ErrorCode.ERR_Wrong;
                reply();
                return;
            }

            response.RealmKey = r2agetrealmkey.RealmKey;
            response.RealmKeyAddress = realmStartSceneConfig.OuterIPPort.ToString();

            reply();
            await TimerComponent.Instance.WaitAsync(1000);
            session?.Dispose();
        }
    }
}
