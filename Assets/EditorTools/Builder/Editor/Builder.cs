/**
* Created by: Edward Atencio
* Created on: 17/06/19 (dd/mm/yy)
*/

namespace UnityEditorTools
{
    using UnityEditor;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

    public static class Builder
    {
        /*************************************************************************************************
        *** Variables
        *************************************************************************************************/
        private const string name = "[Editor-Tools] Builder: ";
        private static readonly string projectPath = Application.dataPath.Replace("Assets", "");

        /*************************************************************************************************
        *** Build
        *************************************************************************************************/
        private static BuildReport Build(string buildName, string buildPath, BuildTarget buildTarget, BuildOptions buildOptions)
        {
            // Get scenes to build
            string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

            if (scenes.Length < 1)
            {
                Debug.Log(string.Concat(name, "There are no scenes to build. Check your build settings!"));
                return null;
            }

            // Kill game task
            new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "CMD.exe",
                    Arguments = string.Concat("/C taskkill /t /f /im ", PlayerSettings.productName, ".exe")
                }
            }.Start();

            // Delete previous build
            FileUtil.DeleteFileOrDirectory(buildPath.Substring(0, buildPath.LastIndexOf('/')));

            Debug.Log(string.Concat(name, "Building (", buildName, ")..."));

            // Build
            BuildReport result = BuildPipeline.BuildPlayer(scenes, buildPath, buildTarget, buildOptions);

            Debug.Log(string.Concat(name
                                    , "Build completed with a result of '", result.summary.result, "'"
                                    , "\nTotal size = ", (result.summary.totalSize * 0.000001).ToString("0.00"), " MB"
                                    , "\nPath = ", result.summary.outputPath
                                    , "\nTotal time = ", result.summary.totalTime
                                    , "\nStarted at = ", result.summary.buildStartedAt
                                    , "\nEnded at = ", result.summary.buildEndedAt
                                    ));

            return result;
        }

        /*************************************************************************************************
        *** Windows
        *************************************************************************************************/
        [MenuItem("Build/Windows - Development Build _F5")]
        private static void WindowsDevelopmentBuild()
        {
            string buildPath = string.Concat(projectPath, "Builds/Development Build/", PlayerSettings.productName, ".exe");

            const BuildOptions buildOptions = BuildOptions.ShowBuiltPlayer
                                              | BuildOptions.AllowDebugging
                                              | BuildOptions.Development;

            Build("Windows - Development Build", buildPath, BuildTarget.StandaloneWindows, buildOptions);
        }
    }
}

