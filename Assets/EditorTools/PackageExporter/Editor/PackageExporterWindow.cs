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

    public class Package
    {
        public string version;
        public int versionMajor;
        public int versionMinor;
        public DefaultAsset folder;
        public string name;
        public string readme = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus eu scelerisque nisl. Curabitur cursus sit amet lorem vitae imperdiet.Aliquam fringilla commodo magna, id ornare lacus rutrum sed. Aliquam erat volutpat.Donec at quam justo. Nam consequat tellus nisi, eget pretium orci hendrerit et. Phasellus dignissim neque vel magna rutrum vehicula.In ac dolor ipsum. Aliquam leo justo, interdum et elit sed, iaculis scelerisque libero. Sed quis molestie lacus, ut eleifend felis. Ut commodo suscipit odio, vel condimentum ante. Ut egestas vel nisl in venenatis.Nunc rhoncus libero nec mauris pellentesque interdum.Lorem ipsum dolor sit amet, consectetur adipiscing elit.Quisque id mattis velit.

Proin tempus bibendum dolor, eu consectetur nisi egestas nec. Ut enim lectus, pulvinar auctor faucibus sed, fringilla eu nisi. Morbi commodo nibh augue, ac lobortis est auctor a. Mauris viverra luctus diam at vestibulum. Ut non gravida ex. Morbi imperdiet hendrerit efficitur. In posuere augue vitae bibendum luctus. Proin sollicitudin ullamcorper posuere. Sed suscipit suscipit consectetur.

Aliquam ornare porttitor urna in laoreet.Morbi ipsum nunc, viverra non sollicitudin vel, porttitor et nibh. In ornare diam ipsum, sed maximus neque dictum ac. Nullam ultricies ipsum a varius molestie. Nullam nec sodales nisi. Vestibulum placerat ligula non tellus dignissim sollicitudin.Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed enim dui, consequat rhoncus tincidunt at, ullamcorper ut enim.

Mauris dapibus non felis nec euismod. Fusce finibus nunc magna. Praesent scelerisque auctor sollicitudin. Maecenas lacinia, elit sit amet sollicitudin vulputate, tellus arcu facilisis metus, et varius est quam gravida turpis. In ultricies massa id turpis sodales porta finibus et nisi. Integer vitae finibus eros. Nullam ac ullamcorper ipsum, eget accumsan nunc. Integer sagittis erat sit amet luctus luctus.Quisque porttitor varius luctus. Pellentesque vehicula lacus sit amet condimentum lacinia.Praesent libero massa, feugiat quis orci eget, scelerisque scelerisque magna. Nunc diam metus, dignissim sed semper sit amet, fringilla euismod urna.Nam scelerisque elit in urna pharetra maximus.

Nam fermentum, tortor eu pharetra condimentum, purus diam faucibus nibh, vitae euismod justo purus sit amet erat. Praesent eu est ornare, vehicula lorem at, tincidunt odio.Etiam efficitur imperdiet pretium. Mauris at convallis quam. Pellentesque nibh mi, euismod sit amet est non, faucibus ullamcorper velit.Sed semper efficitur tellus non fringilla. Morbi eu mauris pretium, volutpat enim eget, tempus nulla.Suspendisse mollis, neque quis auctor congue, eros urna porttitor ante, vitae fringilla lorem metus eu diam.Fusce placerat dapibus mi sit amet eleifend.Duis non purus porttitor, eleifend ante ut, tempor neque.Sed a arcu non ligula consequat tempor.Suspendisse malesuada leo sit amet placerat vestibulum.Ut ex velit, viverra ut libero quis, commodo interdum diam. Quisque gravida metus eu molestie tincidunt. Nam quis ornare ex, ut volutpat quam. In malesuada sodales massa, eu vestibulum diam placerat non.";

        public Package()
        {
            SetVersion(versionMajor, versionMinor);
        }

        public void SetVersion(int major, int minor)
        {
            version = string.Concat(major, '.', minor);
        }
    }

    public class PackageExporterWindow : EditorWindow
    {
        /*************************************************************************************************
        *** Variables
        *************************************************************************************************/
        private static Vector2 scrollPosition;
        private static Vector2 scrollPosition2;
        private static Action openBuildSettigns = () => EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        private static bool once = false;

        public static Package package;

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
                package = new Package();
                once = true;
            }

            EditorGUILayout.BeginVertical(/*GUILayout.Height(500f)*/);
            {
                // Version
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(61f));
                {
                    Label("Version", EditorStyles.boldLabel);
                    FieldPlusMinus("Major", 40f, ref package.versionMajor, () => package.versionMajor++, () => package.versionMajor--);
                    FieldPlusMinus("Minor", 40f, ref package.versionMinor, () => package.versionMinor++, () => package.versionMinor--);
                }
                EditorGUILayout.EndVertical();

                // Package
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Package", EditorStyles.boldLabel);

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("Folder", GUILayout.Width(40f));

                            package.folder = EditorGUILayout.ObjectField("", package.folder, typeof(DefaultAsset), false) as DefaultAsset;

                            if (package.folder != null)
                            {
                                if (!AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(package.folder)))
                                {
                                    package.folder = null;
                                    Debug.LogError(string.Concat(name, "Not a valid folder!"));
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(package.name))
                                        package.name = package.folder.name;
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(1f);
                    }
                    EditorGUILayout.EndVertical();

                    Field("Name", 40f, ref package.name);
                }
                EditorGUILayout.EndVertical();

                // Summary
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Summary", EditorStyles.boldLabel);
                    Label(string.Concat("Version: ", package.version), EditorStyles.helpBox);
                    Label(string.Concat("Package Name: ", package.name), EditorStyles.helpBox);
                }
                EditorGUILayout.EndVertical();

                // Readme
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Readme", EditorStyles.boldLabel);

                    scrollPosition2 = EditorGUILayout.BeginScrollView(scrollPosition2);
                    {
                        package.readme = GUILayout.TextArea(package.readme);
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

                // Export button
                Button("Export Package", () => PackageExporter.Export(package), height: 30f);

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
            //package.Save();
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
