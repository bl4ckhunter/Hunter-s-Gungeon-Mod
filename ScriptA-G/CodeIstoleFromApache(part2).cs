
	using System;
	using UnityEngine;

	namespace ClassLibrary1.Scripts
	{
		// Token: 0x02000083 RID: 131
		public class StolenReaperController : BraveBehaviour
		{
			// Token: 0x0600049B RID: 1179 RVA: 0x000AF978 File Offset: 0x000ADB78
			public StolenReaperController()
			{
				this.MinSpeed = 3f;
				this.MaxSpeed = 6f;
				this.MinSpeedDistance = 10f;
				this.MaxSpeedDistance = 50f;
				this.c_particlesPerSecond = 50;
				this.knockbackComponent = Vector2.zero;
				this.becomesBlackPhantom = false;
			}

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x0600049C RID: 1180 RVA: 0x000AF9D1 File Offset: 0x000ADBD1
			public static StolenReaperController Instance
			{
				get
				{
					return StolenReaperController.m_instance;
				}
			}

			// Token: 0x0600049D RID: 1181 RVA: 0x000AF9D8 File Offset: 0x000ADBD8
			private void Start()
			{
				StolenReaperController.m_instance = this;
				this.ShootPoint = base.aiActor.transform;
				base.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
				base.aiAnimator.PlayUntilFinished("intro", false, null, -1f, false);
				base.specRigidbody.RegisterSpecificCollisionException(GameManager.Instance.PrimaryPlayer.specRigidbody);
				base.specRigidbody.PrimaryPixelCollider.Enabled = false;
				base.specRigidbody.CollideWithTileMap = false;
				base.healthHaver.SetHealthMaximum(150f, null, false);
				base.healthHaver.PreventAllDamage = false;
			base.aiShooter.PostProcessProjectile += HandleCompanionPostProcessProjectile;
				base.aiActor.actorTypes = CoreActorTypes.Ghost;
			    base.aiActor.CanTargetEnemies = true;
			    base.aiActor.CanTargetPlayers = false;
				(base.behaviorSpeculator.AttackBehaviors[1] as TeleportBehavior).MaxUsages = 1;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					base.specRigidbody.RegisterSpecificCollisionException(GameManager.Instance.SecondaryPlayer.specRigidbody);
				}
				if (this.becomesBlackPhantom)
				{
					base.aiActor.BecomeBlackPhantom();
				}
				if (base.aiActor.GetComponentsInChildren<tk2dBaseSprite>(true) != null)
				{
					float glitchInterval = UnityEngine.Random.Range(0.02f, 0.06f);
					float dispProbability = UnityEngine.Random.Range(0.1f, 0.16f);
					float dispIntensity = UnityEngine.Random.Range(0.1f, 0.4f);
					float colorProbability = UnityEngine.Random.Range(0.05f, 0.2f);
					float colorIntensity = UnityEngine.Random.Range(0.1f, 0.25f);
				}
				this.m_currentTargetPlayer = GameManager.Instance.GetRandomActivePlayer();
			}

		private void HandleCompanionPostProcessProjectile(Projectile obj)
		{
			obj.collidesWithPlayer = false;
			obj.TreatedAsNonProjectileForChallenge = true;
			obj.baseData.damage = 0f;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000AFBBD File Offset: 0x000ADDBD
		protected override void OnDestroy()
			{
				base.OnDestroy();
				StolenReaperController.m_instance = null;
			}

			// Token: 0x0600049F RID: 1183 RVA: 0x000AFBCC File Offset: 0x000ADDCC
			private void Update()
			{
				if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
				{
					return;
				}
				if (BossKillCam.BossDeathCamRunning || GameManager.Instance.PreventPausing)
				{
					return;
				}
				if (TimeTubeCreditsController.IsTimeTubing)
				{
					base.gameObject.SetActive(false);
					return;
				}
				this.HandleMotion();
				this.UpdateBlackPhantomParticles();
			}

			// Token: 0x060004A0 RID: 1184 RVA: 0x000AFC38 File Offset: 0x000ADE38
			private void UpdateBlackPhantomParticles()
			{
				if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && !base.aiAnimator.IsPlaying("intro") && !this.becomesBlackPhantom)
				{
					Vector3 vector = base.specRigidbody.UnitBottomLeft.ToVector3ZisY(0f);
					Vector3 vector2 = base.specRigidbody.UnitTopRight.ToVector3ZisY(0f);
					float num = (vector2.y - vector.y) * (vector2.x - vector.x);
					float num2 = (float)this.c_particlesPerSecond * num;
					int num3 = Mathf.CeilToInt(Mathf.Max(1f, num2 * BraveTime.DeltaTime));
					Vector3 minPosition = vector;
					Vector3 maxPosition = vector2;
					Vector3 up = Vector3.up;
					float angleVariance = 120f;
					float magnitudeVariance = 0.5f;
					float? startLifetime = new float?(UnityEngine.Random.Range(1f, 1.65f));
					GlobalSparksDoer.DoRandomParticleBurst(num3, minPosition, maxPosition, up, angleVariance, magnitudeVariance, null, startLifetime, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
				}
			}

			// Token: 0x060004A1 RID: 1185 RVA: 0x000AFD44 File Offset: 0x000ADF44
			private void HandleMotion()
			{
				base.specRigidbody.Velocity = Vector2.zero;
				if (base.aiAnimator.IsPlaying("intro"))
				{
					return;
				}
				if (this.m_currentTargetPlayer == null)
				{
					return;
				}
				if (this.m_currentTargetPlayer.healthHaver.IsDead || this.m_currentTargetPlayer.IsGhost)
				{
					this.m_currentTargetPlayer = GameManager.Instance.GetRandomActivePlayer();
				}
				Vector2 vector = this.m_currentTargetPlayer.CenterPosition + new Vector2 (5f,5f) - base.specRigidbody.UnitCenter;
				float magnitude = vector.magnitude;
				float d = Mathf.Lerp(this.MinSpeed, this.MaxSpeed, (magnitude - this.MinSpeedDistance) / (this.MaxSpeedDistance - this.MinSpeedDistance));
				base.specRigidbody.Velocity = vector.normalized * d;
				base.specRigidbody.Velocity += this.knockbackComponent;
			}

			// Token: 0x04000624 RID: 1572
			private static StolenReaperController m_instance;

			// Token: 0x04000625 RID: 1573
			public static bool PreventShooting;

			// Token: 0x04000626 RID: 1574
			public BulletScriptSelector BulletScript;

			// Token: 0x04000627 RID: 1575
			public Transform ShootPoint;

			// Token: 0x04000628 RID: 1576
			public bool becomesBlackPhantom;

			// Token: 0x04000629 RID: 1577
			public float MinSpeed;

			// Token: 0x0400062A RID: 1578
			public float MaxSpeed;

			// Token: 0x0400062B RID: 1579
			public float MinSpeedDistance;

			// Token: 0x0400062C RID: 1580
			public float MaxSpeedDistance;

			// Token: 0x0400062D RID: 1581
			[NonSerialized]
			public Vector2 knockbackComponent;

			// Token: 0x0400062E RID: 1582
			private PlayerController m_currentTargetPlayer;

			// Token: 0x0400062F RID: 1583
			private int c_particlesPerSecond;
		}
	}

