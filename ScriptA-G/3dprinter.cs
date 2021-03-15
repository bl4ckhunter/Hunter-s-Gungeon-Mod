using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class Printer : PlayerItem
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
		string name = "3D Printer";
		string resourceName = "ClassLibrary1/Resources/Printer";
		GameObject gameObject = new GameObject();
		Printer Crowdfunder = gameObject.AddComponent<Printer>();
		ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
		string shortDesc = "Waste Not Want Not";
		string longDesc = "Selects a random passive when picked up, can be used near a passive/active/gun on the floor to destroy it, after three items have been destroyed spawns a copy of the selected passive, dropping the item resets selected passive as well as the items destroyed counter\n" + "...Maybe One More";
		Crowdfunder.SetupItem(shortDesc, longDesc, "ror");
		Crowdfunder.AddPassiveStatModifier(PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
		Crowdfunder.quality = PickupObject.ItemQuality.D;
		Crowdfunder.SetCooldownType(ItemBuilder.CooldownType.Timed, 0f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
		Crowdfunder.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
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

		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
		int num = -1;
		if (nearestInteractable is PassiveItem)
		{
			num = (nearestInteractable as PassiveItem).PickupObjectId;
			LootEngine.DoDefaultSynergyPoof((nearestInteractable as PickupObject).sprite.WorldCenter, false);
			UnityEngine.Object.Destroy((nearestInteractable as PassiveItem).gameObject);
		}
		else if (nearestInteractable is PlayerItem)
		{
			num = (nearestInteractable as PlayerItem).PickupObjectId;
			LootEngine.DoDefaultSynergyPoof((nearestInteractable as PickupObject).sprite.WorldCenter, false);
			UnityEngine.Object.Destroy((nearestInteractable as PickupObject).gameObject);
		}
		else if (nearestInteractable is Gun)
		{
			num = (nearestInteractable as Gun).PickupObjectId;
			LootEngine.DoDefaultSynergyPoof((nearestInteractable as PickupObject).sprite.WorldCenter, false);
			UnityEngine.Object.Destroy((nearestInteractable as PickupObject).gameObject);
		}
		if (num != -1)
		{
			count += 1; 
		}
		if (count > 2)
		{
			count = 0;
			LootEngine.SpawnItem(target.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
		}
	}
	public override bool CanBeUsed(PlayerController user)	
	{
		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
		return nearestInteractable is PassiveItem | nearestInteractable is PlayerItem | nearestInteractable is Gun;
	}

	


}

internal class ShuffleFlag : MonoBehaviour
{
}