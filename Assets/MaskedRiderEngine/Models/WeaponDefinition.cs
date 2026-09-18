using MaskedRiderEngine.Core;

namespace MaskedRiderEngine.Models
{
    public class WeaponDefinition {
        public string Id {get; init;} 
        public string Name  {get; init;}

        public int BasePower {get; init;}
        public AttackScope Scope {get; init;} = AttackScope.SingleTarget;
        public ElementType Element {get; init;} = ElementType.Physical;

        //Secondary effects
        public double StatusEffectChance {get; init;} = 0.0;
        public StatusEffectType StatusEffect {get; init;} = StatusEffectType.None;

        // Alvin's 60/40 variance rule
        public bool HasVarianceModifier {get; init;} = false;
        // Ammunition rules. MaxAmmo == 0 means a melee
        public int MaxAmmo {get; init;} = 0;
        public int StartingReserveAmmo {get; init;} = 0;
        public int AshCostToCraft {get; init;} = 0;

        public bool IsAmmoBased => MaxAmmo = 0;
    }

    public class WeaponInstance
    {
        public WeaponDefinition Definition {get;}
        public int CurrentAmmo {get; private set;}
        public int ReserveAmmo {get; private set;}
        public WeaponInstance (WeaponDefinition definition)
        {
            Definition = definition;
            CurrentAmmo = definition.MaxAmmo;
            ReserveAmmo = definition.StartingReserveAmmo;
        }
        public string Name => Definition.Name;
        public bool IsAmmoBased => Definition.IsAmmoBased;

        public bool CanFire => !IsAmmoBased || CurrentAmmo > 0;

        public bool TryConsumeRound()
        {
            if (!IsAmmoBased) return true;
            if (CurrentAmmo <= 0) return false;
            CurrentAmmo--;
            return true;
        }

        public bool TryReload()
        {
            if (!IsAmmoBased) return false; // Melee Ammo
            int needed = Definition.MaxAmmo - CurrentAmmo;
            if (needed <= 0) return false;  //Full clip already
            if (ReserveAmmo) return false;  //Out of reserve ammo
        }

        public void AddReserve(int rounds)
        {
            if (rounds > 0) ReserveAmmo += rounds;
        }
    }
}