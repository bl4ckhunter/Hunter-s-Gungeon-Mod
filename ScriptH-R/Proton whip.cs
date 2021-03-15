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
using UnityEngine;

namespace Mod
{

    public class ProtonWhip : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Plasma Whip", "plasmawhip");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:plasma_whip", "ror:plasma_whip");
            gun.gameObject.AddComponent<ProtonWhip>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Gungeon Space Program");
            gun.SetLongDescription("What better way to get rid of an obstacle than sticking a rocket to it and watch it fly off and explode in the distance?");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "plasmawhip_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(177) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.0005f; 
            gun.DefaultModule.cooldownTime = 0.15f;
            gun.DefaultModule.numberOfShotsInClip = 600;
            gun.SetBaseMaxAmmo(600);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.D;
            gun.CanBeDropped = false;
            gun.encounterTrackable.EncounterGuid = "WhipWhipHurrah";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed *= 1.30f;
            projectile.baseData.damage = 0f;
            gun.muzzleFlashEffects = null;
            projectile.AppliesKnockbackToPlayer = false;
            projectile.baseData.range = 100f;
            gun.barrelOffset.localPosition += new Vector3(1.14375f,-0.15f, 0f);
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
            base.PostProcessProjectile(projectile);
            Destroy(projectile.gameObject);
            if(gun.IsFiring && !this.active)
            { this.active = true;
                PlayerController man = base.gun.CurrentOwner as PlayerController;
                Projectile component1 = ((Gun)ETGMod.Databases.Items[179]).DefaultModule.projectiles[0];
                GameManager.Instance.StartCoroutine(HandleFireShortBeam(component1, man));
            }

        }


        public IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source)
        {
            float elapsed = 0f;
            BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, base.gun.CurrentAngle, base.gun.barrelOffset.position);
            PlayerController x = this.gun.CurrentOwner as PlayerController;
            this.beamn = beam;
            yield return null;
            while (gun.IsFiring && x.CurrentGun == base.gun)
            {
                elapsed += BraveTime.DeltaTime;
                this.ContinueFiringBeam(beam, source, base.gun.CurrentAngle, base.gun.barrelOffset.position , projectileToSpawn);
                yield return null;
            }
            this.CeaseBeam(beam);
            yield break;
        }
        private void Yank(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            try
            {
                if (!arg2.healthHaver.IsBoss && arg2 != null)
                {
                    arg2.behaviorSpeculator.Stun(0.01f, true);
                    arg2.knockbackDoer.weight = 10;
                    PlayerController player = base.gun.CurrentOwner as PlayerController;
                    if (BraveInput.GetInstanceForPlayer(player.PlayerIDX).IsKeyboardAndMouse(false))
                    {
                        this.aimpoint = player.unadjustedAimPoint.XY();
                    }
                    else
                    {
                        BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(player.PlayerIDX);
                        Vector2 a2 = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
                        a2 += instanceForPlayer.ActiveActions.Aim.Vector * 8f * BraveTime.DeltaTime;
                        this.m_currentAngle = BraveMathCollege.Atan2Degrees(a2 - player.CenterPosition);
                        this.m_currentDistance = Vector2.Distance(a2, player.CenterPosition);
                        this.m_currentDistance = Mathf.Min(this.m_currentDistance, 15);
                        this.aimpoint = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
                    }
                    Vector2 Target = aimpoint - arg2.UnitCenter;
                    arg2.knockbackDoer.ApplyKnockback(Target, 0.1f * Vector2.Distance(aimpoint, arg2.UnitCenter));
                    if (!arg2.gameObject.GetComponent<Wallbang>())
                    {
                        arg2.CollideWithOthers = true;
                        arg2.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyCollider));
                        arg2.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox));
                        arg2.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider));
                        arg2.gameObject.AddComponent<Wallbang>();
                        arg2.OnCollision += Bang;
                    }
                }
            }
            catch{ }

        }

        private void Bang(CollisionData tileCollision)
        {   
            if(gun.IsFiring && !this.onCooldowncollide)
            { Vector2 point = tileCollision.MyRigidbody.UnitCenter;
                float speed = tileCollision.MyRigidbody.Velocity.magnitude;
                this.Gun(point, speed);
                this.onCooldowncollide = true;
                GameManager.Instance.StartCoroutine(Cooldown());
                }
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(0.4f);
            this.onCooldowncollide = false;
            yield break;
        }

        private void Gun(Vector2 position, float speed)
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].Projectile;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, position, Quaternion.Euler(0f, 0f, base.gun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag8 = component != null;
            if (flag8)
            {
                component.AdditionalScaleMultiplier = 2f;
                component.baseData.damage = 5f * speed * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                component.baseData.force = 0f;
                component.Owner = man;
                component.Shooter = man.specRigidbody;
                if (man.HasPickupID(567) || man.HasPickupID(41) || man.HasPickupID(815))
                { component.gameObject.AddComponent<ExplosiveProjectile>(); }
            }
        }
        private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
        {
            Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
            Projectile component = gameObject.GetComponent<Projectile>();
            component.Owner = source;
            BeamController component2 = gameObject.GetComponent<BeamController>();
            component2.Owner = source;
            component2.chargeDelay = 0f;
            component.baseData.speed = 30f;
            component.ignoreDamageCaps = true;
            component2.HitsPlayers = false;
            component2.HitsEnemies = true;
            Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
            component2.Direction = v;
            component2.Origin = vector;
            component2.ChanceBasedHomingRadius = 8f;
            component2.ChanceBasedHomingAngularVelocity = 900f;
            component.AdjustPlayerProjectileTint(Color.cyan, 5, 0f);
            Projectile sourceProjectile = component; 
            sourceProjectile.OnHitEnemy += Yank;
            sourceProjectile.AppliesPoison = false;
            sourceProjectile.AppliesPoison = false;
            sourceProjectile.baseData.damage = 0f;
            return component2;
        }
        private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint, Projectile projectile)
        {
            Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
            beam.AdjustPlayerBeamTint(Color.cyan, 5, 0f);
            beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
            projectile.AdditionalScaleMultiplier = 4f;
            beam.ChanceBasedHomingRadius = 8f;
            beam.ChanceBasedHomingAngularVelocity = 900f;
            beam.Origin = vector;
            beam.LateUpdatePosition(vector);
        }

        // Token: 0x0600888F RID: 34959 RVA: 0x002E0188 File Offset: 0x002DE388
        internal void CeaseBeam(BeamController beam)
        {
            beam.CeaseAttack();
            this.active = false;
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
            AkSoundEngine.PostEvent("Play_ENM_flame_veil_01", gameObject);
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
                    this.unused = false;
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
        private PlayerController m_owner;
        private Projectile m_projectile;
        private Vector2 crosshair;
        private static FireVolleyOnRollItem m_bomb;
        private bool unused;
        private bool active;
        private BeamController beamn;
        private Vector2 aimpoint;
        private float m_currentAngle;
        private float m_currentDistance;
        private bool onCooldowncollide;
        private bool hitenemy;


        // Token: 0x04007661 RID: 30305

        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }

    internal class Wallbang : MonoBehaviour
    {
    }
}