using System;
using System.Collections.Generic;
using System.Linq;
using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public class FormationManager
    {
        private readonly CombatantState[] _slots;

        public FormationManager(int capacity = GameConfig.PlayerFormationCapacity)
        {
            if (capacity < 1 || capacity > GameConfig.MaxFormationCapacity)
                throw new ArgumentOutOfRangeException(
                    nameof(capacity),
                    $"Formation capacity must be between 1 and {GameConfig.MaxFormationCapacity}.");

            _slots = new CombatantState[capacity];
        }

        public int Capacity => _slots.Length;

        public bool AssignPosition(CombatantState combatant, LineDepth depth)
            => AssignPosition(combatant, (int)depth);

        public bool AssignPosition(CombatantState combatant, int index)
        {
            if (combatant == null) return false;
            if (index < 0 || index >= _slots.Length) return false;
            if (_slots[index] != null) return false;

            _slots[index] = combatant;
            combatant.SlotIndex = index;
            return true;
        }

        public int AssignNextFree(CombatantState combatant)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)
                {
                    _slots[i] = combatant;
                    combatant.SlotIndex = i;
                    return i;
                }
            }
            return -1;
        }

        public bool Vacate(CombatantState combatant)
        {
            int index = Array.IndexOf(_slots, combatant);
            if (index < 0) return false;

            _slots[index] = null;
            combatant.SlotIndex = -1;
            return true;
        }

        public CombatantState GetForemostTarget()
        {
            for (int i = 0; i < _slots.Length; i++)
                if (_slots[i] != null && _slots[i].IsAlive) return _slots[i];

            return null; // Everyone in this formation is down
        }

        public List<CombatantState> GetLivingInOrder()
            => _slots.Where(slot => slot != null && slot.IsAlive).ToList();

        public IEnumerable<CombatantState> GetFirstNTargets(int count)
            => GetLivingInOrder().Take(count);

        public List<CombatantState> GetAdjacentSlots(CombatantState target)
        {
            var adjacent = new List<CombatantState>();
            int index = Array.IndexOf(_slots, target);
            if (index < 0) return adjacent;

            if (index - 1 >= 0 && _slots[index - 1] != null && _slots[index - 1].IsAlive)
                adjacent.Add(_slots[index - 1]);

            if (index + 1 < _slots.Length && _slots[index + 1] != null && _slots[index + 1].IsAlive)
                adjacent.Add(_slots[index + 1]);

            return adjacent;
        }
        public IEnumerable<CombatantState> GetAllActiveCombatants()
            => _slots.Where(slot => slot != null && slot.IsAlive);

        public bool IsWiped() => !GetAllActiveCombatants().Any();
    }
}