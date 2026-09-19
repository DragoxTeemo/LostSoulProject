using System.Collections.Generic;
using MaskedRiderEngine.Core;

namespace MaskedRiderEngine.Models
{
    public class CombatantState
    {
        public MaskedRiderEntity Profile { get; }
        public CharacterActionEconomy Economy { get; }

        public int Sanity { get; set; } = 0;
        public bool IsDefeated { get; private set; } = false;
        public int TurnsInBattle { get; set; } = 0;
        public int BattlesParticipated { get; set; } = 0;

        //Set by FormationManager when the combatant is slotted.
        public int SlotIndex { get; internal set; } = -1;

        private readonly Dictionary<StatusEffectType, StatusEffect> _statuses = new Dictionary<StatusEffectType, StatusEffect>();

        public CombatantState(MaskedRiderEntity profile)
        {
            Profile = profile;
            Economy = new CharacterActionEconomy(profile.Blueprint);
        }

        public string Codename => Profile.Codename;
        public Faction Faction => Profile.Faction;
        public bool IsAlive => !IsDefeated && Profile.CurrentResources > 0;
        public int Speed => Profile.Speed;
        public SanityTier CurrentSanityTier
        {
            get
            {
                if (Sanity >= GameConfig.SanityThresholdManic) return SanityTier.Manic;
                if (Sanity >= GameConfig.SanityThresholdStressed) return SanityTier.Stressed;
                return SanityTier.Sane;
            }
        }

        public void MarkDefeated()
        {
            IsDefeated = true;
            _statuses.Clear();
        }

        public IEnumerable<StatusEffect> ActiveStatuses => _statuses.Values;
        public bool HasStatus(StatusEffectType type) => _statuses.ContainsKey(type);
        public void ApplyStatus(StatusEffectType type, int durationTurns)
        {
            if (type == StatusEffectType.None || durationTurns <= 0) return;

            if (_statuses.TryGetValue(type, out var existing)) existing.Refresh(durationTurns);
            else _statuses[type] = new StatusEffect(type, durationTurns);
        }

        public void ClearStatus(StatusEffectType type) => _statuses.Remove(type);

        public void TickStatuses()
        {
            var expired = new List<StatusEffectType>();
            foreach (var pair in _statuses)
            {
                pair.Value.Tick();
                if (pair.Value.IsExpired) expired.Add(pair.Key);
            }
            foreach (var type in expired) _statuses.Remove(type);
        }
        public bool IsIncapacitated
        {
            get
            {
                foreach (var status in _statuses.Values)
                    if (StatusEffect.PreventsAction(status.Type)) return true;
                return false;
            }
        }
    }
}