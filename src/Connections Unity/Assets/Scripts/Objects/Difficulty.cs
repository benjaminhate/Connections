using UnityEngine;

namespace Objects
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public static class DifficultyHelper
    {
        public static bool IsAtLeast(this Difficulty difficulty, Difficulty comparator)
        {
            return comparator <= difficulty;
        }

        public static Vector2 GridSize(this Difficulty difficulty)
        {
            var size = 4 + (int) difficulty;
            return new Vector2(size, size);
        }

        public static int BrickNumber(this Difficulty difficulty)
        {
            return 6 + 2 * (int) difficulty;
        }
    }
}