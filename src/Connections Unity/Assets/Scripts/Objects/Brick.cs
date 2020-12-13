using System;
using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    [Serializable]
    public class Brick
    {
        public Vector2 position = Vector2.zero;
        public BrickType type = BrickType.None;
        public Direction facingDirection = Direction.Down;

        public List<Connector> connectors = new List<Connector>();

        public bool IsVertical => (type & BrickType.Vertical) != 0;
        public bool IsHorizontal => (type & BrickType.Horizontal) != 0;
        public bool IsRotation => (type & BrickType.Rotation) != 0;

        public override string ToString()
        {
            return $"Type : {type} ; Position : {position}";
        }
    }
}
