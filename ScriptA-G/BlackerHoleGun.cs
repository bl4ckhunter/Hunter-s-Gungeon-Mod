using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Mod
{

    public class Bhole : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Darkest Hour", "heartless");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:darkest_hour", "ror:darkest_hour");
            gun.gameObject.AddComponent<Bhole>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Call Of The Void");
            gun.SetLongDescription("This sucks! - UNDERWHELMING IF DUCT TAPED TO OTHER GUNS, DUCT TAPING GUNS TO IT IS FINE.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "heartless_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(17) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.001f; 
            gun.DefaultModule.cooldownTime = 0.05f;
            gun.DefaultModule.numberOfShotsInClip = 600;
            gun.SetBaseMaxAmmo(600);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "Darkest Hour";
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
            projectile.baseData.damage = 0f;
            projectile.baseData.speed = 40f;
            projectile.baseData.range = 100f;
            projectile.baseData.force = -15f;
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
            this.FlamerSalvo();
            this.Suk();
            projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.AddStunEffect));

        }

        private void Suk()
        {
            PlayerController player = base.gun.CurrentOwner as PlayerController;
            player.CurrentRoom.ApplyActionToNearbyEnemies(player.CenterPosition, 3.5f, new Action<AIActor, float>(this.ProcessEnemy));
        }

       


    // Token: 0x060000CD RID: 205 RVA: 0x000078AC File Offset: 0x00005AAC
    private void AddStunEffect(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
    {
        bool flag = arg2 != null && arg2.healthHaver.IsAlive && !arg2.healthHaver.IsBoss;
        if (flag)
        {
            arg2.behaviorSpeculator.Stun(2f, true);

        }
    }

    private void FlamerSalvo()
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            this.FlamerTrue = 1f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            Projectile projectile = ((Gun)ETGMod.Databases.Items[99]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = man;
                component.AdjustPlayerProjectileTint(Color.black.WithAlpha(Color.black.a / 0.99f), 5, 0f);
                component.Shooter = man.specRigidbody;
                component.baseData.speed = 15f;
                component.baseData.range = 3f;
                BounceProjModifier bouncer = component.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 10;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
                component.BossDamageMultiplier = 10f;
                component.baseData.force = 0f;
                component.AdditionalScaleMultiplier = 1.6f;
                component.SetOwnerSafe(man, "Player");
                component.baseData.damage = this.FlamerTrue;
                component.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
            }
        }


        private void ProcessEnemy(AIActor activeEnemies, float distance)
        {
            for (int i = 0; i < 6; i++)
            {

                if (activeEnemies && activeEnemies.HasBeenEngaged && activeEnemies.healthHaver && activeEnemies.IsNormalEnemy && !activeEnemies.healthHaver.IsDead && !activeEnemies.healthHaver.IsBoss)
                {
                    if (activeEnemies.gameObject.GetComponent<ExplodeOnDeath>())
                    {
                        Destroy(activeEnemies.gameObject.GetComponent<ExplodeOnDeath>());
                    }
                    GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemySuck(activeEnemies));
                    activeEnemies.EraseFromExistenceWithRewards(true);
                    PlayerController man = base.gun.CurrentOwner as PlayerController;
                    if (man.HasPassiveItem(815) && man.HasPickupID(Gungeon.Game.Items["ror:hungering_chamber"].PickupObjectId))
                    { gun.GainAmmo(3); }
                    break;
                }
            }
        }





        private IEnumerator HandleEnemySuck(AIActor target)
        {
            PlayerController man = base.gun.CurrentOwner as PlayerController;
            Transform copySprite = this.CreateEmptySprite(target);
            target.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.Void, DamageCategory.Normal, true, null, false);
            target.EraseFromExistenceWithRewards(false);
            Vector3 startPosition = copySprite.transform.position;
            float elapsed = 0f;
            float duration = 0.5f;
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                bool flag1 = copySprite;
                if (flag1)
                {
                    Vector3 position = man.CurrentGun.PrimaryHandAttachPoint.position;
                    float t = elapsed / duration * (elapsed / duration);
                    copySprite.position = Vector3.Lerp(startPosition, position, t);
                    copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
                    copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), t);
                    position = default(Vector3);
                }
                yield return null;
            }
            bool flag = copySprite;
            if (flag)
            { UnityEngine.Object.Destroy(copySprite.gameObject); }

            yield break;
        }

        // Token: 0x0600002E RID: 46 RVA: 0x00004C5C File Offset: 0x00002E5C
        private Transform CreateEmptySprite(AIActor target)
        {
            GameObject gameObject = new GameObject("suck image");
            gameObject.layer = target.gameObject.layer;
            tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
            gameObject.transform.parent = SpawnManager.Instance.VFX;
            tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
            tk2dSprite.transform.position = target.sprite.transform.position;
            GameObject gameObject2 = new GameObject("image parent");
            gameObject2.transform.position = tk2dSprite.WorldCenter;
            tk2dSprite.transform.parent = gameObject2.transform;
            bool flag = target.optionalPalette != null;
            if (flag)
            {
                tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
            }
            return gameObject2.transform;
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

        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}