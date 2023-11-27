using Random = UnityEngine.Random;

namespace Helpers
{
    public static class RandomChance 
    {
        public static bool Chance(int percent)
        {
            var random = Random.Range(0, 100);
            return random <= percent;
        }
    }
}
