using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.Networking;
using System.Diagnostics;

public class MyWindow : EditorWindow
{
    ScrollView itemContainer;
    VisualTreeAsset itemTemplate;
    [MenuItem("Tools/MyWindow")]
    public static void OpenWindow() { GetWindow<MyWindow>(); }

    private void OnEnable()
    {
        var mainTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Resources/TargetScoreScrolView.uxml");
        var ui = mainTemplate.Instantiate();
        rootVisualElement.Add(ui);
        itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Resources/TargetScoreScrollViewItem.uxml");
        itemContainer = ui.Q<ScrollView>("itemContainer");
        var addItemBtn = ui.Q<Button>("addItemBtn");
        addItemBtn.clicked += InitNewTemplate;
    }
    private void InitNewTemplate()
    {
        string _p = EditorUtility.OpenFolderPanel("EnterPath","","");
        UnityEngine.Debug.Log(_p);
        string[] aMaterialFiles = Directory.GetFiles(_p, "*.jpg", SearchOption.AllDirectories);
        for (int i = 0; i < aMaterialFiles.Length; i++)
        {
            string pathToExe = Application.dataPath.Replace(@"/", @"\") + "\\ARFoundationCheck\\";
            string cmd = pathToExe + "arcoreimg.exe eval-img --input_image_path=" + (aMaterialFiles[i]).Replace(@"/", @"\").Replace("JPG", "jpg");
            var itemUi = itemTemplate.Instantiate();
            aMaterialFiles[i] = aMaterialFiles[i].Replace(@"/", @"\");
            #region SetImageTextures
            byte[] fileData;
            fileData = File.ReadAllBytes(aMaterialFiles[i]);
            Texture2D tex = new Texture2D(2, 2);
            var imageTexture = itemUi.Q<Image>("QualityImage");
            tex.LoadImage(fileData);
            #endregion
            var itemLbl = itemUi.Q<Label>("itemQualityLabel");
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "powershell.exe";
                p.StartInfo.Arguments = cmd;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.Start();
                itemLbl.text = "Quality " + p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
            }

            imageTexture.image = tex;
            imageTexture.style.width = 100;
            imageTexture.style.height = 100;
            itemContainer.Add(itemUi);
        }
    }


}
