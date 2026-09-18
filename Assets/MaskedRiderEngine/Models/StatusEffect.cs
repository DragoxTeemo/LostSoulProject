using System.Security.Cryptography.X509Certificates;
using MaskedRiderEngine.Core;
 
namespace MaskedRiderEngine.Models
{
    public class StatusEffect
    {
        public StaticEffectType Type {get;}
        public int RemainingTurns {get; private set;}
        public StatusEffect(StatusEffectType type, int durationTurns)
        {
            Type = type;
            RemainingTurns = durationTurns;
        }
 
        public bool IsExpired => RemainingTurns <= 0;
 
        public void Tick() => RemainingTurns--;
 
        public void Refresh(int durationTurns)
        {
            if (durationTurns > RemainingTurns) RemainingTurns = durationTurns;
        }
        public static bool PreventsAction(StatusEffectType type) => type == StatusEffectType.Stun || type == StatusEffectType.Sleep;
        public static int DefaultDuration(StatusEffectType type)
        {
            switch (type)
            {
                case StatusEffectType.Stun:  return GameConfig.StunDuration;
                case StatusEffectType.Sleep: return GameConfig.SleepDuration;
                case StatusEffectType.Blind: return GameConfig.BlindDuration;
                case StatusEffectType.Burn:  return GameConfig.BurnDuration;
                case StatusEffectType.Shock: return GameConfig.ShockDuration;
                default: return 0;

            }
        }
        public static int TickDamage(StatusEffectType type)
        {
            switch (type)
            {
                case StatusEffectType.Burn:  return GameConfig.BurnDamagePerTurn;
                case StatusEffectType.Shock: return GameConfig.ShockDamagePerTurn;
                default: return 0;
            }
        }
    }
}