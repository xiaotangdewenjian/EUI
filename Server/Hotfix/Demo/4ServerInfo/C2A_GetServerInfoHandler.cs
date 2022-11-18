
using System;

namespace ET
{
    [FriendClass(typeof(ServerInfoManagerComponent))]
    public class C2A_GetServerInfoHandler : AMRpcHandler<C2A_GetServerInfos, A2C_GetServerInfos>
    {
        protected override async ETTask Run(Session session, C2A_GetServerInfos request, A2C_GetServerInfos response, Action reply)
        {
            string checktoken = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountID);
            if (checktoken == null || checktoken != request.Token)
            {
                response.Error = ErrorCode.ERR_Token;
                reply();
                await TimerComponent.Instance.WaitAsync(1000);
                session.Dispose();
            }
            foreach(var ele in session.DomainScene().GetComponent<ServerInfoManagerComponent>().ServerInfoList)//ServerInfoManagerComponent的Awake会拿到数据库中的服务器
            {
                response.ServerInfoProtoList.Add(ele.ToMessage());
            }
            reply();
            await ETTask.CompletedTask;
        }
    }
}
