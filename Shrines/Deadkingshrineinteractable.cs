using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x02000067 RID: 103
	public class DeadkingShrineInteractable : SimpleInteractable, IPlayerInteractable
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x0002DAF4 File Offset: 0x0002BCF4
		private void Start()
		{   
			this.talkPoint = base.transform.Find("talkpoint");
			talkPoint.position += new Vector3(0.4f, 2f, 0);
			this.m_isToggled = false; 
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			this.m_canUse = true;
			Shader shader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
			Material material = new Material(Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass"));
			material.name = "HologramMaterial";
			Material material2 = material;
			material.SetTexture("_MainTex", base.sprite.renderer.material.GetTexture("_MainTex"));
			material2.SetTexture("_MainTex", base.sprite.renderer.sharedMaterial.GetTexture("_MainTex"));
			material.SetColor(Shader.PropertyToID("_OverrideColor"), new Color32(235, 208, 103, 255));
			base.sprite.renderer.material.shader = shader;
			base.sprite.renderer.material = material;
			base.sprite.renderer.sharedMaterial = material2;
			base.sprite.usesOverrideMaterial = true;
		}

		

		

		// Token: 0x060004C8 RID: 1224 RVA: 0x0002DB48 File Offset: 0x0002BD48
		public void Interact(PlayerController interactor)
		{
			bool flag = TextBoxManager.HasTextBox(this.talkPoint);
			bool flag2 = !flag;
			bool flag3 = flag2;
			if (flag3)
			{
				this.m_canUse = ((this.CanUse != null) ? this.CanUse(interactor, base.gameObject) : this.m_canUse);
				bool flag4 = !this.m_canUse;
				bool flag5 = flag4;
				bool flag6 = flag5;
				if (flag6)
				{   if (interactor.stats.GetStatValue(PlayerStats.StatType.Curse) < 5f)
					{
						TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "There is no curse in your blood. DISGUSTING!", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
					}
					else
					{

						TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "If you have no blood to give, BEGONE!", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
					}
				}
				else
				{
					base.StartCoroutine(this.HandleConversation(interactor));
				}
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0002DC09 File Offset: 0x0002BE09
		private IEnumerator HandleConversation(PlayerController interactor)
		{  
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			interactor.SetInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
			yield return null;
			List<string> conversationToUse = this.conversation;
			this.m_allowMeToIntroduceMyself = false;
			TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, "The Dead King demands a tithe! OF BLOOD!", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
			string acceptanceTextToUse = this.acceptText;
			string declineTextToUse = this.declineText;
			string strig = 30f.ToString();
		    GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, $"<Cut Yourself>", "<Walk Away>");
			int selectedResponse = -1;
			while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
			{
				yield return null;
			}
			bool flag = selectedResponse == 0;
			selfdestrut = false;
			bool flag2 = flag;
			if (flag2)
			{
				TextBoxManager.ClearTextBox(this.talkPoint);
				Action<PlayerController, GameObject> onAccept = this.OnAccept;
				bool flag3 = onAccept != null;
				if (flag3)
				{
					onAccept(interactor, base.gameObject);
				}
				TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 1f, "From blood, brass. All that remains of my kingdom.", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
				yield return new WaitForSeconds(1f);
				onAccept = null;
			}
			else
			{
				Action<PlayerController, GameObject> onDecline = this.OnDecline;
				bool flag4 = onDecline != null;
				if (flag4)
				{
					onDecline(interactor, base.gameObject);
				}
				TextBoxManager.ClearTextBox(this.talkPoint);
				onDecline = null;
			}
			interactor.ClearInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(1f, 0.25f);		
			yield break;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0002DC1F File Offset: 0x0002BE1F
		public void OnEnteredRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0002DC4A File Offset: 0x0002BE4A
		public void OnExitRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0002DC6C File Offset: 0x0002BE6C
		public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
		{
			shouldBeFlipped = false;
			return string.Empty;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0002DC88 File Offset: 0x0002BE88
		public float GetDistanceToPoint(Vector2 point)
		{
			bool flag = base.sprite == null;
			bool flag2 = flag;
			bool flag3 = flag2;
			float result;
			if (flag3)
			{
				result = 100f;
			}
			else
			{
				Vector3 v = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
				result = Vector2.Distance(point, v) / 1.5f;
			}
			return result;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0002DCF4 File Offset: 0x0002BEF4
		public float GetOverrideMaxDistance()
		{
			return -1f;
		}

		// Token: 0x04000210 RID: 528
		public bool m_allowMeToIntroduceMyself = true;
		public bool selfdestrut;
		private static SpawnObjectPlayerItem HoleObject;
		private GameObject synergyobject;
		private GameObject gameObject1;
	}
}
