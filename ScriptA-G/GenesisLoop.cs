using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;

namespace Mod
{
        //Call this method from the Start() method of your ETGModule extension
        public class Gloop : PassiveItem
        {
            // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
            public static void Init()
            {
                string name = "Genesis Loop";
                string resourceName = "ClassLibrary1/Resources/GenesisLoop"; 
                GameObject gameObject = new GameObject();
                Gloop gloop = gameObject.AddComponent<Gloop>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Jellyfish heart - EXPLOSIVE!";
            string longDesc = "Explode when taking damage \n" + "The heart of a wandering vagrant, many explorers have been killed by the blast the creature releases when brought near death \n " +
            "and this the organ that makes it happen. STILL FRESH.";
                gloop.SetupItem( shortDesc, longDesc, "ror");
            ItemBuilder.AddPassiveStatModifier(gloop, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
                gloop.quality = PickupObject.ItemQuality.C;
            gloop.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f); 
            gloop.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            player.OnReceivedDamage += this.Thing;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReceivedDamage -= this.Thing;
            DebrisObject result = base.Drop(player);
            return result;
        }

        public void Thing(PlayerController player)
        {

                        this.Boom(player.specRigidbody.UnitCenter);
                        player.ForceBlank(20f, 0.5f, false, true, null, false, -1f);
        
        }
        public void Boom(Vector3 position)
        {
            ExplosionData defaultSmallExplosionData2 = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
            this.smallPlayerSafeExplosion.effect = defaultSmallExplosionData2.effect;
            this.smallPlayerSafeExplosion.ignoreList = defaultSmallExplosionData2.ignoreList;
            this.smallPlayerSafeExplosion.ss = defaultSmallExplosionData2.ss;
            Exploder.Explode(position, this.smallPlayerSafeExplosion, Vector2.zero, null, false, CoreDamageTypes.None, false);
        }

        private ExplosionData smallPlayerSafeExplosion = new ExplosionData
        {
            damageRadius = 30f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 500f,
            doDestroyProjectiles = true,
            doForce = true,
            debrisForce = 30f,
            preventPlayerForce = true,
            explosionDelay = 0.5f,
            usesComprehensiveDelay = false,
            doScreenShake = true,
            playDefaultSFX = true
        };
        private float healthPercent = 0f;
        private float lastHealthPercent = -1f;
    }
    }