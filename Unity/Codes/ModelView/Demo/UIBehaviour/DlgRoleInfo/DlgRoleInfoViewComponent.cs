
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgRoleInfoViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Text E_GoldText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_GoldText == null )
     			{
		    		this.m_E_GoldText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_Gold");
     			}
     			return this.m_E_GoldText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_GoldText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Text m_E_GoldText = null;
		public Transform uiTransform = null;
	}
}
