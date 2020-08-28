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
        Vertical = 1 << 2
    }
}