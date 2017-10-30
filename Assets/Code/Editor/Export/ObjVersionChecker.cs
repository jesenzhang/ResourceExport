using UnityEngine;
using System.Collections.Generic;
using GameBase;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class ObjVersionChecker
{
    static string PREFAB_VERSION_PATH = Path.GetFullPath(Application.dataPath + "/../Export/" + EditorUtils.UserName() + "/ExportVersion.zl");
    static MD5 md5Util = MD5.Create();

    public static bool IsNewVersion(string assetPath)
    {
        if(!File.Exists(PREFAB_VERSION_PATH))
        {
            File.Create(PREFAB_VERSION_PATH).Close();
        }
        assetPath = Application.dataPath.Replace("Assets","") + "/" + assetPath;
        assetPath = assetPath.Replace("//", "/").Replace("\\", "/");
        ZLText zl = new ZLText(File.ReadAllBytes(PREFAB_VERSION_PATH));
        List<string> list = zl.Read(assetPath);
        string oldmd5 = list.Count == 0 ? string.Empty : list[0];
        string newmd5 = GetMD5H(assetPath);
        if (oldmd5 == newmd5)
        {
            return false;
        }

        Dictionary<string, List<string>> allmd5 = zl.ReadAll();
        allmd5[assetPath] = new List<string>() { newmd5 };
        File.WriteAllBytes(PREFAB_VERSION_PATH,zl.Write(allmd5));
        return true;
    }

    public static string[] Filter(string[] assetpath)
    {
        List<string> newpaths = new List<string>();
        for(int i = 0;i < assetpath.Length;i++)
        {
            if(IsNewVersion(assetpath[i]))
            {
                newpaths.Add(assetpath[i]);
            }
        }
        return newpaths.ToArray();
    }

    public static UnityEngine.Object[] Filter(UnityEngine.Object[] assets)
    {
        if (assets == null || assets.Length == 0)
            return assets;
        List<UnityEngine.Object> newAssets = new List<UnityEngine.Object>();
        for(int i = 0;i < assets.Length;i++)
        {
            if(IsNewVersion(UnityEditor.AssetDatabase.GetAssetPath(assets[i].GetInstanceID())))
            {
                newAssets.Add(assets[i]);
            }
        }
        return newAssets.ToArray();
    }

    public static UnityEngine.Object[] FilterWithDependencies(UnityEngine.Object[] assets)
    {
        if (assets == null || assets.Length == 0)
            return assets;
        List<UnityEngine.Object> newAssets = new List<UnityEngine.Object>();
        for (int i = 0; i < assets.Length; i++)
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(assets[i].GetInstanceID());
            string[] dependencies = UnityEditor.AssetDatabase.GetDependencies(assetPath);
            if(Filter(dependencies).Length != 0)
            {
                newAssets.Add(assets[i]);
            }
        }            
        return newAssets.ToArray();
    }

    private static string GetMD5H(string path)
    {
        byte[] buffer = md5Util.ComputeHash(File.ReadAllBytes(path));
        StringBuilder newmd5 = new StringBuilder();
        for (int i = 0; i < buffer.Length; i++)
        {
            newmd5.Append(buffer[i].ToString("x2"));
        }
        return newmd5.ToString();
    }
}
