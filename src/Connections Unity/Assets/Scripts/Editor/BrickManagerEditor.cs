using System.Linq;
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

            var height = 0;
            var width = 0;
            if (brickManager.gridManager != null)
            {
                height = brickManager.gridManager.grid.height;
                width = brickManager.gridManager.grid.width;    
            }

            if ((brickManager.type & BrickType.Horizontal) != 0)
            {
                var randomizeHorizontal = GUILayout.Button("Randomize Horizontal");
                if (randomizeHorizontal)
                {
                    RandomizeHorizontal(height);   
                }
            }

            if ((brickManager.type & BrickType.Vertical) != 0)
            {
                var randomizeVertical = GUILayout.Button("Randomize Vertical");
                if (randomizeVertical)
                {
                    RandomizeVertical(width);
                }
            }

            if ((brickManager.type & BrickType.Rotation) != 0)
            {
                var randomizeRotation = GUILayout.Button("Randomize Rotation");
                if (randomizeRotation)
                {
                    RandomizeRotation();
                }
            }

            var neighboursButton = GUILayout.Button("Print Neighbours");
            if (neighboursButton)
            {
                var neighbours = brickManager.GetNeighbours();
                neighbours.ForEach(n => Debug.Log(n));
            }
        }

        private void RandomizeVertical(int width)
        {
            var brickManager = (BrickManager) target;
            
            if (!brickManager.IsVertical)
                return;

            var randomPosition = Random.Range(0, width);
            var transform = brickManager.transform;
            transform.localPosition = new Vector3(transform.localPosition.x, randomPosition, 0);
        }

        private void RandomizeHorizontal(int height)
        {
            var brickManager = (BrickManager) target;
            
            if (!brickManager.IsHorizontal)
                return;

            var randomPosition = Random.Range(0, height);
            var transform = brickManager.transform;
            transform.localPosition = new Vector3(randomPosition, transform.localPosition.y, 0);
        }

        private void RandomizeRotation()
        {
            var brickManager = (BrickManager) target;
            
            if (!brickManager.IsRotation)
                return;

            var randomDirection = Random.Range(0, 4);
            switch (randomDirection)
            {
                case 0:
                    brickManager.initialFacingDirection = Direction.Up;
                    break;
                case 1:
                    brickManager.initialFacingDirection = Direction.Right;
                    break;
                case 2:
                    brickManager.initialFacingDirection = Direction.Down;
                    break;
                case 3:
                    brickManager.initialFacingDirection = Direction.Left;
                    break;
            }

            brickManager.transform.rotation = Quaternion.Euler(0, 0, brickManager.initialFacingDirection.ToAngleRotation());
        }
    }
}