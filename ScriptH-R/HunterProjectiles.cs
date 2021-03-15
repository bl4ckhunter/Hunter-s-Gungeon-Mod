using Dungeonator;
using ItemAPI;
using Mod;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace Items
{

    internal class HeartlanceProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.baseData.damage = 25f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.baseData.force = 70f;
                    component.baseData.speed = 20f;
                    component.OnDestruction += Flak;
                    component.OnDestruction += Fierypoop;
                }
            }
        }

        private void Fierypoop(Projectile obj)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition).TimedAddGoopCircle(obj.sprite.WorldBottomCenter, 3.5f, 1f, false);
        }
        private void Flak(Projectile obj)
        {


            PlayerController man = (this.projectile.Owner as PlayerController);
            Projectile projectile = ((Gun)ETGMod.Databases.Items[748]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 0f), true);
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 180f), true);
            GameObject gameObject4 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 270f), true);
            GameObject gameObject5 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 45f), true);
            GameObject gameObject6 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 135f), true);
            GameObject gameObject7 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 225f), true);
            GameObject gameObject8 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 315f), true);
            GameObject gameObject9 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 90f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component9 = gameObject9.GetComponent<Projectile>();
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
                component.baseData.range = 3f;
                component.baseData.damage = 1f;
                component.baseData.force = 0f;
                component.AdditionalScaleMultiplier = 0.3f;
                component.baseData.speed = 15f;
                component.SetOwnerSafe(man, "Player");
                component.ignoreDamageCaps = false;
                BounceProjModifier bouncer = component.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 10;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
            }
            bool flag2 = component != null;
            if (flag2)
            {
                component2.Owner = man;
                component2.Shooter = man.specRigidbody;
                component2.baseData.range = 3f;
                component2.baseData.damage = 1f;
                component2.baseData.force = 0f;
                component2.AdditionalScaleMultiplier = 0.3f;
                component2.baseData.speed = 15f;
                component2.SetOwnerSafe(man, "Player");
                component2.ignoreDamageCaps = false;
                BounceProjModifier bouncer = component2.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component2.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 10;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
            }
            bool flag3 = component != null;
            if (flag3)
            {
                component3.Owner = man;
                component3.Shooter = man.specRigidbody;
                component3.baseData.range = 3f;
                component3.baseData.damage = 1f;
                component3.baseData.force = 0f;
                component3.AdditionalScaleMultiplier = 0.3f;
                component3.baseData.speed = 15f;
                component3.SetOwnerSafe(man, "Player");
                component3.ignoreDamageCaps = false;
                BounceProjModifier bouncer = component3.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component3.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 10;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
            }
            bool flag4 = component != null;
            if (flag4)
            {
                component4.Owner = man;
                component4.Shooter = man.specRigidbody;
                component4.baseData.range = 3f;
                component4.baseData.force = 0f;
                component4.baseData.damage = 1f;
                component4.AdditionalScaleMultiplier = 0.3f;
                component4.baseData.speed = 15f;
                component4.SetOwnerSafe(man, "Player");
                component4.ignoreDamageCaps = false;
                BounceProjModifier bouncer = component4.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component4.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 10;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
            }
            bool flag5 = component != null;
            if (flag5)
            {
                component5.Owner = man;
                component5.Shooter = man.specRigidbody;
                component5.baseData.range = 3f;
                component5.baseData.damage = 1f;
                component5.baseData.force = 0f;
                component5.AdditionalScaleMultiplier = 0.3f;
                component5.baseData.speed = 15f;
                component5.SetOwnerSafe(man, "Player");
                component5.ignoreDamageCaps = false;
                BounceProjModifier bouncer = component5.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component5.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 10;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
            }
            bool flag6 = component != null;
            if (flag6)
            {
                component6.Owner = man;
                component6.Shooter = man.specRigidbody;
                component6.baseData.range = 3f;
                component6.baseData.damage = 1f;
                component6.baseData.force = 0f;
                component6.AdditionalScaleMultiplier = 0.3f;
                component6.baseData.speed = 15f;
                component6.SetOwnerSafe(man, "Player");
                component6.ignoreDamageCaps = false;
                BounceProjModifier bouncer = component6.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component6.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 10;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
            }
            bool flag7 = component != null;
            if (flag7)
            {
                component7.Owner = man;
                component7.Shooter = man.specRigidbody;
                component7.baseData.range = 3f;
                component7.baseData.force = 0f;
                component7.baseData.damage = 1f;
                component7.AdditionalScaleMultiplier = 0.3f;
                component7.baseData.speed = 15f;
                component7.SetOwnerSafe(man, "Player");
                component7.ignoreDamageCaps = false;
                BounceProjModifier bouncer = component7.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component7.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 10;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
            }
            bool flag9 = component != null;
            if (flag9)
            {
                component9.Owner = man;
                component9.Shooter = man.specRigidbody;
                component9.baseData.range = 3f;
                component9.baseData.damage = 1f;
                component9.baseData.force = 0f;
                component9.AdditionalScaleMultiplier = 0.3f;
                component9.baseData.speed = 15f;
                component9.SetOwnerSafe(man, "Player");
                component9.ignoreDamageCaps = false;
                BounceProjModifier bouncer = component9.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component9.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 10;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
            }
        }
        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
    } 
    internal class CarProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {
                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    projectile.baseData.range = 0.01f;
                    projectile.OnDestruction += Carflag;
                }
            }
        }

        private void Carflag(Projectile arg1)
        {
            CarProjectile.CigObject = PickupObjectDatabase.GetById(203).GetComponent<CigaretteItem>();
            this.inAirVFX = CigObject.inAirVFX;
            this.smokeSystem = CigObject.smokeSystem;
            DebrisObject component = base.GetComponent<DebrisObject>();
            AkSoundEngine.PostEvent("Play_OBJ_cigarette_throw_01", base.gameObject);
            component.killTranslationOnBounce = false;
            if (component)
            {
                DebrisObject debrisObject = component;
                debrisObject.OnBounced = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnBounced, new Action<DebrisObject>(this.OnBounced));
                debrisObject.specRigidbody.AddCollisionLayerOverride(2);
                debrisObject.specRigidbody.OnCollision += Bonk;
                DebrisObject debrisObject2 = component;
                debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, new Action<DebrisObject>(this.OnHitGround));
            }
            if (this.inAirVFX != null)
            {
                bool air = true;
                base.StartCoroutine(this.SpawnVFX(air));
            }
        }

        private void Bonk(CollisionData obj)
        {
            if(obj.OtherRigidbody.aiActor != null && obj.OtherRigidbody.aiActor.healthHaver != null && obj.OtherRigidbody.aiActor.healthHaver.IsVulnerable == true)
            {
                    PlayerController man = projectile.Owner as PlayerController;
                    Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[81]).DefaultModule.projectiles[0];
                    ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
                    boomer.explosionData.doScreenShake = false;
                    boomer.explosionData.damageRadius = 4.5f;
                    boomer.explosionData.doExplosionRing = true;
                    boomer.explosionData.damageToPlayer = 0f;
                    boomer.explosionData.force = 0f;
                    boomer.explosionData.preventPlayerForce = true;
                    boomer.explosionData.doDestroyProjectiles = false;
                    boomer.explosionData.pushRadius = boomer.explosionData.damageRadius;
                    boomer.explosionData.damage = 5f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                    boomer.IgnoreQueues = true;
                    Exploder.Explode(obj.OtherRigidbody.UnitCenter, boomer.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
                    UnityEngine.Object.Destroy(obj.MyRigidbody.gameObject);

            }
        }

        private IEnumerator SpawnVFX(bool air)
        {
            while (air)
            {
                SpawnManager.SpawnVFX(this.inAirVFX, this.transform.position, Quaternion.identity, false);
                yield return new WaitForSeconds(0.33f);
            }
            yield break;
        }
        private void OnBounced(DebrisObject obj)
        {
            DeadlyDeadlyGoopManager.IgniteGoopsCircle(base.transform.position.XY(), 1f);
        }

        // Token: 0x060070DC RID: 28892 RVA: 0x002BDD84 File Offset: 0x002BBF84
        private void OnHitGround(DebrisObject obj)
        {   
            
                UnityEngine.Object.Destroy(base.gameObject);
         
        }

        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private static CigaretteItem CigObject;
        private GameObject inAirVFX;
        private GameObject smokeSystem;
    }

    internal class MegathornProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {
                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    projectile.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                    projectile.baseData.range = 3.5f;
                    projectile.OnDestruction += this.Clusterfuck;
                    projectile.OnDestruction += this.Poisonpoop;
                    projectile.OnDestruction += this.Explosion;
                }
            }
        }

        private void Explosion(Projectile projectile)
        {
            AkSoundEngine.PostEvent("Play_wpn_voidcannon_shot_01", gameObject);
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
            PlayerController man = sourceprojectile.Owner as PlayerController;
            Projectile projectile = ((Gun)ETGMod.Databases.Items[593]).DefaultModule.projectiles[0];
            this.gun = sourceprojectile.Owner.CurrentGun;
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
                component.Shooter = sourceprojectile.Owner.specRigidbody;
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


            PlayerController man = gun.CurrentOwner as PlayerController;
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


        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private static CigaretteItem CigObject;
        private GameObject inAirVFX;
        private GameObject smokeSystem;
        private Gun gun;
    }

    internal class LaserShotgunProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.baseData.damage = 25f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.baseData.force = 70f;
                    component.baseData.speed = 20f;
                    component.baseData.range = 1.2f;
                    component.OnDestruction += Flak;
                }
            }
        }


        private void Flak(Projectile obj)
        {


            PlayerController man = (this.projectile.Owner as PlayerController);
            Projectile projectile = ((Gun)ETGMod.Databases.Items[383]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-15f, 15f)), true);
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-15f, 15f)), true);
            GameObject gameObject4 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-15f, 15f)), true);
            GameObject gameObject5 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-15f, 15f)), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            Projectile component4 = gameObject4.GetComponent<Projectile>();
            Projectile component5 = gameObject5.GetComponent<Projectile>();

            bool flag = component != null;
            if (flag)
            {
                component.Owner = man;
                component.Shooter = man.specRigidbody;
                component.SetOwnerSafe(man, "Player");
            }
            bool flag3 = component != null;
            if (flag3)
            {
                component3.Owner = man;
                component3.Shooter = man.specRigidbody;
                component3.SetOwnerSafe(man, "Player");

            }
            bool flag4 = component != null;
            if (flag4)
            {
                component4.Owner = man;
                component4.baseData.force = 0f;
                component4.SetOwnerSafe(man, "Player");
            }
            bool flag5 = component != null;
            if (flag5)
            {
                component5.Owner = man;
                component5.Shooter = man.specRigidbody;
                component5.baseData.force = 0f;
                component5.SetOwnerSafe(man, "Player");

            }

        }
        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
    }
    internal class RockProjectile : MonoBehaviour
    {
        // Token: 0x06000212 RID: 530 RVA: 0x00012114 File Offset: 0x00010314
        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            Projectile projectile = this.projectile;
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag4 = component != null;
            if (flag4)
            {
                component.OnDestruction += Sound;

            }

        }
        private void Sound(Projectile obj)
        {
            AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
            this.Mines_Cave_In = assetBundle2.LoadAsset<GameObject>("Mines_Cave_In");
            Vector2 position = obj.specRigidbody.UnitCenter;
            PlayerController man = (this.projectile.Owner as PlayerController);
            PlayerController player = man;
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
            base.StartCoroutine(this.HandleTriggerRockSlide(man, Mines_Cave_In, aimpoint));

        }
        private IEnumerator HandleTriggerRockSlide(PlayerController user, GameObject RockSlidePrefab, Vector2 targetPosition)
        {
            RoomHandler currentRoom = user.CurrentRoom;
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(RockSlidePrefab, targetPosition, Quaternion.identity);
            HangingObjectController RockSlideController = gameObject.GetComponent<HangingObjectController>();
            RockSlideController.triggerObjectPrefab = null;
            GameObject[] additionalDestroyObjects = new GameObject[]
            {
                RockSlideController.additionalDestroyObjects[1]
            };
            RockSlideController.additionalDestroyObjects = additionalDestroyObjects;
            UnityEngine.Object.Destroy(gameObject.transform.Find("Sign").gameObject);
            RockSlideController.ConfigureOnPlacement(currentRoom);
            yield return new WaitForSeconds(0.01f);
            RockSlideController.Interact(user);
            yield break;
        }


        // Token: 0x040000DF RID: 223
        private Projectile projectile;

        // Token: 0x040000E0 RID: 224
        private PlayerController player;
        private GameObject Mines_Cave_In;
        private Vector2 aimpoint;
        private float m_currentAngle;
        private float m_currentDistance;
    }
    internal class TankProjectile : MonoBehaviour
    {
        // Token: 0x06000212 RID: 530 RVA: 0x00012114 File Offset: 0x00010314
        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            Projectile projectile = this.projectile;
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, this.player.CurrentGun.CurrentAngle) * -Vector2.right);
            this.player.knockbackDoer.ApplyKnockback(dir, 70f);
            bool flag = this.player == null;
            bool flag2 = flag;
            if (flag2)
            {
                this.player.CurrentGun.ammo = this.player.CurrentGun.GetBaseMaxAmmo();
            }
            this.player.CurrentGun.DefaultModule.ammoCost = 1;
            Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[649]).DefaultModule.projectiles[0];
            ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
            BounceProjModifier bouncer = projectile.gameObject.AddComponent<BounceProjModifier>();
            ExplosiveModifier exploder = projectile.gameObject.GetComponent<ExplosiveModifier>();
            PierceProjModifier piercer = projectile.gameObject.AddComponent<PierceProjModifier>();
            piercer.penetration = 6;
            bouncer.bounceTrackRadius = 1000;
            bouncer.bouncesTrackEnemies = true;
            bouncer.numberOfBounces = 6;
            exploder.explosionData = boomer.explosionData;
            bouncer.ExplodeOnEnemyBounce = true;
            exploder.doExplosion = true;
            exploder.doDistortionWave = true;
            projectile.OnHitEnemy += this.Kaboom;
            projectile.OnDestruction += this.Sound;
            AkSoundEngine.PostEvent("Play_BOSS_tank_shot_01", base.gameObject);
        }
        private void Sound(Projectile obj)
        {
            AkSoundEngine.PostEvent("Play_WPN_LowerCaseR_Bomb_boom_01", gameObject);
        }

        private void Kaboom(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg1 != null)
            {
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[649]).DefaultModule.projectiles[0];
                ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
                ExplosionData boom = boomer.explosionData;
                Exploder.Explode(arg1.specRigidbody.UnitCenter, boom, Vector2.zero, null, false, CoreDamageTypes.None, false);
                AkSoundEngine.PostEvent("Play_WPN_anvil_impact_01", gameObject);
            }
        }


        // Token: 0x040000DF RID: 223
        private Projectile projectile;

        // Token: 0x040000E0 RID: 224
        private PlayerController player;
    }
    internal class RoseProjectile : MonoBehaviour
    {
        // Token: 0x06000212 RID: 530 RVA: 0x00012114 File Offset: 0x00010314
        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            Projectile projectile = this.projectile;
            projectile.specRigidbody.OnCollision += this.Kaboom1;
            projectile.specRigidbody.OnTileCollision += Boom;
            projectile.pierceMinorBreakables = true;
        }

        private void Kaboom1(CollisionData obj)
        {
            SpeculativeRigidbody arg2 = obj.MyRigidbody;
            if (arg2 != null)
            {
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[486]).DefaultModule.projectiles[0];
                ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
                ExplosionData boom = boomer.explosionData;
                boom.doScreenShake = false;
                boom.doDestroyProjectiles = false;
                boom.doExplosionRing = true;
                PlayerController man = projectile.Owner as PlayerController;
                boom.damage *= man.stats.GetStatValue(PlayerStats.StatType.Damage);
                boom.damageToPlayer = 0f;
                boom.preventPlayerForce = true;
                Exploder.Explode(arg2.UnitCenterRight, boom, Vector2.zero, null, true, CoreDamageTypes.None, false);
            }
        }

        private void Boom(CollisionData tileCollision)
        {
            Projectile arg1 = tileCollision.MyRigidbody.gameObject.GetComponent<Projectile>(); 
            if (arg1 != null)
            {
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[486]).DefaultModule.projectiles[0];
                ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
                ExplosionData boom = boomer.explosionData;
                boom.doScreenShake = false;
                boom.doDestroyProjectiles = false;
                PlayerController man = arg1.Owner as PlayerController;
                boom.doExplosionRing = true;
                boom.damageToPlayer = 0f;
                boom.preventPlayerForce = true;
                Exploder.Explode(tileCollision.MyRigidbody.UnitCenterRight, boom, Vector2.zero, null, true, CoreDamageTypes.None, false);
            }
        }

        private void Kaboom(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg1 != null && arg2 != null)
            {   
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[486]).DefaultModule.projectiles[0];
                ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
                ExplosionData boom = boomer.explosionData;
                boom.doScreenShake = false;
                boom.doDestroyProjectiles = false;
                boom.doExplosionRing = true;
                PlayerController man = arg1.Owner as PlayerController;
                boom.damageToPlayer = 0f;
                boom.preventPlayerForce = true;
                Exploder.Explode(arg2.specRigidbody.UnitCenter, boom, Vector2.zero, null, true, CoreDamageTypes.None, false);
            }
        }


        // Token: 0x040000DF RID: 223
        private Projectile projectile;

        // Token: 0x040000E0 RID: 224
        private PlayerController player;
    }
    internal class KnightlyProjectile : MonoBehaviour
    {
        // Token: 0x06000212 RID: 530 RVA: 0x00012114 File Offset: 0x00010314
        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            Projectile projectile = this.projectile; PlayerController x = this.player.CurrentGun.CurrentOwner as PlayerController;
            projectile.AdditionalScaleMultiplier = 0.25f;
            projectile.baseData.damage *= 0.25f;
            projectile.baseData.speed = 110f;
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, x.CurrentGun.CurrentAngle) * -Vector2.right);
            x.knockbackDoer.ApplyKnockback(dir, 170f);
            bool flag = x == null;
            bool flag2 = flag;
            if (flag2)
            {
                this.player.CurrentGun.ammo = this.player.CurrentGun.GetBaseMaxAmmo();
            }
            this.player.CurrentGun.DefaultModule.ammoCost = 1;
            Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[180]).DefaultModule.projectiles[0];
            ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
            BounceProjModifier bouncer = projectile.gameObject.AddComponent<BounceProjModifier>();
            ExplosiveModifier exploder = projectile.gameObject.GetComponent<ExplosiveModifier>();
            PierceProjModifier piercer = projectile.gameObject.AddComponent<PierceProjModifier>();
            piercer.penetration = 10;
            bouncer.bounceTrackRadius = 1000;
            bouncer.bouncesTrackEnemies = true;
            bouncer.numberOfBounces = 5;
            exploder.explosionData = boomer.explosionData;
            bouncer.ExplodeOnEnemyBounce = true;
            exploder.doExplosion = true;
            exploder.doDistortionWave = true;
            projectile.OnHitEnemy += this.Kaboom;
            projectile.OnDestruction += Kaboom2;
            x.ForceBlank(1.5f, 0.5f, false, true, projectile.specRigidbody.UnitCenter, false, 300f);
            AkSoundEngine.PostEvent("Play_BOSS_tank_shot_01", base.gameObject);

        }
        private void Kaboom2(Projectile obj)
        {
            if (obj != null)
            {
                PlayerController x = this.player.CurrentGun.CurrentOwner as PlayerController;
                Vector2 dir = x.specRigidbody.UnitCenter - obj.specRigidbody.UnitCenter;
                x.knockbackDoer.ApplyKnockback(dir, 270f);
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[180]).DefaultModule.projectiles[0];
                ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
                ExplosionData boom = boomer.explosionData;
                boomer.doDistortionWave = true;
                boomer.explosionData.damage = 0f;
                Exploder.Explode(obj.specRigidbody.UnitCenter, boom, Vector2.zero, null, false, CoreDamageTypes.None, false);
                x.ForceBlank(5f, 0.5f, false, true, obj.specRigidbody.UnitCenter, false, 300f);
                AkSoundEngine.PostEvent("Play_WPN_anvil_impact_01", gameObject);
            }
        }

        private void Kaboom(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg1 != null)
            {
                PlayerController x = this.player.CurrentGun.CurrentOwner as PlayerController;
                Vector2 dir = x.specRigidbody.UnitCenter - arg1.specRigidbody.UnitCenter;
                x.knockbackDoer.ApplyKnockback(dir, 170f);
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[180]).DefaultModule.projectiles[0];
                ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
                ExplosionData boom = boomer.explosionData;
                boomer.explosionData.doForce = true;
                boomer.doDistortionWave = true;
                Exploder.Explode(arg1.specRigidbody.UnitCenter, boom, Vector2.zero, null, false, CoreDamageTypes.None, false);
                x.ForceBlank(5f, 0.5f, false, true, arg1.specRigidbody.UnitCenter, false, 300f);
                AkSoundEngine.PostEvent("Play_WPN_anvil_impact_01", gameObject);
            }
        }


        // Token: 0x040000DF RID: 223
        private Projectile projectile;

        // Token: 0x040000E0 RID: 224
        private PlayerController player;
    }
    internal class SwarmMissiles : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.baseData.damage = 25f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.baseData.force = 70f;
                    component.baseData.speed = 20f;
                    component.baseData.range = 0.1f;
                    component.OnDestruction += Flak;
                }
            }
        }


        private void Flak(Projectile obj)
        {


            PlayerController man = (this.projectile.Owner as PlayerController);
            Projectile projectile = ((Gun)ETGMod.Databases.Items[372]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle - 180f), true);
            Projectile component = gameObject.GetComponent<Projectile>();

            bool flag = component != null;
            if (flag)
            {
                component.AdditionalScaleMultiplier = 0.4f;
                component.Owner = man;
                component.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                component.Shooter = man.specRigidbody;
                component.baseData.speed = 15f;
                component.SetOwnerSafe(man, "Player");
                component.baseData.damage = 0f;
                component.baseData.force = 0f;
                BounceProjModifier bouncer = projectile.gameObject.AddComponent<BounceProjModifier>();
                bouncer.numberOfBounces = 3;
                ExplosiveModifier exploder = projectile.gameObject.GetComponent<ExplosiveModifier>();
                exploder.IgnoreQueues = true;
                exploder.explosionData.damage = 6f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                exploder.explosionData.force = 0f;
                exploder.explosionData.doScreenShake = false;
            }


        }
        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
    }

    internal class ShrinkProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.OnHitEnemy += Enlarge;
                    this.m_distortMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionRadius"));
                    this.m_distortMaterial.SetFloat("_Strength", 5f);
                    this.m_distortMaterial.SetFloat("_TimePulse", 0.25f);
                    this.m_distortMaterial.SetFloat("_RadiusFactor", 0.30f);
                    this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(component.transform.position));
                    GameManager.Instance.StartCoroutine(ChangeVector(component));
                    Pixelator.Instance.RegisterAdditionalRenderPass(this.m_distortMaterial);
                    projectile.OnDestruction += Blam;

                }
            }
        }


        private void Enlarge(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2 != null && arg2.healthHaver != null && !arg2.healthHaver.IsBoss && !arg2.healthHaver.IsDead)
            {
                Shrinker large = arg2.aiActor.gameObject.GetOrAddComponent<Shrinker>();
                large.largeness -= 0.042f;
                arg2.aiActor.EnemyScale = new Vector2(1, 1) * large.largeness;
                if (large.largeness < 0.45f)
                {
                    arg2.behaviorSpeculator.Stun(10000000f, true);
                    arg2.aiActor.IgnoreForRoomClear = true;
                    arg2.specRigidbody.OnPreRigidbodyCollision += Splat;
                    arg2.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));

                }
            }
            if (arg2.healthHaver != null && arg2 != null && !arg2.healthHaver.IsDead && arg2.healthHaver.IsBoss)
            {
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[17]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(Boomprojectile.gameObject, arg1.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.baseData.speed = 100f;
                    component.AdditionalScaleMultiplier = 2.5f;
                    component.SetOwnerSafe(player, "Player");
                    component.Shooter = player.specRigidbody;
                    component.Owner = player;
                }
            }
        }



        private void Splat(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            ShrinkProjectile.teleporter = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>();
            if (myRigidbody.UnitCenter != null)
            {
                UnityEngine.Object.Instantiate<GameObject>(teleporter.TelefragVFXPrefab, myRigidbody.UnitCenter, Quaternion.identity);
            }
            if (myRigidbody.aiActor.gameObject.GetComponent<ExplodeOnDeath>())
            {
                Destroy(myRigidbody.aiActor.gameObject.GetComponent<ExplodeOnDeath>());
            }
            Destroy(myRigidbody.gameObject);
            player.CurrentGun.GainAmmo(5);
            PhysicsEngine.SkipCollision = true;
        }


        private IEnumerator ChangeVector(Projectile position)
        {
            float elaplsed = 0f;
            while (position != null)
            {
                elaplsed += BraveTime.DeltaTime;
                float floaty = (float)Math.Sin(elaplsed * 50);
                this.m_distortMaterial.SetFloat("_Strength", 30f * floaty);
                this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(position.transform.position));
                yield return null;
            }
            this.Blam(position);
            yield break;
        }









        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private bool onCooldown;
        private GoopDefinition goopDefinition;
        private List<AIActor> activeEnemies2;
        private Vector2 Position;
        private bool cooldown2;
        private bool onCooldown2;
        private int instances;
        private bool onCooldown3;
        private int instances2;

        private void Blam(Projectile obj)
        {
            if (Pixelator.Instance != null)
            {
                Pixelator.Instance.DeregisterAdditionalRenderPass(this.m_distortMaterial);
            }
        }

        private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint)
        {
            Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
            return new Vector4(vector.x, vector.y, 0f, 0f);
        }


        // Token: 0x04000047 RID: 71
        private Material m_distortMaterial;
        private static TeleporterPrototypeItem teleporter;
    }

    internal class Shrinker : MonoBehaviour
    {
        public float largeness = 1;
    }

    internal class Enlargeprojectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.OnHitEnemy += Enlarge;
                    this.m_distortMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionRadius"));
                    this.m_distortMaterial.SetFloat("_Strength", 5f);
                    this.m_distortMaterial.SetFloat("_TimePulse", 0.25f);
                    this.m_distortMaterial.SetFloat("_RadiusFactor", 0.30f);
                    this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(component.transform.position));
                    GameManager.Instance.StartCoroutine(ChangeVector(component));
                    Pixelator.Instance.RegisterAdditionalRenderPass(this.m_distortMaterial);
                    projectile.OnDestruction += Blam;

                }
            }
        }


        private void Enlarge(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2.healthHaver != null && arg2 != null && !arg2.healthHaver.IsDead && !arg2.healthHaver.IsBoss)
            {
                Enlarger large = arg2.aiActor.gameObject.GetOrAddComponent<Enlarger>();
                large.largeness += 0.14f;
                if (!arg2.healthHaver.IsBoss)
                {
                    arg2.aiActor.EnemyScale = new Vector2(1, 1) * large.largeness;
                }
                if (large.largeness > 2f)
                {
                    Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[593]).DefaultModule.projectiles[0];
                    GameObject gameObject = SpawnManager.SpawnProjectile(Boomprojectile.gameObject, arg1.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    bool flag4 = component != null;
                    if (flag4)
                    {
                        component.baseData.damage = 300f;
                        component.baseData.speed = 100f;
                        component.AdditionalScaleMultiplier = 2.5f;
                        component.SetOwnerSafe(player, "Player");
                        component.Shooter = player.specRigidbody;
                        component.Owner = player;
                        component.gameObject.GetOrAddComponent<RadProjectile>();
                    }

                }
                
            }
            if (arg2.healthHaver != null && arg2 != null && !arg2.healthHaver.IsDead && arg2.healthHaver.IsBoss)
            {
                Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[17]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(Boomprojectile.gameObject, arg1.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.baseData.damage *= 2f;
                    component.baseData.speed = 100f;
                    component.AdditionalScaleMultiplier = 2.5f;
                    component.SetOwnerSafe(player, "Player");
                    component.Shooter = player.specRigidbody;
                    component.Owner = player;
                }
            }
        }
        private IEnumerator ChangeVector(Projectile position)
        {
            float elaplsed = 0f;
            while (position != null)
            {
                elaplsed += BraveTime.DeltaTime;
                float floaty = (float)Math.Sin(elaplsed * 50);
                this.m_distortMaterial.SetFloat("_Strength", 30f * floaty);
                this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(position.transform.position));
                yield return null;
            }
            this.Blam(position);
            yield break;
        }








        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private bool onCooldown;
        private GoopDefinition goopDefinition;
        private List<AIActor> activeEnemies2;
        private Vector2 Position;
        private bool cooldown2;
        private bool onCooldown2;
        private int instances;
        private bool onCooldown3;
        private int instances2;

        private void Blam(Projectile obj)
        {
            if (Pixelator.Instance != null)
            {
                Pixelator.Instance.DeregisterAdditionalRenderPass(this.m_distortMaterial);
            }
        }

        private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint)
        {
            Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
            return new Vector4(vector.x, vector.y, 0f, 0f);
        }


        // Token: 0x04000047 RID: 71
        private Material m_distortMaterial;
    }

    internal class Enlarger : MonoBehaviour
    {
        public float largeness = 1;
    }
    internal class GravitonGrenade : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {   
                    component.AdjustPlayerProjectileTint(Color.HSVToRGB(0.53f, 1f, 30f),10,0.5f);
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    projectile.OnDestruction += Suck;
                }


            }
        }

        private void Suck(Projectile obj)
        {
            try
            {
                GameManager.Instance.StartCoroutine(DoDistortionWaveLocal(obj.transform.position, 1.8f, 0.2f, 10f, 0.4f));
                if (player.CurrentRoom != null && player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null)
                {
                    foreach (AIActor aiactor in player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                    {
                        if (!aiactor.healthHaver.IsBoss)
                        {
                            aiactor.knockbackDoer.weight = 10;
                            Vector2 dir = aiactor.transform.position - obj.transform.position;
                            aiactor.knockbackDoer.ApplyKnockback(-dir, 3.5f * (Vector2.Distance(obj.transform.position, aiactor.transform.position) + 0.001f));
                            aiactor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox));
                            aiactor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider));
                        }
                    }

                }
            }
            catch 
            { }

        }

        private IEnumerator DoDistortionWaveLocal(Vector2 center, float distortionIntensity, float distortionRadius, float maxRadius, float duration)
        {
            Material distMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionWave"));
            Vector4 distortionSettings = this.GetCenterPointInScreenUV(center, distortionIntensity, distortionRadius);
            distMaterial.SetVector("_WaveCenter", distortionSettings);
            Pixelator.Instance.RegisterAdditionalRenderPass(distMaterial);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                    elapsed += BraveTime.DeltaTime;
                float t = elapsed / duration;
                t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
                distortionSettings = this.GetCenterPointInScreenUV(center, distortionIntensity, distortionRadius);
                distortionSettings.w = Mathf.Lerp(distortionSettings.w, 0f, t);
                distMaterial.SetVector("_WaveCenter", distortionSettings);
                float currentRadius = Mathf.Lerp(maxRadius, 0f, t);
                distMaterial.SetFloat("_DistortProgress", (currentRadius / (maxRadius * 5)));
                yield return null;
            }
            Pixelator.Instance.DeregisterAdditionalRenderPass(distMaterial);
            UnityEngine.Object.Destroy(distMaterial);
            Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[593]).DefaultModule.projectiles[0];
            ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
            boomer.explosionData.doScreenShake = false;
            boomer.explosionData.damageRadius = 4.5f;
            boomer.explosionData.doExplosionRing = true;
            boomer.explosionData.damageToPlayer = 0f;
            boomer.explosionData.force = 90f;
            boomer.explosionData.preventPlayerForce = true;
            boomer.explosionData.doDestroyProjectiles = false;
            boomer.explosionData.pushRadius = boomer.explosionData.damageRadius;
            boomer.explosionData.damage = 45f * player.stats.GetStatValue(PlayerStats.StatType.Damage);
            boomer.IgnoreQueues = true;
            Exploder.Explode(center, boomer.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
            if (player.PlayerHasActiveSynergy("The Big Blank"))
            {
                player.ForceBlank(4.5f, 0.5f, false, true, center);
            }

            yield break;
        }



        private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint, float dIntensity, float dRadius)
        {
            Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
            return new Vector4(vector.x, vector.y, dRadius, dIntensity);
        }


        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private Material m_distortMaterial;
        private float dRadius;
        private float dIntensity;
    }
    internal class ShockwavePunchProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.baseData.speed = 70f;
                    this.m_distortMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionRadius"));
                    this.m_distortMaterial.SetFloat("_Strength", 85f);
                    this.m_distortMaterial.SetFloat("_TimePulse", 0.25f);
                    this.m_distortMaterial.SetFloat("_RadiusFactor", 0.35f);
                    this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(component.transform.position));
                    GameManager.Instance.StartCoroutine(ChangeVector(component));
                    Pixelator.Instance.RegisterAdditionalRenderPass(this.m_distortMaterial);
                    projectile.OnDestruction += Blam;
                }
            }
        }

        private IEnumerator ChangeVector(Projectile position)
        {
            float elaplsed = 0f;
            while (position != null)
            {
                this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(position.transform.position));
                yield return null;
            }
            this.Blam(position);
            yield break;
        }


        private void Blam(Projectile obj)
        {
            if (Pixelator.Instance != null)
            {
                Pixelator.Instance.DeregisterAdditionalRenderPass(this.m_distortMaterial);
            }
        }

        private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint)
        {
            Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
            return new Vector4(vector.x, vector.y, 0f, 0f);
        }


        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private Material m_distortMaterial;
    }
    internal class Drumbeat : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.baseData.speed = 70f;
                    this.m_distortMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionRadius"));
                    this.m_distortMaterial.SetFloat("_Strength", 1f);
                    this.m_distortMaterial.SetFloat("_TimePulse", 0.25f);
                    this.m_distortMaterial.SetFloat("_RadiusFactor", 1f);
                    this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(man.transform.position));
                    if (UnityEngine.Random.value < 0.1f) {
                        Exploder.DoDistortionWave(man.CenterPosition, -0.15f, 2f, 2f, 0.5f);
                        AkSoundEngine.PostEvent("Play_OBJ_silenceblank_use_01", base.gameObject);
                        this.Collide1(15f);
                        Exploder.DoRadialKnockback(player.CenterPosition, 150f, 15f);
                    }
                }
            }
        }
        private void Collide1(float radius)
        {
            PlayerController man = projectile.Owner as PlayerController;
            List<AIActor> activeEnemies = man.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag = activeEnemies != null;
            if (flag)
            {
                int count = activeEnemies.Count;
                for (int i = 0; i < count; i++)
                {
                    bool flag2 = activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss && activeEnemies[i].specRigidbody != null && Vector2.Distance(activeEnemies[i].specRigidbody.UnitCenter, man.CenterPosition) <= radius;
                    if (flag2)
                    {
                        this.AddCollide(activeEnemies[i]);
                    }
                }
            }
        }

        // Token: 0x0600012F RID: 303 RVA: 0x0000C2D0 File Offset: 0x0000A4D0
        private void AddCollide(AIActor arg2)
        {
            arg2.specRigidbody.CollideWithOthers = true;
            arg2.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyCollider));
            arg2.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox));
            arg2.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider));
            SpeculativeRigidbody specRigidbody = arg2.specRigidbody;
            specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.Bang));
        }

        // Token: 0x06000130 RID: 304 RVA: 0x0000C348 File Offset: 0x0000A548
        private void Bang(CollisionData tileCollision)
        {
            Vector2 unitCenter = tileCollision.MyRigidbody.UnitCenter;
            float magnitude = tileCollision.MyRigidbody.Velocity.magnitude;
            this.Gun(unitCenter, magnitude, tileCollision.MyRigidbody);
        }

        // Token: 0x06000131 RID: 305 RVA: 0x0000C384 File Offset: 0x0000A584
        private void Gun(Vector2 position, float speed, SpeculativeRigidbody target)
        {
            PlayerController man = this.projectile.Owner as PlayerController;
            target.OnCollision = (Action<CollisionData>)Delegate.Remove(target.OnCollision, new Action<CollisionData>(this.Bang));
            PlayerController lastOwner = man;
            Projectile projectile = ((Gun)global::ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].Projectile;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, position, Quaternion.Euler(0f, 0f, man.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag = component != null;
            bool flag2 = flag;
            if (flag2)
            {
                component.AdditionalScaleMultiplier = 2f;
                component.baseData.damage = 2f * speed * lastOwner.stats.GetStatValue(PlayerStats.StatType.Damage);
                component.baseData.force = 0f;
                component.Owner = lastOwner;
                component.Shooter = lastOwner.specRigidbody;
            }
        }
        private IEnumerator ChangeVector(Projectile position)
        {
            float elaplsed = 0f;
            while (elaplsed < 0.3f)
            {
                elaplsed += BraveTime.DeltaTime;
                this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(position.transform.position));
                yield return null;
            }
            this.Blam(position);
            yield break;
        }


        private void Blam(Projectile obj)
        {
            if (Pixelator.Instance != null)
            {
                Pixelator.Instance.DeregisterAdditionalRenderPass(this.m_distortMaterial);
            }
        }

        private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint)
        {
            Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
            return new Vector4(vector.x, vector.y, 0f, 0f);
        }


        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private Material m_distortMaterial;

        public GameObject BlankVFXPrefab;
    }
    internal class DiscoBallProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    Projectile component1 = ((Gun)ETGMod.Databases.Items[87]).DefaultModule.projectiles[0];
                    for (int i = 0; i < 8; i++)
                    {
                        GameManager.Instance.StartCoroutine(HandleFireShortBeam(component1, man, projectile, man.CurrentGun.CurrentAngle + (i * 45f)));
                    }
                }
            }
        }
            public IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, Projectile spawnpoint, float Angle)
            {
                float elapsed = 0f;
                BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, Angle, spawnpoint.transform.position + new Vector3(0.5f, 0.5f, 0));
                yield return null;
                while (spawnpoint != null)
                {
                    elapsed += BraveTime.DeltaTime;
                    this.ContinueFiringBeam(beam, source, Angle + (100*elapsed), spawnpoint.transform.position + new Vector3(0.5f, 0.5f, 0), projectileToSpawn);
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
                BeamController component2 = gameObject.GetComponent<BeamController>();
            component.Owner = null;
            component2.Owner = null;
                component2.chargeDelay = 0f;
                component.baseData.speed = 30f;
                component.baseData.force = 0f;
                component.baseData.damage *= 0.65f;
                component.AppliesPoison = false;
                component2.HitsPlayers = false;
                component2.HitsEnemies = true;
                Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
                component2.Direction = v;
                component2.Origin = vector;
                Projectile sourceProjectile = component;
                return component2;
            }
            private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint, Projectile projectile)
            {   
                Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
            beam.Owner = source;
            projectile.Owner = source;
            beam.AdjustPlayerBeamTint(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1.04f, 1.04f), 5, 0f);
            beam.Owner = null;
            projectile.Owner = null;
            if (beam.projectile.OverrideMotionModule != null)
            { beam.projectile.OverrideMotionModule = null; }
            if (projectile.OverrideMotionModule != null)
            {projectile.OverrideMotionModule = null; }
            beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
                projectile.AdditionalScaleMultiplier = 4f;
                beam.ChanceBasedHomingRadius = 6f;
                beam.Origin = vector;
                beam.LateUpdatePosition(vector);
            }
        
        
        // Token: 0x0600888F RID: 34959 RVA: 0x002E0188 File Offset: 0x002DE388
        internal void CeaseBeam(BeamController beam)
            {
                beam.CeaseAttack();
            }
        


        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
    }
    internal class FlamerProj : MonoBehaviour
    {
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectilee = this.projectile;
                Projectile componente = gameObject.GetComponent<Projectile>();
                bool flag4 = componente != null;
                if (flag4)
                {
                    PlayerController man = player;
                    this.FlamerTrue = 2f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                    AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
                    GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
                    DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
                    Projectile projectile = ((Gun)ETGMod.Databases.Items[89]).DefaultModule.projectiles[0];
                    if (UnityEngine.Random.value > 0.5f)
                    { this.sign = -1f; }
                    else { this.sign = 1f; }
                    float value = UnityEngine.Random.value * 7f * sign;
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0.5f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + value), true);
                    if (UnityEngine.Random.value > 0.5f)
                    { this.sign = -1f; }
                    else { this.sign = 1f; }
                    float value1 = UnityEngine.Random.value * 7f * sign;
                    GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0.5f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + value1), true);
                    if (UnityEngine.Random.value > 0.5f)
                    { this.sign = -1f; }
                    else { this.sign = 1f; }
                    float value2 = UnityEngine.Random.value * 7f * sign;
                    GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0.5f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + value2), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    Projectile component2 = gameObject2.GetComponent<Projectile>();
                    Projectile component3 = gameObject3.GetComponent<Projectile>();
                    bool flag = component != null;
                    if (flag)
                    {
                        component.Owner = man;
                        component.AdjustPlayerProjectileTint(new Color(UnityEngine.Random.Range(5, 40), 0, 0), 5, 0f);
                        component.Shooter = man.specRigidbody;
                        component.baseData.speed = 10f;
                        component.AdditionalScaleMultiplier = 1.5f + UnityEngine.Random.value;
                        component.SetOwnerSafe(man, "Player");
                        component.baseData.damage = this.FlamerTrue;
                        component.AppliesFire = true;
                        component.FireApplyChance = 1f;
                        component.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
                        component3.OnDestruction += (Action<Projectile>)Delegate.Combine(component.OnHitEnemy, new Action<Projectile>(this.Fierypoop));
                    }
                    bool flag2 = component2 != null;
                    if (flag2)
                    {
                        component2.Owner = man;
                        component2.AdjustPlayerProjectileTint(new Color(UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40), 0), 5, 0f);
                        component2.Shooter = man.specRigidbody;
                        component2.baseData.speed = 10f;
                        component2.SetOwnerSafe(man, "Player");
                        component.AdditionalScaleMultiplier = 1.5f + UnityEngine.Random.value;
                        component2.AppliesFire = true;
                        component2.FireApplyChance = 1f;
                        component2.baseData.damage = this.FlamerTrue;
                        component2.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
                        component3.OnDestruction += (Action<Projectile>)Delegate.Combine(component.OnHitEnemy, new Action<Projectile>(this.Fierypoop));

                    }
                    bool flag3 = component3 != null;
                    if (flag3)
                    {
                        component3.Owner = man;
                        component3.AdjustPlayerProjectileTint(new Color(UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40)), 5, 0f);
                        component3.Shooter = man.specRigidbody;
                        component3.SetOwnerSafe(man, "Player");
                        component3.baseData.speed = 10f;
                        component3.AdditionalScaleMultiplier = 0.1f + UnityEngine.Random.value;
                        component3.FireApplyChance = 1f;
                        component3.baseData.damage = this.FlamerTrue;
                        component3.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
                        component3.OnDestruction += (Action<Projectile>)Delegate.Combine(component.OnHitEnemy, new Action<Projectile>(this.Fierypoop));


                    }
                }
            }


        }
        

        private void Fierypoop(Projectile obj)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition).TimedAddGoopCircle(obj.sprite.WorldBottomCenter, 2.5f, 1f, false);
        }



        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private float sign;

        public float FlamerTrue;
    }
    internal class IcerProj : MonoBehaviour
    {
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectiler = this.projectile;
                Projectile componente = gameObject.GetComponent<Projectile>();
                bool flag4 = componente != null;
                if (flag4)
                {
                    PlayerController man = player;
                    this.FlamerTrue = 1f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                    AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
                    GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
                    DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
                    Projectile projectile = ((Gun)ETGMod.Databases.Items[225]).DefaultModule.projectiles[0];
                    if (UnityEngine.Random.value > 0.5f)
                    { this.sign = -1f; }
                    else { this.sign = 1f; }
                    float value = UnityEngine.Random.value * 7f * sign;
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0.5f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + value), true);
                    if (UnityEngine.Random.value > 0.5f)
                    { this.sign = -1f; }
                    else { this.sign = 1f; }
                    float value1 = UnityEngine.Random.value * 7f * sign;
                    GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0.5f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + value1), true);
                    if (UnityEngine.Random.value > 0.5f)
                    { this.sign = -1f; }
                    else { this.sign = 1f; }
                    float value2 = UnityEngine.Random.value * 7f * sign;
                    GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, man.CurrentGun.sprite.WorldCenter + new Vector2(0.5f, 0.0f), Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + value2), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    Projectile component2 = gameObject2.GetComponent<Projectile>();
                    Projectile component3 = gameObject3.GetComponent<Projectile>();
                    bool flag = component != null;
                    if (flag)
                    {
                        component.Owner = man;
                        component.AdjustPlayerProjectileTint(new Color(0f, UnityEngine.Random.Range(5, 20), UnityEngine.Random.Range(5, 40)), 5, 0f);
                        component.Shooter = man.specRigidbody;
                        component.baseData.speed = 10f;
                        component.baseData.force = 0f;
                        component.AdditionalScaleMultiplier = 1.5f + UnityEngine.Random.value;
                        component.SetOwnerSafe(man, "Player");
                        component.baseData.damage = this.FlamerTrue;
                        component.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
                    }
                    bool flag2 = component2 != null;
                    if (flag2)
                    {
                        component2.Owner = man;
                        component2.AdjustPlayerProjectileTint(new Color(0f, UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40)), 5, 0f);
                        component2.Shooter = man.specRigidbody;
                        component2.baseData.speed = 10f;
                        component2.baseData.force = 0f;
                        component2.SetOwnerSafe(man, "Player");
                        component.AdditionalScaleMultiplier = 1.5f + UnityEngine.Random.value;
                        component2.baseData.damage = this.FlamerTrue;
                        component2.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;

                    }
                    bool flag3 = component3 != null;
                    if (flag3)
                    {
                        component3.Owner = man;
                        component3.AdjustPlayerProjectileTint(new Color(UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40)), 5, 0f);
                        component3.Shooter = man.specRigidbody;
                        component3.SetOwnerSafe(man, "Player");
                        component3.baseData.speed = 10f;
                        component3.baseData.force = 0f;
                        component3.AdditionalScaleMultiplier = 0.1f + UnityEngine.Random.value;
                        component3.baseData.damage = 0f;
                        component3.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;

                    }

                }

            }


        }



        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private float sign;

        public float FlamerTrue;
    }
}

internal class WoodPlankProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    Projectile component1 = ((Gun)ETGMod.Databases.Items[610]).DefaultModule.projectiles[0];
                    float value = UnityEngine.Random.Range(0f, 360f);
                    GameManager.Instance.StartCoroutine(HandleFireShortBeam(component1, man, projectile, value + 180));
                    GameManager.Instance.StartCoroutine(HandleFireShortBeam(component1, man, projectile, value ));
                }
            }
        }
        public IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, Projectile spawnpoint, float Angle)
        {
            float elapsed = 0f;
            BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, Angle, spawnpoint.transform.position);
            yield return null;
            while (spawnpoint != null)
            {
                elapsed += BraveTime.DeltaTime;
                this.ContinueFiringBeam(beam, source, Angle, spawnpoint.transform.position, projectileToSpawn);
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
            BeamController component2 = gameObject.GetComponent<BeamController>();
            component2.Owner = source;
            component2.chargeDelay = 0f;
            component.baseData.speed = 30f;
            component.AppliesPoison = false;
            component2.HitsPlayers = false;
            component2.HitsEnemies = true;
            Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
            component2.Direction = v;
            component2.Origin = vector;
            component.baseData.range = 5f;
            Projectile sourceProjectile = component;
            return component2;
        }
        private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint, Projectile projectile)
        {
            Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
            beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
            projectile.AdditionalScaleMultiplier = 4f;
            beam.ChanceBasedHomingRadius = 6f;
            beam.Origin = vector;
            beam.LateUpdatePosition(vector);
        }

        // Token: 0x0600888F RID: 34959 RVA: 0x002E0188 File Offset: 0x002DE388
        internal void CeaseBeam(BeamController beam)
        {
            beam.CeaseAttack();
        }



        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
    }

    internal class BombProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.baseData.damage = 25f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.baseData.force = 70f;
                    component.baseData.speed = 20f;
                    component.baseData.range = 1.2f;
                    component.OnDestruction += Flak;
                }
            }
        }


        private void Flak(Projectile obj)
        {
            if (obj != null)
            {

                PlayerController man = (this.projectile.Owner as PlayerController);
                BombProjectile.m_bomb = PickupObjectDatabase.GetById(567).GetComponent<FireVolleyOnRollItem>();
                ProjectileModule projectilemod = BombProjectile.m_bomb.ModVolley.projectiles[0];
                Projectile projectile = projectilemod.GetCurrentProjectile();
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-15f, 15f)), true);
                Projectile component = gameObject.GetComponent<Projectile>();

                bool flag = component != null;
                if (flag)
                {
                    component.Owner = man;
                    component.Shooter = man.specRigidbody;
                    component.baseData.speed = 7.5f;
                    component.SetOwnerSafe(man, "Player");
                    component.baseData.damage = 10f;
                    component.baseData.force = 100f;
                    component.baseData.range = 50f;
                    component.AdditionalScaleMultiplier = 2f;
                }
            }
        }
        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private static FireVolleyOnRollItem m_bomb;
    }

    internal class ShockProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                this.m_owner = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                this.damage = projectile.baseData.damage;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.OnHitEnemy = this.ChainLightning;
                    component.AdjustPlayerProjectileTint(Color.blue, 5, 0);

                }
            }
        }

        private void ChainLightning(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg1 != null)
            {
                bool flag2 = this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null;
                if (flag2)
                {
                    Vector2 position = arg2.UnitCenter;
                    foreach (AIActor aiactor in this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                    {
                        if (Vector2.Distance(aiactor.CenterPosition, position) < 10f)
                        {
                            PlayerController man = projectile.Owner as PlayerController;
                            GameObject gameObject = SpawnManager.SpawnProjectile(arg1.gameObject, aiactor.CenterPosition, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 359)), true);
                            Projectile component = gameObject.GetComponent<Projectile>();
                            bool flag4 = component != null;
                            if (flag4)
                            {
                                if (component.gameObject.GetComponent<ShockProjectile>())
                                {
                                    Destroy(component.gameObject.GetComponent<ShockProjectile>());
                                }
                                component.SetOwnerSafe(man, "Player");
                                component.Shooter = man.specRigidbody;
                                component.baseData.speed = 6f;
                                ComplexProjectileModifier complexProjectileModifier = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
                                ChainLightningModifier orAddComponent = component.gameObject.GetOrAddComponent<ChainLightningModifier>();
                                orAddComponent.LinkVFXPrefab = complexProjectileModifier.ChainLightningVFX;
                                orAddComponent.damageTypes = complexProjectileModifier.ChainLightningDamageTypes;
                                orAddComponent.maximumLinkDistance = complexProjectileModifier.ChainLightningMaxLinkDistance;
                                orAddComponent.damagePerHit = complexProjectileModifier.ChainLightningDamagePerHit;
                                orAddComponent.damageCooldown = complexProjectileModifier.ChainLightningDamageCooldown;
                                bool flag5 = complexProjectileModifier.ChainLightningDispersalParticles != null;
                                if (flag5)
                                {
                                    orAddComponent.UsesDispersalParticles = true;
                                    orAddComponent.DispersalParticleSystemPrefab = complexProjectileModifier.ChainLightningDispersalParticles;
                                    orAddComponent.DispersalDensity = complexProjectileModifier.ChainLightningDispersalDensity;
                                    orAddComponent.DispersalMinCoherency = complexProjectileModifier.ChainLightningDispersalMinCoherence;
                                    orAddComponent.DispersalMaxCoherency = complexProjectileModifier.ChainLightningDispersalMaxCoherence;
                                }
                                else
                                {
                                    orAddComponent.UsesDispersalParticles = false;
                                }
                                component.baseData.damage = this.damage * m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                                component.baseData.force = 0f;
                                PierceProjModifier pierce = component.gameObject.GetOrAddComponent<PierceProjModifier>();
                                pierce.penetration = 100;
                                component.baseData.range = 0.3f;
                            }
                        }

                    }
                }
            }
        }





        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private Projectile zapperMainProjectile = null;
        public float damage;
    }

internal class StormProjectile : MonoBehaviour
{
    // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
    public void Start()
    {
        {

            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            this.m_owner = (this.projectile.Owner as PlayerController);
            Projectile projectile = this.projectile;
            this.damage = projectile.baseData.damage;
            PlayerController man = projectile.Owner as PlayerController;
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag4 = component != null;
            if (flag4)
            {
                component.SetOwnerSafe(man, "Player");
                component.Shooter = man.specRigidbody;
                component.OnDestruction += ChainLightning;
                component.AdjustPlayerProjectileTint(Color.blue, 5, 0);

            }
        }
    }

    private void ChainLightning(Projectile arg1)
    {
        if (arg1 != null)
        {   
            bool flag2 = this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null;
            if (flag2)
            {
                PlayerController man1 = projectile.Owner as PlayerController;
                GameObject gameObject1 = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(13) as Gun).DefaultModule.projectiles[0].gameObject, man1.CurrentGun.barrelOffset.position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 359)), true);
                Projectile component1 = gameObject1.GetComponent<Projectile>();
                bool flag41 = component1 != null;
                if (flag41)
                {
                    if (component1.gameObject.GetComponent<ShockProjectile>())
                    {
                        Destroy(component1.gameObject.GetComponent<ShockProjectile>());
                    }
                    component1.SetOwnerSafe(man1, "Player");
                    component1.Shooter = man1.specRigidbody;
                    component1.baseData.speed = 6f;
                    ComplexProjectileModifier complexProjectileModifier = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
                    ChainLightningModifier orAddComponent = component1.gameObject.GetOrAddComponent<ChainLightningModifier>();
                    orAddComponent.LinkVFXPrefab = complexProjectileModifier.ChainLightningVFX;
                    orAddComponent.damageTypes = complexProjectileModifier.ChainLightningDamageTypes;
                    orAddComponent.maximumLinkDistance = complexProjectileModifier.ChainLightningMaxLinkDistance;
                    orAddComponent.damagePerHit = complexProjectileModifier.ChainLightningDamagePerHit;
                    orAddComponent.damageCooldown = complexProjectileModifier.ChainLightningDamageCooldown;
                    bool flag5 = complexProjectileModifier.ChainLightningDispersalParticles != null;
                    if (flag5)
                    {
                        orAddComponent.UsesDispersalParticles = true;
                        orAddComponent.DispersalParticleSystemPrefab = complexProjectileModifier.ChainLightningDispersalParticles;
                        orAddComponent.DispersalDensity = complexProjectileModifier.ChainLightningDispersalDensity;
                        orAddComponent.DispersalMinCoherency = complexProjectileModifier.ChainLightningDispersalMinCoherence;
                        orAddComponent.DispersalMaxCoherency = complexProjectileModifier.ChainLightningDispersalMaxCoherence;
                    }
                    else
                    {
                        orAddComponent.UsesDispersalParticles = false;
                    }
                    component1.baseData.damage = this.damage * m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                    component1.baseData.force = 0f;
                    PierceProjModifier pierce = component1.gameObject.GetOrAddComponent<PierceProjModifier>();
                    pierce.penetration = 100;
                    component1.baseData.range = 0.3f;
                }
                Vector2 position = arg1.specRigidbody.UnitCenter;
                foreach (AIActor aiactor in this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                {
                    if (Vector2.Distance(aiactor.CenterPosition, position) < 10f)
                    {   
                        PlayerController man = projectile.Owner as PlayerController;
                        GameObject gameObject = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(13) as Gun).DefaultModule.projectiles[0].gameObject, aiactor.CenterPosition, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 359)), true);
                        Projectile component = gameObject.GetComponent<Projectile>();
                        bool flag4 = component != null;
                        if (flag4)
                        {
                            if (component.gameObject.GetComponent<ShockProjectile>())
                            {
                                Destroy(component.gameObject.GetComponent<ShockProjectile>());
                            }
                            component.SetOwnerSafe(man, "Player");
                            component.Shooter = man.specRigidbody;
                            component.baseData.speed = 6f;
                            ComplexProjectileModifier complexProjectileModifier = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
                            ChainLightningModifier orAddComponent = component.gameObject.GetOrAddComponent<ChainLightningModifier>();
                            orAddComponent.LinkVFXPrefab = complexProjectileModifier.ChainLightningVFX;
                            orAddComponent.damageTypes = complexProjectileModifier.ChainLightningDamageTypes;
                            orAddComponent.maximumLinkDistance = complexProjectileModifier.ChainLightningMaxLinkDistance;
                            orAddComponent.damagePerHit = complexProjectileModifier.ChainLightningDamagePerHit;
                            orAddComponent.damageCooldown = complexProjectileModifier.ChainLightningDamageCooldown;
                            bool flag5 = complexProjectileModifier.ChainLightningDispersalParticles != null;
                            if (flag5)
                            {
                                orAddComponent.UsesDispersalParticles = true;
                                orAddComponent.DispersalParticleSystemPrefab = complexProjectileModifier.ChainLightningDispersalParticles;
                                orAddComponent.DispersalDensity = complexProjectileModifier.ChainLightningDispersalDensity;
                                orAddComponent.DispersalMinCoherency = complexProjectileModifier.ChainLightningDispersalMinCoherence;
                                orAddComponent.DispersalMaxCoherency = complexProjectileModifier.ChainLightningDispersalMaxCoherence;
                            }
                            else
                            {
                                orAddComponent.UsesDispersalParticles = false;
                            }
                            component.baseData.damage = this.damage * m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                            component.baseData.force = 0f;
                            PierceProjModifier pierce = component.gameObject.GetOrAddComponent<PierceProjModifier>();
                            pierce.penetration = 100;
                            component.baseData.range = 0.3f;
                        }
                    }

                }
            }
        }
    }





    // Token: 0x04000047 RID: 71
    private Projectile projectile;
    private PlayerController player;
    private PlayerController m_owner;
    private AIActor aiactor;
    private Projectile zapperMainProjectile = null;
    public float damage;
}
internal class RadProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {
                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                this.m_owner = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.OnHitEnemy = this.Rad;
                    projectile.AdjustPlayerProjectileTint(Color.yellow, 5, 0);

                }
            }
        }

        public void Rad(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {

            GameManager.Instance.StartCoroutine(Irradiate(arg2));

        }

        public IEnumerator Irradiate(SpeculativeRigidbody arg2)
        {  if (arg2.aiActor.gameObject.GetComponent<AiactorRadFlag>())
            {
                AiactorRadFlag Radcount1 = arg2.aiActor.gameObject.GetComponent<AiactorRadFlag>();
                if (Radcount1.RadCount < 5)
                {
                    Radcount1.RadCount += 1;
                }
            }
            bool flag2 = this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null;
            if (flag2 && !this.cooldown2)
            {
                this.cooldown2 = true;
                GameManager.Instance.StartCoroutine(StartCooldown1());
                this.Position = arg2.UnitCenter;
                List<AIActor> activeEnemies = this.player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

                if (activeEnemies != null)
                {
                    int count = activeEnemies.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Vector2.Distance(activeEnemies[i].CenterPosition, this.Position) < 4.5f && !arg2.aiActor.gameObject.GetComponent<AiactorRadFlag>())
                        {
                            arg2.aiActor.gameObject.AddComponent<AiactorRadFlag>();
                            while (arg2 != null && arg2.healthHaver.IsDead != true && arg2.healthHaver.IsAlive && this.instances < 10)
                            {
                                AiactorRadFlag Radcount = arg2.aiActor.gameObject.GetComponent<AiactorRadFlag>();
                                this.instances += 1;
                                int Multiplier = Radcount.RadCount;
                                this.Position = activeEnemies[i].CenterPosition;
                                yield return new WaitForSeconds(0.3f);
                                this.Zap(activeEnemies[i], Multiplier);
                                yield return new WaitForSeconds(0.3f);
                                this.Zap(arg2.aiActor, Multiplier);
                                yield return new WaitForSeconds(0.3f);
                                this.instances -= 1;
                                yield return null;
                            }
                            if (!this.onCooldown && arg2 == null || arg2.healthHaver.IsDead == true || !arg2.healthHaver.IsAlive)
                            {
                                    this.onCooldown = true;
                                    GameManager.Instance.StartCoroutine(StartCooldown());
                                    GameManager.Instance.StartCoroutine(RadGoop(this.Position));
                                    yield return null;
          
                            }

                        }
                    }
                }
            }
            yield break;
        }
        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(2f);
            this.onCooldown = false;
            yield break;
        }
        private IEnumerator StartCooldown1()
        {
            yield return new WaitForSeconds(0.5f);
            this.cooldown2 = false;
            yield break;
        }
        public IEnumerator RadGoop(Vector2 position)
        {
            this.Goopem(position);
            float elapsed = 0f;
            bool staph = new bool();
            while (elapsed < 0.09f && !staph)
            {
                if (elapsed > 0.09f)
                {
                    staph = true;
                }
                elapsed += BraveTime.DeltaTime;
                yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(0.2f);
                bool flag2 = this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null;
                if (flag2)
                {
                    List<AIActor> activeEnemies = this.player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    if (activeEnemies != null)
                    {
                        int count = activeEnemies.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Vector2.Distance(activeEnemies[i].CenterPosition, position) < 4.5f)
                            {
                                GameManager.Instance.StartCoroutine(Irradiate(activeEnemies[i].specRigidbody));
                            }
                        }
                    }
                }
                yield return null;
            }
            yield break;
        }

        private void Goopem(Vector2 position)
        {
                goopDefinition = ScriptableObject.CreateInstance<GoopDefinition>();
                goopDefinition.CanBeIgnited = false;
                goopDefinition.damagesEnemies = false;
                goopDefinition.damagesPlayers = false;
                goopDefinition.baseColor32 = new Color32(200, 255, 0, 255);
                goopDefinition.CanBeFrozen = false;
                goopDefinition.CanBeElectrified = false;
                goopDefinition.goopTexture = ResourceExtractor.GetTextureFromResource("ClassLibrary1/Resources/BaseGoop.png");
                DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
                goopManagerForGoopType.TimedAddGoopCircle(position, 4.5f, 0.5f, false);
            
        }

        private void Zap(AIActor ai, int multiplier)
        {
            if(multiplier > 3)
            { multiplier = 3;}
            ai.aiActor.healthHaver.ApplyDamage(2f * multiplier * player.stats.GetStatValue(PlayerStats.StatType.Damage), Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
            GlobalSparksDoer.DoRadialParticleBurst(20, ai.specRigidbody.HitboxPixelCollider.UnitBottomLeft, ai.specRigidbody.HitboxPixelCollider.UnitTopRight, 360f, 2f, 0f, null, null, Color.yellow, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);


        }








        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private bool onCooldown;
        private GoopDefinition goopDefinition;
        private List<AIActor> activeEnemies2;
        private Vector2 Position;
        private bool cooldown2;
        private bool onCooldown2;
        private int instances;
        private bool onCooldown3;
        private int instances2;
    }

    internal class SlagProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {
                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                this.m_owner = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.AdjustPlayerProjectileTint(new Color32(120, 0, 255, 255), 100, 0);
                    component.Shooter = man.specRigidbody;
                    component.OnHitEnemy = this.Rad;
                    projectile.OnDestruction += this.Goo;

                }
            }
        }

        private void Goo(Projectile obj)
        {
            GameManager.Instance.StartCoroutine(RadGoop(this.Position));
        }

        private void Rad(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            GlobalSparksDoer.DoRadialParticleBurst(10, arg2.HitboxPixelCollider.UnitBottomLeft, arg2.specRigidbody.HitboxPixelCollider.UnitTopRight, 360f, 2f, 0f, null, null, new Color32(120, 0, 255, 255), GlobalSparksDoer.SparksType.DARK_MAGICKS);
            GameManager.Instance.StartCoroutine(Irradiate(arg2));

        }

        public IEnumerator Irradiate(SpeculativeRigidbody arg2)
        {
            bool flag2 = this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null;
            if (flag2 && !this.cooldown2)
            {
                this.cooldown2 = true;
                GameManager.Instance.StartCoroutine(StartCooldown1());
                this.Position = arg2.UnitCenter;
                List<AIActor> activeEnemies = this.player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    int count = activeEnemies.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Vector2.Distance(activeEnemies[i].CenterPosition, this.Position) < 2.5f && !arg2.gameObject.GetComponent<AiactorSlagFlag>())
                        {
                            arg2.aiActor.gameObject.AddComponent<AiactorSlagFlag>();
                            while (arg2 != null && arg2.healthHaver.IsDead != true && arg2.healthHaver.IsAlive && this.instances < 10)
                            {
                                this.instances += 1;
                                this.Position = activeEnemies[i].CenterPosition;
                                yield return new WaitForSeconds(0.1f);
                                this.Zap(activeEnemies[i]);
                                yield return new WaitForSeconds(0.1f);
                                this.Zap(arg2.aiActor);
                                yield return new WaitForSeconds(0.1f);
                                this.instances -= 1;
                                yield return null;
                            }
                            if (!this.onCooldown)
                            {
                                this.onCooldown = true;
                                GameManager.Instance.StartCoroutine(StartCooldown());
                                GameManager.Instance.StartCoroutine(RadGoop(this.Position));
                                yield return null;
                            }

                        }
                    }
                }
            }
            yield break;
        }
        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(2f);
            this.onCooldown = false;
            yield break;
        }
        private IEnumerator StartCooldown1()
        {
            yield return new WaitForSeconds(0.5f);
            this.cooldown2 = false;
            yield break;
        }
        private IEnumerator RadGoop(Vector2 position)
        {
            this.Goopem(position);
            float elapsed = 0f;
            bool staph = new bool();
            while (elapsed < 0.09f && !staph)
            {
                if (elapsed > 0.09f)
                {
                    staph = true;
                }
                elapsed += BraveTime.DeltaTime;
                yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(0.2f);
                bool flag2 = this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null;
                if (flag2)
                {
                    List<AIActor> activeEnemies = this.player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    if (activeEnemies != null)
                    {
                        int count = activeEnemies.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Vector2.Distance(activeEnemies[i].CenterPosition, position) < 2.5f)
                            {
                                GameManager.Instance.StartCoroutine(Irradiate(activeEnemies[i].specRigidbody));
                            }
                        }
                    }
                }
                yield return null;
            }
            yield break;
        }

        private void Goopem(Vector2 position)
        {
            goopDefinition = ScriptableObject.CreateInstance<GoopDefinition>();
            goopDefinition.CanBeIgnited = false;
            goopDefinition.damagesEnemies = false;
            goopDefinition.damagesPlayers = false;
            goopDefinition.baseColor32 = new Color32(120, 0, 255, 255);
            goopDefinition.CanBeFrozen = false;
            goopDefinition.CanBeElectrified = false;
            goopDefinition.goopTexture = ResourceExtractor.GetTextureFromResource("ClassLibrary1/Resources/BaseGoop.png");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(position, 2.5f, 0.5f, false);
        }

        private void Zap(AIActor ai)
        {

            bool flag = ai != null;
            if (flag)
            {
                AIActorDebuffEffect aiactorDebuffEffect = null;
                foreach (AttackBehaviorBase attackBehaviorBase in EnemyDatabase.GetOrLoadByGuid((PickupObjectDatabase.GetById(492) as CompanionItem).CompanionGuid).behaviorSpeculator.AttackBehaviors)
                {
                    bool flag2 = attackBehaviorBase is WolfCompanionAttackBehavior;
                    if (flag2)
                    {
                        aiactorDebuffEffect = (attackBehaviorBase as WolfCompanionAttackBehavior).EnemyDebuff;
                    }
                }
                bool flag3 = aiactorDebuffEffect != null;
                if (flag3)
                {
                    ai.ApplyEffect(aiactorDebuffEffect, 20f, null);
                    Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(ai.sprite);
                    outlineMaterial.SetColor("_OverrideColor", new Color(30f, 0f, 40f));
                }
            }


        }










        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private bool onCooldown;
        private GoopDefinition goopDefinition;
        private List<AIActor> activeEnemies2;
        private Vector2 Position;
        private bool cooldown2;
        private bool onCooldown2;
        private int instances;
    }
    internal class FireGoop : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {
                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                this.m_owner = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.AppliesFire = true;
                    component.Shooter = man.specRigidbody;
                    component.OnHitEnemy = this.Rad;
                    projectile.OnDestruction += this.Goo;

                }
            }
        }

        private void Goo(Projectile obj)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(obj.specRigidbody.UnitCenter, 1f, 0.8f, false);
        }

        private void Rad(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {

            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(arg1.specRigidbody.UnitCenter, 1f, 0.8f, false);
        }



        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private bool onCooldown;
        private GoopDefinition goopDefinition;
        private List<AIActor> activeEnemies2;
        private Vector2 Position;
        private bool cooldown2;
        private bool onCooldown2;
        private int instances;
    }

    internal class BleedGoop : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {
                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                this.m_owner = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.OnHitEnemy = this.Rad;

                }
            }
        }


        private void Rad(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if(arg1.specRigidbody.UnitCenter != null)
            goopDefinition = ScriptableObject.CreateInstance<GoopDefinition>();
            goopDefinition.CanBeIgnited = false;
            goopDefinition.damagesEnemies = false;
            goopDefinition.damagesPlayers = false;
            goopDefinition.baseColor32 = new Color32(255, 0, 0, 255);
            goopDefinition.CanBeFrozen = false;
            goopDefinition.CanBeElectrified = false;
            goopDefinition.goopTexture = ResourceExtractor.GetTextureFromResource("ClassLibrary1/Resources/BaseGoop.png");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(arg2.specRigidbody.UnitCenter, 1.5f, 0.5f, false);
            GameManager.Instance.StartCoroutine(Bleed(arg2));
        }
        private void Sharp(SpeculativeRigidbody arg2)
        {
            PlayerController man = this.projectile.Owner as PlayerController;
            if (arg2 != null)
            {
                arg2.aiActor.healthHaver.ApplyDamage(5f * man.stats.GetStatValue(PlayerStats.StatType.Damage), Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                GlobalSparksDoer.DoRadialParticleBurst(50, arg2.specRigidbody.HitboxPixelCollider.UnitBottomLeft, arg2.specRigidbody.HitboxPixelCollider.UnitTopRight, 90f, 2f, 0f, null, null, Color.red, GlobalSparksDoer.SparksType.RED_MATTER);
            }
        }

        private IEnumerator Bleed(SpeculativeRigidbody arg2)
        {
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield return new WaitForSeconds(0.3f);
            this.Sharp(arg2);
            yield break;

        }


        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private bool onCooldown;
        private GoopDefinition goopDefinition;
        private List<AIActor> activeEnemies2;
        private Vector2 Position;
        private bool cooldown2;
        private bool onCooldown2;
        private int instances;
    }
    internal class AiactorRadFlag : MonoBehaviour
    {
        public int RadCount;

        public void Start()
        { this.RadCount = 1;
        }

    }
    internal class AiactorSlagFlag : MonoBehaviour
    {
    }

    internal class PoisonModifier : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {
                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                this.m_owner = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.OnHitEnemy = this.Rad;
                    projectile.OnDestruction += this.Goo;
                    projectile.AdjustPlayerProjectileTint(Color.green, 5, 0);

                }
            }
        }

        private void Goo(Projectile obj)
        {
            if (obj != null)
            {
                Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[347]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (this.player.CurrentGun == null) ? 0f : this.player.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {

                    component.Owner = this.player;
                    PierceProjModifier pierce = component.gameObject.GetOrAddComponent<PierceProjModifier>();
                    pierce.penetration = 3;
                    component.baseData.range = 1f;
                    component.baseData.damage = 0f;
                    component.SetOwnerSafe(this.player, "Player");
                    component.Shooter = this.player.specRigidbody;
                    component.baseData.force = 0f;
                }
            }
        }

        private void Rad(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
            {
                if (arg1 != null)
                {
                    Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[347]).DefaultModule.projectiles[0];
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg1.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (this.player.CurrentGun == null) ? 0f : this.player.CurrentGun.CurrentAngle), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    bool flag4 = component != null;
                    if (flag4)
                    {

                        component.Owner = this.player;
                        PierceProjModifier pierce = component.gameObject.GetOrAddComponent<PierceProjModifier>();
                        pierce.penetration = 3;
                        component.baseData.range = 1f;
                        component.baseData.damage = 0f;
                        component.SetOwnerSafe(this.player, "Player");
                        component.Shooter = this.player.specRigidbody;
                    }
                } }



        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private bool onCooldown;
        private GoopDefinition goopDefinition;
        private List<AIActor> activeEnemies2;
        private Vector2 Position;
        private bool cooldown2;
        private bool onCooldown2;
        private int instances;
    }
    internal class IcyBullets : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
            public void Start()
            {
                {
                    this.projectile = base.GetComponent<Projectile>();
                    this.player = (this.projectile.Owner as PlayerController);
                    this.m_owner = (this.projectile.Owner as PlayerController);
                    Projectile projectile = this.projectile;
                    PlayerController man = projectile.Owner as PlayerController;
                    Projectile component = gameObject.GetComponent<Projectile>();
                    bool flag4 = component != null;
                    if (flag4)
                    {
                        component.SetOwnerSafe(man, "Player");
                        component.Shooter = man.specRigidbody;
                        component.OnHitEnemy = this.Rad;
                        projectile.OnDestruction += this.Goo;
                        projectile.AdjustPlayerProjectileTint(Color.cyan, 5, 0);

                    }
                }
            }

        private void Goo(Projectile obj)
        {
            if (obj != null)
            {
                Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[223]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (this.player.CurrentGun == null) ? 0f : this.player.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {

                    component.Owner = this.player;
                    PierceProjModifier pierce = component.gameObject.AddComponent<PierceProjModifier>();
                    pierce.penetration = 3;
                    component.baseData.range = 1f;
                    component.baseData.damage = 0f;
                    component.SetOwnerSafe(this.player, "Player");
                    component.Shooter = this.player.specRigidbody;
                    component.baseData.force = 0f;
                }
            }
        }

        private void Rad(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg1 != null)
            {

                Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[223]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg1.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (this.player.CurrentGun == null) ? 0f : this.player.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {

                    component.Owner = this.player;
                    component.baseData.damage = 25f * this.player.stats.GetStatValue(PlayerStats.StatType.Damage);
                    PierceProjModifier pierce = component.gameObject.AddComponent<PierceProjModifier>();
                    pierce.penetration = 3;
                    component.baseData.range = 1f;
                    component.baseData.damage = 0f;
                    component.SetOwnerSafe(this.player, "Player");
                    component.Shooter = this.player.specRigidbody;
                    component.freezeEffect.FreezeAmount = 400 / player.CurrentGun.ClipCapacity;
                }
            }
        }
        

        



        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private bool onCooldown;
        private GoopDefinition goopDefinition;
        private List<AIActor> activeEnemies2;
        private Vector2 Position;
        private bool cooldown2;
        private bool onCooldown2;
        private int instances;
    }
    internal class FireModifier : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {
                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                this.m_owner = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.SetOwnerSafe(man, "Player");
                    component.Shooter = man.specRigidbody;
                    component.OnHitEnemy = this.Rad;
                    projectile.OnDestruction += this.Goo;
                    projectile.AdjustPlayerProjectileTint(Color.red, 5, 0);

                }
            }
        }

        private void Goo(Projectile obj)
        {
            if (obj != null)
            {
                Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[275]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (this.player.CurrentGun == null) ? 0f : this.player.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {

                    component.Owner = this.player;
                    component.baseData.damage = 25f * this.player.stats.GetStatValue(PlayerStats.StatType.Damage);
                    PierceProjModifier pierce = component.gameObject.AddComponent<PierceProjModifier>();
                    pierce.penetration = 3;
                    component.baseData.range = 1f;
                    component.baseData.damage = 0f;
                    component.SetOwnerSafe(this.player, "Player");
                    component.Shooter = this.player.specRigidbody;
                    component.baseData.force = 0f;
                }
            }
        }

        private void Rad(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg1 != null)
            {

                Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[275]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg1.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (this.player.CurrentGun == null) ? 0f : this.player.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {

                    component.Owner = this.player;
                    component.baseData.damage = 25f * this.player.stats.GetStatValue(PlayerStats.StatType.Damage);
                    PierceProjModifier pierce = component.gameObject.AddComponent<PierceProjModifier>();
                    pierce.penetration = 3;
                    component.baseData.range = 1f;
                    component.baseData.damage = 0f;
                    component.SetOwnerSafe(this.player, "Player");
                    component.Shooter = this.player.specRigidbody;
                }
            }
        }



        // Token: 0x04000047 RID: 71
        private Projectile projectile;
        private PlayerController player;
        private PlayerController m_owner;
        private AIActor aiactor;
        private bool onCooldown;
        private GoopDefinition goopDefinition;
        private List<AIActor> activeEnemies2;
        private Vector2 Position;
        private bool cooldown2;
        private bool onCooldown2;
        private int instances;
    }
    internal class ExplosiveProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.OnHitEnemy += Flak;
                    component.AdjustPlayerProjectileTint(Color.white, 5, 0);
                }
            }
        }


        private void Flak(Projectile obj, SpeculativeRigidbody body, bool bol)
        {
            if (obj != null)
            {

                PlayerController man = (this.projectile.Owner as PlayerController);
                ExplosiveProjectile.m_bomb = PickupObjectDatabase.GetById(567).GetComponent<FireVolleyOnRollItem>();
                ProjectileModule projectilemod = ExplosiveProjectile.m_bomb.ModVolley.projectiles[0];
                Projectile projectile = projectilemod.GetCurrentProjectile();
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-15f, 15f)), true);
                Projectile component = gameObject.GetComponent<Projectile>();

                bool flag = component != null;
                if (flag)
                {
                    component.Owner = man;
                    component.Shooter = man.specRigidbody;
                    component.baseData.speed = 7.5f;
                    component.SetOwnerSafe(man, "Player");
                    component.baseData.damage = 0f;
                    component.baseData.range = 2f;
                    ExplosiveModifier explosiveModifier = component.GetComponent<ExplosiveModifier>();
                    explosiveModifier.explosionData.damageRadius = 3f;
                    explosiveModifier.explosionData.damage = obj.baseData.damage / 2;
                    explosiveModifier.explosionData.damageToPlayer = 0f;
                    explosiveModifier.explosionData.force = 0f;

                }
            }
        }
        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private static FireVolleyOnRollItem m_bomb;
    }
    internal class SwordExplosiveProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag = component != null;
                if (flag)
                {
                    component.Owner = man;
                    component.Shooter = man.specRigidbody;
                    component.baseData.speed = 9.5f;
                    component.SetOwnerSafe(man, "Player");
                    if (component.gameObject.GetComponent<PierceProjModifier>())
                    { Destroy(component.gameObject.GetComponent<PierceProjModifier>()); }
                    Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[593]).DefaultModule.projectiles[0];
                    ExplosiveModifier boomer = Boomprojectile.gameObject.GetComponent<ExplosiveModifier>();
                    ExplosiveModifier exploder = projectile.gameObject.AddComponent<ExplosiveModifier>();
                    exploder.explosionData = boomer.explosionData;
                    exploder.explosionData.doScreenShake = false;
                    exploder.IgnoreQueues = true;
                    projectile.OnDestruction += Flak;
                    projectile.OnDestruction += Flak;
                    projectile.OnDestruction += Flak;

                }
            }
        }


        private void Flak(Projectile obj)
        {
            if (obj != null)
            {

                PlayerController man = (this.projectile.Owner as PlayerController);
                Projectile projectile1 = ((Gun)ETGMod.Databases.Items[417]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile1.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle + 180f + UnityEngine.Random.Range(-45f, 45f)), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag = component != null;
                if (flag)
                {
                    component.Owner = man;
                    component.Shooter = man.specRigidbody;
                    component.baseData.speed = 9.5f;
                    component.baseData.range = 4f;
                    component.SetOwnerSafe(man, "Player");
                    component.AdditionalScaleMultiplier = 0.4f;

                }
            }
        }
        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private static FireVolleyOnRollItem m_bomb;
    }
    internal class EnemyRocketProjectile : MonoBehaviour
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
        public void Start()
        {
            {

                this.projectile = base.GetComponent<Projectile>();
                this.player = (this.projectile.Owner as PlayerController);
                Projectile projectile = this.projectile;
                PlayerController man = projectile.Owner as PlayerController;
                Projectile component = gameObject.GetComponent<Projectile>();
                bool flag4 = component != null;
                if (flag4)
                {
                    component.OnHitEnemy += Flak;
                }
            }
        }


        private void Flak(Projectile obj, SpeculativeRigidbody spec, bool bol)
        {
            if (!spec.healthHaver.IsBoss && spec.aiActor != null)
            {
                GameManager.Instance.StartCoroutine(HandleEnemySuck(spec.aiActor, obj));
                PlayerController man = (this.projectile.Owner as PlayerController);
                Projectile projectile = ((Gun)ETGMod.Databases.Items[372]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : man.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                GameManager.Instance.StartCoroutine(HandleEnemySuck(spec.aiActor, component));

                bool flag = component != null;
                if (flag)
                {
                    component.AdditionalScaleMultiplier = 0.3f;
                    component.Owner = man;
                    component.Shooter = man.specRigidbody;
                    component.baseData.speed = 8f;
                    component.SetOwnerSafe(man, "Player");
                    component.baseData.force = 0f;
                    BounceProjModifier bouncer = projectile.gameObject.AddComponent<BounceProjModifier>();
                    bouncer.numberOfBounces = 3;
                    ExplosiveModifier exploder = projectile.gameObject.GetComponent<ExplosiveModifier>();
                    exploder.IgnoreQueues = true;
                    exploder.explosionData.force = 0f;
                    exploder.explosionData.doScreenShake = false;
                }

            }
        }
        private IEnumerator HandleEnemySuck(AIActor target, Projectile hole)
        {
            Transform copySprite = this.CreateEmptySprite(target);
            target.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.Void, DamageCategory.Normal, true, null, false);
            target.EraseFromExistenceWithRewards(false);
            Vector3 startPosition = copySprite.transform.position;
            float elapsed = 0f;
            float duration = 1f;
            while (hole != null)
            {
                elapsed += BraveTime.DeltaTime;
                bool flag1 = copySprite;
                if (flag1)
                {
                    float t = elapsed / duration * (elapsed / duration);
                    copySprite.position = new Vector3(hole.specRigidbody.transform.position.x, hole.specRigidbody.transform.position.y, hole.specRigidbody.transform.position.z - 5f);
                    copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
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
        // Token: 0x04000047 RID: 71
        private Projectile projectile;

        // Token: 0x04000048 RID: 72
        private PlayerController player;
        private static FireVolleyOnRollItem m_bomb;
    }

internal class FlamingChainsaw : MonoBehaviour
{
    // Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
    public void Start()
    {
        {
            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            this.m_owner = (this.projectile.Owner as PlayerController);
            Projectile projectile = this.projectile;
            PlayerController man = projectile.Owner as PlayerController;
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag4 = component != null;
            if (flag4)
            {
                component.SetOwnerSafe(man, "Player");
                component.Shooter = man.specRigidbody;
                component.OnHitEnemy = this.Rad;
                projectile.specRigidbody.OnTileCollision += Bang;
                GameManager.Instance.StartCoroutine(Flametrail(component));
                GameObject original = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Table_Exhaust");
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, component.transform.position, Quaternion.Euler(0f, 0f, 0f));
                gameObject.transform.parent = component.transform;

            }
        }
    }

    private void Bang(CollisionData tileCollision)
    {
        Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[129]).DefaultModule.projectiles[0];
        ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
        boomer.explosionData.damageToPlayer = 0f;
        Exploder.Explode(tileCollision.Contact, boomer.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
    }

    private IEnumerator Flametrail(Projectile component)
    {
        while(component != null) 
        {
            Fierypoop(component);
            yield return null;
        }
        yield break;
    }
    private void Fierypoop(Projectile obj)
    {
        AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
        GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
        DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition).TimedAddGoopCircle(obj.sprite.WorldBottomCenter, 1f, 1f, false);
    }
    private void Rad(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
    {
        if (arg1 != null && arg2 != null)
        {
            GameManager.Instance.StartCoroutine(Sawem(arg2));
           
            
        }
    }

    private IEnumerator Sawem(SpeculativeRigidbody arg2)
    {
        float elapsed = 0;
        Vector2 ExplosionPoint = arg2.UnitCenter;
        while (elapsed < 10f && arg2 != null)
            {
                elapsed += 1f;
                
                ExplosionPoint = arg2.UnitCenter;
                TeleporterPrototypeItem teleporter = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>();
                UnityEngine.Object.Instantiate<GameObject>(teleporter.TelefragVFXPrefab, arg2.UnitCenter, Quaternion.identity);
                arg2.healthHaver.ApplyDamage(5f, Vector2.zero, "flamesaw");
            yield return new WaitForSeconds(0.3f);
            yield return null;
            }
        
        Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[129]).DefaultModule.projectiles[0];
        ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
        boomer.explosionData.damageToPlayer = 0f;
        Exploder.Explode(ExplosionPoint, boomer.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
        yield break;
    }






    // Token: 0x04000047 RID: 71
    private Projectile projectile;
    private PlayerController player;
    private PlayerController m_owner;
    private AIActor aiactor;
    private bool onCooldown;
    private GoopDefinition goopDefinition;
    private List<AIActor> activeEnemies2;
    private Vector2 Position;
    private bool cooldown2;
    private bool onCooldown2;
    private int instances;
}


