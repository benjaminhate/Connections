using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        private static void DrawGizmoGrid(GridManager gridManager, GizmoType gizmoType)
        {
            var transform = gridManager.transform;
            var position = transform.position;
            var positionX = position.x;
            var positionY = position.y;

            var localScale = transform.localScale;
            var scaleX = localScale.x;
            var scaleY = localScale.y;
            
            var height = gridManager.grid.height;
            var width = gridManager.grid.width;

            for (var i = 0; i <= height; i++)
            {
                Gizmos.DrawLine(new Vector3((i + 1 + positionX) * scaleX, (positionY + 1) * scaleY, 0),
                    new Vector3((i + 1 + positionX) * scaleX, (width + 1 + positionY) * scaleY, 0));
            }

            for (var j = 0; j <= width; j++)
            {
                Gizmos.DrawLine(new Vector3((positionX + 1) * scaleX, (j + 1 + positionY) * scaleY, 0),
                    new Vector3((height + 1 + positionX) * scaleX, (j + 1 + positionY) * scaleY, 0));
            }
        }
    }
}