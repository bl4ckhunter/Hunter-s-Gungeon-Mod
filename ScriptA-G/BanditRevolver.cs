using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public class BanditRevolver : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Bandit's Revolver", "bandit");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:bandit's_revolver", "ror:bandit's_revolver");
            gun.gameObject.AddComponent<BanditRevolver>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Dust And Smoke");
            gun.SetLongDescription("The signature weapon of a dastardly bandit stranded a forsaken planet, active reload hides the player from enemies and boosts all damage from guns for 3s.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "bandit_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 20);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(275) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 3.5f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.SetBaseMaxAmmo(200);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "BanditRifle1a";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            if (projectile.gameObject.GetComponent<BounceProjModifier>())
            {
                Destroy(projectile.gameObject.GetComponent<BounceProjModifier>());
            }
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 0.5f;
            projectile.baseData.speed *= 2f;
            gun.LocalActiveReload = true;
            gun.barrelOffset.localPosition = new Vector3(1.5625f, 1.125f, 0f);
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
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile

        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.AdjustPlayerProjectileTint(new Color(0.15f, 0.9f, 0f), 1);
            base.PostProcessProjectile(projectile);
            
            projectile.OnDestruction += Burn;

        }

        private void Burn(Projectile obj)
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            if (man.PlayerHasActiveSynergy("Smoldering Heart"))
            {
                AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
                GoopDefinition goopDefinition1 = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
                GoopDefinition goopDefinition = UnityEngine.Object.Instantiate<GoopDefinition>(goopDefinition1);
                goopDefinition.baseColor32 = new Color32(0, 255, 255, 255);
                goopDefinition.fireColor32 = new Color32(0, 255, 255, 255);
                goopDefinition.UsesGreenFire = true;
                DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
                goopManagerForGoopType.TimedAddGoopCircle(obj.transform.position, 3f, 0.1f, false);
            }
        }

        private void StealthEffect(PlayerController player)
        {
                used = true;
                isStealthed = true;
                activereloadsuccess = false;
                PlayerController owner = player;
                this.BreakStealth(owner);
                owner.ChangeSpecialShaderFlag(1, 1f);
                owner.healthHaver.OnDamaged += new HealthHaver.OnDamagedEvent(this.OnDamaged);
                owner.SetIsStealthed(true, "bandit");
                GameManager.Instance.StartCoroutine(this.Unstealthy());
            
        }
        private IEnumerator Unstealthy()
        {
            
            PlayerController player = base.gun.CurrentOwner as PlayerController;
            player.PostProcessProjectile += Greener;
            gun.CanBeDropped = false;
            yield return new WaitForSeconds(3f);
            yield return null;
            player.PostProcessProjectile -= Greener;
            this.BreakStealth(player);
            gun.CanBeDropped = true;
            yield break;
        }

        private void Greener(Projectile arg1, float arg2)
        {
            Projectile projectile = arg1;
            projectile.baseData.damage *= 2f;
            projectile.AdjustPlayerProjectileTint(new Color(0f, 270f, 0f), 1);
            projectile.AdditionalScaleMultiplier = 1.4f;
        }

        private void BreakStealth(PlayerController player)
        {
            player.ChangeSpecialShaderFlag(1, 0f);
            player.SetIsStealthed(false, "bandit");
            player.healthHaver.OnDamaged -= new HealthHaver.OnDamagedEvent(this.OnDamaged);
            isStealthed = false;
        }
        private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            PlayerController owner = player;
            this.BreakStealth(owner);
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
               

                activereloadsuccess = RoRItems.activereloadsuccess;
                if (activereloadsuccess && !used && !isStealthed && !gun.IsReloading)
                { StealthEffect(man); }


            }


        }





        public override void OnDropped()
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            this.BreakStealth(man);
            base.OnDropped();
        }

     

        // Token: 0x06000264 RID: 612 RVA: 0x0001B298 File Offset: 0x00019498

        // Token: 0x06000265 RID: 613 RVA: 0x0001B2A4 File Offset: 0x000194A4

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                PlayerController man = base.gun.CurrentOwner as PlayerController;
                this.BreakStealth(man);
                HasReloaded = false;
                used = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_magnum_reload_01", base.gameObject);
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
        private bool isStealthed;
        private bool activereloadsuccess;
        private bool used;


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