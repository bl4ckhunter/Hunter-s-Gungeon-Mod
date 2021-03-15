using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using System.Collections.Generic;
using Dungeonator;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class Gesture : PassiveItem
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Curse Of The Drowned";
			string resourceName = "ClassLibrary1/Resources/drowned"; ;
			GameObject gameObject = new GameObject();
			Gesture frailCrown = gameObject.AddComponent<Gesture>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Muted Addiction";
			string longDesc = "Active items charge 55% faster but trigger immediately upon reaching full charge\n" + "The shell of a mighty Whorl, a distant planet's skies rumble from whitin.";
			frailCrown.SetupItem(shortDesc, longDesc, "ror");
			frailCrown.quality = PickupObject.ItemQuality.B;
			frailCrown.AddPassiveStatModifier(PlayerStats.StatType.Curse, 3f, StatModifier.ModifyMethod.ADDITIVE);
			frailCrown.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			frailCrown.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			frailCrown.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			frailCrown.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
		}
		private void LateUpdate()
		{
			bool flag = this.m_pickedUp && this.m_owner != null;
			if (flag)
			{
				foreach (PlayerItem playerItem in this.m_owner.activeItems)
				{
					bool flag2 = !this.affectedItems.Contains(playerItem);
					if (flag2)
					{
						bool flag3 = playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>() != null;
						if (flag3)
						{
							playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().stacks++;
							playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().parentItems.Add(this);
							this.affectedItems.Add(playerItem);
						}
						else
						{
							playerItem.gameObject.AddComponent<Gesture.ReduceChargetimeBehaviour>().parentItems.Add(this);
							this.affectedItems.Add(playerItem);
						}
					}
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00007FB4 File Offset: 0x000061B4
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.LateUpdate();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00007FD4 File Offset: 0x000061D4

		protected override void Update()
		{
			base.Update();
			if(base.Owner)
			{ if (base.Owner.CurrentItem != null && base.Owner.CurrentItem.CooldownPercentage == 0f && base.Owner.CurrentItem.CanBeUsed(base.Owner) && base.Owner.CurrentRoom != null)
			{ PlayerItem currentItem = base.Owner.CurrentItem;
				float num = -1;
				currentItem.Use(base.Owner, out num);
			} } 
	     }
		// Token: 0x06000087 RID: 135 RVA: 0x00008114 File Offset: 0x00006314
		public override DebrisObject Drop(PlayerController player)
		{
			foreach (PlayerItem playerItem in this.affectedItems)
			{
				bool flag = playerItem != null && playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>() != null;
				if (flag)
				{
					bool flag2 = playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().stacks > 1;
					if (flag2)
					{
						playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().stacks--;
						playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().parentItems.Remove(this);
					}
					else
					{
						playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().Destroy();
					}
				}
			}
			this.affectedItems.Clear();
			return base.Drop(player);
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				foreach (PlayerItem playerItem in this.affectedItems)
				{
					bool flag = playerItem != null && playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>() != null;
					if (flag)
					{
						bool flag2 = playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().stacks > 1;
						if (flag2)
						{
							playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().stacks--;
							playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().parentItems.Remove(this);
						}
						else
						{
							playerItem.GetComponent<Gesture.ReduceChargetimeBehaviour>().Destroy();
						}
					}
				}
			}
			base.OnDestroy();
		}
		// Token: 0x04000049 RID: 73
		private List<PlayerItem> affectedItems = new List<PlayerItem>();

		// Token: 0x0200007F RID: 127
		public class ReduceChargetimeBehaviour : MonoBehaviour
		{
			// Token: 0x06000388 RID: 904 RVA: 0x00028154 File Offset: 0x00026354
			public float GetStackedMultiplier()
			{
				float num = 1f;
				for (int i = 0; i < this.stacks; i++)
				{
					num *= 0.45f;
				}
				return num;
			}

			// Token: 0x06000389 RID: 905 RVA: 0x0002818C File Offset: 0x0002638C
			private void Start()
			{
				this.m_item = base.GetComponent<PlayerItem>();
				this.m_primalDamageCooldown = this.m_item.damageCooldown;
				this.m_primalTimeCooldown = this.m_item.timeCooldown;
				this.m_primalRoomCooldown = this.m_item.roomCooldown;
				bool flag = this.m_item.damageCooldown > 0f;
				if (flag)
				{
					this.m_item.damageCooldown *= this.GetStackedMultiplier();
				}
				bool flag2 = this.m_item.timeCooldown > 0f;
				if (flag2)
				{
					this.m_item.timeCooldown *= this.GetStackedMultiplier();
				}
				bool flag3 = this.m_item.roomCooldown > 0;
				if (flag3)
				{
					float num = (float)this.m_item.roomCooldown;
					this.m_item.roomCooldown = Mathf.RoundToInt(num * this.GetStackedMultiplier());
				}
				this.m_roomCooldown = this.m_item.roomCooldown;
				this.m_timeCooldown = this.m_item.timeCooldown;
				this.m_damageCooldown = this.m_item.damageCooldown;
				PlayerItem item = this.m_item;
				item.OnPreDropEvent = (Action)Delegate.Combine(item.OnPreDropEvent, new Action(this.OnPreActiveItemDrop));
			}

			// Token: 0x0600038A RID: 906 RVA: 0x000282CC File Offset: 0x000264CC
			private void OnPreActiveItemDrop()
			{
				foreach (Gesture Gesture in this.parentItems)
				{
					Gesture.affectedItems.Remove(this.m_item);
				}
				this.Destroy();
			}

			// Token: 0x0600038B RID: 907 RVA: 0x00028338 File Offset: 0x00026538
			private void LateUpdate()
			{
				bool flag = this.m_item.damageCooldown != this.m_damageCooldown && this.m_item.damageCooldown > 0f;
				if (flag)
				{
					this.m_primalDamageCooldown = this.m_item.damageCooldown;
					this.m_item.damageCooldown *= this.GetStackedMultiplier();
				}
				else
				{
					bool flag2 = this.stacks != this.stacksLast && this.m_item.damageCooldown > 0f;
					if (flag2)
					{
						this.m_item.damageCooldown = this.m_primalDamageCooldown * this.GetStackedMultiplier();
					}
				}
				bool flag3 = this.m_item.timeCooldown != this.m_timeCooldown && this.m_item.timeCooldown > 0f;
				if (flag3)
				{
					this.m_primalTimeCooldown = this.m_item.timeCooldown;
					this.m_item.timeCooldown *= this.GetStackedMultiplier();
				}
				else
				{
					bool flag4 = this.stacks != this.stacksLast && this.m_item.timeCooldown > 0f;
					if (flag4)
					{
						this.m_item.timeCooldown = this.m_primalTimeCooldown * this.GetStackedMultiplier();
					}
				}
				bool flag5 = this.m_item.roomCooldown != this.m_roomCooldown && this.m_item.roomCooldown > 0;
				if (flag5)
				{
					this.m_primalRoomCooldown = this.m_item.roomCooldown;
					float num = (float)this.m_item.roomCooldown;
					this.m_item.roomCooldown = Mathf.RoundToInt(num * this.GetStackedMultiplier());
				}
				else
				{
					bool flag6 = this.stacks != this.stacksLast && this.m_item.roomCooldown > 0;
					if (flag6)
					{
						float num2 = (float)this.m_primalRoomCooldown;
						this.m_item.roomCooldown = Mathf.RoundToInt(num2 * this.GetStackedMultiplier());
					}
				}
				this.m_damageCooldown = this.m_item.damageCooldown;
				this.m_timeCooldown = this.m_item.timeCooldown;
				this.m_roomCooldown = this.m_item.roomCooldown;
				this.stacksLast = this.stacks;
			}

			// Token: 0x0600038C RID: 908 RVA: 0x00028568 File Offset: 0x00026768
			public void Destroy()
			{
				this.m_item.damageCooldown = this.m_primalDamageCooldown;
				this.m_item.timeCooldown = this.m_primalTimeCooldown;
				this.m_item.roomCooldown = this.m_primalRoomCooldown;
				UnityEngine.Object.Destroy(this);
			}

			// Token: 0x040003D9 RID: 985
			private float m_damageCooldown = -1f;

			// Token: 0x040003DA RID: 986
			private float m_timeCooldown = -1f;

			// Token: 0x040003DB RID: 987
			private int m_roomCooldown = -1;

			// Token: 0x040003DC RID: 988
			private float m_primalDamageCooldown = -1f;

			// Token: 0x040003DD RID: 989
			private float m_primalTimeCooldown = -1f;

			// Token: 0x040003DE RID: 990
			private int m_primalRoomCooldown = -1;

			// Token: 0x040003DF RID: 991
			private PlayerItem m_item;

			// Token: 0x040003E0 RID: 992
			public List<Gesture> parentItems = new List<Gesture>();

			// Token: 0x040003E1 RID: 993
			public int stacks = 1;

			// Token: 0x040003E2 RID: 994
			public int stacksLast = 1;
		}
	}
}

