/**
 * Created by: Edward Atencio
 * Created on: 18/09/19 (dd/mm/yy)
 * Source: https://github.com/JohnAlbin/UnityReadme
 */

namespace UnityEditorTools.PackageExporter
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(ReadmeAsset))]
    public class ReadmeAssetDrawer : Editor
    {
        /*************************************************************************************************
        *** Variables
        *************************************************************************************************/
        private static GUIStyle title { get { return new GUIStyle() { wordWrap = true, fontSize = 24 }; } }

        private static GUIStyle h1 { get { return new GUIStyle() { wordWrap = true, fontSize = 18, fontStyle = FontStyle.Bold }; } }

        private static GUIStyle h2 { get { return new GUIStyle() { wordWrap = true, fontSize = 16, fontStyle = FontStyle.Bold }; } }

        private static GUIStyle body { get { return new GUIStyle(EditorStyles.label) { wordWrap = true, fontSize = 14, richText = true }; } }

        private static GUIStyle code { get { return new GUIStyle(EditorStyles.helpBox) { wordWrap = true, fontSize = 14 }; } }

        private static GUIStyle link
        {
            get
            {
                return new GUIStyle(body)
                {
                    normal = new GUIStyleState() { textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f) },
                    stretchWidth = false
                };
            }
        }

        private static Parser parser = new Parser();

        /*************************************************************************************************
        *** OnGUI
        *************************************************************************************************/
        protected override void OnHeaderGUI()
        {
            ReadmeAsset readme = target as ReadmeAsset;

            GUILayout.BeginHorizontal("In BigTitle");
            {
                const float iconSize = 32f;
                GUILayout.Label(readme.icon
                                , new GUIStyle() { alignment = TextAnchor.MiddleCenter }
                                , GUILayout.Width(iconSize), GUILayout.Height(iconSize));
                GUILayout.Space(4f);

                GUILayout.BeginVertical();
                {
                    GUILayout.Space(1f);
                    GUILayout.Label(readme.title, title);
                }
                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            ReadmeAsset readme = target as ReadmeAsset;
            GUILayout.Space(4f);
            parser.Parse(readme.GetContentLines());
            readme.content = GUILayout.TextArea(readme.content, EditorStyles.helpBox);
        }

        /*************************************************************************************************
        *** Methods
        *************************************************************************************************/
        private bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
        {
            var position = GUILayoutUtility.GetRect(label, link, options);

            Handles.BeginGUI();
            Handles.color = link.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return GUI.Button(position, label, link);
        }
    }
}
