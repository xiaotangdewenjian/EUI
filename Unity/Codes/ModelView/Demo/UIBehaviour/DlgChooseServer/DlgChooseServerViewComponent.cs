
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgChooseServerViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button EEnterButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EEnterButton == null )
     			{
		    		this.m_EEnterButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EEnter");
     			}
     			return this.m_EEnterButton;
     		}
     	}

		public UnityEngine.UI.Image EEnterImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EEnterImage == null )
     			{
		    		this.m_EEnterImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EEnter");
     			}
     			return this.m_EEnterImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EEnterButton = null;
			this.m_EEnterImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_EEnterButton = null;
		private UnityEngine.UI.Image m_EEnterImage = null;
		public Transform uiTransform = null;
	}
}
