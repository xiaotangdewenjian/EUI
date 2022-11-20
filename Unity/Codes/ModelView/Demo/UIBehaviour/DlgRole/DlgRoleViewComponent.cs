
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgRoleViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button EEnterGameButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EEnterGameButton == null )
     			{
		    		this.m_EEnterGameButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EEnterGame");
     			}
     			return this.m_EEnterGameButton;
     		}
     	}

		public UnityEngine.UI.Image EEnterGameImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EEnterGameImage == null )
     			{
		    		this.m_EEnterGameImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EEnterGame");
     			}
     			return this.m_EEnterGameImage;
     		}
     	}

		public UnityEngine.UI.Button ECreateRoleButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ECreateRoleButton == null )
     			{
		    		this.m_ECreateRoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"ECreateRole");
     			}
     			return this.m_ECreateRoleButton;
     		}
     	}

		public UnityEngine.UI.Image ECreateRoleImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ECreateRoleImage == null )
     			{
		    		this.m_ECreateRoleImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"ECreateRole");
     			}
     			return this.m_ECreateRoleImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EEnterGameButton = null;
			this.m_EEnterGameImage = null;
			this.m_ECreateRoleButton = null;
			this.m_ECreateRoleImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_EEnterGameButton = null;
		private UnityEngine.UI.Image m_EEnterGameImage = null;
		private UnityEngine.UI.Button m_ECreateRoleButton = null;
		private UnityEngine.UI.Image m_ECreateRoleImage = null;
		public Transform uiTransform = null;
	}
}
