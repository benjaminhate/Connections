using System;
using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BrickColors", menuName = "Game/Brick/Colors", order = 0)]
    public class BrickColors : ScriptableObject
    {
        public List<BrickColor> colors;

        public void ChangeColor(BrickManager target)
        {
            var targetSpriteRenderer = target.GetComponent<SpriteRenderer>();
            if (targetSpriteRenderer == null)
                return;
            
            var color = colors.Find(c => c.type == target.type);
            if (color != null && color.sprite != null)
            {
                targetSpriteRenderer.sprite = color.sprite;
                targetSpriteRenderer.color = Color.white;
            }
        }
    }

    [Serializable]
    public class BrickColor
    {
        public Sprite sprite;
        public BrickType type;
    }
}