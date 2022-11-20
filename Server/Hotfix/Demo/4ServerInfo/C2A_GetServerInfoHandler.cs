
using System;

namespace ET
{
    [FriendClass(typeof(ServerInfoManagerComponent))]
    public class C2A_GetServerInfoHandler : AMRpcHandler<C2A_GetServerInfos, A2C_GetServerInfos>
    {
        protected override async ETTask Run(Session session, C2A_GetServerInfos request, A2C_GetServerInfos response, Action reply)
        {
            //先检查令牌
            string checktoken = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountID);
            if (checktoken == null || checktoken != request.Token)
            {
                response.Error = ErrorCode.ERR_Token;
                reply();
                await TimerComponent.Instance.WaitAsync(1000);
                session.Dispose();
                return;
            }

            #region 通过的话把account服务器查询到的区服结构体返回给client
            //foreach (var ele in session.DomainScene().GetComponent<ServerInfoManagerComponent>().ServerInfoList)//ServerInfoManagerComponent的Awake会拿到数据库中的服务器
            //{
            //    response.ServerInfoProtoList.Add(ele.ToMessage());
            //}
            response.ServerInfoProtoList.Add(new ServerInfoProto() { ID = 1, Status = 1, ServerName = "破阵子" });
            response.ServerInfoProtoList.Add(new ServerInfoProto() { ID = 1, Status = 1, ServerName = "凌雪阁" });
            response.ServerInfoProtoList.Add(new ServerInfoProto() { ID = 1, Status = 1, ServerName = "暗影岛" });
            response.Error = ErrorCode.ERR_Success;
            reply();
            #endregion
        }
    }
}
