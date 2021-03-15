using System.Linq;
using ItemAPI;
using Steamworks;
using UnityEngine;
using Gungeon;
using System;
using System.Collections.Generic;
using System.Collections;
using Mod;

namespace Items {
    public class StickyProjectile : MonoBehaviour
    {
        public StickyProjectile()
        {
            this.sourceVector = Vector2.zero;
            this.shouldExplodeOnReload = false;
            this.maxLifeTime = 100f;
            this.destroyOnGunChanged = false;
            explosionDamageBasedOnProjectileDamage = false;
            hasDetTimer = false;
        }
        private void Start()
        {
            sourceProjectile = base.GetComponent<Projectile>();
            if (sourceProjectile.Owner is PlayerController)
            {
                player = sourceProjectile.Owner as PlayerController;
            }
            sourceGun = player.CurrentGun;
            sourceProjectile.OnHitEnemy += OnHit;
        }
        private void OnHit(Projectile self, SpeculativeRigidbody enemy, bool fatal)
        {

            sourceVector = self.LastVelocity;
            if (enemy.aiActor)
            {
                
                StickProjectileToEnemy sticky = enemy.aiActor.gameObject.AddComponent<StickProjectileToEnemy>();
                sticky.destroyOnGunChanged = destroyOnGunChanged;
                sticky.shouldExplodeOnReload = shouldExplodeOnReload;
                sticky.explosionDamageBasedOnProjectileDamage = explosionDamageBasedOnProjectileDamage;
                sticky.explosionData = explosionData;
                sticky.maxLifeTime = maxLifeTime;
                sticky.sourceProjectile = sourceProjectile;
                sticky.sourceVector = sourceVector;
                sticky.player = player;
                sticky.sourceGun = sourceGun;
                sticky.hasDetTimer = hasDetTimer;
                sticky.detTimer = detTimer;
            }
        }
        public bool destroyOnGunChanged;
        public bool shouldExplodeOnReload;
        public bool explosionDamageBasedOnProjectileDamage;
        public bool hasDetTimer;
        public ExplosionData explosionData;
        public float maxLifeTime;
        public float detTimer;
        private Projectile sourceProjectile;
        private Vector2 sourceVector;
        private PlayerController player;
        private Gun sourceGun;
    }
    public class StickProjectileToEnemy : MonoBehaviour
{
        private void Start()
    {
        projSprite1 = sourceProjectile.GetComponent<tk2dSprite>();
            CreateEmptySprite(sourceProjectile);
        }

        private void CreateEmptySprite(Projectile target)
        {
            GameObject gameObject = new GameObject("suck image");
            gameObject.layer = target.gameObject.layer;
            gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject);
            tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
            tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            GameObject boomprefab1 = UnityEngine.Object.Instantiate<GameObject>(gameObject, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
            base.GetComponent<tk2dBaseSprite>().AttachRenderer(tk2dSprite);
            tk2dSprite.PlaceAtLocalPositionByAnchor(base.GetComponent<tk2dBaseSprite>().sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
            tk2dSprite.IsPerpendicular = true;
            tk2dSprite.HeightOffGround = 0.1f;
            tk2dSprite.transform.rotation = Quaternion.Euler(0, 0, sourceVector.ToAngle());
            tk2dSprite.transform.parent = base.GetComponent<SpeculativeRigidbody>().transform;
            Stickprefab = boomprefab1;
            RoRItems.CreatedStickyprojs.Add(boomprefab1);
            if (RoRItems.CreatedStickyprojs.Count > 20) {
                GameObject gobject = RoRItems.CreatedStickyprojs[0];
                RoRItems.CreatedStickyprojs.Remove(gobject);
                Destroy(gobject);}
            GameManager.Instance.StartCoroutine(Stickit(boomprefab1,base.GetComponent<SpeculativeRigidbody>()));
        }

        private IEnumerator Stickit(GameObject boomprefab1, SpeculativeRigidbody target)
        { float elapsed = new float();
            while(target != null && boomprefab1 != null && elapsed < maxLifeTime) 
            {
                elapsed += BraveTime.DeltaTime;
                boomprefab1.transform.position = new Vector3(target.UnitCenter.x, target.UnitCenter.y + (target.UnitHeight / 3)*2 , target.transform.position.z); //sprite position
                boomprefab1.transform.rotation = Quaternion.Euler(0, 0, sourceVector.ToAngle());
                boomprefab1.GetComponent<tk2dBaseSprite>().HeightOffGround = base.GetComponent<tk2dBaseSprite>().HeightOffGround + (target.UnitHeight / 3) * 2 + 0.8f;  // Sprite height
                boomprefab1.GetComponent<tk2dBaseSprite>().UpdateZDepth();

                yield return null;
            }
            Destroy(boomprefab1.gameObject);
            if(RoRItems.CreatedStickyprojs.Contains(boomprefab1))
            RoRItems.CreatedStickyprojs.Remove(boomprefab1);
            yield break;
        }

        


    public bool destroyOnGunChanged;
    public bool shouldExplodeOnReload;
    public bool explosionDamageBasedOnProjectileDamage;
    public bool hasDetTimer;
    public ExplosionData explosionData;
    public float maxLifeTime;
    public float detTimer;
    public Projectile sourceProjectile;
    public Vector2 sourceVector;
    public PlayerController player;
    public Gun sourceGun;
    private tk2dSprite projSprite;
    private tk2dSprite enemySprite;
    private float timer;
        private static GameObject Stickprefab;
        private static tk2dSprite projSprite1;
        private Transform copySprite;
        private tk2dBaseSprite tk2dBaseSprite;
    }
}
public class ElementalElites : MonoBehaviour
{
    private void Start()
    {
        aiactor = base.GetComponent<AIActor>();
        BuildPrefab();
        base.GetComponent<AIActor>().EnemyScale = new Vector2(1.3f, 1.3f);
        if (overrideElement == false)
        {
            int RandomElement = UnityEngine.Random.Range(0, 6);
            ElementType = RandomElement;
        }
        int Element = ElementType;
        if (Element == 1)
        {
            this.Immunity = new DamageTypeModifier();
            this.Immunity.damageMultiplier = 0f;
            this.Immunity.damageType = CoreDamageTypes.Poison;
            base.GetComponent<HealthHaver>().damageTypeModifiers.Add(this.Immunity);
            if (aiactor.GetComponent<AIShooter>())
            {
                aiactor.GetComponent<AIShooter>().PostProcessProjectile += GoopProjectile;
            }
            else
            { GameManager.Instance.StartCoroutine(GoopTrail(aiactor)); }
        }
        if (Element == 2)
        {
            this.Immunity = new DamageTypeModifier();
            this.Immunity.damageMultiplier = 0f;
            this.Immunity.damageType = CoreDamageTypes.Fire;
            base.GetComponent<HealthHaver>().damageTypeModifiers.Add(this.Immunity);
            if (aiactor.GetComponent<AIShooter>())
            {
                aiactor.GetComponent<AIShooter>().PostProcessProjectile += GoopProjectile;
            }
            else
            { GameManager.Instance.StartCoroutine(GoopTrail(aiactor)); }
        }
        if (Element == 3)
        {
            this.Immunity = new DamageTypeModifier();
            this.Immunity.damageMultiplier = 0f;
            this.Immunity.damageType = CoreDamageTypes.Electric;
            base.GetComponent<HealthHaver>().damageTypeModifiers.Add(this.Immunity);
            if (aiactor.GetComponent<AIShooter>())
            {
                aiactor.GetComponent<AIShooter>().PostProcessProjectile += GoopProjectile;
            }
            else
            { GameManager.Instance.StartCoroutine(GoopTrail(aiactor)); }
        }
        if (Element == 4)
        {
            if (aiactor.GetComponent<AIShooter>())
            {
                aiactor.GetComponent<AIShooter>().PostProcessProjectile += GoopProjectile;
            }
            else
            { GameManager.Instance.StartCoroutine(GoopTrail(aiactor)); }
        }
        if(Element == 5)
        { Cursem();
            if (aiactor.GetComponent<AIShooter>())
            {
                aiactor.GetComponent<AIShooter>().PostProcessProjectile += CurseProjectile;
            }
        }


    }

    private void CurseProjectile(Projectile obj)
    {
        obj.IsBlackBullet = true;
    }

    private void Cursem()
    {
        GameManager.Instance.StartCoroutine(CurseCircleVfx());
    }

    private IEnumerator CurseCircleVfx()
    {
        GameObject boomprefab1 = UnityEngine.Object.Instantiate<GameObject>(curseprefab, aiactor.CenterPosition, Quaternion.Euler(0f, 0f, 0f));
        boomprefab1.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(aiactor.CenterPosition, tk2dBaseSprite.Anchor.LowerCenter);
        while (aiactor != null)
        {
            boomprefab1.transform.position = aiactor.specRigidbody.UnitCenter;
            if (Vector2.Distance(GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter, aiactor.specRigidbody.UnitCenter) < 8.9)
            { DoCurse(GameManager.Instance.PrimaryPlayer); }
            if (GameManager.Instance.SecondaryPlayer && Vector2.Distance(GameManager.Instance.SecondaryPlayer.specRigidbody.UnitCenter, aiactor.specRigidbody.UnitCenter) < 8.9)
            { DoCurse(GameManager.Instance.PrimaryPlayer); }
            yield return null;
        }
        yield break;
    }

    public static void BuildPrefab()
    {
        GameObject gameObject = SpriteBuilder.SpriteFromResource("ClassLibrary1/Resources/BoomSprites/cursecircle", null, true);
        gameObject.SetActive(false);
        FakePrefab.MarkAsFakePrefab(gameObject);
        UnityEngine.Object.DontDestroyOnLoad(gameObject);
        GameObject gameObject2 = new GameObject("Cursering");
        tk2dSprite tk2dSprite = gameObject2.AddComponent<tk2dSprite>();
        tk2dSprite.SetSprite(gameObject.GetComponent<tk2dBaseSprite>().Collection, gameObject.GetComponent<tk2dBaseSprite>().spriteId);
        tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
        FakePrefab.MarkAsFakePrefab(gameObject2);
        UnityEngine.Object.DontDestroyOnLoad(gameObject2);
        ElementalElites.curseprefab = gameObject2;
    }
    private IEnumerator GoopTrail(AIActor actor)
    {
        Vector2 position = actor.specRigidbody.UnitCenter;
        DeadlyDeadlyGoopManager goop = new DeadlyDeadlyGoopManager();
        if (ElementType == 1)
        {
            goop = PoisonGoo();
        }
        if (ElementType == 2)
        {
            goop = FireGoo();
        }
        if (ElementType == 3)
        {
            goop = WaterGoo();
        }
        if (ElementType == 4)
        {
            goop = WaterGoo();
        }

        while (position != null)
        {
            new WaitForSeconds(0.35f);
            goop.AddGoopCircle(position, 2.5f);
            if (ElementType == 3)
            {
                goop.ElectrifyGoopCircle(position, 2.5f);
            }
            if (ElementType == 4)
            {
                goop.FreezeGoopCircle(position, 2.5f);
            }
            yield return null;
        }
        yield break;
    }

    private void GoopProjectile(Projectile obj)
    {
        GameManager.Instance.StartCoroutine(GoopTrailProjectile(obj));
    }

    private IEnumerator GoopTrailProjectile(Projectile obj)
    {
        DeadlyDeadlyGoopManager goop = new DeadlyDeadlyGoopManager();
        if (ElementType == 1)
        {
            goop = PoisonGoo();
        }
        if (ElementType == 2)
        {
            goop = FireGoo();
        }
        if (ElementType == 3)
        {
            goop = WaterGoo();
        }
        if (ElementType == 4)
        {
            goop = WaterGoo();
        }

        while (obj.specRigidbody.UnitCenter != null)
        {
            new WaitForSeconds(0.35f);
            goop.AddGoopCircle(obj.specRigidbody.UnitCenter, 1.5f);
            if (ElementType == 3)
            {
                goop.ElectrifyGoopCircle(obj.specRigidbody.UnitCenter, 1.5f);
            }
            if (ElementType == 4)
            {
                goop.FreezeGoopCircle(obj.specRigidbody.UnitCenter, 1.5f);
            }
            yield return null;
        }
        yield break;
    }

    private DeadlyDeadlyGoopManager FireGoo()
    {
        AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
        GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
        DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
        return goopManagerForGoopType;
    }
    private DeadlyDeadlyGoopManager PoisonGoo()
    {
        AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
        GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
        DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
        return goopManagerForGoopType;
    }
    private DeadlyDeadlyGoopManager WaterGoo()
    {
        AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
        GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/water goop.asset");
        DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
        return goopManagerForGoopType;
    }

    private void DoCurse(PlayerController targetPlayer)
    {
        if (targetPlayer.IsGhost)
        {
            return;
        }
        targetPlayer.CurrentCurseMeterValue += BraveTime.DeltaTime / 3f;
        targetPlayer.CurseIsDecaying = false;
        if (targetPlayer.CurrentCurseMeterValue > 1f)
        {
            targetPlayer.CurrentCurseMeterValue = 0f;
            StatModifier statModifier = new StatModifier();
            statModifier.amount = 1f;
            statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
            statModifier.statToBoost = PlayerStats.StatType.Curse;
            targetPlayer.ownerlessStatModifiers.Add(statModifier);
            targetPlayer.stats.RecalculateStats(targetPlayer, false, false);
        }
    }

    public bool destroyOnGunChanged;
    public bool shouldExplodeOnReload;
    public bool explosionDamageBasedOnProjectileDamage;
    public bool hasDetTimer;
    public ExplosionData explosionData;
    public float maxLifeTime;
    public float detTimer;
    public Projectile sourceProjectile;
    public Vector2 sourceVector;
    public PlayerController player;
    public Gun sourceGun;
    private tk2dSprite projSprite;
    private tk2dSprite enemySprite;
    private float timer;
    private static GameObject Stickprefab;
    private static tk2dSprite projSprite1;
    private Transform copySprite;
    private tk2dBaseSprite tk2dBaseSprite;
    private DamageTypeModifier Immunity;
    private AIActor aiactor;
    private int ElementType;
    private static GameObject curseprefab;

    public bool overrideElement { get; private set; }
}
