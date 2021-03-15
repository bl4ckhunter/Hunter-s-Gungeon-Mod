using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using System.Collections.Generic;
using System.Timers;

namespace Mod
{
        //Call this method from the Start() method of your ETGModule extension
        public class GunpowderAnt : PassiveItem
        {
        private static GameObject AntQueen;

        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
            {
                string name = "Gunt";
                string resourceName = "ClassLibrary1/Resources/Bandolier"; ;
                GameObject gameObject = new GameObject();
                GunpowderAnt bandolier = gameObject.AddComponent<GunpowderAnt>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Killing for ammo";
                string longDesc = "3% chance to spread ammo on kill \n" + "The bandolier of a long lost explorer, it's full of bullets but the gunpowder is long gone. \n" + "Thankfully the gundead have more than enough to spare";
                bandolier.SetupItem( shortDesc, longDesc, "ror");
                bandolier.quality = PickupObject.ItemQuality.A;
            BuildQueenPrefab();
            bandolier.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            bandolier.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            bandolier.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemyContext += SpawnQuen;
        }

        private void Killdaqueen(PlayerController obj)
        {
            if(queen != null)
            { Destroy(queen); }
        }

        private void SpawnQuen(PlayerController player, HealthHaver target)
        {
            if (queen == null)
            {
                GameObject boomprefab1 = UnityEngine.Object.Instantiate<GameObject>(GunpowderAnt.AntQueen, target.specRigidbody.UnitCenter, Quaternion.identity);
                boomprefab1.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(target.specRigidbody.UnitCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                GameManager.Instance.StartCoroutine(this.HandleAttack(boomprefab1));
                GameManager.Instance.StartCoroutine(this.Eatcorpse(boomprefab1));
                this.queen = boomprefab1;
            }
        }

        private IEnumerator Eatcorpse(GameObject prefab)
        {
            while (prefab != null)
            {
                for (int i = 0; i < StaticReferenceManager.AllCorpses.Count; i++)
                {
                    GameObject gameObject = StaticReferenceManager.AllCorpses[i];
                    if (gameObject && gameObject.GetComponent<tk2dBaseSprite>() && gameObject.transform.position.GetAbsoluteRoom() == base.Owner.CurrentRoom)
                    {
                        float elapsed = 0f;
                        while (gameObject != null)
                        { elapsed += BraveTime.DeltaTime;
                            Vector3 position = gameObject.transform.position;
                            float t = elapsed / 10 * (elapsed / 10);
                            if (gameObject.transform.position.x - prefab.transform.position.x < 0f)
                            {
                                prefab.transform.rotation = Quaternion.Euler(180f, 0f, 180f);
                                prefab.transform.position = Vector3.Lerp(prefab.transform.position, position + new Vector3(1.2f,0,0), t);
                            }
                            if (gameObject.transform.position.x - prefab.transform.position.x > 0f)
                            {
                                prefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                                prefab.transform.position = Vector3.Lerp(prefab.transform.position, position, t);
                            }
                            if (Vector3.Distance(prefab.transform.position, gameObject.transform.position) < 1.5f)
                            {   this.teleporter = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>();
                                UnityEngine.Object.Instantiate<GameObject>(teleporter.TelefragVFXPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, base.Owner.transform.position.z), Quaternion.identity);
                                UnityEngine.Object.Instantiate<GameObject>(teleporter.TelefragVFXPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, base.Owner.transform.position.z), Quaternion.identity);
                                isEating = true;
                                GameManager.Instance.StartCoroutine(HandleAttack2(prefab));
                                Destroy(gameObject);
                                FoodCount += 1f;
                                if (FoodCount > 6f) 
                                { FoodCount = 0f;
                                    this.Orbitalgun();
                                    
                                        }
                            }
                            yield return null;
                        }
                    }
                    
                }
                yield return null;
            }
            yield break;
        }
        public IEnumerator Killthespawn(GameObject gameObject)
        {
            yield return new WaitForSeconds(29f);
            yield return new WaitForSeconds(1f);
            UnityEngine.Object.Destroy(gameObject);
            yield break;
        }

        private IEnumerator HandleAttack2(GameObject prefab)
        {
            isEating = true;
            yield return new WaitForSeconds(0.15f);
            prefab.GetComponent<tk2dBaseSprite>().SetSprite(GunpowderAnt.spriteIds[5]);
            yield return new WaitForSeconds(0.15f);
            prefab.GetComponent<tk2dBaseSprite>().SetSprite(GunpowderAnt.spriteIds[6]);
            yield return new WaitForSeconds(0.15f);
            prefab.GetComponent<tk2dBaseSprite>().SetSprite(GunpowderAnt.spriteIds[7]);
            isEating = false;
            GameManager.Instance.StartCoroutine(this.HandleAttack(prefab));
            yield break;
        }

        private IEnumerator HandleAttack(GameObject prefab)
        {
            while(prefab != null && !this.isEating)
            {
                yield return new WaitForSeconds(0.15f);
                prefab.GetComponent<tk2dBaseSprite>().SetSprite(GunpowderAnt.spriteIds[1]);
                yield return new WaitForSeconds(0.15f);
                prefab.GetComponent<tk2dBaseSprite>().SetSprite(GunpowderAnt.spriteIds[2]);
                yield return new WaitForSeconds(0.15f);
                prefab.GetComponent<tk2dBaseSprite>().SetSprite(GunpowderAnt.spriteIds[3]);
                yield return null;
            }
            yield break;
        }
        private void Orbitalgun()
        {
            PlayerController user = base.Owner as PlayerController;

            
            GameManager.Instance.PrimaryPlayer = user;
                this.gameObjectgun = UnityEngine.Object.Instantiate<GameObject>(ResourceCache.Acquire("Global Prefabs/HoveringGun") as GameObject,
                                user.CenterPosition.ToVector3ZisY(-5f), Quaternion.identity);
                gameObjectgun.transform.parent = user.transform;
                HoveringGunController m_hovers = gameObjectgun.AddComponent<HoveringGunController>();
                m_hovers.Aim = HoveringGunController.AimType.NEAREST_ENEMY;
                m_hovers.Trigger = HoveringGunController.FireType.ON_FIRED_GUN;
                m_hovers.Position = HoveringGunController.HoverPosition.CIRCULATE;
                m_hovers.ShootDuration = 0.5f;
                Gun gun;
                gun = (PickupObjectDatabase.GetById(176) as Gun);
                m_hovers.Initialize(gun, user);
                Antspawn.Add(m_hovers);
                StartCoroutine(Killthespawn(m_hovers.gameObject));






        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            Killdaqueen(base.Owner);
            player.OnKilledEnemyContext -= SpawnQuen;
            foreach(HoveringGunController hover in Antspawn)
            { UnityEngine.Object.Destroy(hover.gameObject);}
            

            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                DebrisObject result = base.Drop(player);
            }
            base.OnDestroy();
        }
        public static void BuildQueenPrefab()
        {
            GameObject gameObject = SpriteBuilder.SpriteFromResource("ClassLibrary1/Resources/Guntsprites/blue2_ant_idle_001", null, true);
            gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            GameObject gameObject2 = new GameObject("GuntQueen");
            tk2dSprite tk2dSprite = gameObject2.AddComponent<tk2dSprite>();
            tk2dSprite.SetSprite(gameObject.GetComponent<tk2dBaseSprite>().Collection, gameObject.GetComponent<tk2dBaseSprite>().spriteId);
            GunpowderAnt.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("ClassLibrary1/Resources/Guntsprites/blue2_ant_idle_001", tk2dSprite.Collection));
            GunpowderAnt.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("ClassLibrary1/Resources/Guntsprites/blue2_ant_idle_002", tk2dSprite.Collection));
            GunpowderAnt.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("ClassLibrary1/Resources/Guntsprites/blue2_ant_idle_003", tk2dSprite.Collection));
            GunpowderAnt.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("ClassLibrary1/Resources/Guntsprites/blue2_ant_idle_004", tk2dSprite.Collection));
            GunpowderAnt.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("ClassLibrary1/Resources/Guntsprites/blue2_ant_fire_001", tk2dSprite.Collection));
            GunpowderAnt.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("ClassLibrary1/Resources/Guntsprites/blue2_ant_fire_002", tk2dSprite.Collection));
            GunpowderAnt.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("ClassLibrary1/Resources/Guntsprites/blue2_ant_fire_003", tk2dSprite.Collection));
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            GunpowderAnt.spriteIds.Add(tk2dSprite.spriteId);
            gameObject2.SetActive(false);
            tk2dSprite.SetSprite(GunpowderAnt.spriteIds[0]);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            tk2dSprite.SetSprite(GunpowderAnt.spriteIds[1]);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            tk2dSprite.SetSprite(GunpowderAnt.spriteIds[2]);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            tk2dSprite.SetSprite(GunpowderAnt.spriteIds[3]);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            tk2dSprite.SetSprite(GunpowderAnt.spriteIds[4]);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            tk2dSprite.SetSprite(GunpowderAnt.spriteIds[5]);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            tk2dSprite.SetSprite(GunpowderAnt.spriteIds[6]);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            tk2dSprite.SetSprite(GunpowderAnt.spriteIds[7]);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            FakePrefab.MarkAsFakePrefab(gameObject2);
            UnityEngine.Object.DontDestroyOnLoad(gameObject2);
            GunpowderAnt.AntQueen = gameObject2;
        }

        public static List<int> spriteIds = new List<int>();

        public static List<int> spritemeepIds = new List<int>();
        public List<HoveringGunController> Antspawn = new List<HoveringGunController> ();
        private bool isEating;
        private GameObject queen;
        private TeleporterPrototypeItem teleporter;
        private HoveringGunController m_hovers;
        private GameObject gameObjectgun;
        private float FoodCount;
    }

}
