using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ET
{
	public static  class DlgLoginSystem
	{

		public static void RegisterUIEvent(this DlgLogin self)
		{
			self.View.E_LoginButton.AddListener(() => { self.OnLoginClickHandler();});
		}

		public static void ShowWindow(this DlgLogin self, Entity contextData = null)
		{
			
		}
		
		public static async void OnLoginClickHandler(this DlgLogin self)
		{
			int errcode =  await LoginHelper.Login(
													self.DomainScene(), 
													ConstValue.LoginAddress, 
													self.View.E_AccountInputField.GetComponent<InputField>().text, 
													self.View.E_PasswordInputField.GetComponent<InputField>().text);
            #region 登录错误啥都不干打印出登录错误
            if (errcode != ErrorCode.ERR_Success)
            {
                Log.Debug("登录错误");
                return;
            }
            #endregion

            #region 服务器错误啥都不干打印出服务器错误
            errcode = await LoginHelper.GetServerInfo(self.DomainScene());
            if (errcode != ErrorCode.ERR_Success)
            {
                Log.Debug("服务器错误");
                return;
            }
            #endregion



            self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
			self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_ChooseServer);

		}
		
		public static void HideWindow(this DlgLogin self)
		{

		}
		
	}
}
