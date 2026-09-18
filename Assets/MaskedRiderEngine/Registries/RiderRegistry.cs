using System.Collections.Generic;
using MaskedRiderEngine.Core;

namespace MaskedRiderEngine.Models
{
    public static class RiderRegistry
    {
        private static readonly Dictionary<string, RegistryData> _riders =
            new Dictionary<string, RegistryData>();

        static RiderRegistry()
        {
            Register(new RegistryData
            {
                Id = "rider.nirvana",
                Name = "Lily",
                Codename = GameConfig.CodenameNirvana,
                Faction = Faction.Player,
                BaseResourceMax = 60,
                UsesHealthPoints = true,
                HasPassiveRegen = true,
                RegenAmount = 0,
                RequiresArmorRepair = false,
                ArmorClass = ArmorWeightClass.None,
                TargetingProfile = TargetingProfile.Any,
                ActionsPerTurn = 1,
                WeaponIds = new List<string>
                {
                    WeaponCatalog.ViciousClawSwipe,
                    WeaponCatalog.TendrilSweep,
                    WeaponCatalog.LilyPunch
                }
            });

            // ===== Alvin / Vector =====
            Register(new RegistryData
            {
                Id = "rider.vector",
                Name = "Alvin",
                Codename = GameConfig.CodenameVector,
                Faction = Faction.Player,
                BaseResourceMax = 32,
                UsesHealthPoints = false,
                HasPassiveRegen = false,
                RegenAmount = 0,
                RequiresArmorRepair = true,
                ArmorClass = ArmorWeightClass.Medium,
                TargetingProfile = TargetingProfile.FrontlineOnly,
                ActionsPerTurn = 1,
                WeaponIds = new List<string>
                {
                    WeaponCatalog.CaneStrike,
                    WeaponCatalog.VectorPunch
                }
            });

            // ===== Eva / Falcon =====
            // ActionsPerTurn = 2 is the ONE place her double attack is encoded.
            Register(new RegistryData
            {
                Id = "rider.falcon",
                Name = "Eva",
                Codename = GameConfig.CodenameFalcon,
                Faction = Faction.Player,
                BaseResourceMax = 38,
                UsesHealthPoints = false,
                HasPassiveRegen = false,
                RegenAmount = 0,
                RequiresArmorRepair = true,
                ArmorClass = ArmorWeightClass.Light,
                TargetingProfile = TargetingProfile.FirstTwo,
                ActionsPerTurn = 2,
                WeaponIds = new List<string>
                {
                    WeaponCatalog.RecklessAttack,
                    WeaponCatalog.UmbrellaBash
                }
            });

            // ===== Elliot / Circuit =====
            Register(new RegistryData
            {
                Id = "rider.circuit",
                Name = "Elliot",
                Codename = GameConfig.CodenameCircuit,
                Faction = Faction.Player,
                BaseResourceMax = 28,
                UsesHealthPoints = false,
                HasPassiveRegen = false,
                RegenAmount = 0,
                RequiresArmorRepair = true,
                ArmorClass = ArmorWeightClass.Medium,
                TargetingProfile = TargetingProfile.Any,
                ActionsPerTurn = 1,
                HasEngineeringDevices = true,
                MaxDevice = 4,
                WeaponIds = new List<string>
                {
                    WeaponCatalog.TeslaDischarge,
                    WeaponCatalog.ThermalEmitter,
                    WeaponCatalog.BlindingDart,
                    WeaponCatalog.DrowsySmoke,
                    WeaponCatalog.CircuitPunch
                }
            });
        }

        private static void Register(RegistryData data) => _riders[data.Codename] = data;

        public static RegistryData Get(string codename)
        {
            if (_riders.TryGetValue(codename, out var data)) return data;
            throw new KeyNotFoundException($"No rider registered with codename '{codename}'.");
        }
        public static IEnumerable<RegistryData> All => _riders.Values;
    }
}