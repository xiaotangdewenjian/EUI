﻿using System.Collections;
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
			if(errcode != ErrorCode.ERR_Success)
			{
				Log.Debug("aaaaaa");
				return;
			}
			self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
			self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Lobby);

		}
		
		public static void HideWindow(this DlgLogin self)
		{

		}
		
	}
}
