/**
 * Created by: Edward Atencio
 * Created on: 18/09/19 (dd/mm/yy)
 */

namespace UnityEditorTools.PackageExporter
{
    using UnityEngine;
    using UnityEditor;
    using System.IO;

    public class PackageExporter
    {
        private const string name = "[Package Exporter] ";

        public static void Export(Package package)
        {
            if (package == null)
            {
                Debug.LogError(string.Concat(name, "Package can't be null"));
                return;
            }

            string readmePath = AssetDatabase.GetAssetPath(package.folder) + "/Readme.txt";

            string readmeContent = string.Concat("# Name = "
                                                 , package.name
                                                 , "\n# Version = "
                                                 , package.version
                                                 , "\n\n"
                                                 , package.readme
                                                 );

            try
            {
                File.WriteAllText(readmePath, readmeContent);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

            AssetDatabase.ImportAsset(readmePath);

            string exportPath = Application.dataPath.Replace("Assets", string.Concat("Builds/Package Exporter/", package.name, ".unitypackage"));
            const ExportPackageOptions exportOptions = ExportPackageOptions.Recurse | ExportPackageOptions.Interactive;

            Directory.CreateDirectory(exportPath.Substring(0, exportPath.LastIndexOf('/')));
            AssetDatabase.ExportPackage(AssetDatabase.GetAssetPath(package.folder), exportPath, exportOptions);
        }

        public static string GetReadme(DefaultAsset folder)
        {
            string readmePath = AssetDatabase.GetAssetPath(folder) + "/Readme.txt";

            if (!File.Exists(readmePath))
                return null;

            try
            {
                return File.ReadAllText(readmePath);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
    }
}
