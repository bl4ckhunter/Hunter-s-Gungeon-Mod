using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class Drug : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Military Grade Drug";
            string resourceName = "ClassLibrary1/Resources/Soldier's_Syringe"; ;
            GameObject gameObject = new GameObject();
            Drug drug = gameObject.AddComponent<Drug>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Going Out Guns Blazing";
            string longDesc = "Chance to fire a radial burst on hit \n" + "A stimulant designed for military use, no one knows if the periodical urge to shoot in random directions is a common side effect or it's due to \n+" +
                "the drug being at least a couple centuries past expiration";
            drug.SetupItem(shortDesc, longDesc, "ror");
            ItemBuilder.AddPassiveStatModifier(drug, PlayerStats.StatType.RateOfFire, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            drug.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            drug.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

            drug.quality = PickupObject.ItemQuality.A;

        }
        public override void Pickup(PlayerController player)
        {
            player.OnDealtDamageContext += this.OnDealtDamage;
            base.Pickup(player);
        } 



        public override DebrisObject Drop(PlayerController player)
        {
            player.OnDealtDamageContext -= this.OnDealtDamage;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnDealtDamageContext -= this.OnDealtDamage;
            }
            base.OnDestroy();
        }

        private IEnumerator Burst(PlayerController usingPlayer)
        {
            bool flag21 = !Drug.onCooldown1;
            if (flag21)
            {
                Drug.onCooldown1 = true;
                GameManager.Instance.StartCoroutine(Drug.StartCooldown1());
                RadialBurstSettings.SpiralWaves = true;
                RadialBurstSettings.BeamSweepDegrees = 360f;
                RadialBurstSettings.NumberWaves = 2;
                this.RadialBurstSettings.DoBurst(usingPlayer, null, null);
                yield return new WaitForSeconds(0.15f);
                this.RadialBurstSettings.DoBurst(usingPlayer, null, null);
                yield break;
            }
        }
        private void OnDealtDamage(PlayerController usingPlayer, float amount, bool fatal, HealthHaver targetr)
        {
            
                {
                    bool flag2 = !Drug.onCooldown;
                    if (flag2)

                {
                    Drug.onCooldown = true;
                    GameManager.Instance.StartCoroutine(Drug.StartCooldown());
                        float value = UnityEngine.Random.value;
                        bool flag = value < 0.15f;
                        if (flag)
                        {
                        IEnumerator routine = Burst(usingPlayer);
                        GameManager.Instance.StartCoroutine(routine);
                    }
                    }
                }

        }
        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(0.5f);
            Drug.onCooldown = false;
            yield break;
        }
        private static IEnumerator StartCooldown1()
        {
            yield return new WaitForSeconds(10f);
            Drug.onCooldown1 = false;
            yield break;
        }

        // Token: 0x040077E5 RID: 30693
        private int NumberWaves;

        private int NumberSubwaves;


        private float TimeBetweenWaves;

        private float BeamSweepDegrees;

        // Token: 0x040077B8 RID: 30648

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("TriggersRadialBulletBurst", false)]
        public RadialBurstInterface RadialBurstSettings;

        private static bool onCooldown; private static bool onCooldown1;

    } 
      
 }     