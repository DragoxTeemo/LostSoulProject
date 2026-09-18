using System.Reflection.Metadata;

namespace MaskedRiderEngine.Models
{
    public class CharacterActionEconomy
    {
        public int MaxActions {get;}
        public int MaxBonusActions {get;}
        public int ActionsSpent {get; private set;}
        public int BonusActionsSpent {get; private set;}
        public CharacterActionEconomy(RegistryData blueprint)
        {
            MaxActions = blueprint.ActionsPerTurn > 0 ? blueprint.ActionsPerTurn : 1;
            MaxBonusActions = blueprint.BonusActionsPerTurn;
        }
        public bool HasActionRemaining => ActionSpent < MaxActions;
        public bool HasBonusActionRemaining => BonusActionsSpent < MaxBonusActions;
        public int ActionsRemaining => MaxActions - ActionsSpent;
        public bool TryConsumeAction()
        {
            if (!HasActionRemaining) return false;
            ActionsSpent++;
            return true;
        }
 
        public bool TryConsumeBonusAction()
        {
            if (!HasBonusActionRemaining) return false;
            BonusActionsSpent++;
            return true;
        }

        public void ResetTurn()
        {
            ActionsSpent = 0;
            BonusActionsSpent = 0;
        }
    }
}