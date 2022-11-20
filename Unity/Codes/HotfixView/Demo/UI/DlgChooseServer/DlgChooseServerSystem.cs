using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[FriendClass(typeof(DlgChooseServer))]
	public static  class DlgChooseServerSystem
	{

		public static void RegisterUIEvent(this DlgChooseServer self)
		{
			self.View.EEnterButton.onClick.AddListener(self.ENterServer);
		}

		public static void ShowWindow(this DlgChooseServer self, Entity contextData = null)
		{
		}

        public static async void ENterServer(this DlgChooseServer self)
		{

			int errcoed = await LoginHelper.GetRole(self.DomainScene());
			if(errcoed != ErrorCode.ERR_Success)
			{
				Log.Debug("没有拿到角色");
				return;
			}
            self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_ChooseServer);
            self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Role);
        }


    }
}
