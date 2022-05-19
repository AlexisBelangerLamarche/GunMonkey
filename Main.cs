using BTD_Mod_Helper;
using MelonLoader;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Unity.Display;

///
/// GOOD LUCK FINDING SHIT BECAUSE I PUT EVERYTHING IN ONE FILE I DONT CARE WOOOOO
///

[assembly: MelonInfo(typeof(GunMonkey.Main), "Gun Monkey", "0.1.5", "Alexis Belanger")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace GunMonkey
{
    public class Main : BloonsTD6Mod
    {

        public override string GithubReleaseURL => "https://api.github.com/repos/AlexisBelangerLamarche/GunMonkey/releases";

        public override void OnMainMenu()
        {
            base.OnMainMenu();
        }

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Template Mod Loaded!");
        }

    }

    public class GunMonkey : ModTower
    {
        public override string TowerSet => MILITARY;

        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 650;

        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 5;
        public override string Description => "Shoots bloons with his gun.";

        public override ParagonMode ParagonMode => ParagonMode.Base555;

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.range -= 5;
            towerModel.GetAttackModel().range -= 5;
            towerModel.GetAttackModel().weapons[0].Rate = 0.5f;

            towerModel.GetWeapon().projectile.ApplyDisplay<Display.Bullet>();


            var projectile = towerModel.GetAttackModel().weapons[0].projectile;
            projectile.GetBehavior<TravelStraitModel>().speed += 20;
            projectile.pierce = 2;
        }

        public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
        {
            return towerSet.First(model => model.towerId == TowerType.SniperMonkey).towerIndex + 1;
        }

    }

    public class GunBaseDisplay : ModTowerDisplay<GunMonkey>
    {
        public override string BaseDisplay => GetDisplay(TowerType.SniperMonkey, 0, 0, 5);

        public override bool UseForTower(int[] tiers)
        {
            return true;
        }
    }

    public class GunProdigy : ModParagonUpgrade<GunMonkey>
    {
        public override int Cost => 400000;
        public override string Description => "A smile will get you far, but a smile and a gun will get you farther.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {

        }
    }
}

namespace GunMonkey.Upgrades.TopPath
{
    public class ExtendedMag : ModUpgrade<GunMonkey>
    {
        public override int Path => TOP;
        public override int Tier => 1;
        public override int Cost => 450;
        public override string Description => "Fires faster!";

        public override void ApplyUpgrade(TowerModel tower)
        {
            foreach (var weaponModel in tower.GetWeapons())
            {
                weaponModel.Rate *= .8f;
            }
        }
    }

    public class DrumMag : ModUpgrade<GunMonkey>
    {
        public override int Path => TOP;
        public override int Tier => 2;
        public override int Cost => 600;
        public override string Description => "Fires even faster!";

        public override void ApplyUpgrade(TowerModel tower)
        {
            foreach (var weaponModel in tower.GetWeapons())
            {
                weaponModel.Rate *= .7f;
            }
        }
    }

    public class BigBullets : ModUpgrade<GunMonkey>
    {
        public override int Path => TOP;
        public override int Tier => 3;
        public override int Cost => 4000;
        public override string Description => "Thats some pretty big bullets.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetWeapon().projectile.ApplyDisplay<Display.BigBullet>();
            tower.GetWeapon().projectile.radius += 20;
            tower.GetWeapon().projectile.scale += 3;
            tower.GetWeapon().projectile.GetDamageModel().damage += 1;
            tower.GetWeapon().projectile.pierce += 50;
        }
    }

    public class UltraBigBullets : ModUpgrade<GunMonkey>
    {
        public override int Path => TOP;
        public override int Tier => 4;
        public override int Cost => 6200;
        public override string Description => "Ok maybe a little too big.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetWeapon().projectile.radius += 20;
            tower.GetWeapon().projectile.scale += 7;
            tower.GetWeapon().projectile.GetDamageModel().damage += 1;
            tower.GetWeapon().projectile.pierce *= 10;
        }
    }

    public class AbsoluteUnit : ModUpgrade<GunMonkey>
    {
        public override int Path => TOP;
        public override int Tier => 5;
        public override int Cost => 59000;
        public override string Description => "The size of an asteroid.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetWeapon().projectile.radius += 20;
            tower.GetWeapon().projectile.scale += 7;
            tower.GetWeapon().projectile.pierce *= 10;
        }
    }
}

namespace GunMonkey.Upgrades.MiddlePath
{
    public class ElectricBullets : ModUpgrade<GunMonkey>
    {
        public override int Path => MIDDLE;
        public override int Tier => 1;
        public override int Cost => 300;
        public override string Description => "Increase pierce greatly.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            foreach (var weaponModel in tower.GetWeapons())
            {
                weaponModel.projectile.pierce *= 3;
            }
        }
    }

    public class ElectricalInterrogation : ModUpgrade<GunMonkey>
    {
        public override int Path => MIDDLE;
        public override int Tier => 2;
        public override int Cost => 300;
        public override string Description => "Gun monkey can now hit camo bloons.";

        public override int Priority => -1;

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
    }

    public class TazerGun : ModUpgrade<GunMonkey>
    {
        public override int Path => MIDDLE;
        public override int Tier => 3;
        public override int Cost => 3200;
        public override string Description => "Gun monkey now wield a powerful tazer that stuns bloons.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            var druidweapon = Game.instance.model.GetTower(TowerType.Druid, 2, 0, 0).GetWeapons()[1].Duplicate();
            tower.GetAttackModel().AddWeapon(druidweapon);

            var bloonImpact = Game.instance.model.GetTower(TowerType.BombShooter, 4);
            var slowModel = bloonImpact.GetDescendant<SlowModel>().Duplicate();
            var slowModifierForTagModel = bloonImpact.GetDescendant<SlowModifierForTagModel>().Duplicate();
            tower.GetAttackModel().weapons[1].projectile.AddBehavior(slowModel);
            tower.GetAttackModel().weapons[1].projectile.AddBehavior(slowModifierForTagModel);
            tower.GetAttackModel().weapons[1].projectile.pierce += 100;
            tower.GetAttackModel().weapons[1].Rate = tower.GetWeapon().Rate * 5f;
            tower.GetAttackModel().weapons[1].projectile.GetBehavior<DamageModel>().immuneBloonProperties = BloonProperties.Lead;
        }
    }

    public class BloontoniumEnergy : ModUpgrade<GunMonkey>
    {
        public override int Path => MIDDLE;
        public override int Tier => 4;
        public override int Cost => 8000;
        public override string Description => "Tazer has increase ceramic damage and more pierce.";

        public override void ApplyUpgrade(TowerModel tower)
        {

            tower.GetAttackModel().weapons[1].projectile.pierce *= 2;
            tower.GetAttackModel().weapons[1].Rate *= 0.5f;
            tower.GetAttackModel().weapons[1].projectile.GetDamageModel().damage += 2;
            tower.GetAttackModel().weapons[1].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Ceramic", "Ceramic", 1, 10, false, false));
        }
    }

    public class SunPoweredTazer : ModUpgrade<GunMonkey>
    {
        public override int Path => MIDDLE;
        public override int Tier => 5;
        public override int Cost => 70000;
        public override string Description => "Why not power it by the sun while were at it?";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetAttackModel().weapons[1].projectile.pierce *= 2;
            tower.GetAttackModel().weapons[1].Rate *= 0.5f;
            tower.GetAttackModel().weapons[1].projectile.GetDamageModel().damage += 5;
        }
    }
}

namespace GunMonkey.Upgrades.BottomPath
{
    public class PerformanceEnhancingDrugs : ModUpgrade<GunMonkey>
    {
        public override int Path => BOTTOM;
        public override int Tier => 2;
        public override int Cost => 150;
        public override string Description => "Increase the range of the gun monkey.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.range += 10;
            foreach (var attackModel in tower.GetAttackModels())
            {
                attackModel.range += 10;  
            }
        }
    }

    public class ArmorPiercingShot : ModUpgrade<GunMonkey>
    {
        public override int Path => BOTTOM;
        public override int Tier => 1;
        public override int Cost => 750;
        public override string Description => "Armor piercing shots can now pop leads.";

        public override int Priority => -2;

        public override void ApplyUpgrade(TowerModel tower)
        {
            foreach (var weaponModel in tower.GetWeapons())
            {
                weaponModel.projectile.GetBehavior<DamageModel>().immuneBloonProperties = BloonProperties.None;
            }
        }
    }

    public class GrenadeLauncher : ModUpgrade<GunMonkey>
    {
        public override int Path => BOTTOM;
        public override int Tier => 3;
        public override int Cost => 1950;
        public override string Description => "Shots now explodes on contact";

        public override void ApplyUpgrade(TowerModel tower)
        {
            var bomb = Game.instance.model.GetTower(TowerType.BombShooter, 2, 0, 0).GetWeapon().projectile.Duplicate();
            var pb = bomb.GetBehavior<CreateProjectileOnContactModel>();
            var sound = bomb.GetBehavior<CreateSoundOnProjectileCollisionModel>();
            var effect = bomb.GetBehavior<CreateEffectOnContactModel>();

            var behavior = new CreateProjectileOnExhaustFractionModel(
                "CreateProjectileOnExhaustFractionModel_",
                pb.projectile, pb.emission, 1f, 1f, true);
            tower.GetWeapon().projectile.AddBehavior(behavior);

            var soundBehavior = new CreateSoundOnProjectileExhaustModel(
                "CreateSoundOnProjectileExhaustModel_",
                sound.sound1, sound.sound2, sound.sound3, sound.sound4, sound.sound5);
            tower.GetWeapon().projectile.AddBehavior(soundBehavior);

            var eB = new CreateEffectOnExhaustedModel("CreateEffectOnExhaustedModel_", "", 0f, false,
                false, effect.effectModel);
            tower.GetWeapon().projectile.AddBehavior(eB);
            tower.GetWeapon().projectile.AddBehavior(pb);
            tower.GetWeapon().projectile.pierce += 1;
        }
    }

    public class TripleBarrel : ModUpgrade<GunMonkey>
    {
        public override int Path => BOTTOM;
        public override int Tier => 4;
        public override int Cost => 4550;
        public override string Description => "Why have one when you can have three?";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetWeapon().emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0, 75, null, false);
        }
    }

    public class GrenadeHurricane : ModUpgrade<GunMonkey>
    {
        public override int Path => BOTTOM;
        public override int Tier => 5;
        public override int Cost => 40950;
        public override string Description => "Even the biggest bloons will fear.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetWeapon().projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage = 1f;
            tower.GetWeapon().projectile.GetDamageModel().damage = 1f;

            tower.GetWeapon().projectile.pierce = 1;

            tower.GetWeapon().projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            tower.GetWeapon().projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Ceramic", "Ceramic", 0.000001f, 0, false, false));// i dont even know if that works but im keeping it

            tower.GetWeapon().projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 45, false, false));

            tower.GetWeapon().emission = new ArcEmissionModel("ArcEmissionModel_", 6, 0, 360, null, false);
        }
    }
}

namespace GunMonkey.Display
{
    public class Bullet : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, Name);
        }
    }

    public class BigBullet : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, Name);
        }
    }
}
