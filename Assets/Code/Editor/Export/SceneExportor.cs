﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public static partial class SceneExportor
{
    [MenuItem("Export/Export Scene")]
    static void _ExportScene()
    {
        string scenePath = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
        Debug.Log("export cur Scene->" + scenePath);

        if(ObjVersionChecker.Filter(AssetDatabase.GetDependencies(scenePath)).Length > 0)
        {
            _ExportScene(EditorApplication.currentScene, EditorUserBuildSettings.activeBuildTarget);
        }
    }
    
    public static void ExportScene()
    {
        string[] arr = System.Environment.GetCommandLineArgs();
        if (arr == null || arr.Length == 0 || arr.Length < 2)
            return;
        string scenePath = arr[7];string platform = arr[8];
        if (scenePath == null || scenePath == "")
            return;

        BuildTarget buildTarget = BuildTarget.StandaloneWindows;
        switch (platform)
        {
            case "ios":
                {
                    buildTarget = BuildTarget.iOS;
                }
                break;
            case "android":
                {
                    buildTarget = BuildTarget.Android;
                }
                break;
        }

        if (ObjVersionChecker.Filter(AssetDatabase.GetDependencies(scenePath)).Length > 0)
        {
            _ExportScene(scenePath, buildTarget);
        }      
    }

    private static void _ExportScene(string scenePath, BuildTarget buildTarget)
    {
        string rootPath = EditorUtils.PlatformPath(buildTarget);
        if (rootPath == null)
            return;
        string fileName = Path.GetFileNameWithoutExtension(scenePath);
        string outpath = Application.dataPath + "/../" + rootPath + "Scene/";
        if (!Directory.Exists(outpath)) Directory.CreateDirectory(outpath);
        
        string res = BuildPipeline.BuildStreamedSceneAssetBundle(new string[] { scenePath }, outpath + fileName, buildTarget);
        if (res != string.Empty)
        {
            Debug.LogError(res);
            return;
        }

        Debug.Log("Export scene over");
    }
}
