/**
 * Created by: Edward Atencio
 * Created on: 18/09/19 (dd/mm/yy)
 */

namespace UnityEditorTools.PackageExporter
{
    using UnityEditor;

    public class PackageInfo
    {
        public int versionMajor;
        public int versionMinor;
        public DefaultAsset folder;
        public string name;
        public string author;
        public string readme;

        public string Version { get { return string.Concat(versionMajor, '.', versionMinor); } }

        public void SetVersion(string version)
        {
            if (version.IndexOf('.') < 0)
            {
                versionMajor = int.Parse(version);
                versionMinor = 0;
            }
            else
            {
                string[] vs = version.Split('.');

                versionMajor = int.Parse(vs[0]);
                versionMinor = int.Parse(vs[1]);
            }
        }
    }
}
