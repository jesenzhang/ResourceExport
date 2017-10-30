using UnityEngine;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using ProtoBuf;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System;

public enum ModelTargetType
{
    Player = 0,
    NPC = 1
}
[Serializable]
public class ExportSettingData : ScriptableObject
{
    [SerializeField]
    //忽略列表
    public List<string> IgnoreList = new List<string>();
    [SerializeField]
    //人物路径列表
    public List<string> CharactorPaths = new List<string>();

    public ExportSettingData Copy()
    {
        ExportSettingData copyskill = ScriptableObject.CreateInstance<ExportSettingData>();
        copyskill.IgnoreList = new List<string>();
        for (int i = 0; i < IgnoreList.Count; i++)
        {
            copyskill.IgnoreList.Add(IgnoreList[i]);
        }

        copyskill.CharactorPaths = new List<string>();
        for (int i = 0; i < CharactorPaths.Count; i++)
        {
            copyskill.CharactorPaths.Add(CharactorPaths[i]);
        }
        return copyskill;
    }
}

public class AssetUtility
{
    //资源根路径
    public static string ResPath = "Res/";
    //主角资源路径
    public static string PlayerPath = "Player/";
    //动画资源路径
    public static string AnimationPath = "Animation/";
    //NPC资源路径
    public static string NPCPath = "NPC/";
    //技能特效资源路径
    public static string SkillEffectPath = "Effect/particle/Skill/";
    //声音资源路径
    public static string AudioPath = "Audio/";
    //特效资源路径
    public static string EffectPath = "Effect/particle/";
    //坐骑资源路径
    public static string RidePath = "Ride/";
    //设置路径
    public static string SettingPath = "Assets/Setting/setting.asset";

    public static ExportSettingData exportSetting = ScriptableObject.CreateInstance<ExportSettingData>();


    // 目录是否存在
    public static void CheckAssetPath(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    public static void inputFiles(string path, Dictionary<string, int> files, string regex, SearchOption opt)
    {
        string[] filesList = Directory.GetFiles(path, regex, opt);
        for (int i = 0; i < filesList.Length; i++)
        {
            string filePath = filesList[i].Replace(@"\", @"/");
            // if (Regex.IsMatch(filePath, regex) == true)
            {
                if (files.ContainsKey(filePath) == false)
                {
                    files.Add(filePath, 0);
                }
                else
                {
                    files[filePath]++;
                }
            }
        }
    }

    public static List<UnityEngine.Object> GetAllAudio()
    {
        string Path = "Assets/" + ResPath+ AudioPath;
        Dictionary<string, int> files = new Dictionary<string, int>();
        inputFiles(Path, files, "*.mp3",SearchOption.AllDirectories);
        List<UnityEngine.Object> allaudio = new List<UnityEngine.Object>();
        foreach (var p in files.Keys)
        {
            if (!exportSetting.IgnoreList.Contains(p))
            {
                UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                allaudio.Add(prefab);
            }
        }
        return allaudio;
    }
    public static List<List<UnityEngine.Object>> GetAllCharactor()
    {
        string Path = "Assets/" + ResPath+ PlayerPath;
        string[] allfolds = Directory.GetDirectories(Path);
        List<List<UnityEngine.Object>> allfbx = new List<List<UnityEngine.Object>>();
        List<UnityEngine.Object> fbx = new List<UnityEngine.Object>();
        List<UnityEngine.Object> animation = new List<UnityEngine.Object>();
        allfbx.Add(fbx);
        allfbx.Add(animation);
        Dictionary<string, int> rbxfiles = new Dictionary<string, int>();
        Dictionary<string, int> animationfiles = new Dictionary<string, int>();
        for (int i = 0; i < allfolds.Length; i++)
        {
            if (exportSetting.CharactorPaths.Contains(allfolds[i]))
            {
                inputFiles(allfolds[i], rbxfiles, "*.fbx", SearchOption.TopDirectoryOnly);
                inputFiles(allfolds[i] + "/" + AnimationPath, animationfiles, "*.controller", SearchOption.TopDirectoryOnly);
                foreach (var p in rbxfiles.Keys)
                {
                    if (!exportSetting.IgnoreList.Contains(p))
                    {
                        UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                        fbx.Add(prefab);
                    }
                }
                foreach (var p in animationfiles.Keys)
                {
                    if (!exportSetting.IgnoreList.Contains(p))
                    {
                        UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                        animation.Add(prefab);
                    }
                }
            }
           
        }
        return allfbx;
    }
    public static List<List<UnityEngine.Object>> GetAllPlayer()
    {
        string Path = "Assets/" + ResPath + PlayerPath;
        string[] allfolds = Directory.GetDirectories(Path);
        List<List<UnityEngine.Object>> allfbx = new List<List<UnityEngine.Object>>();
        List<UnityEngine.Object> fbx = new List<UnityEngine.Object>();
        List<UnityEngine.Object> animation = new List<UnityEngine.Object>();
        allfbx.Add(fbx);
        allfbx.Add(animation);
        Dictionary<string, int> rbxfiles = new Dictionary<string, int>();
        Dictionary<string, int> animationfiles = new Dictionary<string, int>();
        for (int i = 0; i < allfolds.Length; i++)
        {
            if (!exportSetting.CharactorPaths.Contains(allfolds[i]))
            {
                inputFiles(allfolds[i], rbxfiles, "*.fbx", SearchOption.TopDirectoryOnly);
                inputFiles(allfolds[i] + "/" + AnimationPath, animationfiles, "*.controller", SearchOption.TopDirectoryOnly);
                foreach (var p in rbxfiles.Keys)
                {
                    if (!exportSetting.IgnoreList.Contains(p))
                    {
                        UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                        fbx.Add(prefab);
                    }
                }
                foreach (var p in animationfiles.Keys)
                {
                    if (!exportSetting.IgnoreList.Contains(p))
                    {
                        UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                        animation.Add(prefab);
                    }
                }
            }

        }
        return allfbx;
    }
    public static List<List<UnityEngine.Object>> GetAllNpc()
    {
        string Path = "Assets/" + ResPath + NPCPath;
        string[] allfolds = Directory.GetDirectories(Path);
        List<List<UnityEngine.Object>> allfbx = new List<List<UnityEngine.Object>>();
        List<UnityEngine.Object> fbx = new List<UnityEngine.Object>();
        List<UnityEngine.Object> animation = new List<UnityEngine.Object>();
        allfbx.Add(fbx);
        allfbx.Add(animation);
        Dictionary<string, int> rbxfiles = new Dictionary<string, int>();
        Dictionary<string, int> animationfiles = new Dictionary<string, int>();
        for (int i = 0; i < allfolds.Length; i++)
        {
            inputFiles(allfolds[i], rbxfiles, "*.fbx", SearchOption.TopDirectoryOnly);
            inputFiles(allfolds[i], rbxfiles, "*.prefab", SearchOption.TopDirectoryOnly);
            inputFiles(allfolds[i] + "/" + AnimationPath, animationfiles, "*.controller", SearchOption.TopDirectoryOnly);
            foreach (var p in rbxfiles.Keys)
            {
                if (!exportSetting.IgnoreList.Contains(p))
                {
                    UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                    fbx.Add(prefab);
                }
            }
            foreach (var p in animationfiles.Keys)
            {
                UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                animation.Add(prefab);
            }
        }
        return allfbx;
    }
    public static List<UnityEngine.Object> GetAllEffect()
    {
        string Path = "Assets/" + ResPath + EffectPath;
        string[] allfolds = Directory.GetDirectories(Path);
        List<UnityEngine.Object> allfbx = new List<UnityEngine.Object>();
        Dictionary<string, int> rbxfiles = new Dictionary<string, int>();
        for (int i = 0; i < allfolds.Length; i++)
        {
            inputFiles(allfolds[i], rbxfiles, "*.prefab", SearchOption.AllDirectories);
            foreach (var p in rbxfiles.Keys)
            {
                if (!exportSetting.IgnoreList.Contains(p))
                {
                    UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                    allfbx.Add(prefab);
                }
            }
        }
        return allfbx;
    }
    public static List<List<UnityEngine.Object>> GetAllRide()
    {
        string Path = "Assets/" + ResPath + RidePath;
        string[] allfolds = Directory.GetDirectories(Path);
        List<List<UnityEngine.Object>> allfbx = new List<List<UnityEngine.Object>>();
        List<UnityEngine.Object> fbx = new List<UnityEngine.Object>();
        List<UnityEngine.Object> animation = new List<UnityEngine.Object>();
        allfbx.Add(fbx);
        allfbx.Add(animation);
        Dictionary<string, int> rbxfiles = new Dictionary<string, int>();
        Dictionary<string, int> animationfiles = new Dictionary<string, int>();
        for (int i = 0; i < allfolds.Length; i++)
        {
            inputFiles(allfolds[i], rbxfiles, "*.fbx", SearchOption.TopDirectoryOnly);
            inputFiles(allfolds[i], rbxfiles, "*.prefab", SearchOption.TopDirectoryOnly);
            inputFiles(allfolds[i] + "/" + AnimationPath, animationfiles, "*.controller", SearchOption.TopDirectoryOnly);
            foreach (var p in rbxfiles.Keys)
            {
                if (!exportSetting.IgnoreList.Contains(p))
                {
                    UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                    fbx.Add(prefab);
                }
            }
            foreach (var p in animationfiles.Keys)
            {
                UnityEngine.Object prefab = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
                animation.Add(prefab);
            }
        }
        return allfbx;
    }


    public static List<GameObject> GetAllFBX()
    {
        string Path = "Assets/" + ResPath;
        Dictionary<string, int> files = new Dictionary<string, int>();
        inputFiles(Path, files, "*.fbx", SearchOption.AllDirectories);
        List<GameObject> allfbx = new List<GameObject>();
        foreach (var p in files.Keys)
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath<GameObject>(p);
            allfbx.Add(prefab);
        }

        return allfbx;
    }
    public static List<GameObject> GetAllFBXWithType(ModelTargetType mtype)
    {
        string Path = "Assets/" + ResPath;
        if (mtype == ModelTargetType.Player)
            Path = "Assets/" + ResPath + PlayerPath;
        if (mtype == ModelTargetType.NPC)
            Path = "Assets/" + ResPath + NPCPath;
        Dictionary<string, int> files = new Dictionary<string, int>();
        inputFiles(Path, files, "*.fbx", SearchOption.AllDirectories);
        List<GameObject> allfbx = new List<GameObject>();
        foreach (var p in files.Keys)
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath<GameObject>(p);
            allfbx.Add(prefab);
        }

        return allfbx;
    }
    public static GameObject GetFBXWithName(string Name)
    {
        string Path = "Assets/" + ResPath;
        Dictionary<string, int> files = new Dictionary<string, int>();
        inputFiles(Path, files, Name + ".fbx", SearchOption.AllDirectories);
        List<GameObject> allfbx = new List<GameObject>();
        foreach (var p in files.Keys)
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath<GameObject>(p);
            allfbx.Add(prefab);
        }
        if (allfbx.Count >= 1)
            return allfbx[0];
        else
            return null;
    }
    public static List<GameObject> GetAllSkillEffectPrefabs()
    {
        string Path = "Assets/" + ResPath + SkillEffectPath;
        Dictionary<string, int> files = new Dictionary<string, int>();
        inputFiles(Path, files, "*.prefab", SearchOption.AllDirectories);
        List<GameObject> allfbx = new List<GameObject>();
        foreach (var p in files.Keys)
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath<GameObject>(p);
            allfbx.Add(prefab);
        }
        return allfbx;
    }

    public static List<GameObject> GetModelSkillEffectPrefabs(string modelName)
    {
        List<GameObject> allfbx = new List<GameObject>();
        string Path = "Assets/" + ResPath + SkillEffectPath;
        Dictionary<string, int> files = new Dictionary<string, int>();
        inputFiles(Path, files, modelName + "*.prefab",SearchOption.AllDirectories);

        foreach (var p in files.Keys)
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath<GameObject>(p);
            allfbx.Add(prefab);
        }

        return allfbx;
    }
    public static GameObject GetAsset(string Path)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/" + Path);
        return prefab;
    }
    public static string GetPlayerPath(string PlayerName)
    {
        string path = "Assets/" + ResPath + PlayerPath + PlayerName + "/" + PlayerName + ".FBX";

        return path;
    }

    public static string GetNPCPath(string NPCName)
    {
        string path = "Assets/" + ResPath + NPCPath + NPCName + "/" + NPCName + ".FBX";

        return path;
    }
    public static string GetPlayerEffectPath(string PlayerName, string EffectName)
    {
        string path = "Assets/" + ResPath + SkillEffectPath + PlayerName + "/" + EffectName + ".prefab";
        return path;
    }

    public static string GetNPCEffectPath(string NPCName, string EffectName)
    {
        string path = "Assets/" + ResPath + SkillEffectPath + NPCName + "/" + EffectName + ".prefab";
        return path;
    }

    public static GameObject GetPlayerAsset(string PlayerName)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(GetPlayerPath(PlayerName));
        if (prefab == null)
        {
            throw new System.Exception(string.Format("PlayerAsset {0} not found", PlayerName));
        }
        return prefab;
    }

    public static GameObject GetNPCAsset(string NPCName)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(GetNPCPath(NPCName));
        if (prefab == null)
        {
            throw new System.Exception(string.Format("PlayerAsset {0} not found", NPCName));
        }
        return prefab;
    }

    public static GameObject GetFBXAsset(ModelTargetType tartype, string Name)
    {
        GameObject fbx = null;
        if (tartype == ModelTargetType.Player)
            fbx = GetPlayerAsset(Name);
        if (tartype == ModelTargetType.NPC)
            fbx = GetNPCAsset(Name);
        return fbx;
    }

    //获取人物的技能特效
    public static GameObject GetPlayerSkillEffect(string PlayerName, string EffectName)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(GetPlayerEffectPath(PlayerName, EffectName));
        if (prefab == null)
        {
            throw new System.Exception(string.Format(PlayerName + " SkillEffect {0} not found", EffectName));
        }
        return prefab;
    }

    public static GameObject GetNPCSkillEffect(string NPCName, string EffectName)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(GetNPCEffectPath(NPCName, EffectName));
        if (prefab == null)
        {
            throw new System.Exception(string.Format(NPCName + " SkillEffect {0} not found", EffectName));
        }
        return prefab;
    }

    //获取技能特效
    public static GameObject GetSkillEffect(ModelTargetType tartype, string PlayerName, string EffectName)
    {
        string path = "Assets/" + ResPath + SkillEffectPath + PlayerName + "/" + EffectName + ".prefab";
        if (tartype == ModelTargetType.Player)
            path = "Assets/" + ResPath + SkillEffectPath + PlayerName + "/" + EffectName + ".prefab";
        if (tartype == ModelTargetType.NPC)
            path = "Assets/" + ResPath + SkillEffectPath + PlayerName + "/" + EffectName + ".prefab";

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null)
        {
            throw new System.Exception(string.Format(PlayerName + " SkillEffect {0} not found", EffectName));
        }
        return prefab;
    }

    public static RuntimeAnimatorController GetPlayerAnimationCtl(string PlayerName, string CtlName)
    {
        string path = "Assets/" + ResPath + PlayerPath + PlayerName + "/Animation/" + CtlName + ".controller";
        RuntimeAnimatorController prefab = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(path);
        return prefab;
    }
    public static RuntimeAnimatorController GetAnimationCtl(ModelTargetType tartype, string Name, string CtlName)
    {
        string path = "Assets/" + ResPath + PlayerPath + Name + "/Animation/" + CtlName + ".controller";
        if (tartype == ModelTargetType.Player)
            path = "Assets/" + ResPath + PlayerPath + Name + "/Animation/" + CtlName + ".controller";
        if (tartype == ModelTargetType.NPC)
            path = "Assets/" + ResPath + NPCPath + Name + "/Animation/" + CtlName + ".controller";
        RuntimeAnimatorController prefab = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(path);
        return prefab;
    }

    public static void SaveDataAsAsset(UnityEngine.Object data, string Path)
    {
        AssetDatabase.CreateAsset(data, Path);
    }
    public static UnityEngine.Object LoadDataAsAssetFrom(string Path)
    {
        return AssetDatabase.LoadAssetAtPath(Path, typeof(UnityEngine.Object));
    }

    public static void LoadSetting()
    {
        // 导入
        //string path = EditorUtility.OpenFilePanel("Load IgnoreData", Application.dataPath + "/", "asset");
        ///if (path.Length != 0)
        {
           // path = "Assets" + path.Replace(Application.dataPath, "");
            ExportSettingData copydata = AssetDatabase.LoadAssetAtPath(SettingPath, typeof(ExportSettingData)) as ExportSettingData;
            exportSetting = copydata.Copy();
            AssetDatabase.Refresh();
        }
    }
    public static void SaveSetting()
    {
        // 保存
        //string path = EditorUtility.SaveFilePanel("Save IgnoreData", Application.dataPath + "/", "", "asset");
        //if (path.Length != 0)
        {
           // path = "Assets" + path.Replace(Application.dataPath, "");
            SaveDataAsAsset(exportSetting.Copy(), SettingPath);
            AssetDatabase.Refresh();
        }
    }

}
