﻿

using System;

namespace ET
{
    public class C2A_LoginAccountHandler:AMRpcHandler<C2A_LoginAccount,A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            await ETTask.CompletedTask;
            Log.Debug(session.DomainScene().Name);
            response.Message = "Good";
            reply();
        }
    }
}
