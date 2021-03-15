using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using MultiplayerBasicExample;
using Pathfinding;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	public class Tank1 : PlayerItem
	{
		private static CardboardBoxItem tankbox;
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "War Box";
			string resourceName = "ClassLibrary1/Resources/MULTANK";
			GameObject gameObject = new GameObject();
			Tank1 Nchallange = gameObject.AddComponent<Tank1>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Cardboard Tank";
			string longDesc = "Someone asked a construction robot from a far off planet if it could make weapon to surpass metal gear as a joke, and boy did it try, it's made out of paper so don't expect it to block shots but i think the firepower is up there. \n\n\n (Disables teleporting while active, reactivate to turn off prematurely, lasts 20s or until damage taken, secondary weapons fire on damaging an enemy)";
			Nchallange.SetupItem(shortDesc, longDesc, "ror");
			Nchallange.quality = PickupObject.ItemQuality.C;
			Nchallange.SetCooldownType(ItemBuilder.CooldownType.Damage, 600f);
			Nchallange.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Nchallange.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Nchallange.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
		}

		private void DoubleBeam(PlayerController player, float gun1)
		{

			if (!oncoodlown1)
			{  
				this.oncoodlown1 = true;
				GameManager.Instance.StartCoroutine(this.Missilecooldown());
				Projectile projectile = ((Gun)ETGMod.Databases.Items[563]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_hovers.gunposition, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : 90f), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag = component != null;
				if (flag)
				{
					component.Owner = base.LastOwner;
					HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
					component.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
					homingModifier.HomingRadius = 100f;
					homingModifier.AngularVelocity = 500f;
					component.Shooter = base.LastOwner.specRigidbody;
					component.baseData.speed = 20f;
					component.SetOwnerSafe(base.LastOwner, "Player");
				}
				Projectile component1 = ((Gun)ETGMod.Databases.Items[518]).DefaultModule.projectiles[0];
				GameManager.Instance.StartCoroutine(this.m_hovers2.HandleFireShortBeam(component1, base.LastOwner, 3f));
			}

		}

		private IEnumerator ActiveTime()
		{
			yield return new WaitForSeconds(20f);
			if(this.IsCurrentlyActive == true)
			{this.BreakStealth(base.LastOwner);}
			yield break;
		}

		private IEnumerator Missilecooldown()
		{
			yield return new WaitForSeconds(1f);
			this.oncoodlown1 = false;
			yield break;
		}



		// Token: 0x0600708D RID: 28813 RVA: 0x002BB74D File Offset: 0x002B994D
		protected override void OnPreDrop(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			this.BreakStealth(user);
		}
		base.OnPreDrop(user);
	}

	// Token: 0x0600708E RID: 28814 RVA: 0x002BB768 File Offset: 0x002B9968
	protected override void DoEffect(PlayerController user)
	{
			this.m_player = user;
		if (this.m_player && this.m_player.CurrentGun)
		{
			this.m_player.CurrentGun.CeaseAttack(false, null);
		}
			GameManager.Instance.StartCoroutine(this.ActiveTime());
			this.oncoodlown = true; 
			base.IsCurrentlyActive = true;
		this.m_player.healthHaver.OnDamaged += this.OnDamaged;
				Tank1.tankbox = PickupObjectDatabase.GetById(216).GetComponent<CardboardBoxItem>();
				Tank1 component = gameObject.GetComponent<Tank1>();
				component.prefabToAttachToPlayer = Tank1.tankbox.prefabToAttachToPlayer;
				this.instanceBox = user.RegisterAttachedObject(this.prefabToAttachToPlayer, string.Empty, 0f);
			this.m_glintShader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
			this.ProcessGunShader(this.instanceBox);
			this.instanceBoxSprite = this.instanceBox.GetComponent<tk2dSprite>();
				this.instanceBoxSprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.LowerLeft);
				this.instanceBoxSprite.spriteAnimator.Play("cardboard_on");
				user.StartCoroutine(this.HandlePutOn(user, this.instanceBoxSprite));
			user.OnDealtDamage += this.DoubleBeam;
			this.AddGuns();
			}

		private void AddGuns()
		{
			this.gameObjectgun = UnityEngine.Object.Instantiate<GameObject>(ResourceCache.Acquire("Global Prefabs/HoveringGun") as GameObject, base.LastOwner.CenterPosition.ToVector3ZisY(-5f), Quaternion.identity);
			gameObjectgun.transform.parent = base.LastOwner.transform;
			this.m_hovers = gameObjectgun.AddComponent<Tank1.FakeDualWield1>();
			m_hovers.Aim = Tank1.FakeDualWield1.AimType.PLAYER_AIM;
			m_hovers.Trigger = Tank1.FakeDualWield1.FireType.ON_FIRED_GUN;
			m_hovers.Position = Tank1.FakeDualWield1.HoverPosition.OVERHEAD;
			Gun gun;
			gun = (PickupObjectDatabase.GetById(129) as Gun);
			m_hovers.Initialize(gun, base.LastOwner);
			this.gameObjectgun2 = UnityEngine.Object.Instantiate<GameObject>(ResourceCache.Acquire("Global Prefabs/HoveringGun") as GameObject, base.LastOwner.CenterPosition.ToVector3ZisY(-5f), Quaternion.identity);
			gameObjectgun2.transform.parent = base.LastOwner.transform;
			this.m_hovers2 = gameObjectgun2.AddComponent<Commando.FakeDualWield>();
			m_hovers2.Aim = Commando.FakeDualWield.AimType.PLAYER_AIM;
			m_hovers2.Trigger = Commando.FakeDualWield.FireType.ON_FIRED_GUN;
			m_hovers2.Position = Commando.FakeDualWield.HoverPosition.OVERHEAD;
			Gun gun2;
			gun2 = (PickupObjectDatabase.GetById(121) as Gun);
			m_hovers2.Initialize(gun2, base.LastOwner);
		}

			private void ProcessGunShader(GameObject g)
		{
			MeshRenderer component = g.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].shader == this.m_glintShader)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(this.m_glintShader);
			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;
			return;
		}

		// Token: 0x0600708F RID: 28815 RVA: 0x002BB89B File Offset: 0x002B9A9B

		// Token: 0x06007090 RID: 28816 RVA: 0x002BB8D8 File Offset: 0x002B9AD8


		// Token: 0x06007091 RID: 28817 RVA: 0x002BBB40 File Offset: 0x002B9D40
		private IEnumerator HandlePutOn(PlayerController user, tk2dBaseSprite instanceBoxSprite)
	{
		yield return new WaitForSeconds(0.2f);
		if (this.IsCurrentlyActive)
		{
			user.IsVisible = false;
			instanceBoxSprite.renderer.enabled = true;
		}
		yield break;
	}

	// Token: 0x06007092 RID: 28818 RVA: 0x002BBB69 File Offset: 0x002B9D69
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		this.BreakStealth(this.m_player);
	}

	// Token: 0x06007093 RID: 28819 RVA: 0x002BBB78 File Offset: 0x002B9D78
	private void BreakStealth(PlayerController obj)
	{
		this.m_player.healthHaver.OnDamaged -= this.OnDamaged;
		base.IsCurrentlyActive = false;
		obj.OnDealtDamage -= this.DoubleBeam;
		obj.IsVisible = true;
		obj.SetIsStealthed(false, "box");
		obj.SetCapableOfStealing(false, "CardboardBoxItem", null);
		obj.DeregisterAttachedObject(this.instanceBox, false);
		this.instanceBoxSprite.spriteAnimator.PlayAndDestroyObject("cardboard_off", null);
		this.instanceBoxSprite = null;
			if (this.m_hovers.gameObject != null)
			{ UnityEngine.Object.Destroy(this.m_hovers.gameObject); }
			if (this.m_hovers2.gameObject != null)
			{ UnityEngine.Object.Destroy(this.m_hovers2.gameObject); }
		}

	// Token: 0x06007094 RID: 28820 RVA: 0x002BBC28 File Offset: 0x002B9E28
	protected override void DoActiveEffect(PlayerController user)
	{
		this.BreakStealth(user);
	}

	// Token: 0x06007095 RID: 28821 RVA: 0x002BBC34 File Offset: 0x002B9E34
	public void LateUpdate()
	{
		if (base.IsCurrentlyActive)
		{
			if (this.instanceBoxSprite.FlipX != this.m_player.sprite.FlipX)
			{
				this.instanceBoxSprite.FlipX = this.m_player.sprite.FlipX;
			}
			this.instanceBoxSprite.PlaceAtPositionByAnchor(this.m_player.SpriteBottomCenter + (float)((!this.m_player.sprite.FlipX) ? 1 : -1) * new Vector3(-0.5f, 0f, 0f), tk2dBaseSprite.Anchor.LowerCenter);
			if (!this.instanceBoxSprite.spriteAnimator.IsPlaying("cardboard_on"))
			{
				if (this.m_player.specRigidbody.Velocity == Vector2.zero)
				{
					if (this.m_player.spriteAnimator.CurrentClip.name.Contains("backward") || this.m_player.spriteAnimator.CurrentClip.name.Contains("_bw"))
					{
						this.instanceBoxSprite.spriteAnimator.Play("idle_back");
					}
					else
					{
						this.instanceBoxSprite.spriteAnimator.Play("idle");
					}
				}
				else if (this.m_player.spriteAnimator.CurrentClip.name.Contains("run_up") || this.m_player.spriteAnimator.CurrentClip.name.Contains("_bw"))
				{
					this.instanceBoxSprite.spriteAnimator.Play("move_right_backwards");
				}
				else
				{
					this.instanceBoxSprite.spriteAnimator.Play("move_right_forward");
				}
			}
		}
	}

	// Token: 0x06007096 RID: 28822 RVA: 0x002B4278 File Offset: 0x002B2478
	public override void OnItemSwitched(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			this.DoActiveEffect(user);
		}
	}

	// Token: 0x06007097 RID: 28823 RVA: 0x002B486D File Offset: 0x002B2A6D
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007008 RID: 28680
	public GameObject prefabToAttachToPlayer;

	// Token: 0x04007009 RID: 28681
	public float SneakAttackDamageMultiplier;

	// Token: 0x0400700A RID: 28682
	private GameObject instanceBox;
		private Shader m_glintShader;

		// Token: 0x0400700B RID: 28683
		private tk2dSprite instanceBoxSprite;

	// Token: 0x0400700C RID: 28684
	private PlayerController m_player;
		private object m_overrideflatcolorid;
		private GameObject gameObjectgun;
		private Tank1.FakeDualWield1 m_hovers;
		private Commando.FakeDualWield m_hovers2;
		private GameObject gameObjectgun2;
		private bool oncoodlown1;
		private bool oncoodlown;

		private class FakeDualWield1 :  BraveBehaviour, IPlayerOrbital
			{
				// Token: 0x0600887E RID: 34942 RVA: 0x003769F2 File Offset: 0x00374BF2
				public FakeDualWield1()
				{
					this.AimRotationAngularSpeed = 3600f;
					this.ShootDuration = 1f;
					this.CooldownTime = 0f;
					this.ChanceToConsumeTargetGunAmmo = 1f;
					this.m_glintShader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
				}

				// Token: 0x0600887F RID: 34943 RVA: 0x00376A28 File Offset: 0x00374C28
				public void Initialize(Gun targetGun, PlayerController owner)
				{
					this.m_targetGun = targetGun;
					this.m_owner = owner;
					this.m_parentTransform = new GameObject("hover rotator").transform;
					this.m_parentTransform.parent = base.transform.parent;
					base.transform.parent = this.m_parentTransform;
					base.sprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.MiddleCenter);
					base.sprite.SetSprite(targetGun.sprite.Collection, targetGun.sprite.spriteId);
					SpriteOutlineManager.AddOutlineToSprite(base.sprite, new Color(0f, 0f, 0f));
					this.m_shootPointTransform = new GameObject("shoot point").transform;
					this.Oldgun = targetGun.barrelOffset.localPosition;
					this.m_shootPointTransform.parent = base.transform;
					this.m_shootPointTransform.localPosition = targetGun.barrelOffset.localPosition;
					if (this.Position == FakeDualWield1.HoverPosition.CIRCULATE)
					{
						this.SetOrbitalTier(PlayerOrbital.CalculateTargetTier(this.m_owner, this));
						this.SetOrbitalTierIndex(PlayerOrbital.GetNumberOfOrbitalsInTier(this.m_owner, this.GetOrbitalTier()));
						this.m_owner.orbitals.Add(this);
						this.m_ownerCenterAverage = this.m_owner.CenterPosition;
					}
					if (this.Trigger == FakeDualWield1.FireType.ON_DODGED_BULLET)
					{
						this.m_owner.OnDodgedProjectile += this.HandleDodgedProjectileFire;
					}
					if (this.Trigger == FakeDualWield1.FireType.ON_FIRED_GUN)
					{
						this.m_owner.PostProcessProjectile += this.HandleFiredGun;
					}
					if (this.Aim == FakeDualWield1.AimType.NEAREST_ENEMY)
					{
						this.m_fireCooldown = 0.25f;
					}
					this.UpdatePosition();
					LootEngine.DoDefaultSynergyPoof(base.sprite.WorldCenter, false);
					this.m_initialized = true;
				}

				// Token: 0x06008880 RID: 34944 RVA: 0x00376BCE File Offset: 0x00374DCE
				private void HandleFiredGun(Projectile arg1, float arg2)
				{
					if (this.m_fireCooldown <= 0f)
					{
						return;
					}
				}

				// Token: 0x06008881 RID: 34945 RVA: 0x00376BE6 File Offset: 0x00374DE6
				private void HandleDodgedProjectileFire(Projectile sourceProjectile)
				{
					if (this.m_fireCooldown <= 0f && sourceProjectile.collidesWithPlayer)
					{
						this.Fire();
					}
				}

				// Token: 0x06008882 RID: 34946 RVA: 0x00376C09 File Offset: 0x00374E09
				public void LateUpdate()
				{
					if (!this.m_initialized)
					{
						return;
					}
					if (Dungeon.IsGenerating || GameManager.Instance.IsLoadingLevel)
					{
						return;
					}
					this.UpdatePosition();
					this.UpdateFiring();
				}

				// Token: 0x06008883 RID: 34947 RVA: 0x00376C40 File Offset: 0x00374E40
				private void AimAt(Vector2 point, bool instant = false)
				{
					Vector2 v = point - base.sprite.WorldCenter;
					float currentAimTarget = BraveMathCollege.Atan2Degrees(v);
					this.m_currentAimTarget = currentAimTarget;
					if (instant)
					{
						this.m_parentTransform.localRotation = Quaternion.Euler(0f, 0f, this.m_currentAimTarget);
					}
					this.designatedtarget1 = currentAimTarget;
				}

				// Token: 0x06008884 RID: 34948 RVA: 0x00376C94 File Offset: 0x00374E94
				private void UpdatePosition()
				{

					this.AimAt(new Vector2(0f, 0f));
					this.m_parentTransform.localRotation = Quaternion.Euler(0f, 0f, 90f);
					bool flag2 = this.m_parentTransform.localRotation.eulerAngles.z > 90f && this.m_parentTransform.localRotation.eulerAngles.z < 270f;
					if (flag2 && !base.sprite.FlipY)
					{
						base.transform.localPosition += new Vector3(0f, base.sprite.GetUntrimmedBounds().extents.y, 0f);
						this.m_shootPointTransform.localPosition = this.m_shootPointTransform.localPosition.WithY(-this.m_shootPointTransform.localPosition.y);
						base.sprite.FlipY = true;
					}
					else if (!flag2 && base.sprite.FlipY)
					{
						base.sprite.FlipY = false;
						base.transform.localPosition -= new Vector3(0f, base.sprite.GetUntrimmedBounds().extents.y, 0f);
						this.m_shootPointTransform.localPosition = this.m_shootPointTransform.localPosition.WithY(-this.m_shootPointTransform.localPosition.y);
					}
					FakeDualWield1.HoverPosition position = this.Position;
					
					if (this.m_owner.AimCenter.y - this.m_owner.specRigidbody.UnitCenter.y < 0f)
					{
						base.sprite.HeightOffGround = -2f;
						base.sprite.UpdateZDepth();
					}
					if (this.m_owner.AimCenter.y - this.m_owner.specRigidbody.UnitCenter.y >= 0f)
					{
						base.sprite.HeightOffGround = -2f;
						base.sprite.UpdateZDepth();
					}
				    if (this.m_owner.CurrentGun.sprite.WorldCenter.x - this.m_owner.specRigidbody.UnitCenter.x < 0f)
				    {
					this.m_parentTransform.position = (this.m_owner.specRigidbody.UnitCenterRight + new Vector2(0.55f, 0.3f)).ToVector3ZisY(0f);
				     }
				if (this.m_owner.CurrentGun.sprite.WorldCenter.x - this.m_owner.specRigidbody.UnitCenter.x >= 0f)
				{
					this.m_parentTransform.position = (this.m_owner.specRigidbody.UnitCenterLeft + new Vector2(0.1f, 0.3f)).ToVector3ZisY(0f);
				}
				this.gunposition = this.m_parentTransform.position;
				}

				// Token: 0x06008885 RID: 34949 RVA: 0x00376FB0 File Offset: 0x003751B0
				private void HandleOrbitalMotion()
				{
					Vector2 centerPosition = this.m_owner.CenterPosition;
					if (Vector2.Distance(centerPosition, this.m_parentTransform.position.XY()) > 20f)
					{
						this.m_parentTransform.position = centerPosition.ToVector3ZUp(0f);
						if (base.specRigidbody)
						{
							base.specRigidbody.Reinitialize();
						}
					}
					Vector2 vector = centerPosition - this.m_ownerCenterAverage;
					float num = Mathf.Lerp(0.1f, 15f, vector.magnitude / 4f);
					float d = Mathf.Min(num * BraveTime.DeltaTime, vector.magnitude);
					float num2 = 360f / (float)PlayerOrbital.GetNumberOfOrbitalsInTier(this.m_owner, this.GetOrbitalTier()) * (float)this.GetOrbitalTierIndex() + BraveTime.ScaledTimeSinceStartup * this.GetOrbitalRotationalSpeed();
					Vector2 vector2 = this.m_ownerCenterAverage + (centerPosition - this.m_ownerCenterAverage).normalized * d;
					Vector2 vector3 = vector2 + (Quaternion.Euler(0f, 0f, num2) * Vector3.right * this.GetOrbitalRadius()).XY();
					this.m_ownerCenterAverage = vector2;
					vector3 = vector3.Quantize(0.0625f);
					Vector2 velocity = (vector3 - this.m_parentTransform.position.XY()) / BraveTime.DeltaTime;
					if (base.specRigidbody)
					{
						base.specRigidbody.Velocity = velocity;
					}
					else
					{
						this.m_parentTransform.position = vector3.ToVector3ZisY(0f);
						base.sprite.HeightOffGround = 0.5f;
						base.sprite.UpdateZDepth();
					}
					this.m_orbitalAngle = num2 % 360f;
				}

				// Token: 0x06008886 RID: 34950 RVA: 0x00377180 File Offset: 0x00375380
				private void UpdateFiring()
				{
					if (this.m_fireCooldown <= 0f)
					{
						FakeDualWield1.FireType trigger = this.Trigger;
						if (trigger != FakeDualWield1.FireType.ON_RELOAD)
						{
							if (trigger != FakeDualWield1.FireType.ON_COOLDOWN)
							{
								if (trigger != FakeDualWield1.FireType.ON_DODGED_BULLET)
								{
								}
							}
							else if (this.Aim != FakeDualWield1.AimType.NEAREST_ENEMY || this.m_hasEnemyTarget)
							{
								this.Fire();
							}
						}
						else if (this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.IsReloading && (!this.OnlyOnEmptyReload || this.m_owner.CurrentGun.ClipShotsRemaining <= 0))
						{
							this.Fire();
						}
					}
					else
					{
						this.m_fireCooldown = (this.m_fireCooldown -= BraveTime.DeltaTime);
					}
				}

				// Token: 0x17001457 RID: 5207
				// (get) Token: 0x06008887 RID: 34951 RVA: 0x00377269 File Offset: 0x00375469
				public Vector2 ShootPoint
				{
					get
					{
						return this.m_shootPointTransform.position.XY();
					}
				}

				// Token: 0x06008888 RID: 34952 RVA: 0x0037727C File Offset: 0x0037547C
				private void Fire()
				{
					this.m_fireCooldown = this.CooldownTime;
					Projectile currentProjectile = this.m_targetGun.DefaultModule.GetCurrentProjectile();
					bool flag = currentProjectile.GetComponent<BeamController>() != null;
					if (!string.IsNullOrEmpty(this.ShootAudioEvent))
					{
						AkSoundEngine.PostEvent(this.ShootAudioEvent, base.gameObject);
					}
					if (flag)
					{
					}
					else if (this.m_targetGun.Volley != null)
					{
						if (this.ShootDuration > 0f)
						{
							base.StartCoroutine(this.FireVolleyForDuration(this.m_targetGun.Volley, this.m_owner, this.ShootDuration));
						}
						else
						{
							this.FireVolley(this.m_targetGun.Volley, this.m_owner, this.m_parentTransform.eulerAngles.z, new Vector2?(this.ShootPoint));
						}
					}
					else
					{
						ProjectileModule defaultModule = this.m_targetGun.DefaultModule;
						Projectile currentProjectile2 = defaultModule.GetCurrentProjectile();
						if (currentProjectile2)
						{
							float angleForShot = defaultModule.GetAngleForShot(1f, 1f, null);
							if (!flag)
							{
								this.DoSingleProjectile(currentProjectile2, this.m_owner, this.m_parentTransform.eulerAngles.z + angleForShot, new Vector2?(this.ShootPoint), true);
							}
						}
					}
				}

				// Token: 0x06008889 RID: 34953 RVA: 0x00377414 File Offset: 0x00375614
				public IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, float duration, float angle, Vector3 position)
				{
					float elapsed = 0f;
					BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, angle, position);
					this.beamn = beam;
					yield return null;
					while (elapsed < duration)
					{
						if (!this.m_shootPointTransform || !this)
						{
							break;
						}
						if (!this.m_parentTransform)
						{
							break;
						}
						elapsed += BraveTime.DeltaTime;
						this.ContinueFiringBeam(beam, source, this.m_parentTransform.eulerAngles.z, new Vector2?(this.ShootPoint));
						yield return null;
					}
					this.CeaseBeam(beam);
					if (!string.IsNullOrEmpty(this.FinishedShootingAudioEvent) && this)
					{
						AkSoundEngine.PostEvent(this.FinishedShootingAudioEvent, this.gameObject);
					}
					yield break;
				}

				// Token: 0x0600888A RID: 34954 RVA: 0x00377444 File Offset: 0x00375644
				private IEnumerator FireVolleyForDuration(ProjectileVolleyData volley, PlayerController source, float duration)
				{
					float elapsed = 0f;
					float cooldown = 0f;
					while (elapsed < duration)
					{
						elapsed += BraveTime.DeltaTime;
						cooldown -= BraveTime.DeltaTime;
						if (cooldown <= 0f)
						{
							this.FireVolley(volley, source, this.m_parentTransform.eulerAngles.z, new Vector2?(this.ShootPoint));
							cooldown = this.m_targetGun.DefaultModule.cooldownTime;
							for (int i = 0; i < volley.projectiles.Count; i++)
							{
								if (volley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Charged)
								{
									cooldown = Mathf.Max(cooldown, volley.projectiles[i].maxChargeTime);
									cooldown = Mathf.Max(cooldown, 0.5f);
								}
							}
						}
						yield return null;
					}
					this.m_fireCooldown = this.CooldownTime;
					if (!string.IsNullOrEmpty(this.FinishedShootingAudioEvent))
					{
						AkSoundEngine.PostEvent(this.FinishedShootingAudioEvent, this.gameObject);
					}
					yield break;
				}

				// Token: 0x0600888B RID: 34955 RVA: 0x00377474 File Offset: 0x00375674
				private void FireVolley(ProjectileVolleyData volley, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
				{
					if (!string.IsNullOrEmpty(this.OnEveryShotAudioEvent))
					{
						AkSoundEngine.PostEvent(this.OnEveryShotAudioEvent, base.gameObject);
					}
					for (int i = 0; i < volley.projectiles.Count; i++)
					{
						ProjectileModule projectileModule = volley.projectiles[i];
						Projectile currentProjectile = projectileModule.GetCurrentProjectile();
						if (currentProjectile)
						{
							float angleForShot = projectileModule.GetAngleForShot(1f, 1f, null);
							bool flag = currentProjectile.GetComponent<BeamController>() != null;
							if (!flag)
							{
								this.DoSingleProjectile(currentProjectile, source, targetAngle + angleForShot, overrideSpawnPoint, false);
							}
						}
					}
				}

				// Token: 0x0600888C RID: 34956 RVA: 0x00377524 File Offset: 0x00375724
				private void DoSingleProjectile(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint, bool doAudio = false)
				{
					if (doAudio && !string.IsNullOrEmpty(this.OnEveryShotAudioEvent))
					{
						AkSoundEngine.PostEvent(this.OnEveryShotAudioEvent, base.gameObject);
					}
					if (this.ConsumesTargetGunAmmo && this.m_targetGun && this.m_owner.inventory.AllGuns.Contains(this.m_targetGun))
					{
						if (this.m_targetGun.ammo == 0)
						{
							return;
						}
						if (UnityEngine.Random.value < this.ChanceToConsumeTargetGunAmmo)
						{
							this.m_targetGun.LoseAmmo(1);
						}
					}
					Vector2 v = (overrideSpawnPoint == null) ? source.specRigidbody.UnitCenter : overrideSpawnPoint.Value;
					GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, v, Quaternion.Euler(0f, 0f, targetAngle), true);
					Projectile component = gameObject.GetComponent<Projectile>();
					component.Owner = source;
					component.Shooter = source.specRigidbody;
					source.DoPostProcessProjectile(component);
					BounceProjModifier component2 = component.GetComponent<BounceProjModifier>();
					if (component2)
					{
						component2.numberOfBounces = Mathf.Min(3, component2.numberOfBounces);
					}
				}

				// Token: 0x0600888D RID: 34957 RVA: 0x00377650 File Offset: 0x00375850
				private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
				{
					Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
					GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
					Projectile component = gameObject.GetComponent<Projectile>();
					component.Owner = source;
					BeamController component2 = gameObject.GetComponent<BeamController>();
					component2.Owner = source;
					component2.chargeDelay = 0f;
					component2.HitsPlayers = false;
					component2.HitsEnemies = true;
					Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
					component2.Direction = v;
					component2.Origin = vector;
					return component2;
				}

				// Token: 0x0600888E RID: 34958 RVA: 0x003776E8 File Offset: 0x003758E8
				private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint)
				{
					Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
					beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
					beam.Origin = vector;
					beam.LateUpdatePosition(vector);
				}

				// Token: 0x0600888F RID: 34959 RVA: 0x002E0188 File Offset: 0x002DE388
				internal void CeaseBeam(BeamController beam)
				{
					beam.CeaseAttack();
				}

				// Token: 0x06008890 RID: 34960 RVA: 0x00377738 File Offset: 0x00375938
				protected override void OnDestroy()
				{
					if (this.m_owner)
					{
						this.m_owner.OnDodgedProjectile -= this.HandleDodgedProjectileFire;
					}
					if (this.m_owner)
					{
						this.m_owner.PostProcessProjectile -= this.HandleFiredGun;
					}
					if (!string.IsNullOrEmpty(this.FinishedShootingAudioEvent))
					{
						AkSoundEngine.PostEvent(this.FinishedShootingAudioEvent, base.gameObject);
					}
					if (this.Position == FakeDualWield1.HoverPosition.CIRCULATE)
					{
						for (int i = 0; i < this.m_owner.orbitals.Count; i++)
						{
							if (this.m_owner.orbitals[i].GetOrbitalTier() == this.GetOrbitalTier() && this.m_owner.orbitals[i].GetOrbitalTierIndex() > this.GetOrbitalTierIndex())
							{
								this.m_owner.orbitals[i].SetOrbitalTierIndex(this.m_owner.orbitals[i].GetOrbitalTierIndex() - 1);
							}
						}
						this.m_owner.orbitals.Remove(this);
					}
					LootEngine.DoDefaultSynergyPoof(base.sprite.WorldCenter, false);
				}

				// Token: 0x06008891 RID: 34961 RVA: 0x00377875 File Offset: 0x00375A75
				public void Reinitialize()
				{
					if (base.specRigidbody)
					{
						base.specRigidbody.Reinitialize();
					}
					this.m_ownerCenterAverage = this.m_owner.CenterPosition;
				}

				// Token: 0x06008892 RID: 34962 RVA: 0x003778A3 File Offset: 0x00375AA3
				public Transform GetTransform()
				{
					return this.m_parentTransform;
				}

				// Token: 0x06008893 RID: 34963 RVA: 0x003778AB File Offset: 0x00375AAB
				public void ToggleRenderer(bool visible)
				{
					base.sprite.renderer.enabled = visible;
				}

				// Token: 0x06008894 RID: 34964 RVA: 0x003778BE File Offset: 0x00375ABE
				public int GetOrbitalTier()
				{
					return this.m_orbitalTier;
				}

				// Token: 0x06008895 RID: 34965 RVA: 0x003778C6 File Offset: 0x00375AC6
				public void SetOrbitalTier(int tier)
				{
					this.m_orbitalTier = tier;
				}

				// Token: 0x06008896 RID: 34966 RVA: 0x003778CF File Offset: 0x00375ACF
				public int GetOrbitalTierIndex()
				{
					return this.m_orbitalTierIndex;
				}

				// Token: 0x06008897 RID: 34967 RVA: 0x003778D7 File Offset: 0x00375AD7
				public void SetOrbitalTierIndex(int tierIndex)
				{
					this.m_orbitalTierIndex = tierIndex;
				}

				// Token: 0x06008898 RID: 34968 RVA: 0x003778E0 File Offset: 0x00375AE0
				public float GetOrbitalRadius()
				{
					return 2.5f;
				}

				// Token: 0x06008899 RID: 34969 RVA: 0x003778E7 File Offset: 0x00375AE7
				public float GetOrbitalRotationalSpeed()
				{
					return 120f;
				}

				// Token: 0x04008DD2 RID: 36306
				public FakeDualWield1.HoverPosition Position;

				// Token: 0x04008DD3 RID: 36307
				public FakeDualWield1.FireType Trigger;

				// Token: 0x04008DD4 RID: 36308
				public FakeDualWield1.AimType Aim;

				// Token: 0x04008DD5 RID: 36309
				public float AimRotationAngularSpeed;

				// Token: 0x04008DD6 RID: 36310
				public float ShootDuration;

				// Token: 0x04008DD7 RID: 36311
				public float CooldownTime;

				// Token: 0x04008DD8 RID: 36312
				public bool OnlyOnEmptyReload;

				// Token: 0x04008DD9 RID: 36313
				public bool ConsumesTargetGunAmmo;

				// Token: 0x04008DDA RID: 36314
				public float ChanceToConsumeTargetGunAmmo;
				private Shader m_glintShader;

				// Token: 0x04008DDB RID: 36315
				public string ShootAudioEvent;

				// Token: 0x04008DDC RID: 36316
				public string OnEveryShotAudioEvent;

				// Token: 0x04008DDD RID: 36317
				public string FinishedShootingAudioEvent;

				// Token: 0x04008DDE RID: 36318
				private bool m_initialized;

				// Token: 0x04008DDF RID: 36319
				private Transform m_parentTransform;

				// Token: 0x04008DE0 RID: 36320
				private Transform m_shootPointTransform;

				// Token: 0x04008DE1 RID: 36321
				private Gun m_targetGun;

				// Token: 0x04008DE2 RID: 36322
				private PlayerController m_owner;

				// Token: 0x04008DE3 RID: 36323
				private float m_currentAimTarget;

				// Token: 0x04008DE4 RID: 36324
				private bool m_hasEnemyTarget;

				// Token: 0x04008DE5 RID: 36325
				private float m_fireCooldown;

				// Token: 0x04008DE6 RID: 36326
				private Vector2 m_ownerCenterAverage;

				// Token: 0x04008DE7 RID: 36327
				private float m_orbitalAngle;

				// Token: 0x04008DE8 RID: 36328
				private int m_orbitalTier;

				// Token: 0x04008DE9 RID: 36329
				private int m_orbitalTierIndex;
				public Vector3 gunposition;
				public float designatedtarget1;
				public Vector3 Oldgun;
				internal BeamController beamn;


				// Token: 0x020016F3 RID: 5875
				public enum HoverPosition
				{
					// Token: 0x04008DEB RID: 36331
					OVERHEAD,
					// Token: 0x04008DEC RID: 36332
					CIRCULATE
				}

				// Token: 0x020016F4 RID: 5876
				public enum FireType
				{
					// Token: 0x04008DEE RID: 36334
					ON_RELOAD,
					// Token: 0x04008DEF RID: 36335
					ON_COOLDOWN,
					// Token: 0x04008DF0 RID: 36336
					ON_DODGED_BULLET,
					// Token: 0x04008DF1 RID: 36337
					ON_FIRED_GUN
				}

				// Token: 0x020016F5 RID: 5877
				public enum AimType
				{
					// Token: 0x04008DF3 RID: 36339
					NEAREST_ENEMY,
					// Token: 0x04008DF4 RID: 36340
					PLAYER_AIM
				}
			}
		}
	}
	











	


