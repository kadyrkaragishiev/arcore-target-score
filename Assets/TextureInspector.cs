using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public static class UnityUtil
{
#if UNITY_EDITOR
    [MenuItem("Tools/ARFoundationQualityCheck")]
	public static void ParseCurrentElement()
    {
		string getSelectedObj = GetSelectedPathOrFallback();
		if(getSelectedObj != "Assets")
        {
            CopyMaxSize(getSelectedObj);
        }
    }
    private static void CopyMaxSize(string commandPath)
    {
        var texturePath = commandPath;
        string pathToExe = Application.dataPath.Replace(@"/", @"\") + "\\ARFoundationCheck\\";
        string assetBasePath = texturePath.Remove(0, 6);
        string cmd = pathToExe + "arcoreimg.exe eval-img --input_image_path=" + (texturePath).Replace(@"/", @"\").Replace("JPG", "jpg");
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
    public static string GetSelectedPathOrFallback()
	{
		string path = "Assets";

		foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Texture2D), SelectionMode.Assets))
		{
			path = AssetDatabase.GetAssetPath(obj);
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				path = Path.GetFullPath(path);
				break;
			}
		}
		return path;
	}
#endif
}