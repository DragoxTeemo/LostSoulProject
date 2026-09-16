using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace MaskedRiderEngine
{
    public class GameConfig
    {
        public const int MaxSanity = 100;
        public const int MinSanity = 0;
        public const int SanityThresholdStressed = 21;
        public const int SanityThresholdManic = 81;
    } 

    public enum SanityTier
    {
        Sane, // 0-20
        Stressed, //21-80
        Manic // 81-100
    }

    public enum ArmorWeightClass
    {
        None, // Lily has no armor
        Light, // Eva (40% Damage reduction, 60% bleeds to PV)
        Medium // Alvin and Elliot (60% Damage reduction, 40% bleeds to PV)
    }

    public class Coefficients
    {
        public double A {get; set;} = 1;
        public double B {get; set;} = 1;
        public double C {get; set;} = 1;
    }

    public class RiderForm
    {
        public string FormName {get; set;}
        public string Elements {get; set;}
        public Dictionary<string, double> StatWeight {get; set;}
    }

    public class RegistryData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Codename { get; set; }
        
        // Resource & Armor Rules
        public int BaseResourceMax {get; set;} // Baseline HP for Lily, Baseline PV for Alvin, Elliot, Eva
        public bool UsesHealthPoints {get; set;} // True for Lily only, False = PV
        public int RegenAmount {get; set;} //deciding between percent
        public bool RequiresArmorRepair {get; set;} // True for everyone but Lily

        //Engineer Stats for Elliot/Circuit 
        public bool HasEngineeringDevices {get; set;}
        public int MaxDevice {get; set;} = 0;
        public Dictionary<string, RiderForm> Form {get; set;}
        public Dictionary<string, double> StatAffinities {get; set;}
    }
    public class MaskedRiderEntity
    {
        // Reference to the immutable blueprint data
        public RegistryData Blueprint { get; set; }
        public string InstanceId { get; set; }
        public int Level { get; set; } = 1;
        public int Xp { get; set; } = 0;
        public int Position { get; set; } // 1 (back) to 4 (front)
        public int CurrentResources { get; set; } // HP or PV
        public int ArmorIntegrityState { get; set; } = 100;

        //Convenience proxy properties to access blueprint rules cleanly without boilerplate
        public int MaxResource => Blueprint.BaseResourceMax;
        public bool UsesHealthPoints => Blueprint.UsesHealthPoints;
        public bool HasPassiveRegen => Blueprint.HasPassiveRegen;
        public int RegenAmount => Blueprint.RegenAmount;
        public bool RequiresArmorRepair => Blueprint.RequiresArmorRepair;
        public ArmorWeightClass ArmorClass {get; set;} = ArmorWeightClass.None;
        
    }

    public class CombatantState
    {
        public MaskedriderEntity Profile {get; set;} 
        public double Sanity {get; set;} = 0; // Sanity tracking (+2 after every full turn in combat)
        public bool IsDefeated {get; set;} = false;
        public int TurnsInBattle {get; set;} = 0;
        public int BattlesParticipated {get; set;} = 0; // A counter that might have a future use
        public int PositionOrder {get; set;} = 1; // 1 is frontline, 6 is backline (but only 4 for heroes)
        public bool ExtraAttack {get; set;} = false; // Only Eva has extra attack

        public SanityTier CurrentSanityTier
        {
            get
            {
                if (Sanity >= GameConfig.SanityThresholdManic) return SanityTier.Manic;
                if (Sanity >= GameConfig.SanityThresholdStressed) return SanityTier.Stressed;
                return SanityTier.Sane;
            }
        }
    }
    public class SafehouseManager
    {
        private static double _maniaDegradationPenalty = 0.0; 
        public static void ApplyHealer(CombatantState rider, bool isLilyManic, int consecutiveManic = 0)
        {
            double pvRestorePercent = 0.60; //PV stands for Perseverance
            double arRestorePercent = 0.40; // AR stands for Armor Integrity

            bool isLilyStressedOrManic = (rider.Profile.Codename == "Nirvana" && 
                (rider.CurrentSanityTier == SanityTier.Stressed || rider.CurrentSanityTier == SanityTier.Manic));

            if (isLilyStressedOrManic)
            {
                // Base penalty drops efficiency to 45% PV and 30% Armor
                pvRestorePercent = 0.45;
                arRestorePercent = 0.30;

                // Continuing not to treat Lily's condition decreases efficiency by 3% per unmanaged cycle, floored at 15% / 10%
                _maniaDegradationPenalty = Math.Min(0.30, consecutiveManicVisits * 0.03);
                pvRestorePercent -= _maniaDegradationPenalty; 
                arRestorePercent -= _maniaDegradationPenalty;

                pvRestorePercent = Math.Max(0.15, pvRestorePercent);
                arRestorePercent = Math.Max(0.10, arRestorePercent);
            }

            rider.Profile.CurrentResources = Math.Min(
                rider.Profile.MaxResource, 
                rider.Profile.CurrentResources + (int)(rider.Profile.MaxResource * pvRestorePercent)
            );

            // Restore Armor Integrity if the rider uses Technological armor i.e. everyone but Lily
            if (rider.Profile.RequiresArmorRepair)
            {
                rider.Profile.ArmorIntegrityState = Math.Min(
                    100, 
                    rider.Profile.ArmorIntegrityState + (int)(100 * arRestorePercent)
                );
            }
        }
    }

    public class Music
    {
        public static readonly HashSet<string> Albums = new HashSet<string>
        {
            "Prince - Purple Rain",
            "Marvin Gaye - What's Going On",
            "Michael Jackson - Thriller",
            "Jimi Hendrix - Electric Ladyland",
            "The Rolling Stones - Sticky Fingers",
            "Talking Heads - Speaking In Tongues",
            "The Beatles - Let It Be",
            "The Beach Boys - Holland"
        };

        private static readonly Random random = new Random();
        public static int PlaySong(string albumKey)
        {
            if (Albums.Contains(albumKey))
            {
                return random.Next(20,31);
            }
            return 10; // a backup for if the return random.Next() fails
        }
    }    
}