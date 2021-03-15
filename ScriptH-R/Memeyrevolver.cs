using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using Items;
using MonoMod;
using UnityEngine;

namespace Mod
{

    public class MemeRevolver : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Memey Revolver", "revolver");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:memey_revolver", "ror:memey_revolver");
            gun.gameObject.AddComponent<MemeRevolver>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("You're just asking for it.");
            gun.SetLongDescription("This is like, really bad.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "revolver_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 18);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(486) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 3.5f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 2;
            gun.DefaultModule.preventFiringDuringCharge = true;
            gun.SetBaseMaxAmmo(100);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "Memey Revolver";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.chargeProjectiles[0].Projectile);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            gun.barrelOffset.localPosition += new Vector3(0f, 0.5f, 0f);
            projectile.transform.parent = gun.barrelOffset;
            projectile.AdditionalScaleMultiplier = 2.5f;
            projectile.baseData.damage = 0f;
            projectile.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
            gun.DefaultModule.chargeProjectiles[0].VfxPool.type = VFXPoolType.None;
            projectile.baseData.force = 350f;
            gun.CanBeDropped = false;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            gun.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);

            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController x = this.gun.CurrentOwner as PlayerController;
            projectile.AdditionalScaleMultiplier = 0.25f;
            projectile.baseData.damage *= 0.05f;
            projectile.baseData.speed = 110f;
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, x.CurrentGun.CurrentAngle) * -Vector2.right);
            x.knockbackDoer.ApplyKnockback(dir, 170f);
            bool flag = x == null;
            bool flag2 = flag;
            if (flag2)
            {
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            }
            this.gun.DefaultModule.ammoCost = 1;
            Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[180]).DefaultModule.projectiles[0];
            ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
            BounceProjModifier bouncer = projectile.gameObject.AddComponent<BounceProjModifier>();
            ExplosiveModifier exploder = projectile.gameObject.GetComponent<ExplosiveModifier>();
            PierceProjModifier piercer = projectile.gameObject.AddComponent<PierceProjModifier>();
            piercer.penetration = 15;
            bouncer.bounceTrackRadius = 1000;
            bouncer.bouncesTrackEnemies = true;
            bouncer.numberOfBounces = 15;
            exploder.explosionData = boomer.explosionData;
            bouncer.ExplodeOnEnemyBounce = true;
            exploder.doExplosion = true;
            exploder.doDistortionWave = true;
            projectile.OnHitEnemy += this.Kaboom;
            projectile.OnDestruction += Kaboom2;
            x.ForceBlank(1.5f, 0.5f, false, true, projectile.sprite.WorldCenter, false, 300f);
            AkSoundEngine.PostEvent("Play_WPN_LowerCaseR_Angry_Loser_01", base.gameObject);


        }

        private void Kaboom2(Projectile obj)
        {
            if (obj != null)
            {
                PlayerController x = this.gun.CurrentOwner as PlayerController;
                Vector2 dir = x.sprite.WorldCenter - obj.sprite.WorldCenter;
                x.knockbackDoer.ApplyKnockback(-dir, 270f);
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[180]).DefaultModule.projectiles[0];
                ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
                ExplosionData boom = boomer.explosionData;
                boomer.doDistortionWave = true;
                boomer.explosionData.damage = 0f;
                Exploder.Explode(obj.sprite.WorldCenter, boom, Vector2.zero, null, false, CoreDamageTypes.None, false);
                x.ForceBlank(5f, 0.5f, false, true, obj.sprite.WorldCenter, false, 300f);
                AkSoundEngine.PostEvent("Play_WPN_LowerCaseR_Angry_Loser_01", gameObject);
            }
        }

        private void Kaboom(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
           if(arg1 != null)
            {
                PlayerController x = this.gun.CurrentOwner as PlayerController;
                Vector2 dir = x.sprite.WorldCenter - arg1.sprite.WorldCenter;
                x.knockbackDoer.ApplyKnockback(-dir, 170f);
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[180]).DefaultModule.projectiles[0];
                ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
                ExplosionData boom = boomer.explosionData;
                boomer.explosionData.doForce = true;
                boomer.doDistortionWave = true;
                Exploder.Explode(arg1.sprite.WorldCenter, boom, Vector2.zero, null, false, CoreDamageTypes.None, false);
                x.ForceBlank(5f, 0.5f, false, true, arg1.sprite.WorldCenter, false, 300f);
                AkSoundEngine.PostEvent("Play_WPN_LowerCaseR_Angry_Loser_01", gameObject);
            }
        }

       

        

        // Token: 0x0600009D RID: 157 RVA: 0x000079F8 File Offset: 0x00005BF8

        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
        protected void Update()
        {
            if (gun.CurrentOwner)
            {

             this.gun.PreventNormalFireAudio = true;
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
                AkSoundEngine.PostEvent("Play_WPN_LowerCaseR_Angry_Dork_01", base.gameObject);
            }
        }



        // Token: 0x040077B6 RID: 30646
        public float ActivationChance;

        // Token: 0x040077B8 RID: 30648
        public bool TriggersRadialBulletBurst;

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("Play_BOSS_tank_shot_01", false)]
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
        private float LaserTrue;

        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}