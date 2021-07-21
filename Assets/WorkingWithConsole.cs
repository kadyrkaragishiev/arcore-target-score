#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
[CustomEditor(typeof(ImageRef))]
public class WorkingWithConsole : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Check"))
        {
            ARcoreChecking();
        }
    }

    public void ARcoreChecking()
    {
        ImageRef im = (ImageRef)target;
        string pathToExe = Application.dataPath.Replace(@"/", @"\") + "\\ARCoreCheck\\";
        string assetBasePath = AssetDatabase.GetAssetPath(im.img).Remove(0,6);
        //UnityEngine.Debug.Log(assetBasePath);
        //UnityEngine.Debug.Log(AssetDatabase.GetAssetPath(im.img));
        //UnityEngine.Debug.Log(Application.dataPath);
        //string command = pathToExe + "arcoreimg.exe eval-img --input_image_path=C:\\UsingFiles\\UnityProjects\\ARTestProject\\Assets\\ARCoreCheck\\IMG_7432.png";
        string command = pathToExe + "arcoreimg.exe eval-img --input_image_path="+(Application.dataPath+assetBasePath).Replace(@"/",@"\").Replace("JPG","jpg");
        //UnityEngine.Debug.Log(command);
        using (Process p = new Process())
        {
            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = command;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            UnityEngine.Debug.Log(p.StandardOutput.ReadToEnd());
            p.WaitForExit();
            p.Close();
        }
    }

}

#endif
