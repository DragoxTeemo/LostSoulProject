using System.Collections.Generic;

namespace MaskedRiderEngine.Models
{
    public enum AttackScope
    {
        SingleTarget,
        MultiTarget,
        AdjacentAOE,
        Extra
    }

    public enum ElementType
    {
        Physical,
        Electric,
        Fire
    }

    public class CharacterWeapon
    {
        public string Name {get; set;}
        public int BasePower {get; set;} 
        public AttackScope Scope {get; set;}
        public ElementType Element {get; set; }

        //Secondary effects and status 
        public double StatusEffectChance {get; set;} = 0.0;
        public string StatusEffectType {get; set;} = null; // stun, shock, drowsy, burn, blind, more
        public bool HasVarianceModifier {get; set;} = false; // Alvin's 60/40 variance rule
        public int MaxAmmo {get; set;} = 0; // Clip capacity
        public int CurrentAmmo {get; set;} = 0; // Counter for current ammo in use
        public int ReserveAmmo {get; set;} = 0; // Stored backups 
        public int AshCostToCraft {get; set;} = 0;
    }

    public static class CharacterLoadout
    {
        // Lily/Masked Rider Nirvana
        public static readonly List<CharacterWeapon> NirvanaAttacks = new List<CharacterWeapon>
        {
            new CharacterWeapon
            {
                Name = "Tendril Sweep",
                BasePower = 8, // Light Damage
                Scope = AttackScope.AdjacentAOE,
                Element = ElementType.Physical
            },
            new CharacterWeapon
            {
                Name = "Vicious Claw Swipe",
                BasePower = 12,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            },
            // When Lily is out of armor and fights Ink Spot only.
            new CharacterWeapon
            {
                Name = "Punch",
                BasePower = 0,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            }
        };
        // Alvin/Vector
        public static readonly List<CharacterWeapon> VectorAttacks = new List<CharacterWeapon>
        {
            new CharacterWeapon
            {
                Name = "Punch",
                BasePower = 8,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            },
            new CharacterWeapon
            {
                Name = "Cane Strike",
                BasePower = 12,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical,
                HasVarianceModifier = true
            },
            new CharacterWeapon
            {
                Name = "Null", // need to find name
                BasePower = 5,
                Scope = AttackScope.MultiTarget,
                Element = ElementType.Physical
            }
        };

        // Eva/Falcon
        public static readonly List<CharacterWeapon> FalconAttacks = new List<CharacterWeapon>
        {
            new CharacterWeapon
            {
                Name = "Reckless Attack",
                BasePower = 5,
                Scope = AttackScope.Extra, // Eva can attack twice per round
                Element = ElementType.Physical,
                StatusEffectChance = 0.25, //Chance to stun
                StatusEffectType = "Stun"
            },  
            new CharacterWeapon
            {
                Name = "Umbrella Bash",
                BasePower = 14,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical,
                StatusEffectChance = 0.0,
                StatusEffectType = null
            }
        };
        
        // Elliot/Circuit
        public static readonly List<CharacterWeapon> Circuit = new List<CharacterWeapon>
        {
            new CharacterWeapon
            {
                Name = "Punch",
                BasePower = 3,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            },
            new CharacterWeapon
            {
                Name = "Tesla Discharge",
                BasePower = 8,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Electric,
                StatusEffectChance = 0.30,
                StatusEffectType = "Shock",
                MaxAmmo = 6,
                CurrentAmmo = 6,
                ReserveAmmo = 12,
                AshCostToCraft = 10
            },
            new CharacterWeapon
            {
                Name = "Thermal Emitter",
                BasePower = 10,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Fire,
                StatusEffectChance = 0.40,
                StatusEffectType = "Burn",
                MaxAmmo = 6,
                CurrentAmmo = 6,
                ReserveAmmo = 12,
                AshCostToCraft = 10
            },
            new CharacterWeapon
            {
                Name = "Blinding Dart",
                BasePower = 3,             // Tiny damage for balance
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical,
                StatusEffectChance = 0.50, // 50% high chance to blind causing a chance to miss
                StatusEffectType = "Blind",
                MaxAmmo = 6,
                CurrentAmmo = 6,
                ReserveAmmo = 12,
                AshCostToCraft = 8
            },
            new CharacterWeapon
            {
                Name = "Drowsy Smoke",
                BasePower = 0,
                Scope = AttackScope.MultiTarget,
                Element = ElementType.Physical,
                StatusEffectChance = 0.80,
                StatusEffectType = "Sleep",
                MaxAmmo = 8,
                CurrentAmmo = 8,
                ReserveAmmo = 16,
                AshCostToCraft = 6
            }
        };
        public static readonly List<CharacterWeapon> BasicDemonAttacks = new List<CharacterWeapon>
        {
            new CharacterWeapon
            {
                Name = "Claw",
                BasePower = 8,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical,
            },
            new CharacterWeapon
            {
                Name = "Swipe",
                BasePower = 6,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            }
        };
        public static readonly List<CharacterWeapon> HorrorDemonAttack = new List<CharacterWeapon>
        {
            new CharacterWeapon
            {
                Name = "Bite",
                BasePower = 12,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical,
            },
            new CharacterWeapon
            {
                Name = "Screech",
                BasePower = 8,
                Scope = AttackScope.MultiTarget,
                Element = ElementType.Physical
            }
        };

        public static readonly List<CharacterWeapon> ToxicDemonAttack = new List<CharacterWeapon>
        {
            new CharacterWeapon
            {
                Name = "Toxic",
                BasePower = 10,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            },
            new CharacterWeapon
            {
                Name = "Miasma Cloud",
                BasePower = 6,
                Scope = AttackScope.SingleTarget,
                Element = ElementType.Physical
            }
        };
    }
}