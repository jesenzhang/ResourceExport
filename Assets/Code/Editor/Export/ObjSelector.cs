using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class ObjSelector
{
    //配置文件
    static string CONFIG_PATH = "Assets/EXPORT_CONFIG.txt";
    static GameBase.ZLText ZLCONFIG = null;

    //筛选出选中目录下的符合条件的资源
    public static UnityEngine.Object[] Select(string categoryKey)
    {
        // InitConfig();
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
        List<string> categories = ZLCONFIG.Read(categoryKey);

        for (int i = 0; i < categories.Count; i++)
        {
            string[] strs = categories[i].Split('=');
            SelectInDirAllDirectories(ref objs, strs[0], strs[1].Split(','));
        }
        return objs.ToArray();
    }

    public static UnityEngine.Object[] SelectTopOnly(string categoryKey)
    {
        // InitConfig();
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
        List<string> categories = ZLCONFIG.Read(categoryKey);

        for (int i = 0; i < categories.Count; i++)
        {
            string[] strs = categories[i].Split('=');
            SelectInDirTopOnly(ref objs, strs[0], strs[1].Split(','));
        }
        return objs.ToArray();
    }

    static void SelectInDir(ref List<UnityEngine.Object> objs, string dir, string[] ptKey, SearchOption opt)
    {
        for (int i = 0; i < ptKey.Length; i++)
        {
            List<string> curPtList = ZLCONFIG.Read(ptKey[i]);
            for (int j = 0; j < curPtList.Count; j++)
            {
                string[] files = Directory.GetFiles(dir, curPtList[j], opt);
                for (int k = 0; k < files.Length; k++)
                {
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(files[k], typeof(UnityEngine.Object));
                    if (obj != null)
                    {
                        objs.Add(obj);
                    }
                }
            }
        }
    }
    //检索所有子目录
    static void SelectInDirAllDirectories(ref List<UnityEngine.Object> objs, string dir, string[] ptKey)
    {
        SelectInDir(ref objs, dir, ptKey, SearchOption.AllDirectories);
    }
    //只检索一级目录
    static void SelectInDirTopOnly(ref List<UnityEngine.Object> objs, string dir, string[] ptKey)
    {
        SelectInDir(ref objs, dir, ptKey, SearchOption.TopDirectoryOnly);
    }

    public static void InitConfig()
    {
        if (!File.Exists(CONFIG_PATH))
        {
            EditorUtility.DisplayDialog("提示", "配置文件不存在", "OK");
        }
        else
        {
            ZLCONFIG = new GameBase.ZLText(File.ReadAllBytes(CONFIG_PATH));
        }
    }
}
