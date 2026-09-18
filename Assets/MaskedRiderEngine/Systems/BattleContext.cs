using System.Collections.Generic;
using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;
 
namespace MaskedRiderEngine.Systems
{
    public class BattleContext
    {
        public FormationManager PlayerFormation {get;}
        public FormationManager EnemyFormation {get;}
        public int RoundNumber {get; private set;} = 0;
        public BattleContext(int playerCapacity = GameConfig.PlayerFormationCapacity, 
        int enemyCapacity = GameConfig.MaxFormationCapacity)
        {
            PlayerFormation = new FormationManager(playerCapacity);
            EnemyFormation = new FormationManager(enemyCapacity);
        }
        public FormationManager FormationOf(CombatantState combatant)
            => combatant.Faction == Faction.Player ? PlayerFormation : EnemyFormation;
        public FormationManager OpposingFormationOf(CombatantState combatant)
            => combatant.Faction == Faction.Player ? EnemyFormation : PlayerFormation;
 
        public List<CombatantState> GetAllies(CombatantState combatant)
            => FormationOf(combatant).GetLivingInOrder();
 
        public List<CombatantState> GetEnemies(CombatantState combatant)
            => OpposingFormationOf(combatant).GetLivingInOrder();
 
        public bool PlayersWiped => PlayerFormation.IsWiped();
        public bool EnemiesWiped => EnemyFormation.IsWiped();
        public bool IsBattleOver => PlayersWiped || EnemiesWiped;
        public void BeginRound()
        {
            RoundNumber++;
            foreach (var combatant in PlayerFormation.GetLivingInOrder())
                BeginTurnFor(combatant);
 
            foreach (var combatant in EnemyFormation.GetLivingInOrder())
                BeginTurnFor(combatant);
        }
        private void BeginTurnFor(CombatantState combatant)
        {
            combatant.Economy.ResetTurn();
            combatant.TurnsInBattle++;

            if (combatant.Codename == GameConfig.CodenameNirvana)
            {
                combatant.Sanity = System.Math.Min(
                    GameConfig.MaxSanity,
                    combatant.Sanity + GameConfig.SanityGainPerTurn);
            }
 
            StatusEffectService.ApplyTickDamage(combatant, this);
            combatant.TickStatuses();
        }
    }
}