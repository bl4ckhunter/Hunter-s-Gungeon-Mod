using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;
using MultiplayerBasicExample;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class Flamer : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Artificier's Gloves";
            string resourceName = "ClassLibrary1/Resources/Artifact"; ;
            GameObject gameObject = new GameObject();
            Flamer Flamer = gameObject.AddComponent<Flamer>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Hot and Cold";
            string longDesc = "Charge the tank by dealing damage, pressing reload with fuel in the tank depletes the fuel and sprays flames or ice from your gun\n" + "No one knows better than an artificer that the lines between natotech and magic are blurred to the point of non-existence and if we're talking magic burning hands and cone of cold are always fan favourites.";
            Flamer.SetupItem(shortDesc, longDesc, "ror");
            Flamer.quality = PickupObject.ItemQuality.B;
            Flamer.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            Flamer.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            Flamer.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
            Flamer.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
            Flamer.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);

        }
        protected override void Update()
        {
            base.Update();
            if (base.Owner)
            {
                this.CheckDamage();
            }
        }

      

        public override void Pickup(PlayerController player)
        {
            player.OnDealtDamage += this.Chaging;
            player.OnReloadPressed += this.OnReload;
            this.m_FireImmunity = new DamageTypeModifier();
            this.m_FireImmunity.damageMultiplier = 0f;
            this.m_FireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_FireImmunity);
            this.enabled = false;
            base.Pickup(player);
        }

 
                 
   

        private void Chaging(PlayerController arg1, float arg2)
        {

            if (!this.enabled){
                this.charge += arg2/2;
                if (this.charge > 200f)
                {
                    this.charge = 200f;
                }
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            
            player.OnReloadPressed -= this.OnReload;
            player.OnDealtDamage -= this.Chaging;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnReloadPressed -= this.OnReload;
                player.OnDealtDamage -= this.Chaging;
            }
            base.OnDestroy();
        }

        private void OnReload(PlayerController usingPlayer, Gun gun)
        {

            if (this.charge > 0)
            {
                GameManager.Instance.StartCoroutine(FlameOn());
                this.enabled = true;
            }
            
      

        }
        private  IEnumerator FlameOn()
        {
            this.enabled = true;
            while (this.charge > 0f)
            {
                yield return new WaitForSeconds(0.05f);
                this.charge -= 5f;
                if (this.m_owner.CurrentGun.sprite.WorldCenter.x - this.m_owner.specRigidbody.UnitCenter.x < 0f)
                {
                    this.FlamerSalvo();
                }
                if (this.m_owner.CurrentGun.sprite.WorldCenter.x - this.m_owner.specRigidbody.UnitCenter.x > 0f)
                {
                    this.FrostSalvo();
                }
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.CurrentGun.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
                yield return null;
            }
            this.enabled = false;
            yield break;
        }

      

    
        private void CheckDamage()
        {
            this.Damage = base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            bool flag = this.Damage == this.lastDamage;
            if (!flag)
            {
                this.FlamerTrue = this.FlamerBase * this.Damage;
                this.lastDamage = this.Damage;
            }
        }

        private void FlamerSalvo()
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            Projectile projectile = ((Gun)ETGMod.Databases.Items[89]).DefaultModule.projectiles[0];
            if (UnityEngine.Random.value > 0.5f)
            { this.sign = -1f; }
            else { this.sign = 1f; }
            float value = UnityEngine.Random.value * 7f * sign;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.CurrentGun.PrimaryHandAttachPoint.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + value), true);
            if (UnityEngine.Random.value > 0.5f)
            { this.sign = -1f; }
            else { this.sign = 1f; }
            float value1 = UnityEngine.Random.value * 7f * sign;
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.CurrentGun.PrimaryHandAttachPoint.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + value1), true);
            if (UnityEngine.Random.value > 0.5f)
            { this.sign = -1f; }
            else { this.sign = 1f; }
            float value2 = UnityEngine.Random.value * 7f * sign;
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.CurrentGun.PrimaryHandAttachPoint.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + value2), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component2 = gameObject2.GetComponent<Projectile>();
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = base.Owner;
                component.AdjustPlayerProjectileTint(new Color(UnityEngine.Random.Range(5, 40), 0, 0), 5, 0f);
                component.Shooter = base.Owner.specRigidbody;
                component.baseData.speed = 10f;
                component.AdditionalScaleMultiplier = 1.5f + UnityEngine.Random.value;
                component.SetOwnerSafe(base.Owner, "Player");
                component.baseData.damage = 0f;
                component.AppliesFire = true;
                component.FireApplyChance = 1f;
                component.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
                component3.OnDestruction += (Action<Projectile>)Delegate.Combine(component.OnHitEnemy, new Action<Projectile>(this.Fierypoop));
            }
            bool flag2 = component2 != null;
            if (flag2)
            {
                component2.Owner = base.Owner;
                component2.AdjustPlayerProjectileTint(new Color(UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40), 0), 5, 0f);
                component2.Shooter = base.Owner.specRigidbody;
                component2.baseData.speed = 10f;
                component2.SetOwnerSafe(base.Owner, "Player");
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
                component3.Owner = base.Owner;
                component3.AdjustPlayerProjectileTint(new Color(UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40)), 5, 0f);
                component3.Shooter = base.Owner.specRigidbody;
                component3.SetOwnerSafe(base.Owner, "Player");
                component3.baseData.speed = 10f;
                component3.AdditionalScaleMultiplier = 0.1f + UnityEngine.Random.value;
                component3.FireApplyChance = 1f;
                component3.baseData.damage = this.FlamerTrue;
                component3.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
                component3.OnDestruction += (Action<Projectile>)Delegate.Combine(component.OnHitEnemy, new Action<Projectile>(this.Fierypoop));

            }
        }


        private void FrostSalvo()
            {
                Projectile projectile = ((Gun)ETGMod.Databases.Items[225]).DefaultModule.projectiles[0];
                if (UnityEngine.Random.value > 0.5f)
                { this.sign = -1f; }
                else { this.sign = 1f; }
                float value = UnityEngine.Random.value * 7f * sign;
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.CurrentGun.PrimaryHandAttachPoint.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + value), true);
                if (UnityEngine.Random.value > 0.5f)
                { this.sign = -1f; }
                else { this.sign = 1f; }
                float value1 = UnityEngine.Random.value * 7f * sign;
                GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.CurrentGun.PrimaryHandAttachPoint.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + value1), true);
                if (UnityEngine.Random.value > 0.5f)
                { this.sign = -1f; }
                else { this.sign = 1f; }
                float value2 = UnityEngine.Random.value * 7f * sign;
                GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.CurrentGun.PrimaryHandAttachPoint.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + value2), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                Projectile component2 = gameObject2.GetComponent<Projectile>();
                Projectile component3 = gameObject3.GetComponent<Projectile>();
                bool flag = component != null;
                if (flag)
                {
                    component.Owner = base.Owner;
                    component.AdjustPlayerProjectileTint(new Color(0f, UnityEngine.Random.Range(5, 20), UnityEngine.Random.Range(5, 40)), 5, 0f);
                    component.Shooter = base.Owner.specRigidbody;
                    component.baseData.speed = 10f;
                    component.AdditionalScaleMultiplier = 1.5f + UnityEngine.Random.value;
                    component.SetOwnerSafe(base.Owner, "Player");
                    component.baseData.damage = 0f;
                component.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
            }
                bool flag2 = component2 != null;
                if (flag2)
                {
                    component2.Owner = base.Owner;
                    component2.AdjustPlayerProjectileTint(new Color(0f, UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40)), 5, 0f);
                component2.Shooter = base.Owner.specRigidbody;
                    component2.baseData.speed = 10f;
                    component2.SetOwnerSafe(base.Owner, "Player");
                    component.AdditionalScaleMultiplier = 1.5f + UnityEngine.Random.value;
                    component2.baseData.damage = this.FlamerTrue;
                    component2.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
            }
                bool flag3 = component3 != null;
                if (flag3)
                {
                    component3.Owner = base.Owner;
                    component3.AdjustPlayerProjectileTint(new Color(UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40), UnityEngine.Random.Range(5, 40)), 5, 0f);
                    component3.Shooter = base.Owner.specRigidbody;
                    component3.SetOwnerSafe(base.Owner, "Player");
                    component3.baseData.speed = 10f;
                    component3.AdditionalScaleMultiplier = 0.1f + UnityEngine.Random.value;
                    component3.baseData.damage = 0f;
                component3.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
                
            }

            

        }

        private void Fierypoop(Projectile obj)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition).TimedAddGoopCircle(obj.sprite.WorldBottomCenter, 3f, 1f, false);
        }

        // Token: 0x040077B6 RID: 30646
        public float ActivationChance;

        // Token: 0x040077B8 RID: 30648
        public bool TriggersRadialBulletBurst;

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("TriggersRadialBulletBurst", false)]
        public RadialBurstInterface RadialBurstSettings;

        private  bool onCooldown; private  bool onCooldown1;
        private float FlamerBase = 1f;

        // Token: 0x0400001D RID: 29
        private float FlamerTrue;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;
        private bool firingflag;
        private bool engaged;
        private float sign;
        private  bool activated;
        private float charge;
        private bool charged;
        private JetpackItem jetpack;
        private GameObject instanceJetpack;
        private object instanceJetpackSprite;
        private float colorcharge;
        private bool hand;
        private DamageTypeModifier m_FireImmunity;

    }
      
 }     