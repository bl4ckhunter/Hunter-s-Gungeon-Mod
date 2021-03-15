using System;
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class Crowdfunder : PlayerItem
{
	private bool active;
	private float sign;

	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void Init()
	{
		string name = "The Crowdfunder";
		string resourceName = "ClassLibrary1/Resources/Crowdfunder";
		GameObject gameObject = new GameObject();
		Crowdfunder Crowdfunder = gameObject.AddComponent<Crowdfunder>();
		ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
		string shortDesc = "Money sink go BRRT";
		string longDesc = "Shoots your casings, toggle to activate/deactivate. \n" + "Oh casings, you can't very well take them with you when you're dead can you now? So why not trying shooting them at your enemies first?";
		Crowdfunder.SetupItem(shortDesc, longDesc, "ror");
		Crowdfunder.AddPassiveStatModifier(PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
		Crowdfunder.quality = PickupObject.ItemQuality.D;
		Crowdfunder.SetCooldownType(ItemBuilder.CooldownType.Timed, 0f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
	}
	protected override void DoEffect(PlayerController user)
	{ 
		if (!this.active)
		{ this.active = true;
			GameManager.Instance.StartCoroutine(BRRT(user));
		}
	    else
		{ this.active = false; }
		 }
	public override bool CanBeUsed(PlayerController user)
	{
		return user.carriedConsumables.Currency > 0;
	}

	private IEnumerator BRRT(PlayerController player)
	{
		this.active = true;
		while (this.active && player.carriedConsumables.Currency > 0 )
			{
				player.carriedConsumables.Currency -= 1;
				this.Fire(player);
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
			yield return new WaitForSeconds(0.05f);
				this.Fire(player);
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
			yield return new WaitForSeconds(0.05f);
			}
		if (player.carriedConsumables.Currency == 0)
		{ this.active = false; }
		yield break;

	}
	private void Fire(PlayerController player)
	{
		Projectile projectile = ((Gun)ETGMod.Databases.Items[704]).DefaultModule.projectiles[0];
		projectile.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.yellow.a / 2f), 5, 0f);
		if (UnityEngine.Random.value > 0.5f) 
		{ this.sign = -1f; } else { this.sign = 1f; }
		float value = UnityEngine.Random.value * 10f * sign ;
		GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : base.LastOwner.CurrentGun.CurrentAngle + value), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		bool flag = component != null;
		if (flag)
		{
			component.Owner = base.LastOwner;
			component.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.blue.a / 2f), 5, 0f);
			component.Shooter = base.LastOwner.specRigidbody;
			component.baseData.speed = 20f;
			component.baseData.damage = 2f; 
			component.ignoreDamageCaps = true;
			component.AdditionalScaleMultiplier = 0.4f;
		}
	}


}

