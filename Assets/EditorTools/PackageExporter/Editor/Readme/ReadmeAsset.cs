/**
 * Created by: Edward Atencio
 * Created on: 18/09/19 (dd/mm/yy)
 */

namespace UnityEditorTools.PackageExporter
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewReadmeAsset")]
    public class ReadmeAsset : ScriptableObject
    {
        public string title;
        [TextArea] public string content;
        public Texture2D icon;

        public string[] GetContentLines()
        {
            return content.Split('\n');
        }
    }
}
