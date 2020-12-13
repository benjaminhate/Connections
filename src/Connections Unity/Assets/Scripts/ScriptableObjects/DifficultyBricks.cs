using System;
using System.Collections.Generic;
using System.Linq;
using Objects;
using UnityEngine;
using Random = System.Random;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "DifficultyBricks", menuName = "Game/Brick/Difficulty", order = 0)]
    public class DifficultyBricks : ScriptableObject
    {
        public List<DifficultyBrickType> easy;
        public List<DifficultyBrickType> medium;
        public List<DifficultyBrickType> hard;

        private IEnumerable<DifficultyBrickType> GetDifficultyBricks(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return easy;
                case Difficulty.Medium:
                    return medium;
                case Difficulty.Hard:
                    return hard;
                default:
                    return null;
            }
        }

        public BrickType GetRandomBrickType(Random random, Difficulty difficulty)
        {
            var rand = random.Next(1, 101);
            var list = GetDifficultyBricks(difficulty).ToList();
            var randLeft = rand;
            foreach (var difficultyBrickType in list)
            {
                if (randLeft < difficultyBrickType.percentage)
                {
                    return difficultyBrickType.type;
                }

                randLeft -= (int)difficultyBrickType.percentage;
            }

            return !list.Any() ? BrickType.None : list[list.Count() - 1].type;
        }
    }

    [Serializable]
    public class DifficultyBrickType
    {
        public BrickType type;
        public float percentage;
    }
}
