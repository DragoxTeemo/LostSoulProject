using System.Collections.Generic;
using MaskedRiderEngine.Core;
 
namespace MaskedRiderEngine.Progression
{
    public static class Music
    {
        public static readonly HashSet<string> MasterCatalog = new HashSet<string>
        {
            "Prince - Purple Rain",
            "Marvin Gaye - What's Going On",
            "Michael Jackson - Thriller",
            "Jimi Hendrix - Electric Ladyland",
            "The Rolling Stones - Sticky Fingers",
            "Talking Heads - Speaking In Tongues",
            "The Beatles - Let It Be",
            "The Beach Boys - Holland",
            "Beatles" - "White Album"
        };
 
        public const int FallbackEffect = 10;
        public const double CdReliefMultiplier = 1.0;
        public const double VinylReliefMultiplier = 1.5; // Vinyl is the one with the bonus
 
        public static int RollSongEffect(string albumKey)
            => MasterCatalog.Contains(albumKey) ? Rng.Next(20, 31) : FallbackEffect;
    }
}
