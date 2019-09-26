/**
 * Created by: Edward Atencio
 * Created on: 16/09/19 (dd/mm/yy)
 */

namespace UnityEditorTools.Builder
{
    using UnityEngine;
    using UnityEditor;
    using System;

    [Serializable]
    public class Version
    {
        public string version = "";
        [NonSerialized] public int versionMajor = 0;
        [NonSerialized] public int versionMinor = 0;
        public string productName = "";
        public string companyName = "";
        public string bundleIdentifier = "";
        [NonSerialized] public Texture2D icon = null;

        public Version()
        {
            // Get default values
            string[] projectPath = Application.dataPath.Split('/');
            productName = projectPath[projectPath.Length - 2].Replace("-", " ");
            companyName = "DefaultCompany";
            productName = PlayerSettings.productName;

            // then load
            Load();
        }

        public void SetVersion(int major, int minor)
        {
            version = string.Concat(major, '.', minor);
            versionMajor = major;
            versionMinor = minor;
        }

        public void SetBundleIdentifier()
        {
            string companyName = this.companyName.Replace(" ", "").Replace("-", "");
            string productName = this.productName.Replace(" ", "").Replace("-", "");
            bundleIdentifier = string.Concat("com.", companyName, '.', productName);
        }

        public void Save()
        {
            SetVersion(versionMajor, versionMinor);
            if (PlayerSettings.bundleVersion != version)
                PlayerSettings.bundleVersion = version;

            if (PlayerSettings.productName != productName)
                PlayerSettings.productName = productName;

            if (PlayerSettings.companyName != companyName)
                PlayerSettings.companyName = companyName;

            SetBundleIdentifier();
            if (PlayerSettings.applicationIdentifier != bundleIdentifier)
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, bundleIdentifier);

            Texture2D[] textures = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown, IconKind.Any);
            if (textures.Length == 0 || (textures.Length > 0 && textures[0] != icon))
                PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { icon }, IconKind.Any);
        }

        private void Load()
        {
            string version = PlayerSettings.bundleVersion;
            int index = version.IndexOf('.');

            int versionMajor;
            int versionMinor;

            try
            {
                if (index > 0)
                {
                    versionMajor = int.Parse(version.Substring(0, index));
                    versionMinor = int.Parse(version.Substring(index + 1));
                }
                else
                {
                    versionMajor = int.Parse(version);
                    versionMinor = 0;
                }
            }
            catch
            {
                Debug.LogError(string.Concat("[Builder]: Error getting current version, default to 0."));
                versionMajor = 0;
                versionMinor = 0;
            }

            SetVersion(versionMajor, versionMinor);
            productName = PlayerSettings.productName;
            companyName = PlayerSettings.companyName;
            bundleIdentifier = PlayerSettings.applicationIdentifier;

            Texture2D[] textures = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown, IconKind.Any);
            if (textures.Length > 0)
                icon = textures[0];
        }
    }
}
