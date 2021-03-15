using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class List : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Gunreaper's List";
            string resourceName = "ClassLibrary1/Resources/List"; ;
            GameObject gameObject = new GameObject();
            List List = gameObject.AddComponent<List>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Kaliber's Call";
            string longDesc = "Kill exclusively marked enemies for a reward, after a marked enemy dies new mark takes 3s to appear.";
            List.SetupItem(shortDesc, longDesc, "ror");
            List.quality = PickupObject.ItemQuality.C;
            List.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            List.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        { 
            if (fatal && this.mark.healthHaver == enemy)
            { this.kills += 1;
                GameManager.Instance.StartCoroutine(this.Delay());
            }
            if (fatal && this.mark.healthHaver != enemy)
            {
                this.kills += 1;
            }
        }
        private void Reward()
        { float roll = UnityEngine.Random.value;
            if(roll <= 0.15f)
            {
                RoomHandler room = base.Owner.CurrentRoom;
                IntVector2 randomVisibleClearSpot5 = base.Owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
                Chest rainbow_Chest = GameManager.Instance.RewardManager.D_Chest;
                rainbow_Chest.IsLocked = false;
                Chest.Spawn(rainbow_Chest, randomVisibleClearSpot5);
            }
            if (roll >0.15 && roll <= 0.45f)
            { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, base.Owner); }
            if (roll > 0.45 && roll <= 0.65f)
            { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(600).gameObject, base.Owner);
            }
            if (roll > 0.65 && roll <= 0.85f)
            { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, base.Owner); }
            if (roll > 0.85 && roll <= 0.95f)
            { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, base.Owner); }
            if (roll > 0.95f)
            {
                RoomHandler room = base.Owner.CurrentRoom;
                IntVector2? pos = new IntVector2?(room.GetRandomVisibleClearSpot(2, 2));
                RewardManager rm = GameManager.Instance.RewardManager;
                Chest chest = rm.SpawnRoomClearChestAt(pos.Value);
                chest.IsLocked = false;
            }


        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(3f);
          this.NewFloor();
            yield break;
        }
        private void NewFloor()
        { List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

                int index = UnityEngine.Random.Range(0, activeEnemies.Count);
                this.count += 1;
                this.mark = activeEnemies.ElementAt(index);

            if (this.mark != null)
                {
                    
                    Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(this.mark.sprite);
                    outlineMaterial.SetColor("_OverrideColor", new Color(255f, 0f, 0f, 50f));
                    this.mark.PlayEffectOnActor((GameObject)BraveResources.Load("Global VFX/VFX_LockOn_Predator", ".prefab"), Vector3.zero, true, true, true);
                }

        }
        private void NewFloor1()
        {
            this.kills = 0;
            List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                int index = UnityEngine.Random.Range(0, activeEnemies.Count);
                this.count = 1;
                this.mark = activeEnemies.ElementAt(index);
            }
            if (this.mark != null)
            {
                this.mark.PlayEffectOnActor((GameObject)BraveResources.Load("Global VFX/VFX_LockOn_Predator", ".prefab"), Vector3.zero, true, true, true);
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(this.mark.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(255f, 0f, 0f, 50f));
            }

        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            player.OnEnteredCombat = (Action)Delegate.Combine(player.OnEnteredCombat, new Action(this.NewFloor1));
            player.OnRoomClearEvent += this.Tally;
        }

        private void Tally(PlayerController obj)
        {
            if (this.kills <= this.count)
            { this.Reward(); }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= OnEnemyDamaged;
            player.OnEnteredCombat -= NewFloor1;
            DebrisObject result = base.Drop(player);
            return result;
        }

        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnAnyEnemyReceivedDamage -= OnEnemyDamaged;
                player.OnEnteredCombat -= NewFloor1;
            }
            base.OnDestroy();
        }



        private float charge;
        private bool m_radialIndicatorActive;
        private HeatIndicatorController m_radialIndicator;
        private int count;
        private AIActor mark;
        private int kills;
    }
      
 }     