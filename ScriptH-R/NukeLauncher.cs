using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Mod
{

    public class SmallBoiNuke : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Small Boi", "nuke");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:small_boi", "ror:small_boi");
            gun.gameObject.AddComponent<SmallBoiNuke>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Big Boom");
            gun.SetLongDescription("Nuclear Pinheads have long been shunned by their gundead bretheren for their propension for friendly fire, so nowadays they have grudgingly decided to side with gungeoneers, it's a shaky alliance at best tho. They have a strict code of honor and firing a new one while one of them is already in the field will cause the older one to explode in outrage\n\n" + " WARNING - these are actual LIVE pinheads, as in, they're actually alive, and while they easily bounce over enemies they WILL fall into pits and you WILL be caught in the backblast if you shoot while in blast range of a live one or another explosion. No gun-dropping this either, it's a nuclear weapon for the love of everything! What is wrong with you? ");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "nuke_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom("klobb", true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 3f;
            gun.DefaultModule.cooldownTime = 3f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.SetBaseMaxAmmo(40);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "Small Boi";
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
            projectile.baseData.damage *= 1.10f;
            projectile.baseData.speed *= 1f;
            projectile.transform.parent = gun.barrelOffset;
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
            PlayerController man = this.gun.CurrentOwner as PlayerController;
            this.gun.ClipShotsRemaining -= 1;
            UnityEngine.Object.Destroy(projectile.gameObject);
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, man.CurrentGun.CurrentAngle) * -Vector2.right);
            man.knockbackDoer.ApplyKnockback(dir, 40f);
            this.KillBombBoi();
            base.PostProcessProjectile(projectile);
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {
            this.Owner = gun.CurrentOwner;
            this.Spawn();
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }
        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
        protected void Update()
        {
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
            }
          
        }

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

        private void KillBombBoi()
        {
            if (this.Bomb != null)
            { this.Bomb.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.Void, DamageCategory.Normal, true, null, false); }
        }

        private void Spawn()
        {
            PlayerController man = this.gun.CurrentOwner as PlayerController;
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("4d37ce3d666b4ddda8039929225b7ede");
            AIActor aiactor = AIActor.Instantiate(orLoadByGuid, man.CurrentGun.sprite.WorldCenter - new Vector2(1f,1f), Quaternion.identity);
            aiactor.HasBeenEngaged = true;
            aiactor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
            this.Bomb = aiactor;
            aiactor.IsHarmlessEnemy = true;
            aiactor.IgnoreForRoomClear = true;
            this.aiactor = aiactor;
            aiactor.healthHaver.OnDeath += this.Nukem;
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, man.CurrentGun.CurrentAngle) * Vector2.right);
            aiactor.knockbackDoer.ApplyKnockback(dir, 200f);
        }

        private void Nukem(Vector2 target2)
        {
            PlayerController owner = base.gun.CurrentOwner as PlayerController;
            Vector2 worldCenter = this.Bomb.sprite.WorldCenter;
            Vector3 target = this.Bomb.sprite.WorldCenter;
            Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[481]).DefaultModule.chargeProjectiles[0].Projectile;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, this.Bomb.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (owner.CurrentGun == null) ? 0f : owner.CurrentGun.CurrentAngle + 180f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag4 = component != null;
            if (flag4)
            {
                component.baseData.damage = 20f * owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                component.baseData.speed = 0f;
                component.AdditionalScaleMultiplier = 2.5f;
                component.SetOwnerSafe(owner, "Player");
                component.Shooter = owner.specRigidbody;
                component.Owner = owner;
            }
            this.Boom(target);
            owner.ForceBlank(3f, 0.5f, false, true, worldCenter, false, -1f);
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition1 = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            GoopDefinition goopDefinition = UnityEngine.Object.Instantiate<GoopDefinition>(goopDefinition1);
            goopDefinition.baseColor32 = new Color32(0, 255, 255, 255);
            goopDefinition.fireColor32 = new Color32(0, 255, 255, 255);
            goopDefinition.UsesGreenFire = true;
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(this.Bomb.sprite.WorldCenter, 5f, 0.1f, false);
            this.Nuke = assetBundle.LoadAsset<GameObject>("assets/data/vfx prefabs/impact vfx/vfx_explosion_nuke.prefab");
            GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(this.Nuke);
            gameObject1.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(this.Bomb.specRigidbody.UnitCenter, tk2dBaseSprite.Anchor.LowerCenter);
            gameObject1.transform.position = gameObject.transform.position.Quantize(0.0625f);
            gameObject1.GetComponent<tk2dBaseSprite>().UpdateZDepth();
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
            damageRadius = 4.5f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 30f,
            doExplosionRing = true,
            doDestroyProjectiles = true,
            doForce = true,
            debrisForce = 30f,
            preventPlayerForce = true,
            explosionDelay = 0f,
            usesComprehensiveDelay = false,
            doScreenShake = false,
            playDefaultSFX = true
        };

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

        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}