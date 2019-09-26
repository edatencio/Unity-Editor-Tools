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

    public class PackageExporterWindow : EditorWindow
    {
        /*************************************************************************************************
        *** Variables
        *************************************************************************************************/
        private static Vector2 scrollPosition;
        private static bool once = false;
        public static PackageInfo packageInfo;
        public static DefaultAsset folder;
        public static DefaultAsset currentFolder;

        /*************************************************************************************************
        *** MenuItem
        *************************************************************************************************/
        [MenuItem("Tools/Package Exporter _F4")]
        public static void ShowWindow()
        {
            once = false;
            folder = null;
            currentFolder = null;
            GetWindow<PackageExporterWindow>(false, "Package Exporter", true);
        }

        /*************************************************************************************************
        *** OnGUI
        *************************************************************************************************/
        private void OnGUI()
        {
            if (!once)
            {
                packageInfo = new PackageInfo();
                once = true;
            }

            EditorGUILayout.BeginVertical();
            {
                // Package
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Package", EditorStyles.boldLabel);

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("Folder", GUILayout.Width(40f));

                            folder = EditorGUILayout.ObjectField("", folder, typeof(DefaultAsset), false) as DefaultAsset;

                            if (folder != null)
                            {
                                if (!AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(folder)))
                                {
                                    folder = null;
                                    Debug.LogError(string.Concat(name, "Not a valid folder!"));
                                }
                                else
                                {
                                    if (currentFolder != folder)
                                    {
                                        currentFolder = folder;
                                        packageInfo = PackageExporter.GetPackageInfo(folder);
                                    }

                                    if (string.IsNullOrEmpty(packageInfo.name))
                                        packageInfo.name = packageInfo.folder.name;
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(1f);
                    }
                    EditorGUILayout.EndVertical();

                    Field("Name", 40f, ref packageInfo.name);
                    Field("Author", 40f, ref packageInfo.author);
                }
                EditorGUILayout.EndVertical();

                // Version
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(61f));
                {
                    Label("Version", EditorStyles.boldLabel);
                    FieldPlusMinus("Major", 40f, ref packageInfo.versionMajor, () => packageInfo.versionMajor++, () => packageInfo.versionMajor--);
                    FieldPlusMinus("Minor", 40f, ref packageInfo.versionMinor, () => packageInfo.versionMinor++, () => packageInfo.versionMinor--);
                }
                EditorGUILayout.EndVertical();

                // Summary
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Summary", EditorStyles.boldLabel);
                    Label(string.Concat("Version: ", packageInfo.Version), EditorStyles.helpBox);
                    Label(string.Concat("Package Name: ", packageInfo.name), EditorStyles.helpBox);
                    Label(string.Concat("Author: ", packageInfo.author), EditorStyles.helpBox);
                }
                EditorGUILayout.EndVertical();

                // Readme
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    Label("Readme", EditorStyles.boldLabel);

                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                    {
                        packageInfo.readme = EditorGUILayout.TextArea(packageInfo.readme, new GUIStyle(EditorStyles.textField) { wordWrap = true });
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

                // Export button
                Button("Export Package", () => PackageExporter.Export(packageInfo), height: 30f);

                // Footer button
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(4f);
                    Button("Open Packages Folder", PackageExporter.OpenFolder, 150f);
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(8f);
            }
            EditorGUILayout.EndVertical();
        }

        /*************************************************************************************************
        *** Methods
        *************************************************************************************************/
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
