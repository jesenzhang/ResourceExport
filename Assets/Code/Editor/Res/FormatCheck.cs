using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FormatCheck : Editor
{
    private static string UI_PATH = "Assets/UI/Icon";
    private static string PLAYER_PATH = "Assets/Res/Player";
    private static string NPC_PATH = "Assets/Res/NPC";
    private static string SCENE_PATH = "Assets/Res/SceneAsset";
    private static string EFFECT_PATH = "Assets/Res/Effect";
    private static string OTHER_PATH = "Assets/Res/Textures";
    private static string ALL_PATH = "Assets/Res";

    [MenuItem("CheckFormat/Check All Asset's Format &A")]
    private static void CheckAll()
    {
        EditorAppUtil.ClearConsoleLog();
        bool checkFlag = false;
        if (Selection.objects != null && Selection.objects.Length != 0)
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                Object o = Selection.objects[i];
                string path = AssetDatabase.GetAssetPath(o.GetInstanceID()).Replace("\\", "/").Replace("//", "/");
                if (AssetDatabase.IsValidFolder(path))
                {
                    if(string.Equals(path,ALL_PATH))
                    {
                        if(Directory.Exists(UI_PATH)) CheckUI(UI_PATH);
                        if (Directory.Exists(NPC_PATH)) CheckNPC(NPC_PATH);
                        if (Directory.Exists(PLAYER_PATH)) CheckPlayer(PLAYER_PATH);
                        if (Directory.Exists(SCENE_PATH)) CheckScene(SCENE_PATH);
                        if (Directory.Exists(EFFECT_PATH)) CheckEffect(EFFECT_PATH);
                        if (Directory.Exists(OTHER_PATH)) CheckOther(OTHER_PATH);
                        checkFlag = true;
                        break;
                    }
                    else
                    {
                        CheckUI(path);
                        CheckNPC(path);
                        CheckPlayer(path);
                        CheckScene(path);
                        CheckEffect(path);
                        CheckOther(path);
                    }
                    checkFlag = true;
                }
            }
        }
        if (!checkFlag)
            EditorUtility.DisplayDialog("提示", "请先选中要进行检查的资源目录", "OK");
    }

    private static void CheckUI(string path)
    {
        if (!path.StartsWith(UI_PATH))
            return;
        List<string> files = GetFiles(path);
        for(int i = 0;i < files.Count;i++)
        {
            System.Type t = AssetDatabase.GetMainAssetTypeAtPath(files[i]);
            if(t.FullName == "UnityEngine.Texture2D")
            {
                Texture2D t2d = AssetDatabase.LoadAssetAtPath<Texture2D>(files[i]);
                CheckTextureCommonFormat(files[i],t2d);
            }
        }
    }

    private static void CheckNPC(string path)
    {
        if (!path.StartsWith(NPC_PATH))
            return;
        List<string> files = GetFiles(path);
        for (int i = 0; i < files.Count; i++)
        {
            System.Type t = AssetDatabase.GetMainAssetTypeAtPath(files[i]);
            if (t.FullName == "UnityEngine.Texture2D")
            {
                Texture2D t2d = AssetDatabase.LoadAssetAtPath<Texture2D>(files[i]);
                CheckTextureCommonFormat(files[i], t2d);
            }
        }
    }

    private static void CheckPlayer(string path)
    {
        if (!path.StartsWith(PLAYER_PATH))
            return;
        List<string> files = GetFiles(path);
        for (int i = 0; i < files.Count; i++)
        {
            System.Type t = AssetDatabase.GetMainAssetTypeAtPath(files[i]);
            if (t.FullName == "UnityEngine.Texture2D")
            {
                Texture2D t2d = AssetDatabase.LoadAssetAtPath<Texture2D>(files[i]);
                CheckTextureCommonFormat(files[i], t2d);
            }
        }
    }

    private static void CheckScene(string path)
    {
        if (!path.StartsWith(SCENE_PATH))
            return;
        List<string> files = GetFiles(path);
        for (int i = 0; i < files.Count; i++)
        {
            System.Type t = AssetDatabase.GetMainAssetTypeAtPath(files[i]);
            if (t.FullName == "UnityEngine.Texture2D")
            {
                Texture2D t2d = AssetDatabase.LoadAssetAtPath<Texture2D>(files[i]);
                CheckTextureCommonFormat(files[i], t2d);
            }
        }
    }

    private static void CheckEffect(string path)
    {
        if (!path.StartsWith(EFFECT_PATH))
            return;
        List<string> files = GetFiles(path);
        for (int i = 0; i < files.Count; i++)
        {
            System.Type t = AssetDatabase.GetMainAssetTypeAtPath(files[i]);
            if (t.FullName == "UnityEngine.Texture2D")
            {
                Texture2D t2d = AssetDatabase.LoadAssetAtPath<Texture2D>(files[i]);
                CheckTextureCommonFormat(files[i], t2d);
            }
        }
    }

    private static void CheckOther(string path)
    {
        if (!path.StartsWith(OTHER_PATH))
            return;
        List<string> files = GetFiles(path);
        for (int i = 0; i < files.Count; i++)
        {
            System.Type t = AssetDatabase.GetMainAssetTypeAtPath(files[i]);
            if (t.FullName == "UnityEngine.Texture2D")
            {
                Texture2D t2d = AssetDatabase.LoadAssetAtPath<Texture2D>(files[i]);
                CheckTextureCommonFormat(files[i], t2d);
            }
        }
    }

    private static void CheckTextureCommonFormat(string path,Texture2D t2d)
    {
        path = path.ToLower();
        if (t2d != null)
        {
            bool hasError = false;
            string error = string.Empty;
            if(t2d.width != t2d.height)
            {
                error = "非正方形 ";
                hasError = true;
            }
            if(!path.EndsWith(".tga") && !path.EndsWith(".png"))
            {
                error += "后缀不是tga或png ";
                hasError = true;
            }
            if(hasError)
                Debug.LogError("ERROR FORMAT: " + error + path);
        }
    }

    private static List<string> GetFiles(string root)
    {
        List<string> ret = new List<string>();
        string[] files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
                continue;
            ret.Add(files[i]);
        }
        return ret;
    }
}
