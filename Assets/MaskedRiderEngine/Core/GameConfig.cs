namespace MaskedRiderEngine.Core
{

    public static class GameConfig 
    {
        //Sanity
        public const int MaxSanity = 100;
        public const int MinSanity = 0;
        public const int SanityThresholdStressed = 21;
        public const int SanityThresholdManic = 81;
        public const double StressedDamageMultiplier = 1.10;

        //Armor
        public const int MaxArmorIntegrity = 100;
        public const double LightArmorReduction = 0.40;
        public const double MediumArmorReduction = 0.60;

        //Vector/Alvin
        public const double VectorMomentumPerTurn = 0.05;
        public const double VectorMomentumCap = 1.50;
        public const double VectorVarianceSuccessChance = 0.60;
        public const double VectorVarianceHighMulti = 1.25;
        public const double VectorVarianceLowMulti = 0.80;
        public const int VectorRallyPosition = 15;

        // Accuracy
        public const double BlindMissChance = 0.40;

        //Formation 
        public const int PlayerFormationCapacity = 4;
        public const int MaxFormationCapacity = 6;

        //Safehouse 
        public const double BasePvRestorePercent = 0.60;
        public const double BaseArRestorePercent = 0.40;
        public const double ImpairedPvRestorePercent = 0.45;
        public const double ImpairedArRestorePercent = 0.30;
        public const double DegradationPerUnmanagedVisit = 0.03;
        public const double MaxDegradationPenalty = 0.30;
        public const double PvRestoreFloor = 0.15;
        public const double ArRestoreFloor = 0.10;

        //Status effect turns
        public const int StunDureaction = 1;
        public const int SleepDuration = 2, BlindDuration = 2, ShockDuration = 2;
        public const int BurnDuration = 3, ShockDamagePerTurn = 3;
        public const int BurnDamagePerTurn = 4;

        // Codename
        public const string CodenameNirvana = "Nirvana";
        public const string CodenameVector = "Vector";
        public const string CodenameFalcon = "Falcon";
        public const string CodenameCircuit = "Circuit";
    }
}