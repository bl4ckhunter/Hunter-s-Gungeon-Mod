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

    public class Thorn : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Megathorn", "thorn");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:megathorn", "ror:megathorn");
            gun.gameObject.AddComponent<Thorn>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("The Green Tempest");
            gun.SetLongDescription("In Gungeon, trees cut down you.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "thorn_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 18);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(328) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 3.5f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.DefaultModule.preventFiringDuringCharge = true;
            gun.SetBaseMaxAmmo(40);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "Megathorn";
            ProjectileModule.ChargeProjectile chargeProjectile = new ProjectileModule.ChargeProjectile();
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
                {
                    chargeProjectile
                };
            gun.DefaultModule.chargeProjectiles[0].ChargeTime = 0.8f;
            gun.DefaultModule.chargeProjectiles[0].UsedProperties = ((Gun)global::ETGMod.Databases.Items[328]).DefaultModule.chargeProjectiles[0].UsedProperties;
            gun.DefaultModule.chargeProjectiles[0].VfxPool = ((Gun)global::ETGMod.Databases.Items[328]).DefaultModule.chargeProjectiles[0].VfxPool;
            gun.DefaultModule.chargeProjectiles[0].VfxPool.type = ((Gun)global::ETGMod.Databases.Items[328]).DefaultModule.chargeProjectiles[0].VfxPool.type;
            ProjectileModule.ChargeProjectile module = gun.DefaultModule.chargeProjectiles[0];
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(((Gun)global::ETGMod.Databases.Items[328]).DefaultModule.chargeProjectiles[0].Projectile);
            module.Projectile = projectile;
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.gameObject.AddComponent<MegathornProjectile>();
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage = 0f;
            projectile.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
            projectile.baseData.force = 350f;
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
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, x.CurrentGun.CurrentAngle) * -Vector2.right);
            x.knockbackDoer.ApplyKnockback(dir, 70f);
            gun.ClipShotsRemaining = 0;

        }
        private void Poisonpoop(Projectile arg1)
        {
            {
                AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
                GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition).TimedAddGoopCircle(arg1.sprite.WorldBottomCenter, 4f, 1f, false);
            }
        }
        private void Clusterfuck(Projectile obj)
        {
            this.AtgSalvo(obj);
        }

        private void AtgSalvo(Projectile sourceprojectile)
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            Projectile projectile = ((Gun)ETGMod.Databases.Items[593]).DefaultModule.projectiles[0];

            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, sourceprojectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (this.gun == null) ? 0f : this.gun.CurrentAngle + 0f), true);
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, sourceprojectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (this.gun == null) ? 0f : this.gun.CurrentAngle - 60f), true);
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, sourceprojectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (this.gun == null) ? 0f : this.gun.CurrentAngle + 60f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component2 = gameObject2.GetComponent<Projectile>();
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = man;
                HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
                component.AdjustPlayerProjectileTint(new Color(1f, 1f, 1f), 5, 0f);
                homingModifier.HomingRadius = 100f;
                homingModifier.AngularVelocity = 100f;
                component.Shooter = this.gun.CurrentOwner.specRigidbody;
                PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 2;
                BounceProjModifier bouncer = component.gameObject.AddComponent<BounceProjModifier>();
                bouncer.numberOfBounces = 2;
                bouncer.ExplodeOnEnemyBounce = true;
                component.baseData.speed = 20f;
                component.baseData.force = 0f;
                component.SetOwnerSafe(man, "Player");
                component.baseData.damage = 10f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                component.OnDestruction += this.Poisonpoop;
                component.OnDestruction += this.Flak;
            }
            bool flag2 = component2 != null;
            if (flag2)
            {
                component2.Owner = man;
                HomingModifier homingModifier2 = component2.gameObject.AddComponent<HomingModifier>();
                component2.AdjustPlayerProjectileTint(new Color(1f, 1f, 1f), 5, 0f);
                homingModifier2.HomingRadius = 100f;
                homingModifier2.AngularVelocity = 100f;
                component2.Shooter = man.specRigidbody;
                PierceProjModifier piercer = component2.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 2;
                BounceProjModifier bouncer = component2.gameObject.AddComponent<BounceProjModifier>();
                bouncer.numberOfBounces = 2;
                bouncer.ExplodeOnEnemyBounce = true;
                component.baseData.force = 0f;
                component2.baseData.speed = 20f;
                component2.SetOwnerSafe(man, "Player");
                component2.baseData.damage = 10f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
               component2.OnDestruction += this.Poisonpoop;
                component2.OnDestruction += this.Flak;
            }
            bool flag3 = component3 != null;
            if (flag3)
            {
                component3.Owner = this.gun.CurrentOwner;
                HomingModifier homingModifier3 = component3.gameObject.AddComponent<HomingModifier>();
                component3.AdjustPlayerProjectileTint(new Color(1f, 1f, 1f), 5, 0f);
                homingModifier3.HomingRadius = 100f;
                homingModifier3.AngularVelocity = 100f;
                PierceProjModifier piercer = component3.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 2;
                BounceProjModifier bouncer = component3.gameObject.AddComponent<BounceProjModifier>();
                bouncer.numberOfBounces = 2;
                bouncer.ExplodeOnEnemyBounce = true;
                component.baseData.force = 0f;
                bouncer.ExplodeOnEnemyBounce = true;
                component3.Shooter = man.specRigidbody;
                component3.SetOwnerSafe(this.gun.CurrentOwner, "Player");
                component3.baseData.speed = 20f;
                component3.baseData.damage = 10f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                component3.OnDestruction += this.Poisonpoop;
                component3.OnDestruction += this.Flak;
            }
        }

        private void Flak(Projectile obj)
        {
            
            
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            this.lastDamage = 10f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
            Projectile projectile = ((Gun)ETGMod.Databases.Items[16]).DefaultModule.projectiles[0];
            projectile.AdjustPlayerProjectileTint(new Color(1f, 0f, 1f), 5, 0f);
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 0f), true);
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 180f), true);
            GameObject gameObject4 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 270f), true);
            GameObject gameObject5 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 45f), true);
            GameObject gameObject6 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 135f), true);
            GameObject gameObject7 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 225f), true);
            GameObject gameObject8 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 315f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            Projectile component4 = gameObject4.GetComponent<Projectile>();
            Projectile component5 = gameObject5.GetComponent<Projectile>();
            Projectile component6 = gameObject6.GetComponent<Projectile>();
            Projectile component7 = gameObject7.GetComponent<Projectile>();
            Projectile component2 = gameObject8.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = man;
                component.Shooter = man.specRigidbody;
                component.AdjustPlayerProjectileTint(new Color(1f, 0f, 1f), 5, 0f);
                component.baseData.speed = 10f;
                component.SetOwnerSafe(this.gun.CurrentOwner, "Player");
                component.ignoreDamageCaps = false;
            }
            bool flag2 = component != null;
            if (flag2)
            {
                component2.Owner = man;
                component2.Shooter = man.specRigidbody;
                component2.AdjustPlayerProjectileTint(new Color(1f, 0f, 1f), 5, 0f);
                component2.baseData.speed = 10f;
                component2.SetOwnerSafe(this.gun.CurrentOwner, "Player");
                component2.ignoreDamageCaps = false;
            }
            bool flag3 = component != null;
            if (flag3)
            {
                component3.Owner = man;
                component3.Shooter = man.specRigidbody;
                component3.AdjustPlayerProjectileTint(new Color(1f, 0f, 1f), 5, 0f);
                component3.SetOwnerSafe(this.gun.CurrentOwner, "Player");
                component3.ignoreDamageCaps = false;
            }
            bool flag4 = component != null;
            if (flag4)
            {
                component4.Owner = man;
                component4.Shooter = man.specRigidbody;
                component4.AdjustPlayerProjectileTint(new Color(1f, 0f, 1f), 5, 0f);
                component4.baseData.speed = 10f;
                component4.SetOwnerSafe(this.gun.CurrentOwner, "Player");
                component4.ignoreDamageCaps = false;
            }
            bool flag5 = component != null;
            if (flag5)
            {
                component5.Owner = man;
                component5.Shooter = man.specRigidbody;
                component5.AdjustPlayerProjectileTint(new Color(1f, 0f, 1f), 5, 0f);
                component5.baseData.speed = 10f;
                component5.SetOwnerSafe(this.gun.CurrentOwner, "Player");
                component5.ignoreDamageCaps = false;
            }
            bool flag6 = component != null;
            if (flag6)
            {
                component6.Owner = man;
                component6.AdjustPlayerProjectileTint(new Color(1f, 0f, 1f), 5, 0f);
                component6.Shooter = man.specRigidbody;
                component6.baseData.speed = 10f;
                component6.SetOwnerSafe(this.gun.CurrentOwner, "Player");
                component6.ignoreDamageCaps = false;
            }
            bool flag7 = component != null;
            if (flag7)
            {
                component7.Owner = man;
                component7.AdjustPlayerProjectileTint(new Color(1f, 0f, 1f), 5, 0f);
                component7.Shooter = man.specRigidbody;
                component7.baseData.speed = 10f;
                component7.SetOwnerSafe(this.gun.CurrentOwner, "Player");
                component7.ignoreDamageCaps = false;
            }
        }

        // Token: 0x0600009D RID: 157 RVA: 0x000079F8 File Offset: 0x00005BF8


        private void Explosion(Projectile projectile)
        {
            AkSoundEngine.PostEvent("Play_wpn_voidcannon_shot_01", gameObject);
        }

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
                if (player.PlayerHasActiveSynergy("Plant_Life"))
                {
                    PickupObject pickupObject = Gungeon.Game.Items["ror:gungeon_bloom"];
                    player.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
                }
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_BOSS_blobulord_burst_01", base.gameObject);
            }
        }



        // Token: 0x040077B6 RID: 30646
        public float ActivationChance;

        // Token: 0x040077B8 RID: 30648
        public bool TriggersRadialBulletBurst;

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("Play_BOSS_blobulord_burst_01", false)]
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