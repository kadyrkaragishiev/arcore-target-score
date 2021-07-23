using UnityEngine;
using UnityEditor;
using System.Diagnostics;
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public static class ContextMenuOpenListener
{
#if UNITY_EDITOR
    static ContextMenuOpenListener()
    {
        EditorApplication.contextualPropertyMenu += OnContextMenuOpening;
    }

    private static void OnContextMenuOpening(GenericMenu menu, SerializedProperty property)
    {
        UnityEngine.Debug.Log("ContextMenu opening for property " + property.propertyPath);
    }
}
internal static class TextureImporterContextMenuExtensions
{
    [MenuItem("CONTEXT/TextureImporter/Check/Texture Quality")]
    private static void CopyMaxSize(MenuCommand command)
    {
        var textureImporter = (TextureImporter)command.context;
        var texturePath = textureImporter.assetPath;
        string pathToExe = Application.dataPath.Replace(@"/", @"\") + "\\ARFoundationCheck\\";
        string assetBasePath = texturePath.Remove(0, 6);
        string cmd = pathToExe + "arcoreimg.exe eval-img --input_image_path=" + (Application.dataPath + assetBasePath).Replace(@"/", @"\").Replace("JPG", "jpg");
        UnityEngine.Debug.Log(cmd);
        using (Process p = new Process())
        {
            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = cmd;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            EditorUtility.DisplayDialog("Quality:", p.StandardOutput.ReadToEnd(), "OK");
            p.WaitForExit();
            p.Close();
        }
    }
#endif

}

