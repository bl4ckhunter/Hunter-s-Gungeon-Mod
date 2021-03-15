using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using ItemAPI;
using Mod;
using MultiplayerBasicExample;
using UnityEngine;
public class Bcard : PlayerItem
{
	private bool active;
	private float sign;
	private bool useable;

	private ItemQuality Spawnquality;

	private PassiveItem target;
	private bool reshuffle;
	private int count;

	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void Init()
	{
		string name = "Blank Card";
		string resourceName = "ClassLibrary1/Resources/blankcard";
		GameObject gameObject = new GameObject();
		Bcard Crowdfunder = gameObject.AddComponent<Bcard>();
		ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
		string shortDesc = "Basically Cheating";
		string longDesc = "On use grants a copy of the nearest card on the floor\n" + "This breaks everything!";
		Crowdfunder.SetupItem(shortDesc, longDesc, "ror");
		Crowdfunder.quality = PickupObject.ItemQuality.A;
		ItemBuilder.AddPassiveStatModifier(Crowdfunder, PlayerStats.StatType.Coolness, 5, StatModifier.ModifyMethod.ADDITIVE);
		Crowdfunder.SetCooldownType(ItemBuilder.CooldownType.Timed, 0f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
		Crowdfunder.SetCooldownType(ItemBuilder.CooldownType.Damage, 1200f);
		Crowdfunder.Spawnquality = (PickupObject.ItemQuality)UnityEngine.Random.Range(1, 6);
		Crowdfunder.target = LootEngine.GetItemOfTypeAndQuality<PassiveItem>(Crowdfunder.Spawnquality, GameManager.Instance.RewardManager.ItemsLootTable, false);
	}
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		this.Spawnquality = (PickupObject.ItemQuality)UnityEngine.Random.Range(1, 6);
		this.target = LootEngine.GetItemOfTypeAndQuality<PassiveItem>(this.Spawnquality, GameManager.Instance.RewardManager.ItemsLootTable, false);

	}

	protected override void DoEffect(PlayerController user)
	{   
		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 15f, user);
		foreach (PlayerItem item in RoRItems.cards)
		{
			if (item.PickupObjectId == (nearestInteractable as PlayerItem).PickupObjectId)
			{
				PickupObject pickupObject5 = PickupObjectDatabase.GetById((nearestInteractable as PlayerItem).PickupObjectId);
				LootEngine.GivePrefabToPlayer(pickupObject5.gameObject, user);
			}
		}
	}
	public override bool CanBeUsed(PlayerController user)
	{
		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 15f, user);
		bool flaggy = false;
		foreach (PlayerItem item in RoRItems.cards)
		{
			if (item.PickupObjectId == (nearestInteractable as PlayerItem).PickupObjectId)
			{
				flaggy = true;
			}
		}
		return flaggy;
	}




}
