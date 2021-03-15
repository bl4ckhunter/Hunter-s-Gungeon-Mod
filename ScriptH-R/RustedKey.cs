using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using Dungeonator;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class Key : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Rusty Key";
            string resourceName = "ClassLibrary1/Resources/Rusted_Key";
            GameObject gameObject = new GameObject();
            Key Key = gameObject.AddComponent<Key>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Hidden cache";
            string longDesc = "A token of friendship from Flynt, its value is purely sentimental as the box this key opens is not even on the same planet as the gungeon and probably long gone\n" + "but the sentient locks that inhabit the place will leave you an open chest in the first cleared room of every floor out of goodwill.";
            Key.SetupItem(shortDesc, longDesc, "ror");
            Key.CanBeDropped = false;
            Key.quality = PickupObject.ItemQuality.C;
            Key.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
        }
        public override void Pickup(PlayerController player)
        {
            Key.Value = 2f;
            player.OnNewFloorLoaded += this.Thing;
            player.OnRoomClearEvent += this.Chest;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {

            player.OnNewFloorLoaded -= this.Thing;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnNewFloorLoaded -= this.Thing;
            }
            base.OnDestroy();
        }

        private void Thing (PlayerController player)
        { Key.Value = 2f;}
        public void Chest(PlayerController player)
        {
            bool flag = Key.Value > 1f;
            if (flag)
            {
                RoomHandler room = player.CurrentRoom;
                IntVector2? pos = new IntVector2?(room.GetRandomVisibleClearSpot(2, 2));
                RewardManager rm = GameManager.Instance.RewardManager;
                Chest chest = rm.SpawnRoomClearChestAt(pos.Value);
                chest.IsLocked = false;
                Key.Value = 0f;
            }
        }

        private static float Value;

    } 
}