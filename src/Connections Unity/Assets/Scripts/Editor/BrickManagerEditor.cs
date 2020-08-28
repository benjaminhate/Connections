using Objects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(BrickManager))]
    [CanEditMultipleObjects]
    public class BrickManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var brickManager = (BrickManager) target;
            if (brickManager.colors != null)
            {
                brickManager.colors.ChangeColor(brickManager);
            }

            if ((brickManager.type & BrickType.Horizontal) != 0)
            {
                var randomizeHorizontal = GUILayout.Button("Randomize Horizontal");
                if (randomizeHorizontal)
                {
                    brickManager.RandomizeHorizontal(4);   
                }
            }

            if ((brickManager.type & BrickType.Vertical) != 0)
            {
                var randomizeVertical = GUILayout.Button("Randomize Vertical");
                if (randomizeVertical)
                {
                    brickManager.RandomizeVertical(4);
                }
            }

            if ((brickManager.type & BrickType.Rotation) != 0)
            {
                var randomizeRotation = GUILayout.Button("Randomize Rotation");
                if (randomizeRotation)
                {
                    brickManager.RandomizeRotation();
                }
            }
        }
    }
}