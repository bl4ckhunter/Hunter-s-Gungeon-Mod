using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x02000067 RID: 103
	public class MobiusDonationInteractible : SimpleInteractable, IPlayerInteractable
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x0002DAF4 File Offset: 0x0002BCF4
		private void Start()
		{   
			this.talkPoint = base.transform.Find("talkpoint");
			talkPoint.position += new Vector3(0.3f, 1, 0);
			this.m_isToggled = false;
			this.m_canUse = true;
			gameObject.AddAnimation("idle", "ClassLibrary1/Resources/BoomSprites/mobiuscoffer", 20, NPCBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			base.spriteAnimator.Play("idle");
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			MobiusDonationInteractible.HoleObject = PickupObjectDatabase.GetById(155).GetComponent<SpawnObjectPlayerItem>();
			MobiusDonationInteractible hellcomponent = gameObject.GetComponent<MobiusDonationInteractible>();
			Vector2 vector = new Vector2(base.specRigidbody.UnitCenter.x, base.specRigidbody.UnitCenter.y);
			Vector3 vector3 = new Vector3(vector.x, vector.y, 0f);
			tk2dBaseSprite sprite = base.sprite;
				Shader shader = Shader.Find("Brave/Effects/SimplicityDerivativeShader");
				Material material = new Material(Shader.Find("Brave/Effects/SimplicityDerivativeShader"));
				material.name = "HologramMaterial";
				Material material2 = material;
				material.SetTexture("_MainTex", sprite.renderer.material.GetTexture("_MainTex"));
				material2.SetTexture("_MainTex", sprite.renderer.sharedMaterial.GetTexture("_MainTex"));
				sprite.renderer.material.shader = shader;
				sprite.renderer.material = material;
				sprite.renderer.sharedMaterial = material2;
				sprite.usesOverrideMaterial = true;
			hellcomponent.synergyobject = MobiusDonationInteractible.HoleObject.objectToSpawn;
			BlackHoleDoer holer = synergyobject.GetComponent<BlackHoleDoer>();
			gameObject1 = UnityEngine.Object.Instantiate<GameObject>(holer.HellSynergyVFX, new Vector3(base.transform.position.x +1f, base.transform.position.y, base.transform.position.z + 5f), Quaternion.Euler(0f, 0f, 0f)); ;
			MeshRenderer component = gameObject1.GetComponent<MeshRenderer>();
			base.StartCoroutine(this.HoldPortalOpen(component, vector, gameObject1));
		}

		


		private IEnumerator HoldPortalOpen(MeshRenderer component, Vector2 vector, GameObject gameObject1)
		{
			float elapsed = new float();
			while (component != null)
			{
				elapsed += BraveTime.DeltaTime;
				float t = Mathf.Clamp01(elapsed / 0.25f);
				component.material.SetFloat("_UVDistCutoff", Mathf.Lerp(0f, 0.21f, t));
				yield return null;
			}
			yield break;
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
				{
					TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "The only thing you could slip inside are some casings, and you have none.", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
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
			TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, "The Möbius Coffer beckons, you do not dare touch it, but something small could be thrown inside.", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
			string acceptanceTextToUse = this.acceptText;
			string declineTextToUse = this.declineText;
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, "<Throw your casings at the Möbius Coffer>", "<Walk Away>");
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
				TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 3f, "You throw your casings best you can, some vanish into the ether, most make it, and as it appeared the Möbius Coffer is gone, leaving you to wonder if you'll ever see your hard earned money ever again.", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
				selfdestrut = true;
				yield return new WaitForSeconds(2f);
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
			if (selfdestrut == true)
			{ Destroy(base.gameObject);
				interactor.ForceBlank(30f, 0.5f, false, false, base.gameObject.transform.position, false, -1f);
				Destroy(gameObject1);
			}
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
