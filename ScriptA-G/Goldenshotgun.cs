using System;
using System.Collections;
using System.Collections.Generic;
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
using SaveAPI;

namespace Mod
{

    public class GoldenShotgun : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("The Crucible", "basher");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:the_crucible", "ror:the_crucible");
            gun.gameObject.AddComponent<GoldenShotgun>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Gilded Blast");
            gun.SetLongDescription("The Golden King imbued his favourite shotgun with his own blood, in the hopes that afflicting the Spectre with his curse would allow him to slay it. \n It was all in vain.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "basher_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 20);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
            ProjectileModule projectileModule = gun.DefaultModule;
                projectileModule.ammoCost = 1;
                projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                projectileModule.cooldownTime = 0.6f;
                projectileModule.angleVariance = 0f;
                projectileModule.numberOfShotsInClip = 1;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                gun.DefaultModule.projectiles[0] = projectile;
                projectile.baseData.damage *= 0.35f;
                projectile.baseData.range = 0.001f;
            projectile.gameObject.AddComponent<GoldBlast>();
                bool flag = projectileModule != gun.DefaultModule;
                if (flag)
                {
                    projectileModule.ammoCost = 0;
                }
                gun.barrelOffset.localPosition = new Vector3(1, 1.15f, 0f);
                projectile.transform.parent = gun.barrelOffset;
            // Here we just take the default projectile module and change its settings how we want it to be
            gun.SetBaseMaxAmmo(150);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.C;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(736) as Gun).muzzleFlashEffects;
            gun.encounterTrackable.EncounterGuid = "Goldbasher";
            Shader shader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
            Material material = new Material(Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass"));
            material.name = "HologramMaterial";
            Material material2 = material;
            material.SetTexture("_MainTex", gun.sprite.renderer.material.GetTexture("_MainTex"));
            material2.SetTexture("_MainTex", gun.sprite.renderer.sharedMaterial.GetTexture("_MainTex"));
            material.SetColor(Shader.PropertyToID("_OverrideColor"), new Color32(235, 208, 103, 255));
            gun.sprite.renderer.material.shader = shader;
            gun.sprite.renderer.material = material;
            gun.sprite.renderer.sharedMaterial = material2;
            gun.sprite.usesOverrideMaterial = true;
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.SetupUnlockOnCustomStat(CustomTrackedStats.MOBIUS_CHEST_ALLTIME_MONEY, 500f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            gun.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile

        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);


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
            AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", gameObject);
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
                    this.unused = false;
                }
            }

        }
        public override void OnDropped()
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            this.GaveKrit = false;

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
                AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);

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


        // Token: 0x04007661 RID: 30305

        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }

    internal class GoldBlast : MonoBehaviour
    {
        public void Start()
        {


            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            Projectile projectile = this.projectile;
            PlayerController man = projectile.Owner as PlayerController;
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag4 = component != null;
            PickupObject bobject = UnityEngine.Object.Instantiate<PickupObject>(PickupObjectDatabase.GetById(278));
            GameActorFreezeEffect Freeze = (bobject.GetComponent<BulletStatusEffectItem>().FreezeModifierEffect);
            Engolden = Freeze;
            Engolden.AppliesTint = false;
            Engolden.duration = 100f;
            Engolden.crystalNum = 0;
            component.OnDestruction += BlastGold;
        }

        private void BlastGold(Projectile obj)
        {
            BuildGoop();
            SlashDoer.DoSwordSlash(obj.transform.position, player.CurrentGun.CurrentAngle, player, -6f, SlashDoer.ProjInteractMode.IGNORE, 30f, 2f, null, null, 1, 1, 6f, 25f, false);
            goopManagerForGoopType.TimedAddGoopArc(obj.specRigidbody.UnitCenter, 6f,55f, new Vector2((float)Math.Cos(player.CurrentGun.CurrentAngle*(Math.PI / 180)), (float)Math.Sin(player.CurrentGun.CurrentAngle * (Math.PI / 180))),0.1f);
        }
        private void BuildGoop()
        {
            goopDefinition = ScriptableObject.CreateInstance<GoopDefinition>();
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition1 = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/oil goop.asset");
            goopDefinition.CanBeIgnited = false;
            goopDefinition.damagesEnemies = false;
            goopDefinition.damagesPlayers = false;
            goopDefinition.baseColor32 = new Color32(235, 208, 103, 255);
            goopDefinition.CanBeFrozen = false;
            goopDefinition.usesAcidAudio = true;
            goopDefinition.isOily = true;
            goopDefinition.usesLifespan = true;
            goopDefinition.lifespan = 5f;
            goopDefinition.usesOverrideOpaqueness = true;
            goopDefinition.overrideOpaqueness = 3.4f;
            goopDefinition.CanBeElectrified = true;
            goopDefinition.goopTexture = goopDefinition1.goopTexture;
            goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            RoRItems.OnGoopTouched += PopStars;
        }

        private void PopStars(DeadlyDeadlyGoopManager arg1, GameActor arg2, IntVector2 arg3)
        {
            if (arg1 == goopManagerForGoopType)
            {
                if (arg2 is AIActor)
                {
                    Shader shader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
                    Material material = new Material(Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass"));
                    material.name = "HologramMaterial";
                    Material material2 = material;
                    material.SetTexture("_MainTex", arg2.sprite.renderer.material.GetTexture("_MainTex"));
                    material2.SetTexture("_MainTex", arg2.sprite.renderer.sharedMaterial.GetTexture("_MainTex"));
                    material.SetColor(Shader.PropertyToID("_OverrideColor"), new Color32(235, 208, 103, 255));
                    material2.SetColor(Shader.PropertyToID("_OverrideColor"), new Color32(235, 208, 103, 255));
                    arg2.sprite.renderer.material.shader = shader;
                    arg2.sprite.renderer.material = material;
                    arg2.sprite.renderer.sharedMaterial = material2;
                    arg2.sprite.usesOverrideMaterial = true;
                    arg2.healthHaver.flashesOnDamage = false;
                    Engolden.AppliesTint = false;
                    Engolden.duration = 100f;
                    Engolden.crystalNum = 0;
                    arg2.ApplyEffect(Engolden, 0.1f, null);
                    if (!arg2.gameObject.GetComponent<SpawnCasings>())
                    { arg2.gameObject.AddComponent<SpawnCasings>(); }
                }
            }
        }

        private Projectile projectile;
        private PlayerController player;
        private GoopDefinition goopDefinition;
        private DeadlyDeadlyGoopManager goopManagerForGoopType;
        private GameActorFreezeEffect Engolden;
    }
    
    
}