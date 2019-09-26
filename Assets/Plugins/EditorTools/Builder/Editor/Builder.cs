/**
* Created by: Edward Atencio
* Created on: 17/06/19 (dd/mm/yy)
*/

namespace UnityEditorTools.Builder
{
    using UnityEditor;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

#pragma warning disable CS0618 // Resolution dialog setting is obsolete

    public static class Builder
    {
        /*************************************************************************************************
        *** Variables
        *************************************************************************************************/
        private const string name = "[Builder] ";
        private static readonly string buildsPath = string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 6), "Builds/");

        /*************************************************************************************************
        *** Build
        *************************************************************************************************/
        private static BuildReport Build(string buildName, string buildPath, BuildTarget buildTarget, BuildOptions buildOptions, ResolutionDialogSetting resolutionDialogSetting)
        {
            // Get scenes to build
            string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

            if (scenes.Length < 1)
            {
                Debug.Log(string.Concat(name, "There are no scenes to build. Check your build settings!"));
                return null;
            }

            Debug.Log(string.Concat(name, "Building <", buildName, ">"));

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

            // Setup
            ResolutionDialogSetting currentResolutionDialogSetting = PlayerSettings.displayResolutionDialog;
            PlayerSettings.displayResolutionDialog = resolutionDialogSetting;

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

            PlayerSettings.displayResolutionDialog = currentResolutionDialogSetting;

            return result;
        }

        /*************************************************************************************************
        *** Open folder
        *************************************************************************************************/
        public static void OpenFolder()
        {
            System.IO.Directory.CreateDirectory(buildsPath);
            Application.OpenURL(buildsPath);
        }

        /*************************************************************************************************
        *** Windows
        *************************************************************************************************/
        public static void Production(Version version)
        {
            string buildPath = string.Concat(buildsPath, "Production/");

            const BuildTarget buildTarget = BuildTarget.StandaloneWindows; //x86
            const BuildOptions buildOptions = BuildOptions.ShowBuiltPlayer;
            const ResolutionDialogSetting resolutionDialogSetting = ResolutionDialogSetting.HiddenByDefault;

            BuildReport buildReport = Build("Production (x86)", string.Concat(buildPath, PlayerSettings.productName, ".exe"), buildTarget, buildOptions, resolutionDialogSetting);

            if (buildReport.summary.result == BuildResult.Succeeded)
            {
                string json = JsonUtility.ToJson(version, true);
                System.IO.File.WriteAllText(string.Concat(buildPath, "Version.json"), json);
            }
        }

        public static void Windows()
        {
            string buildPath = string.Concat(buildsPath, "Windows/", PlayerSettings.productName, ".exe");

            const BuildTarget buildTarget = BuildTarget.StandaloneWindows; //x86
            const BuildOptions buildOptions = BuildOptions.ShowBuiltPlayer;
            const ResolutionDialogSetting resolutionDialogSetting = ResolutionDialogSetting.HiddenByDefault;

            Build("Windows (x86)", buildPath, buildTarget, buildOptions, resolutionDialogSetting);
        }

        public static void WindowsDevelopmentBuild()
        {
            string buildPath = string.Concat(buildsPath, "Development Build/", PlayerSettings.productName, ".exe");

            const BuildTarget buildTarget = BuildTarget.StandaloneWindows; //x86
            const BuildOptions buildOptions = BuildOptions.ShowBuiltPlayer
                                              | BuildOptions.AllowDebugging
                                              | BuildOptions.Development;
            const ResolutionDialogSetting resolutionDialogSetting = ResolutionDialogSetting.Enabled;

            Build("Windows (x86) [Development]", buildPath, buildTarget, buildOptions, resolutionDialogSetting);
        }
    }

#pragma warning restore CS0618 // Type or member is obsolete
}

