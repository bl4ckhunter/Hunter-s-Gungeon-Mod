using System;
using System.Collections.Generic;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x02000072 RID: 114
	public abstract class SimpleInteractable : BraveBehaviour
	{
		// Token: 0x04000105 RID: 261
		public Action<PlayerController, GameObject> OnAccept;

		// Token: 0x04000106 RID: 262
		public Action<PlayerController, GameObject> OnDecline;

		// Token: 0x04000107 RID: 263
		public List<string> conversation;

		// Token: 0x04000108 RID: 264
		public Func<PlayerController, GameObject, bool> CanUse;

		// Token: 0x04000109 RID: 265
		public Transform talkPoint;

		// Token: 0x0400010A RID: 266
		public string text;

		// Token: 0x0400010B RID: 267
		public string acceptText;

		// Token: 0x0400010C RID: 268
		public string declineText;

		// Token: 0x0400010D RID: 269
		public bool isToggle;

		// Token: 0x0400010E RID: 270
		protected bool m_isToggled;

		// Token: 0x0400010F RID: 271
		protected bool m_canUse = true;
	}
}