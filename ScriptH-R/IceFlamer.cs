using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Mod
{

    public class Icer : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("The Cold One", "icer");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:the_cold_one", "ror:the_cold_one");
            gun.gameObject.AddComponent<Icer>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Crack Them Open");
            gun.SetLongDescription("WARNING - CONTAINS PRESSURIZED SUPERCOOLED FLUID - UNDERWHELMING IF DUCT TAPED TO OTHER GUNS, DUCT TAPING GUNS TO IT IS FINE.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "icer_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(223) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.cooldownTime = 0.05f;
            gun.DefaultModule.numberOfShotsInClip = 600;
            gun.SetBaseMaxAmmo(600);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "The Cold One";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            projectile.baseData.damage = 1f;
            projectile.baseData.speed = 10f;
            projectile.baseData.range = 8f;
            gun.barrelOffset.localPosition += new Vector3(1.3f, 0f, 0f);
            projectile.transform.parent = gun.barrelOffset;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            gun.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            icerInt = gun.PickupObjectId;

        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            this.FlamerSalvo();

        }

        private void FlamerSalvo()
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            this.FlamerTrue = 1f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            Projectile projectile = ((Gun)ETGMod.Databases.Items[225]).DefaultModule.projectiles[0];
            if (UnityEngine.Random.value > 0.5f)
            { this.sign = -1f; }
            else { this.sign = 1f; }
            float value = UnityEngine.Random.value * 7f * sign;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0.5f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + value), true);
            if (UnityEngine.Random.value > 0.5f)
            { this.sign = -1f; }
            else { this.sign = 1f; }
            float value1 = UnityEngine.Random.value * 7f * sign;
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0.5f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + value1), true);
            if (UnityEngine.Random.value > 0.5f)
            { this.sign = -1f; }
            else { this.sign = 1f; }
            float value2 = UnityEngine.Random.value * 7f * sign;
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0.5f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + value2), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component2 = gameObject2.GetComponent<Projectile>();
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = man;
                component.AdjustPlayerProjectileTint(new Color(0f, UnityEngine.Random.Range(5, 20), UnityEngine.Random.Range(5, 40)), 5, 0f);
                component.Shooter = man.specRigidbody;
                component.baseData.speed = 10f;
                component.baseData.force = 0f;
                component.AdditionalScaleMultiplier = 1.5f + UnityEngine.Random.value;
                component.SetOwnerSafe(man, "Player");
                component.baseData.damage = this.FlamerTrue;
                component.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
            }
            bool flag2 = component2 != null;
            if (flag2)
            {
                component2.Owner = man;
                component2.AdjustPlayerProjectileTint(new Color(0f, UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40)), 5, 0f);
                component2.Shooter = man.specRigidbody;
                component2.baseData.speed = 10f;
                component2.baseData.force = 0f;
                component2.SetOwnerSafe(man, "Player");
                component.AdditionalScaleMultiplier = 1.5f + UnityEngine.Random.value;
                component2.baseData.damage = this.FlamerTrue;
                component2.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;

            }
            bool flag3 = component3 != null;
            if (flag3)
            {
                component3.Owner = man;
                component3.AdjustPlayerProjectileTint(new Color(UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40)), 5, 0f);
                component3.Shooter = man.specRigidbody;
                component3.SetOwnerSafe(man, "Player");
                component3.baseData.speed = 10f;
                component3.baseData.force = 0f;
                component3.AdditionalScaleMultiplier = 0.1f + UnityEngine.Random.value;
                component3.baseData.damage = 0f;
                component3.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;

            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {
            this.Owner = gun.CurrentOwner;
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_ENM_flame_veil_01", gameObject);
        }
        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
        protected void Update()
        {
            if (gun.CurrentOwner)
            {
                PlayerController man = base.gun.CurrentOwner as PlayerController;
                if (man.PlayerHasActiveSynergy("Of Fire And Ice") && !man.HasPickupID(Gungeon.Game.Items["ror:dragon's_dream"].PickupObjectId) && !addedgun)
                { man.inventory.AddGunToInventory(Gungeon.Game.Items["ror:dragon's_dream"] as Gun, true);
                    addedgun = true;
                }
                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }

        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_ENM_flame_veil_01", base.gameObject);
            }
        }



        // Token: 0x040077B6 RID: 30646
        public float ActivationChance;

        // Token: 0x040077B8 RID: 30648
        public bool TriggersRadialBulletBurst;

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("TriggersRadialBulletBurst", false)]
        public RadialBurstInterface RadialBurstSettings;

        private bool onCooldown; private bool onCooldown1;
        private float FlamerBase = 7f;

        // Token: 0x0400001D RID: 29
        private float FlamerTrue;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;
        private bool firingflag;
        private bool engaged;
        private float sign;
        private bool activated;
        private object LastOwner;
        private AIActor Bomb;
        private GameObject Nuke;
        private Shader m_glintShader;
        private bool alive;
        private GameActor Owner;
        private PlayerController player;
        private AIActor aiactor;
        private int flat9;
        internal static int icerInt;
        public bool addedgun;

        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}