using System;

namespace Objects
{
    public enum Direction
    {
        Down = 0,
        Right = 1,
        Up = 2,
        Left = 3,
    }

    public static class DirectionHelper
    {
        public static Direction RandomDirection(Random random)
        {
            return (Direction) random.Next(0, 4);
        }

        public static Direction Add(this Direction direction, uint value)
        {
            return (Direction) (((int) direction + value) % 4);
        }

        public static Direction ComposeDirection(this Direction direction, Direction composition)
        {
            switch (composition)
            {
                case Direction.Up:
                    return direction.OppositeDirection();
                case Direction.Right:
                    return direction.Add(1);
                case Direction.Left:
                    return direction.Add(3);
                case Direction.Down:
                default:
                    return direction;
            }
        }

        public static Direction OppositeDirection(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return Direction.Up;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Left:
                    return Direction.Right;
                default:
                    return direction;
            }
        }
        
        public static float ToAngleRotation(this Direction direction)
        {
            return 90f * (int) direction;
        }
    }
}