using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mod
{
        //Call this method from the Start() method of your ETGModule extension
        public class Thesoul : PassiveItem
        {
		private float pushRadius;
		private float secondRadius;
		private float finalRadius;
		private float pushStrength;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
            {
                string name = "The Soul";
                string resourceName = "ClassLibrary1/Resources/BrittleCrown"; ;
                GameObject gameObject = new GameObject();
               Thesoul frailCrown = gameObject.AddComponent<Thesoul>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Rise and fall";
                string longDesc = "Casings per kill, get hit and lose it all \n" + "A crown found on an enigmatic planet, great success and terrible failures seem to befall whomever holds it in rapid succession. \n" + 
                "You cannot bear to give it up, oh well, nothing ventured nothing gained.";
                frailCrown.SetupItem( shortDesc, longDesc, "ror");
                frailCrown.quality = PickupObject.ItemQuality.C;
            frailCrown.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
			StartCoroutine(Deflec());
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        private IEnumerator Deflec()
        {
			this.pushRadius = 4f;
			this.secondRadius = 6f;
			this.finalRadius = 8f;
			this.pushStrength = 10f;
			base.Update();
			float innerRadiusSqrDistance = this.pushRadius * this.pushRadius;
			float outerRadiusSqrDistance = this.secondRadius * this.secondRadius;
			float finalRadiusSqrDistance = this.finalRadius * this.finalRadius;
			float pushStrengthRadians = this.pushStrength * 0.0174532924f;
			List<Projectile> ensnaredProjectiles = new List<Projectile>();
			List<Vector2> initialDirections = new List<Vector2>();
			GameObject[] octantVFX = new GameObject[8];
			while (base.Owner)
			{
				Vector2 playerCenter = base.Owner.CenterPosition;
				ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
				for (int i = 0; i < allProjectiles.Count; i++)
				{
					Projectile projectile = allProjectiles[i];
					if (projectile.Owner != base.Owner && !(projectile.Owner is PlayerController))
					{
						Vector2 worldCenter = projectile.sprite.WorldCenter;
						Vector2 vector = worldCenter - playerCenter;
						float num = Vector2.SqrMagnitude(vector);
						if (num < innerRadiusSqrDistance && !ensnaredProjectiles.Contains(projectile))
						{
							projectile.RemoveBulletScriptControl();
							ensnaredProjectiles.Add(projectile);
							initialDirections.Add(projectile.Direction);
							int num2 = BraveMathCollege.VectorToOctant(vector);
							if (octantVFX[num2] == null)
							{
								FortuneFavorItem spark  = PickupObjectDatabase.GetById(105).GetComponent<FortuneFavorItem>();
								octantVFX[num2] = base.Owner.PlayEffectOnActor(spark.sparkOctantVFX, Vector3.zero, true, true, false);
								octantVFX[num2].transform.rotation = Quaternion.Euler(0f, 0f, (float)(-45 + -45 * num2));
							}
						}
					}
				}
				for (int j = 0; j < ensnaredProjectiles.Count; j++)
				{
					Projectile projectile2 = ensnaredProjectiles[j];
					if (!projectile2)
					{
						ensnaredProjectiles.RemoveAt(j);
						initialDirections.RemoveAt(j);
						j--;
					}
					else
					{
						Vector2 worldCenter2 = projectile2.sprite.WorldCenter;
						Vector2 a = playerCenter - worldCenter2;
						float num3 = Vector2.SqrMagnitude(a);
						if (num3 > finalRadiusSqrDistance)
						{
							ensnaredProjectiles.RemoveAt(j);
							initialDirections.RemoveAt(j);
							j--;
						}
						else if (num3 > outerRadiusSqrDistance)
						{
							projectile2.Direction = Vector3.RotateTowards(projectile2.Direction, initialDirections[j], pushStrengthRadians * BraveTime.DeltaTime * 0.5f, 0f).XY().normalized;
						}
						else
						{
							Vector2 v = a * -1f;
							float num4 = 1f;
							if (num3 / innerRadiusSqrDistance < 0.75f)
							{
								num4 = 3f;
							}
							v = ((v.normalized + initialDirections[j].normalized) / 2f).normalized;
							projectile2.Direction = Vector3.RotateTowards(projectile2.Direction, v, pushStrengthRadians * BraveTime.DeltaTime * num4, 0f).XY().normalized;
						}
					}
				}
				for (int k = 0; k < 8; k++)
				{
					if (octantVFX[k] != null && !octantVFX[k])
					{
						octantVFX[k] = null;
					}
				}
				yield return null;
			}
			yield break;
		}

    }
}
