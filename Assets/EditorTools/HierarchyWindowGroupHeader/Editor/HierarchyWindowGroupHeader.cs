/**
 * Created by: Edward Atencio
 * Created on: 18/06/19 (dd/mm/yy)
 * Source: http://diegogiacomelli.com.br/unitytips-hierarchy-window-group-header/
 */

namespace UnityEditorTools
{
    using UnityEngine;
    using UnityEditor;

    [InitializeOnLoad]
    public class HierarchyWindowGroupHeader : MonoBehaviour
    {
        private static readonly Color backgroundColor = Color.gray;
        private static readonly Color borderColor = Color.white;

        static HierarchyWindowGroupHeader()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (gameObject != null && gameObject.name.IndexOf('-') == 0)
            {
                Rect backgroundRect = selectionRect;

#if UNITY_2019
                backgroundRect.x += 25;
                backgroundRect.width -= 25;
#endif

                Rect borderTopRect = backgroundRect;
                borderTopRect.height = 1;

                Rect borderBottomRect = backgroundRect;
                borderBottomRect.height = 1;
                borderBottomRect.y = backgroundRect.yMax;

                Rect borderRightRect = backgroundRect;
                borderRightRect.width = 1;
                borderRightRect.x = backgroundRect.xMax;

                Rect borderLeftRect = backgroundRect;
                borderLeftRect.width = 1;

                Rect labelRect = backgroundRect;
                labelRect.height -= 2;

                EditorGUI.DrawRect(backgroundRect, backgroundColor);
                EditorGUI.DrawRect(borderTopRect, borderColor);
                EditorGUI.DrawRect(borderBottomRect, borderColor);
                EditorGUI.DrawRect(borderRightRect, borderColor);
                EditorGUI.DrawRect(borderLeftRect, borderColor);
                EditorGUI.DropShadowLabel(labelRect, gameObject.name.Substring(1).ToUpperInvariant());
            }
        }
    }
}
