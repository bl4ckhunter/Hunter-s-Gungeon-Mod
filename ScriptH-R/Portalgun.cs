using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dungeonator;
using ItemAPI;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class PortalGun : PlayerItem
{
	private bool active;
	private Vector2 blueaimpoint;
	private float sign;
	private bool redportal;
	private GameObject Activeredportal;
	private Vector2 redeaimpoint;
	private bool blueportal;
	private GameObject BluePortalprefab;
	private  GameObject RedPortalprefab;
	private GameObject Activeblueportal;
	private Vector2 aimpoint;
	private float m_currentAngle;
	private float m_currentDistance;

	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void Init()
	{
		string name = "Portal Gun";
		string resourceName = "ClassLibrary1/Resources/portal";
		GameObject gameObject = new GameObject();
		PortalGun Crowdfunder = gameObject.AddComponent<PortalGun>();
		ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
		string shortDesc = "Use With Caution";
		string longDesc = "The one and only. (shooting a portal before both ends are open or with a laser will destroy it)";
		Crowdfunder.SetupItem(shortDesc, longDesc, "ror");
		Crowdfunder.quality = PickupObject.ItemQuality.A;
		Crowdfunder.UsesNumberOfUsesBeforeCooldown = true;
		Crowdfunder.numberOfUses = 2;
		Crowdfunder.SetCooldownType(ItemBuilder.CooldownType.Timed, 30f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
		Crowdfunder.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

	}
	protected override void DoEffect(PlayerController user)
	{
		PlayerController player = base.LastOwner;
		if (BraveInput.GetInstanceForPlayer(player.PlayerIDX).IsKeyboardAndMouse(false))
		{
			this.aimpoint = player.unadjustedAimPoint.XY();
		}
		else
		{
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(player.PlayerIDX);
			Vector2 a2 = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
			a2 += instanceForPlayer.ActiveActions.Aim.Vector * 8f * BraveTime.DeltaTime;
			this.m_currentAngle = BraveMathCollege.Atan2Degrees(a2 - player.CenterPosition);
			this.m_currentDistance = Vector2.Distance(a2, player.CenterPosition);
			this.m_currentDistance = Mathf.Min(this.m_currentDistance, 15);
			this.aimpoint = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
		}
		if (!this.active)
		{
			if (BluePortalprefab)
			{
				Destroy(BluePortalprefab);
			}

			if (RedPortalprefab)
			{
				Destroy(RedPortalprefab);
			}
			this.active = true;
			this.FireBlue(base.LastOwner, 1, aimpoint);

		}
		else
		{
			this.blueportal = true;
			this.active = false;
			this.FireRed(base.LastOwner, 1, aimpoint);
		}
	}
	public static List<int> bluespriteIds = new List<int>();
	public static List<int> redspriteIds = new List<int>();

	private void RedCalcollide(CollisionData obj)
	{
		{ if (obj.OtherRigidbody.GetComponent<AIActor>() && !obj.OtherRigidbody.GetComponent<AIActor>().healthHaver.IsBoss && BluePortalprefab)
			{
				Vector3 exitpoint = BluePortalprefab.transform.position - new Vector3(obj.OtherRigidbody.UnitCenter.x - obj.MyRigidbody.UnitCenter.x, obj.OtherRigidbody.UnitCenter.y - obj.MyRigidbody.UnitCenter.y,0)* 2f;
				obj.OtherRigidbody.transform.position = exitpoint;
				obj.OtherRigidbody.specRigidbody.Reinitialize();
				obj.OtherRigidbody.specRigidbody.RecheckTriggers = true;
			}
		}
	}
	private void BlueCalcollide(CollisionData obj)
	{
		{
			if (obj.OtherRigidbody.GetComponent<AIActor>() && !obj.OtherRigidbody.GetComponent<AIActor>().healthHaver.IsBoss && RedPortalprefab)
			{
				Vector3 exitpoint = RedPortalprefab.transform.position - new Vector3(obj.OtherRigidbody.UnitCenter.x - obj.MyRigidbody.UnitCenter.x, obj.OtherRigidbody.UnitCenter.y - obj.MyRigidbody.UnitCenter.y, 0) * 2f;
				obj.OtherRigidbody.transform.position = exitpoint;
				obj.OtherRigidbody.aiActor.IgnoreForRoomClear = true;
				obj.OtherRigidbody.specRigidbody.Reinitialize();
				obj.OtherRigidbody.specRigidbody.RecheckTriggers = true;
			}

		}
	}
	private void FireRed(PlayerController player, int one, Vector2 position)
	{
		Projectile projectile = ((Gun)ETGMod.Databases.Items[599]).DefaultModule.projectiles[0];
		GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, position, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : base.LastOwner.CurrentGun.CurrentAngle), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		bool flag1 = component != null;
		if (flag1)
		{
			component.Owner = base.LastOwner;
			component.baseData.speed = 0f;
			component.AdjustPlayerProjectileTint(Color.HSVToRGB(0.075f, 1f, 3f), 5, 0f);
			component.baseData.damage = 0f;
			component.AdditionalScaleMultiplier = 1.5f;
			component.baseData.range = 6f;
			PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
			component.collidesWithPlayer = true;
			component.collidesWithEnemies = true;
			component.collidesWithProjectiles = true;
			piercer.penetration = 100000;
			component.specRigidbody.OnCollision += RedCalcollide;
			component.specRigidbody.OnPreRigidbodyCollision += RedPrecollide;
			this.RedPortalprefab = gameObject;
		}

	}
	private void FireBlue(PlayerController player, int one, Vector2 position)
	{
		Projectile projectile = ((Gun)ETGMod.Databases.Items[599]).DefaultModule.projectiles[0];
		GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, position, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : base.LastOwner.CurrentGun.CurrentAngle), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		bool flag1 = component != null;
		if (flag1)
		{
			component.baseData.speed = 0f;
			component.Owner = base.LastOwner;
			component.AdjustPlayerProjectileTint(Color.HSVToRGB(0.583333f, 1f, 3f), 5, 0f);
			component.baseData.damage = 0f;
			component.AdditionalScaleMultiplier = 1.5f;
			component.baseData.range = 6f;
			PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
			piercer.penetration = 100000;
			component.collidesWithPlayer = true;
			component.collidesWithEnemies = true;
			component.collidesWithProjectiles = true;
			component.specRigidbody.OnCollision += BlueCalcollide;
			component.specRigidbody.OnPreRigidbodyCollision += BluePrecollide;
			this.BluePortalprefab = gameObject;
		}

	}

	private void BluePrecollide(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{   if (RedPortalprefab)
		{
			if (otherRigidbody.projectile && !otherRigidbody.GetComponent<FromBlueProjectile>())
			{
				PhysicsEngine.SkipCollision = true;
				Vector3 exitpoint = RedPortalprefab.transform.position - new Vector3(otherRigidbody.UnitCenter.x - myRigidbody.UnitCenter.x, otherRigidbody.UnitCenter.y - myRigidbody.UnitCenter.y, 0) * 2f;
				BlueSpawnProjectile(otherRigidbody.projectile, exitpoint, otherRigidbody.Velocity.ToAngle());
				if (otherRigidbody.GetComponent<SpawnProjModifier>())
				{ Destroy(otherRigidbody.GetComponent<SpawnProjModifier>()); }
				if (otherRigidbody.GetComponent<FromRedProjectile>())
				{ Destroy(otherRigidbody.GetComponent<FromRedProjectile>()); }
				if (otherRigidbody.GetComponent<ExplosiveModifier>())
				{ Destroy(otherRigidbody.GetComponent<ExplosiveModifier>()); }
				Destroy(otherRigidbody.gameObject);
			}
			if (otherRigidbody.projectile && otherRigidbody.GetComponent<FromBlueProjectile>())
			{ PhysicsEngine.SkipCollision = true; }
			if (otherRigidbody.GetComponent<PlayerController>())
			{
				Vector3 exitpoint = RedPortalprefab.transform.position - new Vector3(otherRigidbody.UnitCenter.x - myRigidbody.UnitCenter.x, otherRigidbody.UnitCenter.y - myRigidbody.UnitCenter.y, 0) * 2f;
				if (LastOwner.IsValidPlayerPosition(exitpoint))
				{ otherRigidbody.transform.position = exitpoint; }
				otherRigidbody.specRigidbody.Reinitialize();
				otherRigidbody.specRigidbody.RecheckTriggers = true;
			}
		}
		if(otherRigidbody.projectile && !RedPortalprefab)
		{
			if (otherRigidbody.GetComponent<SpawnProjModifier>())
			{ Destroy(otherRigidbody.GetComponent<SpawnProjModifier>()); }
			if (otherRigidbody.GetComponent<FromBlueProjectile>())
			{ Destroy(otherRigidbody.GetComponent<FromBlueProjectile>()); }
			if (otherRigidbody.GetComponent<ExplosiveModifier>())
			{ Destroy(otherRigidbody.GetComponent<ExplosiveModifier>()); }
			Destroy(otherRigidbody.gameObject);
		}

	}
	private void RedPrecollide(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (BluePortalprefab)
		{
			if (otherRigidbody.projectile && !otherRigidbody.GetComponent<FromRedProjectile>())
			{
				PhysicsEngine.SkipCollision = true;
				Vector3 exitpoint = BluePortalprefab.transform.position - new Vector3(otherRigidbody.UnitCenter.x - myRigidbody.UnitCenter.x, otherRigidbody.UnitCenter.y - myRigidbody.UnitCenter.y, 0) * 2f;
				RedSpawnProjectile(otherRigidbody.projectile, exitpoint, otherRigidbody.Velocity.ToAngle());
				if (otherRigidbody.GetComponent<SpawnProjModifier>())
				{ Destroy(otherRigidbody.GetComponent<SpawnProjModifier>()); }
				if (otherRigidbody.GetComponent<FromBlueProjectile>())
				{ Destroy(otherRigidbody.GetComponent<FromBlueProjectile>()); }
				if (otherRigidbody.GetComponent<ExplosiveModifier>())
				{ Destroy(otherRigidbody.GetComponent<ExplosiveModifier>()); }
				Destroy(otherRigidbody.gameObject);
			}
			if (otherRigidbody.projectile && otherRigidbody.GetComponent<FromRedProjectile>())
			{ PhysicsEngine.SkipCollision = true; }
			if (otherRigidbody.GetComponent<PlayerController>())
			{
				Vector3 exitpoint = BluePortalprefab.transform.position - new Vector3(otherRigidbody.UnitCenter.x - myRigidbody.UnitCenter.x, otherRigidbody.UnitCenter.y - myRigidbody.UnitCenter.y, 0) * 2f;
				if (LastOwner.IsValidPlayerPosition(exitpoint))
				{ otherRigidbody.transform.position = exitpoint; }
				otherRigidbody.specRigidbody.Reinitialize();
				otherRigidbody.specRigidbody.RecheckTriggers = true;
			}
		}
		if (otherRigidbody.projectile && !BluePortalprefab)
		{
			if (otherRigidbody.GetComponent<SpawnProjModifier>())
			{ Destroy(otherRigidbody.GetComponent<SpawnProjModifier>()); }
			if (otherRigidbody.GetComponent<FromBlueProjectile>())
			{ Destroy(otherRigidbody.GetComponent<FromBlueProjectile>()); }
			if (otherRigidbody.GetComponent<ExplosiveModifier>())
			{ Destroy(otherRigidbody.GetComponent<ExplosiveModifier>()); }
			Destroy(otherRigidbody.gameObject);
		}
	}

	private void RedSpawnProjectile(Projectile projectile, Vector3 pos, float rot)
	{
		GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, pos, Quaternion.Euler(0, 0, rot), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		bool flag1 = component != null;
		if (flag1)
		{
			component.Owner = base.LastOwner;
			component.Shooter = base.LastOwner.specRigidbody;
			component.SetOwnerSafe(base.LastOwner, "Player");
			component.AdditionalScaleMultiplier = 1f;
			component.collidesWithEnemies = true;
			component.gameObject.AddComponent<FromRedProjectile>();
		}
	}
	private void BlueSpawnProjectile(Projectile projectile, Vector3 pos, float rot)
	{
		GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, pos, Quaternion.Euler(0, 0, rot), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		bool flag1 = component != null;
		if (flag1)
		{
			component.Owner = base.LastOwner;
			component.Shooter = base.LastOwner.specRigidbody;
			component.SetOwnerSafe(base.LastOwner, "Player");
			component.AdditionalScaleMultiplier = 1f;
			component.collidesWithEnemies = true;
			component.gameObject.AddComponent<FromBlueProjectile>();
		}
	}
}

internal class FromRedProjectile : MonoBehaviour
{
}

internal class FromBlueProjectile : MonoBehaviour
{
}