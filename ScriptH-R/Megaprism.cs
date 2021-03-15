using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ClassLibrary1.Scripts;
using Dungeonator;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using Items;
using MonoMod;
using UnityEngine;

public class Prismone : GunBehaviour
{


    public static void Add()
    {
        // Get yourself a new gun "base" first.
        // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
        Gun gun = ETGMod.Databases.Items.NewGun("Prism", "Prismone");
        // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
        Game.Items.Rename("outdated_gun_mods:prism", "ror:prism");
        gun.gameObject.AddComponent<Prismone>();
        //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
        gun.SetShortDescription("Blindingly Bright");
        gun.SetLongDescription("Shed a light (fires multicolored beams duct taping guns to it is fine but it doesn't work when duct-taped to other guns)");
        // This is required, unless you want to use the sprites of the base gun.
        // That, by default, is the pea shooter.
        // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
        // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
        // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
        gun.SetupSprite(null, "Prismone_idle_001", 8);
        // ETGMod automatically checks which animations are available.
        // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
        gun.SetAnimationFPS(gun.shootAnimation, 12);
        gun.SetAnimationFPS(gun.reloadAnimation, 12);
        // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
        // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
        gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(177) as Gun, true, false);
        // Here we just take the default projectile module and change its settings how we want it to be.
        gun.DefaultModule.ammoCost = 1;
        gun.DefaultModule.angleVariance = 0f;
        gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
        gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
        gun.reloadTime = 0.0005f;
        gun.DefaultModule.cooldownTime = 0.1f;
        gun.DefaultModule.numberOfShotsInClip = 500;
        gun.SetBaseMaxAmmo(500);
        gun.CanBeDropped = false;
        // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
        gun.quality = PickupObject.ItemQuality.A;
        gun.encounterTrackable.EncounterGuid = "Prismone";
        //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
        //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
        Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
        projectile.gameObject.SetActive(false);
        FakePrefab.MarkAsFakePrefab(projectile.gameObject);
        UnityEngine.Object.DontDestroyOnLoad(projectile);
        gun.DefaultModule.projectiles[0] = projectile;
        projectile.baseData.speed *= 1.30f;
        projectile.baseData.damage = 150f;
        gun.muzzleFlashEffects = null;
        projectile.AppliesKnockbackToPlayer = false;
        projectile.baseData.range = 100f;
        gun.barrelOffset.localPosition += new Vector3(0.8f, 0.2f, 0f);
        //projectile.baseData allows you to modify the base properties of your projectile module.
        //In our case, our gun uses modified projectiles from the ak-47.
        //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
        //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
        //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
        projectile.transform.parent = gun.barrelOffset;
        ETGMod.Databases.Items.Add(gun, null, "ANY");
        gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
        gun.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
        gun.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
        //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
        //The x and y values determine the size of your custom projectile

    }

    public override void PostProcessProjectile(Projectile projectile)
    {
        base.PostProcessProjectile(projectile);
        Destroy(projectile.gameObject);
        if (gun.IsFiring && !this.active)
        {
            this.active = true;
            this.color1 = Color.HSVToRGB(0.166f, 1f, 1.018f);
            this.color3 = Color.HSVToRGB(0.5f, 1f, 1.018f);
            this.color5 = Color.HSVToRGB(0.833f, 1f, 1.018f);
            this.color4 = Color.HSVToRGB(0.666f, 1f, 1.018f);
            this.color = Color.HSVToRGB(0f, 1f, 1.018f);
            this.color2 = Color.HSVToRGB(0.333f, 1f, 1.018f);

            PlayerController man = base.gun.CurrentOwner as PlayerController;
            Projectile component1 = UnityEngine.Object.Instantiate<Projectile>(((Gun)ETGMod.Databases.Items[121]).DefaultModule.projectiles[0]);
            if (man.HasPickupID(661))
            {
                component1 = UnityEngine.Object.Instantiate<Projectile>(((Gun)ETGMod.Databases.Items[87]).DefaultModule.projectiles[0]);
                component1.baseData.force = 0f;
                component1.baseData.damage *= 5f;
                this.color1 = Color.HSVToRGB(0.166f, 1f, 0.99f);
                this.color3 = Color.HSVToRGB(0.5f, 1f, 0.99f);
                this.color5 = Color.HSVToRGB(0.833f, 1f, 0.99f);
                this.color4 = Color.HSVToRGB(0.666f, 1f, 0.99f);
                this.color = Color.HSVToRGB(0f, 1f, 0.99f);
                this.color2 = Color.HSVToRGB(0.333f, 1f, 0.99f);
            }

            GameManager.Instance.StartCoroutine(HandleFireShortBeam(component1, man));
            GameManager.Instance.StartCoroutine(HandleFireShortBeam1(component1, man));
            GameManager.Instance.StartCoroutine(HandleFireShortBeam2(component1, man));
            GameManager.Instance.StartCoroutine(HandleFireShortBeam3(component1, man));
            GameManager.Instance.StartCoroutine(HandleFireShortBeam4(component1, man));
            GameManager.Instance.StartCoroutine(HandleFireShortBeam5(component1, man)); 
        }

    }


    public IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source)
    {
        float elapsed = 0f;
        BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, base.gun.CurrentAngle, gun.barrelOffset.position);
        PlayerController x = this.gun.CurrentOwner as PlayerController;
        this.beamn = beam;
        yield return null;
        while (gun.IsFiring && x.CurrentGun == base.gun)
        {
            elapsed += BraveTime.DeltaTime; float floaty = (float)Math.Sin(elapsed * elapsed);
            this.ContinueFiringBeam(beam, source, base.gun.CurrentAngle + (150 * floaty) / (elapsed*(elapsed/2)), gun.barrelOffset.position, projectileToSpawn, color);
            yield return null;
        }
        this.CeaseBeam(beam);
        yield break;
    }
    public IEnumerator HandleFireShortBeam3(Projectile projectileToSpawn, PlayerController source)
    {
        float elapsed = 0f;
        BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, base.gun.CurrentAngle, gun.barrelOffset.position);
        PlayerController x = this.gun.CurrentOwner as PlayerController;
        this.beamn = beam;
        yield return null;
        while (gun.IsFiring && x.CurrentGun == base.gun)
        {
            elapsed += BraveTime.DeltaTime; float floaty = (float)Math.Sin(elapsed * elapsed +0.75);
            this.ContinueFiringBeam(beam, source, base.gun.CurrentAngle + (150 * floaty) / (elapsed * (elapsed / 2)), gun.barrelOffset.position, projectileToSpawn, color3);
            yield return null;
        }
        this.CeaseBeam(beam);
        yield break;
    }
    public IEnumerator HandleFireShortBeam4(Projectile projectileToSpawn, PlayerController source)
    {
        float elapsed = 0f;
        BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, base.gun.CurrentAngle, gun.barrelOffset.position);
        PlayerController x = this.gun.CurrentOwner as PlayerController;
        this.beamn = beam;
        yield return null;
        while (gun.IsFiring && x.CurrentGun == base.gun)
        {
            elapsed += BraveTime.DeltaTime; float floaty = (float)Math.Sin(elapsed * elapsed +0.5);
            this.ContinueFiringBeam(beam, source, base.gun.CurrentAngle + (150 * floaty) / (elapsed * (elapsed / 2)), gun.barrelOffset.position, projectileToSpawn, color4);
            yield return null;
        }
        this.CeaseBeam(beam);
        yield break;
    }
    public IEnumerator HandleFireShortBeam5(Projectile projectileToSpawn, PlayerController source)
    {
        float elapsed = 0f;
        BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, base.gun.CurrentAngle, gun.barrelOffset.position);
        PlayerController x = this.gun.CurrentOwner as PlayerController;
        this.beamn = beam;
        yield return null;
        while (gun.IsFiring && x.CurrentGun == base.gun)
        {
            elapsed += BraveTime.DeltaTime; float floaty = (float)Math.Sin(elapsed * elapsed -0.5);
            this.ContinueFiringBeam(beam, source, base.gun.CurrentAngle + (150 * floaty) / (elapsed * (elapsed / 2)), gun.barrelOffset.position, projectileToSpawn, color5);
            yield return null;
        }
        this.CeaseBeam(beam);
        yield break;
    }
    public IEnumerator HandleFireShortBeam1(Projectile projectileToSpawn, PlayerController source)
    {
        float elapsed = 0f;
        BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, base.gun.CurrentAngle, gun.barrelOffset.position);
        PlayerController x = this.gun.CurrentOwner as PlayerController;
        this.beamn = beam;
        yield return null;
        while (gun.IsFiring && x.CurrentGun == base.gun)
        {
            elapsed += BraveTime.DeltaTime; float floaty = (float)Math.Sin((elapsed * elapsed) -1);
            this.ContinueFiringBeam2(beam, source, base.gun.CurrentAngle + (150 * floaty)/ (elapsed * (elapsed / 2)), gun.barrelOffset.position, projectileToSpawn, color2);
            yield return null;
        }
        this.CeaseBeam(beam);
        yield break;
    }
    public IEnumerator HandleFireShortBeam2(Projectile projectileToSpawn, PlayerController source)
    {
        float elapsed = 0f;
        BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, base.gun.CurrentAngle, gun.barrelOffset.position);
        PlayerController x = this.gun.CurrentOwner as PlayerController;
        this.beamn = beam;
        yield return null;
        while (gun.IsFiring && x.CurrentGun == base.gun)
        {
            elapsed += BraveTime.DeltaTime; float floaty = (float)Math.Sin((elapsed * elapsed) + 1);
            this.ContinueFiringBeam1(beam, source, base.gun.CurrentAngle + (150 * floaty) / (elapsed * (elapsed / 2)), gun.barrelOffset.position, projectileToSpawn, color1);
            yield return null;
        }
        this.CeaseBeam(beam);
        yield break;
    }


  
    private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
    {
        Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
        GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
        Projectile component = gameObject.GetComponent<Projectile>();
        component.Owner = source;
        BeamController component2 = gameObject.GetComponent<BeamController>();
        component2.Owner = source;
        component.AdditionalScaleMultiplier = 3f;
        component2.HitsPlayers = false;
        component2.HitsEnemies = true;
        Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
        component2.Direction = v;
        component2.Origin = vector;
        Projectile sourceProjectile = component;
        sourceProjectile.AppliesPoison = false;
        component.baseData.speed = 100f;
        component.baseData.damage *= 0.2f;
        return component2;
    }
    private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint, Projectile projectile, Color color)
    {
        Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
        beam.AdjustPlayerBeamTint(color, 5, 0f);
        beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
        projectile.AdditionalScaleMultiplier = 4f;
        beam.Origin = vector;
        beam.LateUpdatePosition(vector);
    }
    private void ContinueFiringBeam1(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint, Projectile projectile, Color color)
    {
        Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
        beam.AdjustPlayerBeamTint(color, 5, 0f);
        beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
        projectile.AdditionalScaleMultiplier = 4f;
        beam.Origin = vector;
        beam.LateUpdatePosition(vector);
    }
    private void ContinueFiringBeam2(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint, Projectile projectile, Color color)
    {
        Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
        beam.AdjustPlayerBeamTint(color, 5, 0f);
        beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
        projectile.AdditionalScaleMultiplier = 4f;
        beam.Origin = vector;
        beam.LateUpdatePosition(vector);
    }

    // Token: 0x0600888F RID: 34959 RVA: 0x002E0188 File Offset: 0x002DE388
    internal void CeaseBeam(BeamController beam)
    {
        beam.CeaseAttack();
        this.active = false;
    }











    // Token: 0x0600748E RID: 29838 RVA: 0x00007B97 File Offset: 0x00005D97


    // Token: 0x06007490 RID: 29840 RVA: 0x002D6F20 File Offset: 0x002D5120


    // Token: 0x06007491 RID: 29841 RVA: 0x002D70E0 File Offset: 0x002D52E0
    // Token: 0x06007492 RID: 29842 RVA: 0x002D7130 File Offset: 0x002D5330

    // Token: 0x0400765E RID: 30302
    public float trackingSpeed;

    // Token: 0x0400765F RID: 30303
    public float trackingTime;

    // Token: 0x04007660 RID: 30304
    [CurveRange(0f, 0f, 1f, 1f)]
    public AnimationCurve trackingCurve;

    // Token: 0x04007661 RID: 30305
    private PlayerController m_player;








    public override void OnPostFired(PlayerController player, Gun gun)
    {
        this.Owner = gun.CurrentOwner;
        gun.PreventNormalFireAudio = true;
        AkSoundEngine.PostEvent("Play_ENM_flame_veil_01", gameObject);
    }
    private bool HasReloaded;
    //This block of code allows us to change the reload sounds.
    protected void Update()
    {
        if (gun.CurrentOwner)
        {

            if (!gun.PreventNormalFireAudio)
            {
                this.gun.PreventNormalFireAudio = true;
            }
            if (!gun.IsReloading && !HasReloaded)
            {
                this.HasReloaded = true;
                this.unused = false;
            }
        }

    }

    public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
    {
        if (gun.IsReloading && this.HasReloaded)
        {

            HasReloaded = false;
            AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
            base.OnReloadPressed(player, gun, bSOMETHING);
            AkSoundEngine.PostEvent("Play_ENM_flame_veil_01", base.gameObject);

        }

    }





    // Token: 0x040077B6 RID: 30646
    public float ActivationChance;

    // Token: 0x040077B8 RID: 30648
    public bool TriggersRadialBulletBurst;

    // Token: 0x040077B9 RID: 30649
    [ShowInInspectorIf("TriggersRadialBulletBurst", false)]
    public RadialBurstInterface RadialBurstSettings;


    private bool onCooldown; private bool onCooldown1;
    private float FlamerBase = 7f;

    // Token: 0x0400001D RID: 29
    private float FlamerTrue;

    // Token: 0x0400001E RID: 30
    private float Damage;

    // Token: 0x0400001F RID: 31
    private float lastDamage = -1f;
    private bool firingflag;
    private bool engaged;
    private float sign;
    private bool activated;
    private object LastOwner;
    private AIActor Bomb;
    private GameObject Nuke;
    private Shader m_glintShader;
    private bool alive;
    private GameActor Owner;
    private PlayerController player;
    private AIActor aiactor;
    private int flat9;
    private PlayerController m_owner;
    private Projectile m_projectile;
    private Vector2 crosshair;
    private static FireVolleyOnRollItem m_bomb;
    private bool unused;
    private bool active;
    private BeamController beamn;
    private Vector2 aimpoint;
    private float m_currentAngle;
    private float m_currentDistance;
    private bool onCooldowncollide;
    private bool hitenemy;
    private bool black;
    private Color color;
    private Color color1;
    private Color color2;
    private Color color3;
    private Color color4;
    private Color color5;


    // Token: 0x04007661 RID: 30305

    //All that's left now is sprite stuff. 
    //Your sprites should be organized, like how you see in the mod folder. 
    //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
    //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
    //By default this gun is a one-handed weapon
    //If you need a basic two handed .json. Just use the jpxfrd2.json.
    //And finally, don't forget to add your Gun to your ETGModule class!
}