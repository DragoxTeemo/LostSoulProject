using System.Collections.Generic;
using System.Data.Common;
using MaskedRiderEngine.Core;
 
namespace MaskedRiderEngine.Models
{
    public static class DemonRegistry
    {
        public const string BasicDemonborn = "BasicDemonborn";
        public const string HorrorDemonborn = "HorrorDemonborn";
        public const string ToxicDemonborn = "ToxicDemonborn";
        public const string InkSpot = "InkSpot";
        private static readonly Dictionary<string, RegistryData> _demons = new Dictionary<string, RegistryData>();

        static DemonRegistry()
        {
            Register(new RegistryData
            {
                Id = "demon.basic",
                Name = "Demonborn",
                Codename = BasicDemonborn,
                Faction = Faction.Enemy,
                BaseResourceMax = 40,
                UsesHealthPoints = true,
                RequiresArmorRepair = false,
                ArmorClass = ArmorWeightClass.None,
                TargetingProfile = TargetingProfile.FrontlineOnly,
                WeaponIds = new List<string> { WeaponCatalog.DemonClaw, WeaponCatalog.DemonSwipe},
                XpReward = 12,
                AshReward = 5
            });
            Register(new RegistryData
            {
               Id = "demon.toxic",
                Name = "Toxic Demonborn",
                Codename = ToxicDemonborn,
                Faction = Faction.Enemy,
                BaseResourceMax = 32,
                UsesHealthPoints = true,
                RequiresArmorRepair = false,
                ArmorClass = ArmorWeightClass.None,
                TargetingProfile = TargetingProfile.BacklineTwo,
                WeaponIds = new List<string> { WeaponCatalog.MiasmaCloud, WeaponCatalog.ToxicStrike },
                XpReward = 14,
                AshReward = 6
            });
            Register(new RegistryData
            {
                Id = "demon.horror",
                Name = "Horror Demonborn",
                Codename = HorrorDemonborn,
                Faction = Faction.Enemy,
                BaseResourceMax = 60,
                UsesHealthPoints = true,
                RequiresArmorRepair = false,
                ArmorClass = ArmorWeightClass.None,
                TargetingProfile = TargetingProfile.FirstTwo,
                WeaponIds = new List<string> { WeaponCatalog.HorrorBite, WeaponCatalog.HorrorScreech },
                XpReward = 25,
                AshReward = 12
            });
            Register(new RegistryData
            {
                Id = "demon.inkspot",
                Name = "Ink Spot",
                Codename = InkSpot,
                Faction = Faction.Enemy,
                BaseResourceMax = 1,
                UsesHealthPoints = true,
                RequiresArmorRepair = false,
                ArmorClass = ArmorWeightClass.None,
                TargetingProfile = TargetingProfile.Any,
                WeaponIds = new List<string>(),
                XpReward = 0,
                AshReward = 0
            });
        }
        private static void Register(RegistryData data) => _demons[data.Codename] = data;
 
        public static RegistryData Get(string codename)
        {
            if (_demons.TryGetValue(codename, out var data)) return data;
            throw new KeyNotFoundException($"No demon registered with codename '{codename}'.");
        }
 
        public static IEnumerable<RegistryData> All => _demons.Values;
    }
}