/**
 * Created by: Edward Atencio
 * Created on: 18/09/19 (dd/mm/yy)
 */

namespace UnityEditorTools.PackageExporter
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;
    using Builder;
    using Version = Builder.Version;

    public class PackageExporterWindow : EditorWindow
    {
        /*************************************************************************************************
        *** Variables
        *************************************************************************************************/
        private static Vector2 scrollPosition;
        private static Vector2 scrollPosition2;
        private static Action openBuildSettigns = () => EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        private static Version version;
        private static bool once = false;

        private static string readme;

        /*************************************************************************************************
        *** MenuItem
        *************************************************************************************************/
        [MenuItem("Tools/Package Exporter _F4")]
        public static void ShowWindow()
        {
            once = false;
            GetWindow<PackageExporterWindow>(false, "Package Exporter", true);
        }

        /*************************************************************************************************
        *** OnGUI
        *************************************************************************************************/
        private void OnGUI()
        {
            if (!once)
            {
                // If the window is compiled while open, ShowWindow() doesn't execute and so the
                // version object doesn't load and all the values are default to empty
                version = new Version();
                once = true;
                readme = LoremIpsum.text;
            }

            EditorGUILayout.BeginVertical(/*GUILayout.Height(500f)*/);
            {
                // Icon and version
                GUILayout.Space(6f);
                EditorGUILayout.BeginHorizontal();
                {
                    // Icon
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    TextureField("Icon", ref version.icon, EditorStyles.boldLabel);
                    EditorGUILayout.EndVertical();

                    // Version
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(92f));
                    {
                        Label("Version", EditorStyles.boldLabel);
                        FieldPlusMinus("Major", 40f, ref version.versionMajor, () => version.versionMajor++, () => version.versionMajor--);
                        FieldPlusMinus("Minor", 40f, ref version.versionMinor, () => version.versionMinor++, () => version.versionMinor--);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

                // Product and company names
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Package", EditorStyles.boldLabel);

                    string packageName = "", packagePath = "";
                    Field("Name", 100f, ref packageName);
                    Field("Path", 100f, ref packagePath);
                }
                EditorGUILayout.EndVertical();

                // Info
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Info", EditorStyles.boldLabel);
                    Label(string.Concat("Version: ", version.version), EditorStyles.helpBox);
                    Label(string.Concat("Product Name: ", version.productName), EditorStyles.helpBox);
                    Label(string.Concat("Company Name: ", version.companyName), EditorStyles.helpBox);
                    Label(string.Concat("Bundle Identifier: ", version.bundleIdentifier), EditorStyles.helpBox);
                }
                EditorGUILayout.EndVertical();

                // Files
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Files", EditorStyles.boldLabel);

                    string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

                    GUILayoutOption[] options = null;

                    if (scenes.Length >= 5)
                        options = new GUILayoutOption[] { GUILayout.Height(94f) };

                    GUILayout.BeginVertical(EditorStyles.helpBox, options);
                    {
                        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                        {
                            for (int i = 0; i < scenes.Length; i++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(scenes[i].Replace("Assets/", ""));

                                GUILayout.FlexibleSpace();
                                GUILayout.Label(i.ToString());
                                GUILayout.EndHorizontal();
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();

                // Readme
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Readme", EditorStyles.boldLabel);

                    scrollPosition2 = EditorGUILayout.BeginScrollView(scrollPosition2);
                    {
                        readme = GUILayout.TextArea(readme/*, GUIStyle.none*/);
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

                // Export button
                Button("Export Package", () => Debug.Log("Export"), height: 30f);

                // Footer button
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(4f);
                    Button("Open Packages Folder", Builder.OpenFolder, 150f);
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(8f);
            }
            EditorGUILayout.EndVertical();

            // Save
            version.Save();
        }

        /*************************************************************************************************
        *** Methods
        *************************************************************************************************/
        private static void TextureField(string name, ref Texture2D texture, GUIStyle labelStyle)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(1f);
                GUILayout.BeginVertical();
                {
                    GUILayout.Space(0f);
                    GUILayout.Label(name, labelStyle);
                    texture = EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(64), GUILayout.Height(64)) as Texture2D;
                    GUILayout.Space(2f);
                }
                GUILayout.EndVertical();
                GUILayout.Space(1f);
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void Box(params string[] value)
        {
            string text = "";

            for (int i = 0; i < value.Length; i++)
                text += value[i];

            GUILayout.Box(text);
        }

        private static void Field(string name, ref string value)
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label(name);
                    value = GUILayout.TextField(value);
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(1f);
            }
            EditorGUILayout.EndVertical();
        }

        private static void Field(string name, float labelWidth, ref string value)
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label(name, GUILayout.Width(labelWidth));
                    value = GUILayout.TextField(value);
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(1f);
            }
            EditorGUILayout.EndVertical();
        }

        private static void Label(string name, GUIStyle style = null)
        {
            GUILayout.Space(0f);
            GUILayout.Label(name, style ?? EditorStyles.label);
        }

        private static void Label(string name, float width, GUIStyle style = null)
        {
            GUILayout.Space(0f);
            GUILayout.Label(name, style ?? EditorStyles.label, GUILayout.Width(width));
        }

        private static void FieldPlusMinus(string name, float? labelWidth, ref int value, Action plusAction, Action minusAction)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (labelWidth != null)
                    GUILayout.Label(name, GUILayout.Width((float)labelWidth));
                else
                    GUILayout.Label(name);

                value = EditorGUILayout.IntField(value);

                if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20f)))
                {
                    GUI.FocusControl(null);
                    plusAction.Invoke();
                }

                if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20f)))
                {
                    GUI.FocusControl(null);

                    if (value > 0)
                        minusAction.Invoke();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void Button(string name, Action action, float? width = null, float? height = null, Color? color = null)
        {
            Color currentColor = GUI.backgroundColor;

            if (color != null)
                GUI.backgroundColor = (Color)color;

            List<GUILayoutOption> options = new List<GUILayoutOption>(2);

            if (width != null) options.Add(GUILayout.Width((float)width));
            if (height != null) options.Add(GUILayout.Height((float)height));

            if (GUILayout.Button(name, options.ToArray()))
                action.Invoke();

            GUI.backgroundColor = currentColor;
        }
    }
}
