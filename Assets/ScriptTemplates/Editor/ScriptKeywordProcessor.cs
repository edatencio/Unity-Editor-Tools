/**
 * Created by: Edward Atencio
 * Created on: 14/06/19 (dd/mm/yy)
 * Based on: https://gist.github.com/JoaoBorks/af59720a4baba84f080e2df686cacba2
 */

namespace UnityEditorTools
{
    using UnityEngine;
    using UnityEditor;

    internal sealed class ScriptKeywordProcessor : AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            string[,] keywords = new string[,]
            {
                {"#AUTHOR#", "Edward Atencio" },
                {"#CREATION_DATE#", System.DateTime.Now.ToString("dd/MM/yy") },
                {"#COMPANY_NAME#", ParseName(PlayerSettings.companyName) },
                {"#PRODUCT_NAME#", ParseName(PlayerSettings.productName) }
            };

            path = path.Replace(".meta", "");

            int index = path.LastIndexOf('.');

            if (index < 0)
                return;

            if (string.CompareOrdinal(path, index, ".cs", 0, 3) != 0)
                return;

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;

            string fileContent = System.IO.File.ReadAllText(path);

            for (int i = 0; i < keywords.Length / 2; i++)
                fileContent = fileContent.Replace(keywords[i, 0], keywords[i, 1]);

            System.IO.File.WriteAllText(path, fileContent);
            AssetDatabase.Refresh();
        }

        private static string ParseName(string name)
        {
            return System.Text.RegularExpressions.Regex.Replace(name, "[^a-zA-Z]", "");
        }
    }
}
