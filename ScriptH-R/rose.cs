using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using ClassLibrary1.Scripts;
using Dungeonator;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using Items;
using MonoMod;
using Swordtress;
using UnityEngine;

namespace Mod
{

    public class Rose : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Crescent Rose", "rescentrose");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:crescent_rose", "ror:crescent_rose");
            gun.gameObject.AddComponent<Rose>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Fire and Slice");
            gun.SetLongDescription("The signature weapon of an huntress from a distant dimension");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "rescentrose_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 20);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(486) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.5f; 
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.SetBaseMaxAmmo(80);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "Rose1a";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.gameObject.GetComponent<ExplosiveModifier>().explosionData.doScreenShake = false;
            projectile.gameObject.GetComponent<ExplosiveModifier>().explosionData.damage *= 0.25f;
            projectile.baseData.damage *= 0.25f;
            projectile.baseData.range = 100f;
            projectile.baseData.speed = 200f;
            projectile.AdjustPlayerProjectileTint(Color.red, 10, 0f);
            gun.barrelOffset.localPosition += new Vector3(3f, 2.1f, 0f);
            gun.CanReloadNoMatterAmmo = true;
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            projectile.transform.parent = gun.barrelOffset;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            gun.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            Rose.RoseshytheID = gun.PickupObjectId;
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            PlayerController man = base.gun.CurrentOwner as PlayerController;
           if (man.PlayerHasActiveSynergy("Petal Burst"))
            {
                projectile.baseData.damage *= 1.35f;
                man.CurrentGun.ClipShotsRemaining = 0;
                GameManager.Instance.StartCoroutine(Waitforsslash1());
            }


        }


        private IEnumerator Waitforsslash1()
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(0.101f);
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            man.CurrentGun.Reload();
            SlashDoer.DoSwordSlash(man.transform.position, base.gun.CurrentAngle, man, 0f, SlashDoer.ProjInteractMode.DESTROY, 400f, 5f, null, null, 1, 1, 6f, 55f);
        }









        // Token: 0x0600748E RID: 29838 RVA: 0x00007B97 File Offset: 0x00005D97


        // Token: 0x06007490 RID: 29840 RVA: 0x002D6F20 File Offset: 0x002D5120


        // Token: 0x06007491 RID: 29841 RVA: 0x002D70E0 File Offset: 0x002D52E0
        // Token: 0x06007492 RID: 29842 RVA: 0x002D7130 File Offset: 0x002D5330

        // Token: 0x0400765E RID: 30302
        public float trackingSpeed;

        // Token: 0x0400765F RID: 30303
        public float trackingTime;

        // Token: 0x04007660 RID: 30304
        [CurveRange(0f, 0f, 1f, 1f)]
        public AnimationCurve trackingCurve;

        // Token: 0x04007661 RID: 30305
        private PlayerController m_player;
    







    public override void OnPostFired(PlayerController player, Gun gun)
        {
            this.Owner = gun.CurrentOwner;
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_sniperrifle_shot_01", gameObject);
        }
        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
        protected void Update()
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            if (gun.CurrentOwner)
            {

                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
                if (man.PlayerHasActiveSynergy("Petal Burst"))
                {
                    gun.reloadTime = 0.75f  * man.stats.GetStatValue(PlayerStats.StatType.ReloadSpeed);
                }
                if (!man.PlayerHasActiveSynergy("Petal Burst"))
                {
                    gun.reloadTime = 1.5f * man.stats.GetStatValue(PlayerStats.StatType.ReloadSpeed);
                }


            }


        }

       

        public override void OnDropped()
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            base.OnDropped();
        }

     

        // Token: 0x06000264 RID: 612 RVA: 0x0001B298 File Offset: 0x00019498

        // Token: 0x06000265 RID: 613 RVA: 0x0001B2A4 File Offset: 0x000194A4

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_sword_impact_01", base.gameObject);
                SlashDoer.DoSwordSlash(player.transform.position, base.gun.CurrentAngle, player, 0f, SlashDoer.ProjInteractMode.DESTROY, 400f, 5f, null, null, 1, 1, 6f, 55f);

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
        private PlayerController m_owner;
        private Projectile m_projectile;
        private Vector2 crosshair;
        private static FireVolleyOnRollItem m_bomb;
        private bool unused;
        private bool GaveKrit;
        private TeleporterPrototypeItem teleporter;
        private bool Synergized;
        private static Projectile rojectile;
        public static int RoseshytheID;


        // Token: 0x04007661 RID: 30305

        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}