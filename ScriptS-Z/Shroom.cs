using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections.Generic;
using System.Collections;
using Gungeon;

namespace Mod
{
        //Call this method from the Start() method of your ETGModule extension
        public class Shroom : PassiveItem
        {
            // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
            public static void Init()
            {
                string name = "Gatling Mushroom";
                string resourceName = "ClassLibrary1/Resources/Bustling_Fungus"; ;
                GameObject gameObject = new GameObject();
                Shroom shroom = gameObject.AddComponent<Shroom>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Stand your ground";
                string longDesc = "A cousin of the better known bustling fungus this symbiontic shroom has shown marvelous adaptation to the gungeon's environment.\n" + 
                "Thought it has completely lost the ability to heal the host the original species had, it can help in a more direct, gungeon suited fashion, namely by giving the host a gigantic gatling gun.\n" +
                "Powerful tool as it is it still retains the original species's signature defense mechanism and will hide when hit or jostled by a dodgeroll, needing around 35 seconds to gather it's composure again. \n" 
                ; 
            shroom.SetupItem( shortDesc, longDesc, "ror");
                shroom.quality = PickupObject.ItemQuality.S;
            shroom.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f); 
            shroom.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnDealtDamageContext += this.OnDealtDamage;
            player.OnReceivedDamage += this.Blank;
            player.OnPreDodgeRoll += this.Blank;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnDealtDamageContext -= this.OnDealtDamage;
            player.OnReceivedDamage -= this.Blank;
            player.OnPreDodgeRoll -= this.Blank;
            bool flag2 = Shroom.active;
            if (flag2)
            {
                player.inventory.DestroyGun(this.m_extantGun);
                IEnumerator routine = StartCooldown();
                GameManager.Instance.StartCoroutine(routine);
                Shroom.onCooldown = true;
            }
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnDealtDamageContext -= this.OnDealtDamage;
                player.OnReceivedDamage -= this.Blank;
                player.OnPreDodgeRoll -= this.Blank;
                bool flag2 = Shroom.active;
                if (flag2)
                {
                    player.inventory.DestroyGun(this.m_extantGun);
                    IEnumerator routine = StartCooldown();
                    GameManager.Instance.StartCoroutine(routine);
                    Shroom.onCooldown = true;
                }
            }
            base.OnDestroy();
        }

        private void Blank(PlayerController player)
        {
            try{ Vector2 centerPosition = player.CenterPosition;
          
                bool flag2 = Shroom.active;
                if (flag2)
                {
                    player.inventory.DestroyGun(this.m_extantGun);
                    IEnumerator routine = StartCooldown();
                    GameManager.Instance.StartCoroutine(routine);
                    Shroom.onCooldown = true;
                }
            }
            catch
            { IEnumerator routine = StartCooldown();
            GameManager.Instance.StartCoroutine(routine);
            Shroom.onCooldown = true;
            }
        }
        private static IEnumerator StartCooldown()
        {
            Shroom.active = false;
            yield return
            new WaitForSeconds(35f);
            Shroom.onCooldown = false;
            yield break;
        }
        private void OnDealtDamage(PlayerController player, float amount, bool fatal, HealthHaver target)
        {
            bool flag = !Shroom.onCooldown;
            if (flag) 
            {
            bool flag1 = !Shroom.active;
            if (flag1)
                {
                    Shroom.active = true;
                    Gun gun;
                    gun = (PickupObjectDatabase.GetById(546) as Gun);
                    this.m_extantGun = player.inventory.AddGunToInventory(gun, true);
                } 
            }

        }

        // Token: 0x060001D8 RID: 472 RVA: 0x0000E788 File Offset: 0x0000C988


        public PlayerController LastOwner;

                private static bool onCooldown;
                private static bool active;
        private Gun m_extantGun;

        private float Boost = 0f;

        // Token: 0x040000BB RID: 187
        private float LastCheckBoost = -1f;

        // Token: 0x040000BC RID: 188
        private float CheckBoost = 0f;

        // Token: 0x040000BD RID: 189
        private float TrueColorBoost;

        // Token: 0x040000BE RID: 190
        private float ColorBoost;

        // Token: 0x040000BF RID: 191
        private PlayerController m_player;

    }
}

