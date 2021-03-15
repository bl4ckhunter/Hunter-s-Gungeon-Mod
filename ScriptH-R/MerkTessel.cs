using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Dungeonator;
using Gungeon.Utilities;
using ItemAPI;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class Blad : PassiveItem
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584
		public static void Init()
		{
			string name = "Bloodsoaked Tassel";
			string resourceName = "ClassLibrary1/Resources/Tassel"; ;
			GameObject gameObject = new GameObject();
			Blad NeonBlade = gameObject.AddComponent<Blad>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Wet with blood";
			string longDesc = "Oh my!";
			NeonBlade.SetupItem(shortDesc, longDesc, "ror");
			NeonBlade.quality = PickupObject.ItemQuality.EXCLUDED;
			NeonBlade.CanBeDropped = false;
		}

		protected void UpdateBlinkShadow(PlayerController Owner, Vector2 delta, bool canWarpDirectly)
		{
			int? num = new int?(ReflectionHelpers.ReflectGetField<int>(typeof(PlayerController), "m_overrideFlatColorID", Owner));
			if (this.m_extantBlinkShadow == null)
			{
				GameObject go = new GameObject("blinkshadow");
				this.m_extantBlinkShadow = tk2dSprite.AddComponent(go, Owner.sprite.Collection, Owner.sprite.spriteId);
				this.m_extantBlinkShadow.transform.position = this.m_cachedBlinkPosition + (Owner.sprite.transform.position.XY() - Owner.specRigidbody.UnitCenter);
				this.m_extantBlinkShadow.gameObject.AddComponent<tk2dSpriteAnimator>().Library = Owner.spriteAnimator.Library;
				if (num != null)
				{
					this.m_extantBlinkShadow.renderer.material.SetColor(num.Value, (!canWarpDirectly) ? new Color(0.4f, 0f, 0f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f));
				}
				this.m_extantBlinkShadow.usesOverrideMaterial = true;
				this.m_extantBlinkShadow.FlipX = Owner.sprite.FlipX;
				this.m_extantBlinkShadow.FlipY = Owner.sprite.FlipY;
				Action<tk2dSprite> onBlinkShadowCreated = Owner.OnBlinkShadowCreated;
				if (onBlinkShadowCreated != null)
				{
					onBlinkShadowCreated(this.m_extantBlinkShadow);
				}
			}
			else
			{
				if (delta == Vector2.zero)
				{
					this.m_extantBlinkShadow.spriteAnimator.Stop();
					this.m_extantBlinkShadow.SetSprite(Owner.sprite.Collection, Owner.sprite.spriteId);
				}
				else
				{
					float? num2 = null;
					try
					{
						num2 = new float?(ReflectionHelpers.ReflectGetField<float>(typeof(PlayerController), "m_currentGunAngle", Owner));
					}
					catch (Exception)
					{
					}
					string text = string.Empty;
					if (num2 != null)
					{
						text = this.GetBaseAnimationName(Owner, delta, num2.Value, false, true);
					}
					if (!string.IsNullOrEmpty(text) && !this.m_extantBlinkShadow.spriteAnimator.IsPlaying(text))
					{
						this.m_extantBlinkShadow.spriteAnimator.Play(text);
					}
				}
				if (num != null)
				{
					this.m_extantBlinkShadow.renderer.material.SetColor(num.Value, (!canWarpDirectly) ? new Color(0.4f, 0f, 0f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f));
				}
				this.m_extantBlinkShadow.transform.position = this.m_cachedBlinkPosition + (Owner.sprite.transform.position.XY() - Owner.specRigidbody.UnitCenter);
			}
			this.m_extantBlinkShadow.FlipX = Owner.sprite.FlipX;
			this.m_extantBlinkShadow.FlipY = Owner.sprite.FlipY;
		}
		private bool RenderBodyHand(PlayerController Onwer)
		{
			return !Onwer.ForceHandless && Onwer.CurrentSecondaryGun == null && (Onwer.CurrentGun == null || Onwer.CurrentGun.Handedness != GunHandedness.TwoHanded);
		}
		protected virtual string GetBaseAnimationName(PlayerController Owner, Vector2 v, float gunAngle, bool invertThresholds = false, bool forceTwoHands = false)
		{
			bool flag = this.RenderBodyHand(Owner);
			string text = string.Empty;
			bool flag2 = Owner.CurrentGun != null;
			if (flag2 && Owner.CurrentGun.Handedness == GunHandedness.NoHanded)
			{
				forceTwoHands = true;
			}
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
			{
				flag2 = false;
			}
			float num = 155f;
			float num2 = 25f;
			if (invertThresholds)
			{
				num = -155f;
				num2 -= 50f;
			}
			float num3 = 120f;
			float num4 = 60f;
			float num5 = -60f;
			float num6 = -120f;
			bool flag3 = gunAngle <= num && gunAngle >= num2;
			if (invertThresholds)
			{
				flag3 = (gunAngle <= num || gunAngle >= num2);
			}
			if (Owner.IsGhost)
			{
				if (flag3)
				{
					if (gunAngle < num3 && gunAngle >= num4)
					{
						text = "ghost_idle_back";
					}
					else
					{
						float num7 = 105f;
						if (Mathf.Abs(gunAngle) > num7)
						{
							text = "ghost_idle_back_left";
						}
						else
						{
							text = "ghost_idle_back_right";
						}
					}
				}
				else if (gunAngle <= num5 && gunAngle >= num6)
				{
					text = "ghost_idle_front";
				}
				else
				{
					float num8 = 105f;
					if (Mathf.Abs(gunAngle) > num8)
					{
						text = "ghost_idle_left";
					}
					else
					{
						text = "ghost_idle_right";
					}
				}
			}
			else if (Owner.IsFlying)
			{
				if (flag3)
				{
					if (gunAngle < num3 && gunAngle >= num4)
					{
						text = "jetpack_up";
					}
					else
					{
						text = "jetpack_right_bw";
					}
				}
				else if (gunAngle <= num5 && gunAngle >= num6)
				{
					text = ((!flag) ? "jetpack_down" : "jetpack_down_hand");
				}
				else
				{
					text = ((!flag) ? "jetpack_right" : "jetpack_right_hand");
				}
			}
			else if (v == Vector2.zero || Owner.IsStationary)
			{
				if (Owner.IsPetting)
				{
					text = "pet";
				}
				else if (flag3)
				{
					if (gunAngle < num3 && gunAngle >= num4)
					{
						text = (((!forceTwoHands && flag2) || Owner.ForceHandless) ? ((!flag) ? "idle_backward" : "idle_backward_hand") : "idle_backward_twohands");
					}
					else
					{
						text = (((!forceTwoHands && flag2) || Owner.ForceHandless) ? "idle_bw" : "idle_bw_twohands");
					}
				}
				else if (gunAngle <= num5 && gunAngle >= num6)
				{
					text = (((!forceTwoHands && flag2) || Owner.ForceHandless) ? ((!flag) ? "idle_forward" : "idle_forward_hand") : "idle_forward_twohands");
				}
				else
				{
					text = (((!forceTwoHands && flag2) || Owner.ForceHandless) ? ((!flag) ? "idle" : "idle_hand") : "idle_twohands");
				}
			}
			else if (flag3)
			{
				string text2 = ((!forceTwoHands && flag2) || Owner.ForceHandless) ? "run_right_bw" : "run_right_bw_twohands";
				if (gunAngle < num3 && gunAngle >= num4)
				{
					text2 = (((!forceTwoHands && flag2) || Owner.ForceHandless) ? ((!flag) ? "run_up" : "run_up_hand") : "run_up_twohands");
				}
				text = text2;
			}
			else
			{
				string text3 = "run_right";
				if (gunAngle <= num5 && gunAngle >= num6)
				{
					text3 = "run_down";
				}
				if ((forceTwoHands || !flag2) && !Owner.ForceHandless)
				{
					text3 += "_twohands";
				}
				else if (flag)
				{
					text3 += "_hand";
				}
				text = text3;
			}
			if (Owner.UseArmorlessAnim && !Owner.IsGhost)
			{
				text += "_armorless";
			}
			return text;
		}

		protected override void Update()
		{
			if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
			{
				return;
			}
			if (base.Owner && this.m_pickedUp)
			{
				this.HandleBlink(base.Owner);
			}
			base.Update();
		}
		protected void ClearBlinkShadow()
		{
			if (this.m_extantBlinkShadow)
			{
				UnityEngine.Object.Destroy(this.m_extantBlinkShadow.gameObject);
				this.m_extantBlinkShadow = null;
			}
		}
		protected Vector2 AdjustInputVector(Vector2 rawInput, float cardinalMagnetAngle, float ordinalMagnetAngle)
		{
			float num = BraveMathCollege.ClampAngle360(BraveMathCollege.Atan2Degrees(rawInput));
			float num2 = num % 90f;
			float num3 = (num + 45f) % 90f;
			float num4 = 0f;
			if (cardinalMagnetAngle > 0f)
			{
				if (num2 < cardinalMagnetAngle)
				{
					num4 = -num2;
				}
				else if (num2 > 90f - cardinalMagnetAngle)
				{
					num4 = 90f - num2;
				}
			}
			if (ordinalMagnetAngle > 0f)
			{
				if (num3 < ordinalMagnetAngle)
				{
					num4 = -num3;
				}
				else if (num3 > 90f - ordinalMagnetAngle)
				{
					num4 = 90f - num3;
				}
			}
			num += num4;
			return (Quaternion.Euler(0f, 0f, num) * Vector3.right).XY() * rawInput.magnitude;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000067E0 File Offset: 0x000049E0
		public void HandleBlink(PlayerController Owner)
		{
			if (GameManager.Instance.Dungeon && GameManager.Instance.Dungeon.IsEndTimes)
			{
				this.ClearBlinkShadow();
				return;
			}
			if (Owner.WasPausedThisFrame)
			{
				this.ClearBlinkShadow();
				return;
			}
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(Owner.PlayerIDX);
			GungeonActions gungeonActions = ReflectionHelpers.ReflectGetField<GungeonActions>(typeof(PlayerController), "m_activeActions", Owner);
			this.AdjustInputVector(gungeonActions.Move.Vector, BraveInput.MagnetAngles.movementCardinal, BraveInput.MagnetAngles.movementOrdinal);
			bool flag = false;
			bool flag2 = false;
			if (instanceForPlayer.GetButtonDown(GungeonActions.GungeonActionType.DodgeRoll))
			{
				flag2 = true;
			}
			if (instanceForPlayer.ActiveActions.DodgeRollAction.IsPressed)
			{
				this.m_timeHeldBlinkButton += BraveTime.DeltaTime;
				if (this.m_timeHeldBlinkButton < 0.1f)
				{
					this.m_cachedBlinkPosition = Owner.specRigidbody.UnitCenter;
				}
				else
				{
					Vector2 cachedBlinkPosition = this.m_cachedBlinkPosition;
					if (BraveInput.GetInstanceForPlayer(Owner.PlayerIDX).IsKeyboardAndMouse(false))
					{
						this.m_cachedBlinkPosition = Owner.unadjustedAimPoint.XY() - (Owner.CenterPosition - Owner.specRigidbody.UnitCenter);
					}
					else if (gungeonActions != null)
					{
						this.m_cachedBlinkPosition += gungeonActions.Aim.Vector.normalized * BraveTime.DeltaTime * 15f;
					}
					this.m_cachedBlinkPosition = BraveMathCollege.ClampToBounds(this.m_cachedBlinkPosition, GameManager.Instance.MainCameraController.MinVisiblePoint, GameManager.Instance.MainCameraController.MaxVisiblePoint);
					this.UpdateBlinkShadow(Owner, this.m_cachedBlinkPosition - cachedBlinkPosition, this.CanBlinkToPoint(Owner, this.m_cachedBlinkPosition, Owner.transform.position.XY() - Owner.specRigidbody.UnitCenter));
				}
			}
			else if (instanceForPlayer.ActiveActions.DodgeRollAction.WasReleased || flag2)
			{
				if (this.m_timeHeldBlinkButton >= 0.3f)
				{
					flag = true;
				}
			}
			else
			{
				this.m_timeHeldBlinkButton = 0f;
			}
			if (flag)
			{
				Owner.healthHaver.TriggerInvulnerabilityPeriod(0.001f);
				Owner.DidUnstealthyAction();
				this.BlinkToPoint(Owner, this.m_cachedBlinkPosition);
				this.m_timeHeldBlinkButton = 0f;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00006A24 File Offset: 0x00004C24
		public void BlinkToPoint(PlayerController Owner, Vector2 targetPoint)
		{
			this.m_cachedBlinkPosition = targetPoint;
			this.lockedDodgeRollDirection = (this.m_cachedBlinkPosition - Owner.specRigidbody.UnitCenter).normalized;
			Vector2 centerOffset = Owner.transform.position.XY() - Owner.specRigidbody.UnitCenter;
			bool flag = this.CanBlinkToPoint(Owner, this.m_cachedBlinkPosition, centerOffset);
			this.ClearBlinkShadow();
			if (flag)
			{
				this.m_CurrentlyBlinking = true;
				base.StartCoroutine(this.HandleBlinkTeleport(Owner, this.m_cachedBlinkPosition, this.lockedDodgeRollDirection));
				return;
			}
			Vector2 a = Owner.specRigidbody.UnitCenter - this.m_cachedBlinkPosition;
			float num = a.magnitude;
			Vector2? vector = null;
			float num2 = 0f;
			a = a.normalized;
			while (num > 0f)
			{
				num2 += 1f;
				num -= 1f;
				Vector2 vector2 = this.m_cachedBlinkPosition + a * num2;
				if (this.CanBlinkToPoint(Owner, vector2 + new Vector2(1f, 0f), centerOffset))
				{
					vector = new Vector2?(vector2);
					break;
				}
			}
			if (vector != null && Vector2.Dot((vector.Value - Owner.specRigidbody.UnitCenter).normalized, this.lockedDodgeRollDirection) > 0f)
			{
				this.m_cachedBlinkPosition = vector.Value;
				this.m_CurrentlyBlinking = true;
				base.StartCoroutine(this.HandleBlinkTeleport(Owner, this.m_cachedBlinkPosition, this.lockedDodgeRollDirection));
			}
		}

		protected bool CanBlinkToPoint(PlayerController Owner, Vector2 point, Vector2 centerOffset)
		{
			bool flag = Owner.IsValidPlayerPosition(point + centerOffset);
			if (flag && Owner.CurrentRoom != null)
			{
				CellData cellData = GameManager.Instance.Dungeon.data[point.ToIntVector2(VectorConversions.Floor)];
				if (cellData == null)
				{
					return false;
				}
				RoomHandler nearestRoom = cellData.nearestRoom;
				if (cellData.type != CellType.FLOOR)
				{
					flag = false;
				}
				if (Owner.CurrentRoom.IsSealed && nearestRoom != Owner.CurrentRoom)
				{
					flag = false;
				}
				if (Owner.CurrentRoom.IsSealed && cellData.isExitCell)
				{
					flag = false;
				}
				if (nearestRoom.visibility == RoomHandler.VisibilityStatus.OBSCURED || nearestRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
				{
					flag = false;
				}
			}
			if (Owner.CurrentRoom == null)
			{
				flag = false;
			}
			return !(Owner.IsDodgeRolling | Owner.IsFalling | Owner.IsCurrentlyCoopReviving | Owner.IsInMinecart | Owner.IsInputOverridden) && flag;
		}
		private void OnBlinkStarted(PlayerController obj, Vector2 dirVec)
		{
			if (GameManager.Instance.Dungeon && GameManager.Instance.Dungeon.IsEndTimes)
			{
				return;
			}
			obj.StartCoroutine(this.HandleAfterImageStop1(obj));
		}
		private IEnumerator HandleAfterImageStop1(PlayerController player)
		{
			yield break;
		}
		private IEnumerator HandleBlinkTeleport(PlayerController Owner, Vector2 targetPoint, Vector2 targetDirection)
		{
			targetPoint -= new Vector2(0.75f, 0.125f);
			this.OnBlinkStarted(Owner, targetDirection);
			List<AIActor> list = ReflectionHelpers.ReflectGetField<List<AIActor>>(typeof(PlayerController), "m_rollDamagedEnemies", Owner);
			if (list != null)
			{
				list.Clear();
				typeof(PlayerController).GetField("m_rollDamagedEnemies", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Owner, list);
			}
			if (Owner.knockbackDoer)
			{
				Owner.knockbackDoer.ClearContinuousKnockbacks();
			}
			Owner.IsEthereal = true;
			Owner.IsVisible = false;
			float RecoverySpeed = GameManager.Instance.MainCameraController.OverrideRecoverySpeed;
			bool IsLerping = GameManager.Instance.MainCameraController.IsLerping;
			yield return new WaitForSeconds(0.1f);
			GameManager.Instance.MainCameraController.OverrideRecoverySpeed = 80f;
			GameManager.Instance.MainCameraController.IsLerping = true;
			if (Owner.IsPrimaryPlayer)
			{
				GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = true;
				GameManager.Instance.MainCameraController.OverridePlayerOnePosition = targetPoint;
				yield return new WaitForSeconds(0.12f);
				Owner.specRigidbody.Velocity = Vector2.zero;
				Owner.specRigidbody.Position = new Position(targetPoint);
				GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = false;
			}
			else
			{
				GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = true;
				GameManager.Instance.MainCameraController.OverridePlayerTwoPosition = targetPoint;
				yield return new WaitForSeconds(0.12f);
				Owner.specRigidbody.Velocity = Vector2.zero;
				Owner.specRigidbody.Position = new Position(targetPoint);
				GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = false;
			}
			GameManager.Instance.MainCameraController.OverrideRecoverySpeed = RecoverySpeed;
			GameManager.Instance.MainCameraController.IsLerping = IsLerping;
			Owner.IsEthereal = false;
			Owner.IsVisible = true;
			this.m_CurrentlyBlinking = false;
			if (Owner.CurrentFireMeterValue <= 0f)
			{
				yield break;
			}
			Owner.CurrentFireMeterValue = Mathf.Max(0f, Owner.CurrentFireMeterValue -= 0.5f);
			if (Owner.CurrentFireMeterValue == 0f)
			{
				Owner.IsOnFire = false;
				yield break;
			}
			yield break;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000E510 File Offset: 0x0000C710
		// Token: 0x060001D3 RID: 467 RVA: 0x0000E534 File Offset: 0x0000C734

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E5CE File Offset: 0x0000C7CE

		// Token: 0x060001D5 RID: 469 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		public override void Pickup(PlayerController player)
		{  
			base.Pickup(player);

			Blad.m_BlinkPassive = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>();
			Blad component = gameObject.GetComponent<Blad>();
			component.AdditionalInvulnerabilityFrames = Blad.m_BlinkPassive.AdditionalInvulnerabilityFrames;
			component.ScarfPrefab = Blad.m_BlinkPassive.ScarfPrefab;
			component.BlinkpoofVfx = Blad.m_BlinkPassive.BlinkpoofVfx;
			if (this.ScarfPrefab)
			{
				this.m_scarf = UnityEngine.Object.Instantiate<GameObject>(this.ScarfPrefab.gameObject).GetComponent<ScarfAttachmentDoer>();
				this.m_scarf.Initialize(player);
			}
			if (player.IsDodgeRolling)
			{
				player.ForceStopDodgeRoll();
			}

			if (this.m_pickedUp)
			{
				return;
			}
			this.m_CurrentlyBlinking = false;
			this.m_cachedBlinkPosition = player.transform.position;
			if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
			{
				PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
			}
			if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
			{
				PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
			}
			else
			{
				PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
			}
			this.afterimage = player.gameObject.AddComponent<AfterImageTrailController>();
			this.afterimage.spawnShadows = false;
			this.afterimage.shadowTimeDelay = 0.05f;
			this.afterimage.shadowLifetime = 0.3f;
			this.afterimage.minTranslation = 0.05f;
			this.afterimage.dashColor = Color.cyan;
			this.afterimage.maxEmission = 0f;
			this.afterimage.minEmission = 0f;
			this.afterimage.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");
			player.OnRollStarted += this.OnRollStarted;
			player.OnBlinkShadowCreated = (Action<tk2dSprite>)Delegate.Combine(player.OnBlinkShadowCreated, new Action<tk2dSprite>(this.OnBlinkCloneCreated));


		}
		public void OnBlinkCloneCreated(tk2dSprite cloneSprite)
		{
			SpawnManager.SpawnVFX(this.BlinkpoofVfx, cloneSprite.WorldCenter, Quaternion.identity);
		}

		// Token: 0x0600704C RID: 28748 RVA: 0x002B9980 File Offset: 0x002B7B80
		private void OnRollStarted(PlayerController obj, Vector2 dirVec)
		{
			if (GameManager.Instance.Dungeon && GameManager.Instance.Dungeon.IsEndTimes)
			{
				return;
			}
			obj.StartCoroutine(this.HandleAfterImageStop1(obj));
		}
		// Token: 0x060001D6 RID: 470 RVA: 0x0000E660 File Offset: 0x0000C860
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			this.ClearBlinkShadow();
			this.m_CurrentlyBlinking = false;
			this.m_cachedBlinkPosition = player.transform.position;
			if (PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
			{
				PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
				if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
				{
					PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
				}
			}
			if (this.m_scarf)
			{
				UnityEngine.Object.Destroy(this.m_scarf.gameObject);
				this.m_scarf = null;
			}
			if (this.afterimage)
			{
				UnityEngine.Object.Destroy(this.afterimage);
			}
			this.afterimage = null;
			player.OnRollStarted -= this.OnRollStarted;
			player.OnBlinkShadowCreated = (Action<tk2dSprite>)Delegate.Remove(player.OnBlinkShadowCreated, new Action<tk2dSprite>(this.OnBlinkCloneCreated));
			debrisObject.GetComponent<Blad>().m_pickedUpThisRun = true;
			return debrisObject;

		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000763C File Offset: 0x0000583C
		protected override void OnDestroy()
		{
			this.ClearBlinkShadow();
			this.m_CurrentlyBlinking = false;
			if (this.m_scarf)
			{
				UnityEngine.Object.Destroy(this.m_scarf.gameObject);
				this.m_scarf = null;
			}
			if (this.m_pickedUp && this.m_owner && PassiveItem.ActiveFlagItems != null && PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
			{
				PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
				if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
				{
					PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
				}
			}
			if (this.m_owner != null)
			{
				PlayerController owner = this.m_owner;
				if (this.afterimage)
				{
					UnityEngine.Object.Destroy(this.afterimage);
				}
				this.afterimage = null;
			}
			base.OnDestroy();
		}


		private Shader m_glintShader;

		// Token: 0x040000BC RID: 188
		private float Boost = 0f;

		// Token: 0x040000BD RID: 189
		private float TrueColorBoost;

		// Token: 0x040000BE RID: 190
		private float ColorBoost;

		// Token: 0x040000BF RID: 191
		private PlayerController m_player;


		private bool flag;
		private bool used;
		private bool active;
		private ScarfAttachmentDoer m_scarf;
		private int AdditionalInvulnerabilityFrames;
		public ScarfAttachmentDoer ScarfPrefab;
		private bool flag1;
		private AfterImageTrailController downwellAfterimage;
		// Token: 0x0400009D RID: 157
		protected PlayerController m_buffedTarget;

		// Token: 0x0400009E RID: 158
		protected StatModifier m_temporaryModifier;
		private StatModifier m_temporaryModifier1;
		private StatModifier m_temporaryModifier2;
		private StatModifier m_temporaryModifier3;
		private StatModifier m_temporaryModifier4;
		private Vector2 m_cachedBlinkPosition;
		private Vector2 lockedDodgeRollDirection;
		private bool m_CurrentlyBlinking;

		public object CheckBoost;
		public object LastCheckBoost;
		public GameObject BlinkpoofVfx;

		private AfterImageTrailController afterimage;
		private float m_timeHeldBlinkButton;
		private tk2dSprite m_extantBlinkShadow;
		private static BlinkPassiveItem m_BlinkPassive;
		private float DodgeRollTimeMultiplier;
		private float DodgeRollDistanceMultiplier;
	}

}

