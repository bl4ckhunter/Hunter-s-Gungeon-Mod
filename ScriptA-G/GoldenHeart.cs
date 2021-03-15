using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using Items;
using SaveAPI;

namespace Mod
{
        //Call this method from the Start() method of your ETGModule extension
        public class GoldenHeart : PassiveItem
        {
        private GoopDefinition goopDefinition;
        private DeadlyDeadlyGoopManager goopManagerForGoopType;
        private GameActorFreezeEffect Engolden;
        private float time;

        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
            {
                string name = "Halcyon Seed";
                string resourceName = "ClassLibrary1/Resources/Hseed"; ;
                GameObject gameObject = new GameObject();
                GoldenHeart GoldenHeart = gameObject.AddComponent<GoldenHeart>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Graviton kick!";
                string longDesc = "The heart of the Golden King, thought the Spectre tore it from his body aeons ago a molten alloy of gold and brass still gushes from it."; 
                ItemBuilder.AddPassiveStatModifier(GoldenHeart, PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);
                ItemBuilder.AddPassiveStatModifier(GoldenHeart, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
            GoldenHeart.SetupItem( shortDesc, longDesc, "ror");
                GoldenHeart.quality = PickupObject.ItemQuality.B;
                GoldenHeart.SetupUnlockOnCustomStat(CustomTrackedStats.MOBIUS_CHEST_ALLTIME_MONEY, 1000f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
                GoldenHeart.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
                GoldenHeart.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
                GoldenHeart.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            BuildGoop();          
            PickupObject bobject = UnityEngine.Object.Instantiate<PickupObject>(PickupObjectDatabase.GetById(278));
            GameActorFreezeEffect Freeze = (bobject.GetComponent<BulletStatusEffectItem>().FreezeModifierEffect);
            Engolden = Freeze;
        }

        public override DebrisObject Drop(PlayerController player)
        {
           
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void Update()
        {
            base.Update();
            if (base.Owner)
            {
                

                time += BraveTime.DeltaTime;
                if(goopManagerForGoopType == null || goopDefinition == null) 
                {
                    BuildGoop();
                    PickupObject bobject = UnityEngine.Object.Instantiate<PickupObject>(PickupObjectDatabase.GetById(278));
                    GameActorFreezeEffect Freeze = (bobject.GetComponent<BulletStatusEffectItem>().FreezeModifierEffect);
                    Engolden = Freeze;
                }
                if (time > 0.04f)
                {
                    time = 0f;
                    goopManagerForGoopType.AddGoopCircle(base.Owner.CenterPosition, 2f);
                }
            }
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
            goopDefinition.lifespan = 6f;
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


        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {

            }
            base.OnDestroy();
        }
    }
}

