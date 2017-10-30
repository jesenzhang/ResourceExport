
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using GameBase;

class CharacterExport
{
    [MenuItem("Export/Export Character")]
    private static void Execute()
    {
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //objs = ObjVersionChecker.FilterWithDependencies(objs);
        if (objs == null || objs.Length == 0)
        {
            Debug.LogError("there is no object selected!");
            return;
        }
        ExecuteUnPack(objs, EditorUserBuildSettings.activeBuildTarget);
    }

    public static void ExecuteUnPack(Object[] objs, BuildTarget buildTarget)
    {
        string rootPath = EditorUtils.PlatformPath(buildTarget);
        if (rootPath == null)
            return;
        foreach (Object obj in objs)
        {
            string assetpath = AssetDatabase.GetAssetPath(obj).Replace("//", "/").Replace("\\", "/").Replace("Assets/", "");
            string assetname = Path.GetFileNameWithoutExtension(assetpath);
            string outpath = Application.dataPath + "/../" + rootPath + Path.GetDirectoryName(assetpath) + "/";
            if (!Directory.Exists(outpath)) Directory.CreateDirectory(outpath);

            GameObject character = obj as GameObject;
            GameObject characterClone = (GameObject)Object.Instantiate(obj);
            foreach (Animator anim in characterClone.GetComponentsInChildren<Animator>())
                anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            foreach (SkinnedMeshRenderer smr in characterClone.GetComponentsInChildren<SkinnedMeshRenderer>())
                Object.DestroyImmediate(smr.gameObject);

            characterClone.AddComponent<SkinnedMeshRenderer>();
            Object characterBasePrefab = GetPrefab(characterClone, "characterbase");
            BuildPipeline.BuildAssetBundle(characterBasePrefab, null, outpath + assetname + "_characterbase", BuildAssetBundleOptions.CollectDependencies, buildTarget);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(characterBasePrefab));
            GameObject.DestroyImmediate(characterClone);
            foreach (SkinnedMeshRenderer smr in character.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                List<Object> equipobj = new List<Object>();

                GameObject equipClone = (GameObject)EditorUtility.InstantiatePrefab(smr.gameObject);
                GameObject equipParent = equipClone.transform.parent.gameObject;
                equipClone.transform.parent = null;
                Object.DestroyImmediate(equipParent);
                Object rendererPrefab = GetPrefab(equipClone, "rendererobject");
                equipobj.Add(rendererPrefab);

                List<string> boneNames = new List<string>();
                foreach (Transform t in smr.bones)
                    boneNames.Add(t.name);

                Debug.LogError("bone count->" + boneNames.Count);

                string stringholderpath = "Assets/bonenames.asset";

                StringContentHolder holder = ScriptableObject.CreateInstance<StringContentHolder>();
                holder.content = boneNames.ToArray();
                AssetDatabase.CreateAsset(holder, stringholderpath);
                equipobj.Add(AssetDatabase.LoadAssetAtPath(stringholderpath, typeof(StringContentHolder)));

                BuildPipeline.BuildAssetBundle(null, equipobj.ToArray(), outpath + smr.name, BuildAssetBundleOptions.CollectDependencies, buildTarget);
                GameObject.DestroyImmediate(equipClone);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rendererPrefab));
                AssetDatabase.DeleteAsset(stringholderpath);
            }
        }

        Debug.Log("****************** over ************************");
    }

    private static Object GetPrefab(GameObject go, string name)
    {
        if (!Directory.Exists("Assets/temp/"))
            Directory.CreateDirectory("Assets/temp/");
        Object tempPrefab = EditorUtility.CreateEmptyPrefab("Assets/temp/" + name + ".prefab");
        tempPrefab = EditorUtility.ReplacePrefab(go, tempPrefab);
        Object.DestroyImmediate(go);
        return tempPrefab;
    }
}


