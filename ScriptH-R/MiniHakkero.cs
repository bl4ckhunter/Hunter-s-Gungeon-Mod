using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Mod
{

    public class Spark : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Master Spark", "mspark");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:master_spark", "ror:master_spark");
            gun.gameObject.AddComponent<Spark>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("It's All About Firepower");
            gun.SetLongDescription("We call it the Master Spark because no one wants to call it The Mini Hakkero. This ancient artifact was wielded by many great wizards and witches throughout centuries, and the most iconic user was a thieving witch (who I shall not name because curse her) who just used this great artifact like blasted cannon that could tear through any great foe or library with ease, hence the name it eventually became of Master Spark. Seems however said witch dropped it while flying around, and it ended up here." +
            "What on Earth, why would use such a grand artifact capable of nigh anything as a goddamn beam cannon, undignified?\n\n" +
            "-Patchouli");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "mspark_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(383) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.burstShotCount = 70;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 4f;
            gun.DefaultModule.angleVariance = 20f;
            gun.DefaultModule.cooldownTime = 0.025f;
            gun.DefaultModule.numberOfShotsInClip = 50;
            gun.SetBaseMaxAmmo(500);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.S;
            gun.encounterTrackable.EncounterGuid = "Master Spark";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
            gun.barrelOffset.localPosition += new Vector3(0f, 0.35f, 0f);
            gun.CanBeDropped = false;
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

        }

        

        public override void PostProcessProjectile(Projectile projectile)
        {
            GameManager.Instance.StartCoroutine(Continuousfire());
            base.PostProcessProjectile(projectile); 
            UnityEngine.Object.Destroy(projectile.gameObject);
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            Projectile component1 = ((Gun)ETGMod.Databases.Items[518]).DefaultModule.projectiles[0];

            if (!this.onCooldown)
            {
                this.onCooldown = true;
                GameManager.Instance.StartCoroutine(StartCooldown());
                GameManager.Instance.StartCoroutine(HandleFireShortBeam(component1, man, 5f));
                GameManager.Instance.StartCoroutine(Continuousfire());

            }

        }

        private IEnumerator Continuousfire()
        { 
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            while (base.gun.ClipShotsRemaining > 0 && man.CurrentGun == base.gun )
            {
                this.Fire2();
                this.Fire2();
                this.Fire2();
                yield return null; 
            }
            yield break;
        }



        private void Fire()
        {
            if (!this.offcooldown)
            {
                this.offcooldown = true;
                GameManager.Instance.StartCoroutine(StartCooldown1());
                PlayerController x = this.gun.CurrentOwner as PlayerController;
                Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[383]).DefaultModule.projectiles[0];
                float angle = base.gun.CurrentAngle + (UnityEngine.Random.Range(-20f, 20f));
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, x.CurrentGun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.gun.CurrentOwner.CurrentGun == null) ? 0f : angle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.baseData.damage = 3.5f * x.stats.GetStatValue(PlayerStats.StatType.Damage);
                    component.ignoreDamageCaps = true;
                    component.baseData.speed = 300f;
                    component.baseData.force = 0f;
                    component.SetOwnerSafe(x, "Player");
                    component.Shooter = x.specRigidbody;
                    component.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
                    component.AdditionalScaleMultiplier = 1f;
                }
            }
        }

        private void Fire2()
        {
            if (!this.offcooldown2)
            {
                this.offcooldown2 = true;
                GameManager.Instance.StartCoroutine(StartCooldown2());
                PlayerController x = this.gun.CurrentOwner as PlayerController;
                Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[383]).DefaultModule.projectiles[0];
                float angle = base.gun.CurrentAngle + (UnityEngine.Random.Range(-20f, 20f));
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, x.CurrentGun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.gun.CurrentOwner.CurrentGun == null) ? 0f : angle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.baseData.damage = 15f * x.stats.GetStatValue(PlayerStats.StatType.Damage);
                    component.baseData.speed = 300f;
                    component.baseData.force = 0f;
                    component.SetOwnerSafe(x, "Player");
                    component.Shooter = x.specRigidbody;
                    component.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
                    component.AdditionalScaleMultiplier = 1f;
                }
            }
        }

        private IEnumerator StartCooldown1()
        {
            yield return new WaitForSeconds(0.05f);
            this.offcooldown = false;
            yield break;
        }

        private IEnumerator StartCooldown2()
        {
            yield return new WaitForSeconds(0.05f);
            this.offcooldown2 = false;
            yield break;
        }

        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(5f);
            this.onCooldown = false;
            yield break;
        }

        public IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, float duration)
        {
            float elapsed = 0f;
            float elapsed2 = 0f;
            BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, base.gun.CurrentAngle, base.gun.sprite.WorldCenter);
            Projectile component = projectileToSpawn.GetComponent<Projectile>();
            component.AdditionalScaleMultiplier = 1.5f;
            beam.projectile.AdditionalScaleMultiplier = 1.5f;
            PlayerController x = this.gun.CurrentOwner as PlayerController;
            this.beamn = beam;
            yield return null;
            while (base.gun.ClipShotsRemaining > 0 && x.CurrentGun == base.gun)
            {
                elapsed += BraveTime.DeltaTime;
                elapsed2 += BraveTime.DeltaTime;
                this.ContinueFiringBeam(beam, source, base.gun.CurrentAngle, base.gun.sprite.WorldCenter, projectileToSpawn);
                if(elapsed2 > 0.06f )
                {
                    elapsed2 = 0f;
                    base.gun.GainAmmo(-1);
                    base.gun.ClipShotsRemaining -= 1;
                }
                yield return null;
            }
            this.CeaseBeam(beam);
            yield break;
        }
        private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
        {
            Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
            Projectile component = gameObject.GetComponent<Projectile>();
            component.Owner = source;
            component.AdditionalScaleMultiplier = 1.5f;
            component.baseData.speed = 25f;
            BeamController component2 = gameObject.GetComponent<BeamController>();
            component2.projectile.AdditionalScaleMultiplier = 1.5f;
            component2.Owner = source;
            component2.chargeDelay = 0f;
            component.ignoreDamageCaps = true;
            component2.HitsPlayers = false;
            component2.HitsEnemies = true;
            Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
            component2.Direction = v;
            component2.Origin = vector;
            return component2;
        }
        private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint, Projectile projectile)
        {
            Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
            beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
            projectile.AdditionalScaleMultiplier = 2f;
            beam.Origin = vector;
            beam.LateUpdatePosition(vector);
        }

        // Token: 0x0600888F RID: 34959 RVA: 0x002E0188 File Offset: 0x002DE388
        internal void CeaseBeam(BeamController beam)
        {
            beam.CeaseAttack();
        }



        public override void OnPostFired(PlayerController player, Gun gun)
        {
            this.Owner = gun.CurrentOwner;
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_raidenlaser_shot_01", gameObject);
        }
        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
        protected void Update()
        {
            if (gun.CurrentOwner)
            {
                PlayerController man = base.gun.CurrentOwner as PlayerController;
                
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
        private BeamController beamn;
        private PlayerController m_buffedTarget;
        private StatModifier m_temporaryModifier;
        private bool offcooldown;
        private bool offcooldown2;

        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}