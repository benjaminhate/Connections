using System;
using System.Collections.Generic;
using Random = System.Random;

namespace Objects
{
    [Serializable]
    public class Grid
    {
        public int height = 4;
        public int width = 4;
        public List<Brick> content;

        public void GenerateLevel(int seed)
        {
            var random = new Random(seed);
            content = new List<Brick>();
        }

        public void RandomizeLevel()
        {
            
        }
    }
}
