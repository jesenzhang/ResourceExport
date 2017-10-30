using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class OBJExportor
{
    [MenuItem("Export/Export OBJ")]
    public static void ExportOBJ()
    {
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //objs = ObjVersionChecker.FilterWithDependencies(objs);
        if (objs == null || objs.Length == 0)
        {
            Debug.LogError("there is no object selected!");
            return;
        }

        ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs);
    }

    public static void ExportOBJ(BuildTarget buildTarget, Object[] objs)
    {
        string rootPath = EditorUtils.PlatformPath(buildTarget);
        if (rootPath == null)
            return;

        foreach (Object obj in objs)
        {
            string assetpath = AssetDatabase.GetAssetPath(obj).Replace("//","/").Replace("\\","/").Replace("Assets/","");
            string assetname = Path.GetFileNameWithoutExtension(assetpath);
            string outpath = Application.dataPath + "/../" +  rootPath + Path.GetDirectoryName(assetpath) + "/";
            if (!Directory.Exists(outpath)) Directory.CreateDirectory(outpath);

            BuildPipeline.BuildAssetBundle(obj, null, outpath + assetname, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, buildTarget);
        }
        Debug.Log("Export over");
    }
}
