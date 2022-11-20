
using System;

namespace ET.Demo._6Realm
{
    public class C2R_LoginRealmHandler:AMRpcHandler<C2R_LoginRealm, R2C_LoginRealm>
    {
        protected override async ETTask Run(Session session, C2R_LoginRealm request, R2C_LoginRealm response, Action reply)
        {
            #region 先检查realm令牌
            string checktoken = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);
            if (checktoken == null || checktoken != request.RealmTokenKey)
            {
                response.Error = ErrorCode.ERR_Token;
                reply();
                await TimerComponent.Instance.WaitAsync(1000);
                session.Dispose();
                return;
            }
            #endregion

            session.DomainScene().GetComponent<TokenComponent>().Remove(request.AccountId);//不在链接realm，以后不会再验证了，所以移除
            
            // 取模固定分配一个Gate,得到gate的地址
            StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainScene().Zone, request.AccountId);

            #region 从gate获得gatesessionkey
            G2R_GetLoginGateKey g2RGetLoginKey = (G2R_GetLoginGateKey)await MessageHelper.CallActor(config.InstanceId, new R2G_GetLoginGateKey() { AccountId = request.AccountId });
            if (g2RGetLoginKey.Error != ErrorCode.ERR_Success)
            {
                response.Error = ErrorCode.ERR_Wrong;
                reply();
                return;
            }
            #endregion

            response.GateAddress = config.OuterIPPort.ToString();//127.0.0.1:10003

            #region 把gatesession和返回给client
            response.GateSessionKey = g2RGetLoginKey.GateSessionKey;
            Log.Debug(response.GateAddress);
            
            response.Error = ErrorCode.ERR_Success;
            reply();
            #endregion

        }
    }
}
