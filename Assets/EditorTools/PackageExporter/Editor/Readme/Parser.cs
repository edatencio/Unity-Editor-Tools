/**
 * Created by: Edward Atencio
 * Created on: 18/09/19 (dd/mm/yy)
 */

namespace UnityEditorTools.PackageExporter
{
    using UnityEngine;
    using UnityEditor;

    public class Parser
    {
        private const string openColorTag = "<color=teal>";
        private const string closeColorTag = "</color>";
        private const string token = "```";
        private bool tokenOpen = false;

        private static GUIStyle title { get { return new GUIStyle() { wordWrap = true, fontSize = 24 }; } }

        private static GUIStyle h1 { get { return new GUIStyle() { wordWrap = true, fontSize = 18, fontStyle = FontStyle.Bold }; } }

        private static GUIStyle h2 { get { return new GUIStyle() { wordWrap = true, fontSize = 16, fontStyle = FontStyle.Bold }; } }

        private static GUIStyle body { get { return new GUIStyle(EditorStyles.label) { wordWrap = true, fontSize = 14, stretchWidth = false, richText = true }; } }

        private static GUIStyle code { get { return new GUIStyle(EditorStyles.helpBox) { wordWrap = true, fontSize = 14, stretchWidth = false }; } }

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

        public void Parse(string[] lines)
        {
            tokenOpen = false;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf("##") == 0)
                {
                    GUILayout.Label(lines[i].Substring(2), h2);
                    continue;
                }

                if (lines[i].IndexOf('#') == 0)
                {
                    GUILayout.Label(lines[i].Substring(1), h1);
                    GUILayout.Box(GUIContent.none, EditorStyles.helpBox, GUILayout.Height(5f));
                    continue;
                }

                if (lines[i].Length == 3 && lines[i].IndexOf("---") == 0)
                {
                    GUILayout.Box(GUIContent.none, EditorStyles.helpBox, GUILayout.Height(5f));
                    continue;
                }

                ParseCodeRichText(ref lines[i]);

                GUILayout.Label(lines[i], body);
            }
        }

        private void ParseCodeRichText(ref string text)
        {
            int index = text.IndexOf(token);

            if (index >= 0)
            {
                text = string.Concat(text.Substring(0, index)
                                     , tokenOpen ? closeColorTag : openColorTag
                                     , text.Substring(index + token.Length));

                tokenOpen = !tokenOpen;

                if (text.IndexOf(token) > 0)
                    ParseCodeRichText(ref text);
            }
        }

        private void ParseCodeLabel(string text)
        {
            int index = text.IndexOf(token);
            if (index < 0)
            {
                if (!tokenOpen)
                    GUILayout.Label(text, body);
                else
                    GUILayout.Label(text, code);
            }
            else
            {
                if (!tokenOpen)
                    GUILayout.Label(text.Substring(0, index), body);
                else
                    GUILayout.Label(text.Substring(0, index), code);

                tokenOpen = !tokenOpen;

                if (text.Length - (index + 3) > 0)
                    ParseCodeLabel(text.Substring(index + 3));
            }
        }
    }
}
