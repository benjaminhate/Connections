using System;
using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game/Configuration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        public List<DifficultyConfiguration> difficultyConfigurations;

        public DifficultyConfiguration GetConfiguration(Difficulty difficulty)
        {
            var config = difficultyConfigurations.Find(c => c.difficulty == difficulty);
            if(config == null) throw new ArgumentException($"Difficulty {difficulty} not found");

            return config;
        }
    }

    [Serializable]
    public class DifficultyConfiguration
    {
        public Difficulty difficulty;
        
        public int cameraSize;
        public float cameraX;
        public float cameraY;
    }
}
