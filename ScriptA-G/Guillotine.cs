using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using Dungeonator;
using System.Collections.Generic;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class Guillotine : PassiveItem
    {
        public GameObject TelefragVFXPrefab;
        private Vector3 position;
        private bool tentacling;

        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Cursed Guillotine";
            string resourceName = "ClassLibrary1/Resources/Guillotine"; ;
            GameObject gameObject = new GameObject();
            Guillotine Guillotine = gameObject.AddComponent<Guillotine>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Cull the weak";
            string longDesc = "Enemies under 25% health are instantly executed\n" + "This guillotine has been used to execute many beings from behind the Curtain, neither the stains of eldritch blood nor the grudges of the dying could be washed away.";
            Guillotine.SetupItem(shortDesc, longDesc, "ror");
            Guillotine.quality = PickupObject.ItemQuality.A;
            ItemBuilder.AddPassiveStatModifier(Guillotine, PlayerStats.StatType.Curse, 3, StatModifier.ModifyMethod.ADDITIVE);
            Guillotine.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            Guillotine.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
        }

        protected override void Update()
        {
            base.Update();
            if (base.Owner) 
            {
                this.Stats();
            }
        }

        private void Stats() {
            List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
                foreach (AIActor ai in activeEnemies)
                {
                    if (activeEnemies != null)
                    {
                        float hp = ai.healthHaver.GetCurrentHealthPercentage();
                        bool flag = hp <= 0.25f;
                        bool flag2 = ai.healthHaver.IsBoss;
                        if (flag && !flag2)
                        { this.Execute(ai); }
                    }
                }

        }
        private void Execute(AIActor ai) {
            this.position = ai.specRigidbody.UnitBottomCenter;
            ai.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.Void, DamageCategory.Normal, true, null, false);
            ai.knockbackDoer.SetImmobile(true, "Guillotine");
            GameManager.Instance.StartCoroutine(Tentacler(ai)); }
        private IEnumerator Tentacler(AIActor ai)
        {
            this.TelefragVFXPrefab = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Tentacleport");
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TelefragVFXPrefab);
            gameObject.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(position, tk2dBaseSprite.Anchor.LowerCenter);
            gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
            gameObject.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            new WaitForSeconds(0.4f);
            ai.EraseFromExistenceWithRewards(false);
            yield break;
        }






    }

       




    }
    
      

