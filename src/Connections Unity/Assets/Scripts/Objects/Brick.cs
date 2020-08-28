using System;
using UnityEngine;

namespace Objects
{
    [Serializable]
    public class Brick
    {
        public Vector2 position;
        public BrickType type;
        public Direction facingDirection;
    }
}
