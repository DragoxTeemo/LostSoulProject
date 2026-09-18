using System;
using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;
 
namespace MaskedRiderEngine.Progression
{
    public static class SafehouseManager
    {
        public class RestorationResult
        {
            public int ResourceRestored { get; set; }
            public int ArmorRestored { get; set; }
            public double PvPercentApplied { get; set; }
            public double ArPercentApplied { get; set; }
            public bool WasImpaired { get; set; }
        }
        /// <param name="consecutiveUntreatedVisits">
        /// How many safehouse visits in a row Lily's condition has gone
        /// unmanaged. Each one costs 3% efficiency, capped at 30%.
        /// </param>
        
        public static RestorationResult ApplyHealer(CombatantState rider, int consecutiveUntreatedVisits = 0)
        {
            var result = new RestorationResult();
            double pvRestorePercent = GameConfig.BasePvRestorePercent;
            double arRestorePercent = GameConfig.BaseArRestorePercent;
 
            bool isImpaired = rider.Codename == GameConfig.CodenameNirvana
                && (rider.CurrentSanityTier == SanityTier.Stressed
                    || rider.CurrentSanityTier == SanityTier.Manic);

            if (isImpaired)
            {
                pvRestorePercent = GameConfig.ImpairedPvRestorePercent;
                arRestorePercent = GameConfig.ImpairedArRestorePercent;
 
                double degradation = Math.Min(GameConfig.MaxDegradationPenalty, consecutiveUntreatedVisits * GameConfig.DegradationPerUnmanagedVisit);
            pvRestorePercent = Math.Max(GameConfig.PvRestoreFloor, pvRestorePercent - degradation);
                arRestorePercent = Math.Max(GameConfig.ArRestoreFloor, arRestorePercent - degradation);
            }
            int before = rider.Profile.CurrentResources;
            rider.Profile.CurrentResources = Math.Min(
                rider.Profile.MaxResource,
                rider.Profile.CurrentResources + (int)(rider.Profile.MaxResource * pvRestorePercent));
            result.ResourceRestored = rider.Profile.CurrentResources - before;
 
            if (rider.Profile.RequiresArmorRepair
                && rider.Profile.ArmorClass != ArmorWeightClass.None)
            {
                int armorBefore = rider.Profile.ArmorIntegrityState;
                rider.Profile.ArmorIntegrityState = Math.Min(
                    GameConfig.MaxArmorIntegrity,
                    rider.Profile.ArmorIntegrityState
                        + (int)(GameConfig.MaxArmorIntegrity * arRestorePercent));
                result.ArmorRestored = rider.Profile.ArmorIntegrityState - armorBefore;
            }
            result.PvPercentApplied = pvRestorePercent;
            result.ArPercentApplied = arRestorePercent;
            result.WasImpaired = isImpaired;
            return result;
        }
    }
}