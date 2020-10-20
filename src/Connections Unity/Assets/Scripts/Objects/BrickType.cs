using System;

namespace Objects
{
    [Serializable]
    [Flags]
    public enum BrickType
    {
        None = 0,
        Rotation = 1 << 0,
        Horizontal = 1 << 1,
        Vertical = 1 << 2,
        
        RotationVertical = Rotation | Vertical,
        RotationHorizontal = Rotation | Horizontal,
        HorizontalVertical = Horizontal | Vertical,
        
        Complete = Rotation | Horizontal | Vertical,
    }

    public static class BrickTypeHelper
    {
        public static BrickType RandomType(Random random, Difficulty difficulty)
        {
            var isRotation = random.Next(0,2) > 0;
            var isHorizontal = random.Next(0, 2) > 0;
            var isVertical = random.Next(0, 2) > 0;

            if (isRotation && isHorizontal && isVertical && difficulty.IsAtLeast(Difficulty.Easy))
                return BrickType.Complete;

            if (isHorizontal && isVertical && difficulty.IsAtLeast(Difficulty.Easy))
                return BrickType.HorizontalVertical;
            if (isRotation && isVertical && difficulty.IsAtLeast(Difficulty.Medium))
                return BrickType.RotationVertical;
            if (isRotation && isHorizontal && difficulty.IsAtLeast(Difficulty.Medium))
                return BrickType.RotationHorizontal;

            if (isRotation && difficulty.IsAtLeast(Difficulty.Easy))
                return BrickType.Rotation;
            if (isHorizontal && difficulty.IsAtLeast(Difficulty.Hard))
                return BrickType.Horizontal;
            if (isVertical && difficulty.IsAtLeast(Difficulty.Hard))
                return BrickType.Vertical;
            
            return BrickType.None;
        }
    }
}