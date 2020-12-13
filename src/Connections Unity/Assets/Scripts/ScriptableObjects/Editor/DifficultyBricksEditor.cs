using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace ScriptableObjects.Editor
{
    [CustomEditor(typeof(DifficultyBricks))]
    public class DifficultyBricksEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var controller = (DifficultyBricks) target;
            CheckListPercentage(controller.easy, "Easy");
            CheckListPercentage(controller.medium, "Medium");
            CheckListPercentage(controller.hard, "Hard");
        }

        private void CheckListPercentage(IEnumerable<DifficultyBrickType> list, string listName)
        {
            var percentageSum = list.Sum(i => i.percentage);
            if (percentageSum < 100f)
            {
                EditorGUILayout.HelpBox(
                    $"The list {listName} does not have a percentage sum of 100. You must increase the percentages.",
                    MessageType.Warning);
            }

            if (percentageSum > 100f)
            {
                EditorGUILayout.HelpBox(
                    $"The list {listName} does not have a percentage sum of 100. You must decrease the percentages.",
                    MessageType.Error);
            }
        }
    }
}