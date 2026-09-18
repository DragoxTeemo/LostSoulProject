using System.Collections.Generic;
using MaskedRiderEngine.Core;

namespace MaskedRiderEngine.Models
{
    /// <summary>
    /// All weapon definitions, players and demons alike, keyed by id.
    /// Replaces CharacterLoadout. Blueprints now reference weapons by id rather
    /// than holding lists of shared mutable objects, which also removes the
    /// "nothing maps codename 'Circuit' to CharacterLoadout.Circuit" gap.
    /// </summary>
    public static class WeaponCatalog
    {
        // ---------------- Lily / Masked Rider Nirvana ----------------
        public const string TendrilSweep      = "nirvana.tendril_sweep";
        public const string ViciousClawSwipe  = "nirvana.vicious_claw_swipe";
        public const string LilyPunch         = "nirvana.punch";

        // ---------------- Alvin / Vector ----------------
        public const string VectorPunch = "vector.punch";
        public const string CaneStrike = "vector.cane_strike";
        public const string SweepingCane = "vector.sweeping_cane"; // was Name = "Null"

        // ---------------- Eva / Falcon ----------------
        public const string RecklessAttack = "falcon.reckless_attack";
        public const string UmbrellaBash = "falcon.umbrella_bash";

        // ---------------- Elliot / Circuit ----------------
        public const string CircuitPunch = "circuit.punch";
        public const string TeslaDischarge = "circuit.tesla_discharge";
        public const string ThermalEmitter = "circuit.thermal_emitter";
        public const string BlindingDart = "circuit.blinding_dart";
        public const string DrowsySmoke = "circuit.drowsy_smoke";

        // ---------------- Demonborn ----------------
        public const string DemonClaw = "demon.claw";
        public const string DemonSwipe = "demon.swipe";
        public const string HorrorBite = "demon.horror_bite";
        public const string HorrorScreech = "demon.horror_screech";
        public const string ToxicStrike = "demon.toxic_strike";
        public const string MiasmaCloud = "demon.miasma_cloud";

        private static readonly Dictionary<string, WeaponDefinition> _catalog =
            new Dictionary<string, WeaponDefinition>();

        static WeaponCatalog()
        {
            Register(new WeaponDefinition
            {
                Id = TendrilSweep,
                Name = "Tendril Sweep",
                BasePower = 8,
                Scope = AttackScope.AdjacentAOE,
                Element = ElementType.Physical
            });
            Register(new WeaponDefinition
            {
                Id = ViciousClawSwipe,
                Name = "Vicious Claw Swipe",
                BasePower = 12,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            });
            // Out of armor, versus Ink Spot only. BasePower 0 is deliberate.
            Register(new WeaponDefinition
            {
                Id = LilyPunch,
                Name = "Punch",
                BasePower = 0,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            });

            Register(new WeaponDefinition
            {
                Id = VectorPunch,
                Name = "Punch",
                BasePower = 8,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            });
            Register(new WeaponDefinition
            {
                Id = CaneStrike,
                Name = "Cane Strike",
                BasePower = 12,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical,
                HasVarianceModifier = true
            });
            Register(new WeaponDefinition
            {
                Id = RecklessAttack,
                Name = "Reckless Attack",
                BasePower = 5,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical,
                StatusEffectChance = 0.25,
                StatusEffect = StatusEffectType.Stun
            });
            Register(new WeaponDefinition
            {
                Id = UmbrellaBash,
                Name = "Umbrella Bash",
                BasePower = 14,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            });

            Register(new WeaponDefinition
            {
                Id = CircuitPunch,
                Name = "Punch",
                BasePower = 3,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            });
            Register(new WeaponDefinition
            {
                Id = TeslaDischarge,
                Name = "Tesla Discharge",
                BasePower = 8,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Electric,
                StatusEffectChance = 0.30,
                StatusEffect = StatusEffectType.Shock,
                MaxAmmo = 6,
                StartingReserveAmmo = 12,
                AshCostToCraft = 10
            });
            Register(new WeaponDefinition
            {
                Id = ThermalEmitter,
                Name = "Thermal Emitter",
                BasePower = 10,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Fire,
                StatusEffectChance = 0.40,
                StatusEffect = StatusEffectType.Burn,
                MaxAmmo = 6,
                StartingReserveAmmo = 12,
                AshCostToCraft = 10
            });
            Register(new WeaponDefinition
            {
                Id = BlindingDart,
                Name = "Blinding Dart",
                BasePower = 3,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical,
                StatusEffectChance = 0.50,
                StatusEffect = StatusEffectType.Blind,
                MaxAmmo = 6,
                StartingReserveAmmo = 12,
                AshCostToCraft = 8
            });
            // BasePower 0 is deliberate: pure status weapon.
            Register(new WeaponDefinition
            {
                Id = DrowsySmoke,
                Name = "Drowsy Smoke",
                BasePower = 0,
                Scope = AttackScope.MultiTarget,
                Element = ElementType.Physical,
                StatusEffectChance = 0.80,
                StatusEffect = StatusEffectType.Sleep,
                MaxAmmo = 8,
                StartingReserveAmmo = 16,
                AshCostToCraft = 6
            });

            Register(new WeaponDefinition
            {
                Id = DemonClaw, Name = "Claw", 
                BasePower = 8,
                Scope = AttackScope.SingleTarget, 
                Element = ElementType.Physical
            });
            Register(new WeaponDefinition
            {
                Id = DemonSwipe, Name = "Swipe", 
                BasePower = 6,
                Scope = AttackScope.SingleTarget, 
                Element = ElementType.Physical
            });
            Register(new WeaponDefinition
            {
                Id = HorrorBite, Name = "Bite", 
                BasePower = 12,
                Scope = AttackScope.SingleTarget, 
                Element = ElementType.Physical
            });
            Register(new WeaponDefinition
            {
                Id = HorrorScreech, Name = "Screech", 
                BasePower = 8,
                Scope = AttackScope.MultiTarget, Element = 
                ElementType.Physical
            });
            Register(new WeaponDefinition
            {
                Id = ToxicStrike, Name = "Toxic", 
                BasePower = 10,
                Scope = AttackScope.SingleTarget, 
                Element = ElementType.Physical
            });
            // Was SingleTarget despite the name. Corrected to AdjacentAOE.
            Register(new WeaponDefinition
            {
                Id = MiasmaCloud, 
                Name = "Miasma Cloud", BasePower = 6,
                Scope = AttackScope.AdjacentAOE, 
                Element = ElementType.Physical
            });
        }

        private static void Register(WeaponDefinition definition)
            => _catalog[definition.Id] = definition;

        public static WeaponDefinition Get(string id)
        {
            if (_catalog.TryGetValue(id, out var definition)) return definition;
            throw new KeyNotFoundException($"No weapon registered with id '{id}'.");
        }

        public static IEnumerable<WeaponDefinition> All => _catalog.Values;
    }
}