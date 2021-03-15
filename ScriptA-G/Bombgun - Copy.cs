using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClassLibrary1.Scripts;
using Dungeonator;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Mod
{

    public class GravGun : GunBehaviour
    {


        // Token: 0x060000A0 RID: 160 RVA: 0x000063DC File Offset: 0x000045DC
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("potato", "dark");
            Game.Items.Rename("outdated_gun_mods:potato", "ror:potato");
            gun.gameObject.AddComponent<GravGun>();
            gun.SetShortDescription("GravGun");
            gun.SetLongDescription("a gun only for testing");
            GunExt.SetupSprite(gun, null, "dark_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);


            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(57) as Gun, true, false);



            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.InfiniteAmmo = true;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.SetBaseMaxAmmo(0);
            gun.gunHandedness = GunHandedness.OneHanded;

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun gun2 = PickupObjectDatabase.GetById(145) as Gun;


            //gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;

            //gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            //gun.DefaultModule.customAmmoType = 


            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;

            //gun.DefaultModule.customAmmoType = "locrtfsf_idle_001";
            Gun gun3 = PickupObjectDatabase.GetById(504) as Gun;
            //gun.DefaultModule.customAmmoType = gun3.CustomAmmoType;
            Guid.NewGuid().ToString();
            //gun.encounterTrackable.EncounterGuid = "why wont you work please work im going mad";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 2f;
            projectile.baseData.speed *= 2f;
            projectile.baseData.range *= 2f;
            //projectile.baseData.speed *= 0.7f;
            ///	projectile.SetProjectileSpriteRight("locrtfsf_projectile_001", 7, 7, null, null);

            gunId = gun.PickupObjectId;

        }


        //public static GunBehaviour lGun = gun;





        // Token: 0x060000A1 RID: 161 RVA: 0x00006510 File Offset: 0x00004710
        protected void Update()
        {
            

            if (gun.CurrentOwner)
            {
                this.Jumpmethod();
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


        // Token: 0x060000A3 RID: 163 RVA: 0x00006629 File Offset: 0x00004829
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            if (gun.DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam)
            {
                AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
            }

        }

        // Token: 0x060000A4 RID: 164 RVA: 0x00006644 File Offset: 0x00004844
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
                //gun.Volley.projectiles.Clear();
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
            }
        }



        public void Jumpmethod()
        {
            PlayerController owner = base.gun.CurrentOwner as PlayerController;
            if (base.gun == owner.CurrentGun)
            {
                if (!this.canYump)
                {
                    owner.OnReloadPressed += this.Yump;
                    this.canYump = true;
                }

                Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, 0) * -Vector2.up);
                owner.knockbackDoer.ApplyKnockback(dir, 2.0f);
                List<AIActor> activeEnemies = owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                    foreach (AIActor ai in activeEnemies)
                    {

                        bool flag2 = ai.healthHaver.IsBoss;
                        if (!flag2 && ai != null)
                        {
                            Vector2 dir1 = (Vector2)(Quaternion.Euler(0, 0, 0) * Vector2.down);
                            ai.knockbackDoer.ApplyKnockback(dir1, 10f);
                        }
                    }
            }



        }

        private void Yump(PlayerController owner, Gun gun)
        {
            if (!this.isjumping && gun == owner.CurrentGun)
            {
                this.isjumping = true;
                GameManager.Instance.StartCoroutine(Jump(owner));
            }
        }

        private IEnumerator Jump(PlayerController owner)
        {
            owner.healthHaver.IsVulnerable = false;
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, 0) * Vector2.up);
            owner.knockbackDoer.ApplyKnockback(dir, 230f);
            yield return new WaitForSeconds(0.7f);
            owner.healthHaver.IsVulnerable = true;
            yield return new WaitForSeconds(1.0f);
            this.isjumping = false;
            yield break;
        }

        Gun randomGunId;

        public static int gunId;

        public static bool useAlt = true;

        private bool HasReloaded;
        private bool isjumping;
        private bool canYump;
        private StatModifier[] passiveStatModifiers;




        //		Gun gunAlt;

        internal class LostGunProjectile : MonoBehaviour
        {

            public void Start()
            {
                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                this.projectile.sprite.spriteId = this.projectile.sprite.GetSpriteIdByName("locrtfsf_projectile_001");

            }

            private Projectile projectile;

            private PlayerController player;

        }
    }
}


