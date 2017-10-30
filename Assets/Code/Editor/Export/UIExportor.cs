using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using GameBase;

public class UIExportor
{
    [MenuItem("Export/Export UI Widget")]
    static void ExportWidget()
    {
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        objs = ObjVersionChecker.Filter(objs);
        if (objs == null || objs.Length == 0)
        {
            Debug.LogError("no ui selected");
            return;
        }

        //string exportPath = "Export/UI/";
        string exportPath = EditorUtils.PlatformPath(EditorUserBuildSettings.activeBuildTarget);
        if (exportPath == null)
            return;

        exportPath += "UI/Widgets/";

        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);

        foreach (Object obj in objs)
        {
            ProcessUIWidget(obj, exportPath, EditorUserBuildSettings.activeBuildTarget);
        }
    }

    [MenuItem("Export/Export UI")]
    static void ExportUI()
    {
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        objs = ObjVersionChecker.Filter(objs);
        if (objs == null || objs.Length == 0)
        {
            Debug.LogError("no ui selected");
            return;
        }

        ExportUI(EditorUserBuildSettings.activeBuildTarget, objs);
    }

    [MenuItem("Export/Export ALL With Config")]
    public static void AutoExportAll()
    {
        //初始化筛选器配置文件
        ObjSelector.InitConfig();

        var args = System.Environment.GetCommandLineArgs();

        BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;

        Object[] uiobjs = ObjVersionChecker.Filter(ObjSelector.SelectTopOnly("UI"));
        ExportUI(buildTarget, uiobjs);

        Object[] widobjs = ObjVersionChecker.Filter(ObjSelector.Select("WIDGETS"));
        ExportWidget(buildTarget, widobjs);

        Object[] objss = ObjVersionChecker.FilterWithDependencies(ObjSelector.Select("OBJ"));
        OBJExportor.ExportOBJ(buildTarget, objss);

        Debug.Log("Export All Done");
    }

    static void ExportWidget(BuildTarget buildTarget, params Object[] objs)
    {
        string exportPath = EditorUtils.PlatformPath(EditorUserBuildSettings.activeBuildTarget);
        if (exportPath == null)
            return;

        exportPath += "UI/Widgets/";

        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);

        foreach (Object obj in objs)
        {
            ProcessUIWidget(obj, exportPath, buildTarget);
        }
    }

    static void ExportUI(BuildTarget buildTarget, params Object[] objs)
    {
        //string exportPath = "Export/UI/";
        string exportPath = EditorUtils.PlatformPath(buildTarget);
        if (exportPath == null)
            return;

        exportPath += "UI/";

        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);

        string path = Application.dataPath;
        path += "/UI/Data/";
        path += EditorUtils.UserName() + "/";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        ZLText zlt = null;
        using (FileStream fs = new FileStream(path + "UIAtlasData.zl", FileMode.OpenOrCreate))
        {
            if (fs.Length > 0)
            {
                byte[] temp = new byte[fs.Length];
                fs.Read(temp, 0, temp.Length);
                zlt = new ZLText(temp);
            }
        }

        Dictionary<string, List<string>> dic = null;
        if (zlt != null)
            dic = zlt.ReadAll();
        else
            dic = new Dictionary<string, List<string>>();

        List<string> atlasNames = null;
        if (dic.ContainsKey("AtlasNames"))
            atlasNames = dic["AtlasNames"];
        else
        {
            atlasNames = new List<string>();
            dic.Add("AtlasNames", atlasNames);
        }

        /*
           List<string> textList = null;
           using(FileStream fs = new FileStream(path + "UITextData.txt", FileMode.OpenOrCreate))
           {
           if(fs.Length > 0)
           {
           byte[] temp = new byte[fs.Length];
           fs.Read(temp, 0, temp.Length);
           }
           }
           */

        string textPath = path + "UITextData.txt";
        if (!File.Exists(textPath))
            File.Create(textPath);
        //string[] textArr = File.ReadAllLines(textPath);
        string[] textArr = null;
        try
        {
            File.ReadAllLines(textPath);
        }
        catch
        {
        }
        List<string> textList = new List<string>();
        if (textArr != null)
            Debug.Log("text arr->" + textArr.Length);
        if (textArr != null && textArr.Length > 0)
            textList.AddRange(textArr);

        if (textList.Count == 0)
            textList.Add(" ");

        foreach (Object obj in objs)
        {
            ProcessUI(obj, atlasNames, dic, textList, exportPath, buildTarget);
        }

        zlt = new ZLText();
        byte[] atlasData = zlt.Write(dic);
        if (atlasData != null)
        {
            using (FileStream fs = new FileStream(path + "UIAtlasData.zl", FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
                fs.Write(atlasData, 0, atlasData.Length);
            }
        }
        else
            Debug.LogError("write atlas data failed");

        File.WriteAllLines(textPath, textList.ToArray());
    }

    private static void ProcessUI(Object obj, List<string> atlasNames, Dictionary<string, List<string>> dic, List<string> textList, string exportPath, BuildTarget buildTarget)
    {
        GameObject go = (GameObject)Object.Instantiate(obj);

        /*
		   UIPanel panel = go.GetComponent<UIPanel>();
		   if(panel == null)
		   {
		   Debug.LogError("target select has not UIPanel");
		   return;
		   }
		   */

        string name = go.name;
        int index = name.IndexOf("(");
        if (index >= 0)
            name = name.Substring(0, index);

        UIData uiData = go.AddComponent<UIData>();

        UISprite[] sprites = go.GetComponentsInChildren<UISprite>(true);
        UILabel[] labels = go.GetComponentsInChildren<UILabel>(true);
        UIEvent[] events = go.GetComponentsInChildren<UIEvent>(true);
        List<string> list = new List<string>();
        if (sprites != null)
        {
            foreach (UISprite sprite in sprites)
            {
                if (sprite.atlas != null)
                {
                    if (!list.Contains(sprite.atlas.name))
                        list.Add(sprite.atlas.name);
                }
                //UISpriteData data = sprite.gameObject.AddComponent<UISpriteData>();
            }

            for (int i = 0, count = list.Count; i < count; i++)
            {
                if (!atlasNames.Contains(list[i]))
                    atlasNames.Add(list[i]);
            }

            if (dic.ContainsKey(name))
                dic[name] = list;
            else
                dic.Add(name, list);

            foreach (UISprite sprite in sprites)
            {
                GameBase.UISpriteData data = sprite.gameObject.AddComponent<GameBase.UISpriteData>();
                data.sprite = sprite;
                if (sprite.atlas == null)
                {
                    Debug.LogError("ui sprite atlas is null->" + name + "^" + sprite.name);
                    return;
                }
                index = atlasNames.IndexOf(sprite.atlas.name);
                sprite.atlas = null;
                if (index < 0)
                {
                    Object.DestroyImmediate(go);
                    return;
                }
                else
                {
                    data.index = index;
                }
                uiData.sprites.Add(data);
            }
        }

        if (labels != null)
        {
            foreach (UILabel label in labels)
            {
                UILabelData data = label.gameObject.AddComponent<UILabelData>();
                data.label = label;
                data.label.trueTypeFont = null;
                data.label.font = null;
                index = textList.IndexOf(label.text);
                if (index < 0)
                {
                    index = textList.Count;
                    textList.Add(label.text);
                }
                data.index = index;
                uiData.labels.Add(data);
            }
        }

        if (events != null)
        {
            foreach (UIEvent e in events)
            {
                uiData.events.Add(e);
            }
        }

        Object prefab = GetPrefab(go, name);
        Object.DestroyImmediate(go);
        BuildPipeline.BuildAssetBundle(prefab, null, exportPath + prefab.name, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, buildTarget);
    }

    private static void ProcessUIWidget(Object obj, string exportPath, BuildTarget buildTarget)
    {
        GameObject go = (GameObject)Object.Instantiate(obj);

        string name = go.name;
        int index = name.IndexOf("(");
        if (index >= 0)
            name = name.Substring(0, index);

        UISprite[] sprites = go.GetComponentsInChildren<UISprite>(true);
        UILabel[] labels = go.GetComponentsInChildren<UILabel>(true);
        //UIEvent[] events = go.GetComponentsInChildren<UIEvent>();
        if (sprites != null)
        {
            foreach (UISprite sprite in sprites)
            {
                sprite.atlas = null;
            }
        }

        if (labels != null)
        {
            foreach (UILabel label in labels)
            {
                label.trueTypeFont = null;
                label.font = null;
            }
        }

        Object prefab = GetPrefab(go, name);
        Object.DestroyImmediate(go);
        BuildPipeline.BuildAssetBundle(prefab, null, exportPath + prefab.name, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, buildTarget);
    }

    private static Object GetPrefab(GameObject go, string name)
    {
        string path = "Assets/temp/";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        Object temp = EditorUtility.CreateEmptyPrefab(path + name + ".prefab");
        temp = EditorUtility.ReplacePrefab(go, temp);
        Object.DestroyImmediate(go);
        return temp;
    }
}
