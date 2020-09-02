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
        public static float ToAngleRotation(this Direction direction)
        {
            return 90f * (int) direction;
        }
    }
}