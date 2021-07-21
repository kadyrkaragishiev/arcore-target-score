using UnityEngine;
using UnityEditor;
using System.Diagnostics;

[InitializeOnLoad]
public static class ContextMenuOpenListener
{
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
    private static object copied;

    [MenuItem("CONTEXT/TextureImporter/Check/Texture Quality")]
    private static void CopyMaxSize(MenuCommand command)
    {
        var textureImporter = (TextureImporter)command.context;
        copied = textureImporter.textureType;
        var texturePath = textureImporter.assetPath;
        string pathToExe = Application.dataPath.Replace(@"/", @"\") + "\\ARCoreCheck\\";
        string assetBasePath = texturePath.Remove(0, 6);
        string cmd = pathToExe + "arcoreimg.exe eval-img --input_image_path=" + (Application.dataPath + assetBasePath).Replace(@"/", @"\").Replace("JPG", "jpg");
        using (Process p = new Process())
        {
            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = cmd;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            UnityEngine.Debug.Log(p.StandardOutput.ReadToEnd());
            p.WaitForExit();
            p.Close();
        }
        UnityEngine.Debug.Log("Copied TextureImporterType: " + texturePath);
    }

    [MenuItem("CONTEXT/TextureImporter/Paste/Texture Type")]
    private static void PasteTextureType(MenuCommand command)
    {
        var textureImporter = (TextureImporter)command.context;
        textureImporter.textureType = (TextureImporterType)copied;

        UnityEngine.Debug.Log("Pasted TextureImporterType: " + copied);
    }
}

