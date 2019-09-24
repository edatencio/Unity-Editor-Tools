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

        private const string openToken = "###-";
        private const string closeToken = "###~";
        private const string authorKey = "# Author";
        private const string nameKey = "# Name";
        private const string versionKey = "# Version";

        public static void Export(PackageInfo package)
        {
            if (package == null)
            {
                Debug.LogError(string.Concat(name, "Package can't be null"));
                return;
            }

            string readmePath = AssetDatabase.GetAssetPath(package.folder) + "/Readme.txt";

            string readmeContent = string.Concat(openToken
                                                 , "\n", authorKey, " = ", package.author
                                                 , "\n", nameKey, " = ", package.name
                                                 , "\n", versionKey, " = ", package.Version
                                                 , "\n", closeToken, "\n\n"
                                                 , package.readme);

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

        public static PackageInfo GetPackageInfo(DefaultAsset folder)
        {
            string readmePath = AssetDatabase.GetAssetPath(folder) + "/Readme.txt";
            PackageInfo package = new PackageInfo() { folder = folder };

            if (!File.Exists(readmePath))
                return package;

            bool headerFinished = false;
            try
            {
                foreach (string line in File.ReadAllLines(readmePath))
                {
                    if (!headerFinished)
                    {
                        if (string.CompareOrdinal(line, 0, authorKey, 0, authorKey.Length) == 0)
                            package.author = line.Substring(authorKey.Length + 3);

                        if (string.CompareOrdinal(line, 0, nameKey, 0, nameKey.Length) == 0)
                            package.name = line.Substring(nameKey.Length + 3);

                        if (string.CompareOrdinal(line, 0, versionKey, 0, versionKey.Length) == 0)
                            package.SetVersion(line.Substring(versionKey.Length + 3));

                        if (string.CompareOrdinal(line, 0, closeToken, 0, closeToken.Length) == 0)
                            headerFinished = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(line))
                            package.readme += line;

                        if (string.IsNullOrEmpty(line) && !string.IsNullOrEmpty(package.readme))
                            package.readme += "\n\n";
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

            return package;
        }
    }
}
