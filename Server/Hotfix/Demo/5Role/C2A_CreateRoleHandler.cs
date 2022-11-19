

using System;

namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public class C2A_CreateRoleHandler:AMRpcHandler<C2A_CreateRole,A2C_CreateRole>
    {
        protected override async ETTask Run(Session session, C2A_CreateRole request, A2C_CreateRole response, Action reply)
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



            #region 检查数据库
            var roleinfo = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<RoleInfo>(d => d.Name == request.Name && d.ServerID == request.ServerID);
            if (roleinfo != null && roleinfo.Count > 0)
            {
                reply();
                response.Error = ErrorCode.ERR_NameChong;
                return;
            }



            #endregion

           
            RoleInfo newroleInfo = session.AddChildWithId<RoleInfo>(IdGenerater.Instance.GenerateUnitId(request.ServerID));

            newroleInfo.Name = request.Name;
            newroleInfo.AccountID = request.AccountID;
            newroleInfo.ServerID = request.ServerID;


            newroleInfo.Status = (int)RoleInfoState.Normal;
            newroleInfo.CreateTime = TimeHelper.ServerNow();
            newroleInfo.LastLoginTime = 0;

            await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<RoleInfo>(newroleInfo);
            response.RoleInfostruct = newroleInfo.ToMessage();//因为之能传送结构体
            reply();

            newroleInfo?.Dispose();
            await ETTask.CompletedTask;
        }
    }
}
