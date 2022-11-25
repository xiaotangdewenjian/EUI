
using System;

namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public class C2A_GetRoleHandler : AMRpcHandler<C2A_GetRole, A2C_GetRole>
    {
        protected override async ETTask Run(Session session, C2A_GetRole request, A2C_GetRole response, Action reply)
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

            var roleInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<RoleInfo>(d => d.AccountID == request.AccountID && d.ServerID == request.ServerID && d.Status == 1);

            if (roleInfos == null || roleInfos.Count == 0)
            {
                response.Error = ErrorCode.ERR_Wrong;
                reply();
                return;
            }

            foreach (var roleInfo in roleInfos)
            {
                response.RoleInfoList.Add(roleInfo.ToMessage());
                roleInfo?.Dispose();
            }
            roleInfos.Clear();

            reply();
        }

    }
}
