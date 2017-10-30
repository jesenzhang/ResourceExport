using UnityEngine;
using System.Collections.Generic;

public class SimpleFractureObject : FracturedObject
{
    public Vector3 mForce = new Vector3(0, 0, 0);
    public float mRadius = 1;

    public void BreakAll()
    {
        if (transform.childCount <= 0)
            return;
        FracturedChunk chunk = transform.GetChild(0).GetComponent<FracturedChunk>();
        if (chunk != null)
            chunk.Impact(mForce, chunk.transform.position, mRadius);
    }

    public void BreakByGravity()
    {
        Rigidbody[] rs = GetComponentsInChildren<Rigidbody>();
        if (rs == null)
            return;
        for (int i = 1; i < rs.Length; i++)
        {
            rs[i].isKinematic = false;
        }
    }

    public static GameObject CreateFractureObject(GameObject sourceObject, FracturedParamData data)
    {
        if(sourceObject.transform.childCount != 1)
        {
            sourceObject = CreateCombineFractureObject(sourceObject);
        }
        return CreateSingleFractureObject(sourceObject, data);
    }

    private static GameObject CreateSingleFractureObject(GameObject sourceObject,FracturedParamData data)
    {
        GameObject fracturedObject = new GameObject();
        fracturedObject.name = "Simple Fractured Object";
        fracturedObject.transform.position = Vector3.zero;
        SimpleFractureObject fractureComponent = fracturedObject.AddComponent<SimpleFractureObject>();

        fractureComponent.SourceObject = sourceObject;
        fractureComponent.StartStatic = true;
        fractureComponent.TotalMass = data.TotalMass;
        fractureComponent.SplitMaterial = null;

        fractureComponent.GenerateNumChunks = data.GenerateNumChunks;
        fractureComponent.SplitRegularly = data.SplitRegularly;
        fractureComponent.SplitXProbability = data.SplitXProbability;
        fractureComponent.SplitYProbability = data.SplitYProbability;
        fractureComponent.SplitZProbability = data.SplitZProbability;

        fractureComponent.SplitSizeVariation = data.SplitSizeVariation;
        fractureComponent.SplitXVariation = data.SplitXVariation;
        fractureComponent.SplitYVariation = data.SplitYVariation;
        fractureComponent.SplitZVariation = data.SplitZVariation;

        fractureComponent.EventDetachedMinLifeTime = data.EventDetachedMinLifeTime;
        fractureComponent.EventDetachedMaxLifeTime = data.EventDetachedMaxLifeTime;
        
        for (int i = 0; i < data.supportPlaneNames.Count; i++)
        {
            fractureComponent.AddSupportPlane();
            UltimateFracturing.SupportPlane supportPlane = fractureComponent.ListSupportPlanes[i];
            supportPlane.GUIName = data.supportPlaneNames[i];
            supportPlane.v3PlanePosition = data.supportPlanePoss[i];
            supportPlane.qPlaneRotation = data.supportPlaneQuaternions[i];
            supportPlane.v3PlaneScale = data.supportPlaneScales[i];
        }

        fractureComponent.RandomSeed = data.RandomSeed;

        List<GameObject> listChunks;
        UltimateFracturing.Fracturer.FractureToChunks(fractureComponent, true, out listChunks);

        return fracturedObject;
    }

    private static GameObject CreateCombineFractureObject(GameObject sourceObject)
    {
        GameObject combinedMeshObject = new GameObject();
        combinedMeshObject.name = "Combined Mesh";
        combinedMeshObject.transform.position = Vector3.zero;
        CombinedMesh combineMesh = combinedMeshObject.AddComponent<CombinedMesh>();

        combineMesh.RootNode = sourceObject;
        combineMesh.PivotMode = CombinedMesh.EPivotMode.Keep;
        List<MeshFilter> listMeshFilters = new List<MeshFilter>();
        BuildMeshFilterListRecursive(sourceObject, listMeshFilters);
        combineMesh.MeshObjects = listMeshFilters.ToArray();
        combineMesh.Combine(null);
        sourceObject.SetActive(false);

        return combinedMeshObject;
    }

    private static void BuildMeshFilterListRecursive(GameObject node, List<MeshFilter> listMeshFilters)
    {
        MeshFilter meshFilter = node.GetComponent<MeshFilter>();

        if (meshFilter && node.GetComponent<Renderer>())
        {
            listMeshFilters.Add(meshFilter);
        }

        for (int nChild = 0; nChild < node.transform.childCount; nChild++)
        {
            BuildMeshFilterListRecursive(node.transform.GetChild(nChild).gameObject, listMeshFilters);
        }
    }
}

public class FracturedParamData
{
    public float TotalMass = 0;

    public int GenerateNumChunks = 0;
    public bool SplitRegularly = false;
    public float SplitXProbability = 0;
    public float SplitYProbability = 0;
    public float SplitZProbability = 0;

    public float SplitSizeVariation = 0;
    public float SplitXVariation = 0;
    public float SplitYVariation = 0;
    public float SplitZVariation = 0;

    public float EventDetachedMinLifeTime = 0;
    public float EventDetachedMaxLifeTime = 0;

    public int RandomSeed = 0;

    public List<string> supportPlaneNames = new List<string>();
    public List<Vector3> supportPlanePoss = new List<Vector3>();
    public List<Quaternion> supportPlaneQuaternions = new List<Quaternion>();
    public List<Vector3> supportPlaneScales = new List<Vector3>();

    public void AddSupportPlane(string guiName,Vector3 pos,Vector3 eul,Vector3 scale)
    {
        supportPlaneNames.Add(guiName);
        supportPlanePoss.Add(pos);
        supportPlaneQuaternions.Add(Quaternion.Euler(eul));
        supportPlaneScales.Add(scale);
    }
}
