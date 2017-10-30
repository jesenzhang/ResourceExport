using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SimpleFractureObject))]
public class SimpleFracturedObjectEditor : Editor
{
    SerializedProperty PropSourceObject;
    //SerializedProperty PropGenerateIslands;
    //SerializedProperty PropGenerateChunkConnectionInfo;
    SerializedProperty PropStartStatic;
    //SerializedProperty PropChunkConnectionMinArea;
    //SerializedProperty PropChunkConnectionStrength;
    //SerializedProperty PropChunkHorizontalRadiusSupportStrength;
    //SerializedProperty PropChunkIslandConnectionMaxDistance;
    SerializedProperty PropTotalMass;
    SerializedProperty PropChunkPhysicMaterial;
    //SerializedProperty PropMinColliderVolumeForBox;
    //SerializedProperty PropCapPrecisionFix;
    //SerializedProperty PropInvertCapNormals;

    SerializedProperty PropFracturePattern;
    SerializedProperty PropVoronoiVolumeOptimization;
    SerializedProperty PropVoronoiProximityOptimization;
    SerializedProperty PropVoronoiMultithreading;
    SerializedProperty PropVoronoiCellsXCount;
    SerializedProperty PropVoronoiCellsYCount;
    SerializedProperty PropVoronoiCellsZCount;
    SerializedProperty PropVoronoiCellsXSizeVariation;
    SerializedProperty PropVoronoiCellsYSizeVariation;
    SerializedProperty PropVoronoiCellsZSizeVariation;
    SerializedProperty PropNumChunks;
    //SerializedProperty PropSplitsWorldSpace;
    SerializedProperty PropSplitRegularly;
    SerializedProperty PropSplitXProbability;
    SerializedProperty PropSplitYProbability;
    SerializedProperty PropSplitZProbability;
    SerializedProperty PropSplitSizeVariation;
    SerializedProperty PropSplitXVariation;
    SerializedProperty PropSplitYVariation;
    SerializedProperty PropSplitZVariation;
    SerializedProperty PropSplitMaterial;
    //SerializedProperty PropSplitMappingTileU;
    //SerializedProperty PropSplitMappingTileV;

    //SerializedProperty PropEventDetachMinMass;
    //SerializedProperty PropEventDetachMinVelocity;
    //SerializedProperty PropEventDetachExitForce;
    //SerializedProperty PropEventDetachUpwardsModifier;
    SerializedProperty PropEventDetachSound;
    //SerializedProperty PropEventDetachPrefabsArray;
    //SerializedProperty PropEventDetachCollisionCallMethod;
    //SerializedProperty PropEventDetachCollisionCallGameObject;
    SerializedProperty PropEventDetachedMinLifeTime;
    SerializedProperty PropEventDetachedMaxLifeTime;
    //SerializedProperty PropEventDetachedOffscreenLifeTime;
    //SerializedProperty PropEventDetachedMinMass;
    //SerializedProperty PropEventDetachedMinVelocity;
    //SerializedProperty PropEventDetachedMaxSounds;
    //SerializedProperty PropEventDetachedSoundArray;
    //SerializedProperty PropEventDetachedMaxPrefabs;
    //SerializedProperty PropEventDetachedPrefabsArray;
    //SerializedProperty PropEventDetachedCollisionCallMethod;
    //SerializedProperty PropEventDetachedCollisionCallGameObject;
    //SerializedProperty PropEventExplosionSound;
    //SerializedProperty PropEventExplosionPrefabInstanceCount;
    //SerializedProperty PropEventExplosionPrefabsArray;
    SerializedProperty PropEventImpactSound;
    //SerializedProperty PropEventImpactPrefabsArray;

    SerializedProperty PropRandomSeed;
    SerializedProperty PropDecomposePreview;
    //SerializedProperty PropAlwaysComputeColliders;
    //SerializedProperty PropShowChunkConnectionLines;
    //SerializedProperty PropShowChunkColoredState;
    //SerializedProperty PropShowChunkColoredRandomly;
    SerializedProperty PropSaveMeshDataToAsset;
    SerializedProperty PropMeshAssetDataFile;
    //SerializedProperty PropVerbose;

    SerializedProperty PropIntegrateWithConcaveCollider;
    SerializedProperty PropConcaveColliderMaxHullVertices;

    bool m_bProgressCancelled;

    [MenuItem("GameObject/Create Other/Ultimate Game Tools/Simple Fractured Object")]
    static void CreateFracturedObject()
    {
        GameObject fracturedObject = new GameObject();
        fracturedObject.name = "Simple Fractured Object";
        fracturedObject.transform.position = Vector3.zero;
        fracturedObject.AddComponent<SimpleFractureObject>();

        Selection.activeGameObject = fracturedObject;
    }

    void Progress(string strTitle, string strMessage, float fT)
    {
        if (EditorUtility.DisplayCancelableProgressBar(strTitle, strMessage, fT))
        {
            UltimateFracturing.Fracturer.CancelFracturing();
            m_bProgressCancelled = true;
        }
    }

    void OnEnable()
    {
        PropSourceObject = serializedObject.FindProperty("SourceObject");
        //PropGenerateIslands = serializedObject.FindProperty("GenerateIslands");
        //PropGenerateChunkConnectionInfo = serializedObject.FindProperty("GenerateChunkConnectionInfo");
        PropStartStatic = serializedObject.FindProperty("StartStatic");
        //PropChunkConnectionMinArea = serializedObject.FindProperty("ChunkConnectionMinArea");
        //PropChunkConnectionStrength = serializedObject.FindProperty("ChunkConnectionStrength");
        //PropChunkHorizontalRadiusSupportStrength = serializedObject.FindProperty("ChunkHorizontalRadiusSupportStrength");
        //PropChunkIslandConnectionMaxDistance = serializedObject.FindProperty("ChunkIslandConnectionMaxDistance");
        PropTotalMass = serializedObject.FindProperty("TotalMass");
        PropChunkPhysicMaterial = serializedObject.FindProperty("ChunkPhysicMaterial");
        //PropMinColliderVolumeForBox = serializedObject.FindProperty("MinColliderVolumeForBox");
        //PropCapPrecisionFix = serializedObject.FindProperty("CapPrecisionFix");
        //PropInvertCapNormals = serializedObject.FindProperty("InvertCapNormals");

        PropFracturePattern = serializedObject.FindProperty("FracturePattern");
        PropVoronoiVolumeOptimization = serializedObject.FindProperty("VoronoiVolumeOptimization");
        PropVoronoiProximityOptimization = serializedObject.FindProperty("VoronoiProximityOptimization");
        PropVoronoiMultithreading = serializedObject.FindProperty("VoronoiMultithreading");
        PropVoronoiCellsXCount = serializedObject.FindProperty("VoronoiCellsXCount");
        PropVoronoiCellsYCount = serializedObject.FindProperty("VoronoiCellsYCount");
        PropVoronoiCellsZCount = serializedObject.FindProperty("VoronoiCellsZCount");
        PropVoronoiCellsXSizeVariation = serializedObject.FindProperty("VoronoiCellsXSizeVariation");
        PropVoronoiCellsYSizeVariation = serializedObject.FindProperty("VoronoiCellsYSizeVariation");
        PropVoronoiCellsZSizeVariation = serializedObject.FindProperty("VoronoiCellsZSizeVariation");
        PropNumChunks = serializedObject.FindProperty("GenerateNumChunks");
        //PropSplitsWorldSpace = serializedObject.FindProperty("SplitsWorldSpace");
        PropSplitRegularly = serializedObject.FindProperty("SplitRegularly");
        PropSplitXProbability = serializedObject.FindProperty("SplitXProbability");
        PropSplitYProbability = serializedObject.FindProperty("SplitYProbability");
        PropSplitZProbability = serializedObject.FindProperty("SplitZProbability");
        PropSplitSizeVariation = serializedObject.FindProperty("SplitSizeVariation");
        PropSplitXVariation = serializedObject.FindProperty("SplitXVariation");
        PropSplitYVariation = serializedObject.FindProperty("SplitYVariation");
        PropSplitZVariation = serializedObject.FindProperty("SplitZVariation");
        PropSplitMaterial = serializedObject.FindProperty("SplitMaterial");
        //PropSplitMappingTileU = serializedObject.FindProperty("SplitMappingTileU");
        //PropSplitMappingTileV = serializedObject.FindProperty("SplitMappingTileV");

        //PropEventDetachMinMass = serializedObject.FindProperty("EventDetachMinMass");
        //PropEventDetachMinVelocity = serializedObject.FindProperty("EventDetachMinVelocity");
        //PropEventDetachExitForce = serializedObject.FindProperty("EventDetachExitForce");
        //PropEventDetachUpwardsModifier = serializedObject.FindProperty("EventDetachUpwardsModifier");
        PropEventDetachSound = serializedObject.FindProperty("EventDetachSound");
        //PropEventDetachPrefabsArray = serializedObject.FindProperty("EventDetachPrefabsArray");
        //PropEventDetachCollisionCallMethod = serializedObject.FindProperty("EventDetachCollisionCallMethod");
        //PropEventDetachCollisionCallGameObject = serializedObject.FindProperty("EventDetachCollisionCallGameObject");
        PropEventDetachedMinLifeTime = serializedObject.FindProperty("EventDetachedMinLifeTime");
        PropEventDetachedMaxLifeTime = serializedObject.FindProperty("EventDetachedMaxLifeTime");
        //PropEventDetachedOffscreenLifeTime = serializedObject.FindProperty("EventDetachedOffscreenLifeTime");
        //PropEventDetachedMinMass = serializedObject.FindProperty("EventDetachedMinMass");
        //PropEventDetachedMinVelocity = serializedObject.FindProperty("EventDetachedMinVelocity");
        //PropEventDetachedMaxSounds = serializedObject.FindProperty("EventDetachedMaxSounds");
        //PropEventDetachedSoundArray = serializedObject.FindProperty("EventDetachedSoundArray");
        //PropEventDetachedMaxPrefabs = serializedObject.FindProperty("EventDetachedMaxPrefabs");
        //PropEventDetachedPrefabsArray = serializedObject.FindProperty("EventDetachedPrefabsArray");
        //PropEventDetachedCollisionCallMethod = serializedObject.FindProperty("EventDetachedCollisionCallMethod");
        //PropEventDetachedCollisionCallGameObject = serializedObject.FindProperty("EventDetachedCollisionCallGameObject");
        //PropEventExplosionSound = serializedObject.FindProperty("EventExplosionSound");
        //PropEventExplosionPrefabInstanceCount = serializedObject.FindProperty("EventExplosionPrefabsInstanceCount");
        //PropEventExplosionPrefabsArray = serializedObject.FindProperty("EventExplosionPrefabsArray");
        PropEventImpactSound = serializedObject.FindProperty("EventImpactSound");
        //PropEventImpactPrefabsArray = serializedObject.FindProperty("EventImpactPrefabsArray");

        PropRandomSeed = serializedObject.FindProperty("RandomSeed");
        PropDecomposePreview = serializedObject.FindProperty("DecomposePreview");
        //PropShowChunkConnectionLines = serializedObject.FindProperty("ShowChunkConnectionLines");
        //PropShowChunkColoredState = serializedObject.FindProperty("ShowChunkColoredState");
        //PropShowChunkColoredRandomly = serializedObject.FindProperty("ShowChunkColoredRandomly");
        PropSaveMeshDataToAsset = serializedObject.FindProperty("SaveMeshDataToAsset");
        PropMeshAssetDataFile = serializedObject.FindProperty("MeshAssetDataFile");
        //PropAlwaysComputeColliders = serializedObject.FindProperty("AlwaysComputeColliders");
        //PropVerbose = serializedObject.FindProperty("Verbose");

        PropIntegrateWithConcaveCollider = serializedObject.FindProperty("IntegrateWithConcaveCollider");
        PropConcaveColliderMaxHullVertices = serializedObject.FindProperty("ConcaveColliderMaxHullVertices");
    }

    public void OnSceneGUI()
    {
        SimpleFractureObject fracturedComponent = target as SimpleFractureObject;

        if (fracturedComponent == null)
        {
            return;
        }

        // Chunk connections

        if (fracturedComponent.ShowChunkConnectionLines)
        {
            Color handlesColor = Handles.color;
            Handles.color = new Color32(255, 0, 0, 255); //new Color32(155, 89, 182, 255);

            foreach (FracturedChunk chunkA in fracturedComponent.ListFracturedChunks)
            {
                if (chunkA)
                {
                    if (chunkA.ListAdjacentChunks.Count > 0)
                    {
                        Handles.DotCap(0, chunkA.transform.position, Quaternion.identity, HandleUtility.GetHandleSize(chunkA.transform.position) * 0.05f);
                    }

                    foreach (FracturedChunk.AdjacencyInfo chunkAdjacency in chunkA.ListAdjacentChunks)
                    {
                        if (chunkAdjacency.chunk)
                        {
                            Handles.DrawLine(chunkA.transform.position, chunkAdjacency.chunk.transform.position);
                        }
                    }
                }
            }

            Handles.color = handlesColor;
        }

        // Support planes

        bool bPlanesChanged = false;

        if (fracturedComponent.ListSupportPlanes != null)
        {
            foreach (UltimateFracturing.SupportPlane supportPlane in fracturedComponent.ListSupportPlanes)
            {
                if (supportPlane.GUIShowInScene == false)
                {
                    continue;
                }

                Vector3 v3WorldPosition = fracturedComponent.transform.TransformPoint(supportPlane.v3PlanePosition);
                Quaternion qWorldRotation = fracturedComponent.transform.rotation * supportPlane.qPlaneRotation;
                Vector3 v3WorldScale = Vector3.Scale(supportPlane.v3PlaneScale, fracturedComponent.transform.localScale);

                Handles.Label(v3WorldPosition, supportPlane.GUIName);

                // Normalize qWorldRotation

                float fSum = 0;
                for (int i = 0; i < 4; ++i) fSum += qWorldRotation[i] * qWorldRotation[i];
                float fMagnitudeInverse = 1.0f / Mathf.Sqrt(fSum);
                for (int i = 0; i < 4; ++i) qWorldRotation[i] *= fMagnitudeInverse;

                Vector3 v3PlanePosition = supportPlane.v3PlanePosition;
                Quaternion qPlaneRotation = supportPlane.qPlaneRotation;
                Vector3 v3PlaneScale = supportPlane.v3PlaneScale;

                // Use tools

                switch (Tools.current)
                {
                    case Tool.Move:

                        v3PlanePosition = fracturedComponent.transform.InverseTransformPoint(Handles.PositionHandle(v3WorldPosition, qWorldRotation));
                        break;

                    case Tool.Rotate:

                        qPlaneRotation = Quaternion.Inverse(fracturedComponent.transform.rotation) * Handles.RotationHandle(qWorldRotation, v3WorldPosition);
                        break;

                    case Tool.Scale:

                        v3PlaneScale = Vector3.Scale(Handles.ScaleHandle(v3WorldScale, v3WorldPosition, qWorldRotation, HandleUtility.GetHandleSize(v3WorldPosition)), new Vector3(1.0f / fracturedComponent.transform.localScale.x, 1.0f / fracturedComponent.transform.localScale.y, 1.0f / fracturedComponent.transform.localScale.z));
                        break;
                }

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(fracturedComponent);
                    bPlanesChanged = true;

                    switch (Tools.current)
                    {
                        case Tool.Move: supportPlane.v3PlanePosition = v3PlanePosition; break;
                        case Tool.Rotate: supportPlane.qPlaneRotation = qPlaneRotation; break;
                        case Tool.Scale: supportPlane.v3PlaneScale = v3PlaneScale; break;
                    }
                }
            }
        }

        if (bPlanesChanged)
        {
            fracturedComponent.ComputeSupportPlaneIntersections();
            fracturedComponent.MarkNonSupportedChunks();
        }
    }

    public override void OnInspectorGUI()
    {
        int nIndentationJump = 2;
        Vector4 v4GUIColor = GUI.contentColor;

        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.

        serializedObject.Update();

        SimpleFractureObject fracturedComponent = serializedObject.targetObject as SimpleFractureObject;

        // Show the custom GUI controls

        bool bDecomposePreviewUpdate = false;
        bool bReassignSplitMaterial = false;
        bool bComputeFracturing = false;
        bool bComputeColliders = false;
        bool bDeleteColliders = false;
        bool bMarkNonSupportedChunks = false;
        bool bRecomputePlanes = false;

        EditorGUILayout.Space();
        fracturedComponent.GUIExpandMain = EditorGUILayout.Foldout(fracturedComponent.GUIExpandMain, new GUIContent("Main", "Main fracturing parameters"));

        if (fracturedComponent.GUIExpandMain)
        {
            GUI.contentColor = PropSourceObject.objectReferenceValue == null ? Color.red : GUI.contentColor;
            PropSourceObject.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("源物体", "要进行破碎处理的物体"), PropSourceObject.objectReferenceValue, typeof(GameObject), true);
            GUI.contentColor = v4GUIColor;
            //PropGenerateIslands.boolValue = EditorGUILayout.Toggle(new GUIContent("Island Generation", "Detect isolated meshes when splitting and make them separated chunks (f.e. if you split a U shape horizontally, it will give 3 objects with island generation (the bottom and the two tips separated), instead of 2 (the bottom on one hand and the two tips as the same object on the other)."), //PropGenerateIslands.boolValue);
            //PropGenerateChunkConnectionInfo.boolValue = EditorGUILayout.Toggle(new GUIContent("Chunk Interconnection", "Will generate a connection graph between chunks to enable structural behavior."), PropGenerateChunkConnectionInfo.boolValue);
            //GUI.enabled = PropGenerateChunkConnectionInfo.boolValue;
            PropStartStatic.boolValue = EditorGUILayout.Toggle(new GUIContent("默认静止", "如果勾选该选项,运行时物件不会下落或者破碎,直到受到外力"), PropStartStatic.boolValue);
            //PropChunkConnectionMinArea.floatValue = EditorGUILayout.FloatField(new GUIContent("Interconnection Min Area", "Minimum area between 2 connected chunks to consider them connected. Setting it to zero won't consider all chunks connected, only those that share at least a common face area no matter how small."), PropChunkConnectionMinArea.floatValue);
            //PropChunkConnectionStrength.floatValue = EditorGUILayout.Slider(new GUIContent("Interconnection Strength", "When a chunk attached to the object is hit and detached, this controls how many connected chunks will detach too. 0.0 will make the whole object collapse, 1.0 won't detach any connected chunks."), PropChunkConnectionStrength.floatValue, 0.0f, 1.0f);
            EditorGUI.BeginChangeCheck();
            //PropChunkHorizontalRadiusSupportStrength.floatValue = EditorGUILayout.FloatField(new GUIContent("Support Hor. Strength", "Controls the maximum horizontal distance a chunk must be from a support chunk to stay attached to the object. If its distance is greater than this value, it will fall."), PropChunkHorizontalRadiusSupportStrength.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                bMarkNonSupportedChunks = true;
            }
            //GUI.enabled = PropGenerateChunkConnectionInfo.boolValue && //PropGenerateIslands.boolValue;
            //PropChunkIslandConnectionMaxDistance.floatValue = EditorGUILayout.FloatField(new GUIContent("Island Max Connect Dist.", "When feeding a source object, and Island Generation is active, it may detect multiple closed meshes inside and some of them may be connected to others. This controls how far a face from one island can be from another island to consider the two islands connected."), PropChunkIslandConnectionMaxDistance.floatValue);
            GUI.enabled = true;
            EditorGUI.BeginChangeCheck();
            PropTotalMass.floatValue = EditorGUILayout.FloatField(new GUIContent("总质量", "原物体的总质量,拆分后会根据破碎块的大小计算每个分块的质量,这个值的大小和受到的外力的大小会影响分块的运动轨迹"), PropTotalMass.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                fracturedComponent.ComputeChunksMass(PropTotalMass.floatValue);
            }
            PropChunkPhysicMaterial.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("物理材质", "每个小分块的物理材质是一致的"), PropChunkPhysicMaterial.objectReferenceValue, typeof(PhysicMaterial), true);
            //PropMinColliderVolumeForBox.floatValue = EditorGUILayout.FloatField(new GUIContent("Min Collider Volume", "Chunks with a volume less than this value will have a box collider instead of a mesh collider to speed up collisions."), PropMinColliderVolumeForBox.floatValue);
            //PropCapPrecisionFix.floatValue = EditorGUILayout.FloatField(new GUIContent("Cap Precision Fix (Adv.)", "Change this value from 0 only if you experience weird triangles added to the mesh or unexpected crashes! This usually happens on meshes with very thin faces due to floating point errors. A good range is usually 0.001 to 0.02, larger values will introduce small visible variations on some splits. If there's only 2 or 3 problematic faces, then an alternative would be using another random seed."), PropCapPrecisionFix.floatValue);
            //PropInvertCapNormals.boolValue = EditorGUILayout.Toggle(new GUIContent("Reverse Cap Normals", "Check this if for some reason the interior faces have reversed lighting."), PropInvertCapNormals.boolValue);

            EditorGUI.BeginChangeCheck();
            PropSplitMaterial.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("内面材质", "物件破碎后可以看到内表面,该材质是内表面的材质"), PropSplitMaterial.objectReferenceValue, typeof(Material), true);
            if (EditorGUI.EndChangeCheck())
            {
                bReassignSplitMaterial = true;
            }

            //PropSplitMappingTileU.floatValue = EditorGUILayout.FloatField(new GUIContent("Interior Mapping U Tile", "U Tiling of the interior faces mapping."), PropSplitMappingTileU.floatValue);
            //PropSplitMappingTileV.floatValue = EditorGUILayout.FloatField(new GUIContent("Interior Mapping V Tile", "V Tiling of the interior faces mapping."), PropSplitMappingTileV.floatValue);
        }

        EditorGUILayout.Space();
        fracturedComponent.GUIExpandSplits = true;// EditorGUILayout.Foldout(fracturedComponent.GUIExpandSplits, new GUIContent("Fracturing & Interior Material", "These parameters control the way the slicing will be performed."));

        if (fracturedComponent.GUIExpandSplits)
        {
            bool bProbabilityXChanged = false;
            bool bProbabilityYChanged = false;
            bool bProbabilityZChanged = false;
            float fProbabilityXBefore = PropSplitXProbability.floatValue;
            float fProbabilityYBefore = PropSplitYProbability.floatValue;
            float fProbabilityZBefore = PropSplitZProbability.floatValue;

            EditorGUILayout.PropertyField(PropFracturePattern, new GUIContent("破碎算法", ""));

            if (PropFracturePattern.enumNames[PropFracturePattern.enumValueIndex] == FracturedObject.EFracturePattern.Voronoi.ToString())
            {
                EditorGUILayout.PropertyField(PropVoronoiVolumeOptimization, new GUIContent("Use Volume Optimization", "Will compute fracturing faster when using large meshes or with huge empty space."));
                EditorGUILayout.PropertyField(PropVoronoiProximityOptimization, new GUIContent("Use Proximity Optimiza.", "Disable this if you find intersecting chunks."));
                EditorGUILayout.PropertyField(PropVoronoiMultithreading, new GUIContent("Use Multithreading", "Will use multithreading for some Voronoi computation steps. It isn't fully tested/optimized but it should work. Only disable if you experience any problems."));
                EditorGUILayout.PropertyField(PropVoronoiCellsXCount, new GUIContent("Cells In Local X", "Voronoi will generate X*Y*Z cells. This is number of cells to generate in the X dimension."));
                EditorGUILayout.PropertyField(PropVoronoiCellsYCount, new GUIContent("Cells In Local Y", "Voronoi will generate X*Y*Z cells. This is number of cells to generate in the Y dimension."));
                EditorGUILayout.PropertyField(PropVoronoiCellsZCount, new GUIContent("Cells In Local Z", "Voronoi will generate X*Y*Z cells. This is number of cells to generate in the Z dimension."));
                PropVoronoiCellsXSizeVariation.floatValue = EditorGUILayout.Slider(new GUIContent("X Cells Variation", "Greater values will increase difference in size and positioning of cells in the X dimension"), PropVoronoiCellsXSizeVariation.floatValue, 0.0f, 1.0f);
                PropVoronoiCellsYSizeVariation.floatValue = EditorGUILayout.Slider(new GUIContent("Y Cells Variation", "Greater values will increase difference in size and positioning of cells in the Y dimension"), PropVoronoiCellsYSizeVariation.floatValue, 0.0f, 1.0f);
                PropVoronoiCellsZSizeVariation.floatValue = EditorGUILayout.Slider(new GUIContent("Z Cells Variation", "Greater values will increase difference in size and positioning of cells in the Z dimension"), PropVoronoiCellsZSizeVariation.floatValue, 0.0f, 1.0f);
            }

            if (PropFracturePattern.enumNames[PropFracturePattern.enumValueIndex] == FracturedObject.EFracturePattern.BSP.ToString())
            {
                PropNumChunks.intValue = (int)EditorGUILayout.Slider(new GUIContent("分块总数", "破碎后的分块的总数"), PropNumChunks.intValue,1,200);
                //PropSplitsWorldSpace.boolValue = EditorGUILayout.Toggle(new GUIContent("Slice In World Space", "Controls if the slicing will be performed in local object space or in world space. Note that the original object orientation is considered, not the fractured object."), PropSplitsWorldSpace.boolValue);
                PropSplitRegularly.boolValue = EditorGUILayout.Toggle(new GUIContent("均匀分解", "是否在XYZ三个方向进行均匀分解,即三个方向的分块数量相差不大"), PropSplitRegularly.boolValue);

                GUI.enabled = PropSplitRegularly.boolValue == false;
                EditorGUI.BeginChangeCheck();
                PropSplitXProbability.floatValue = EditorGUILayout.Slider(new GUIContent("X分割", "X方向的分解能力,值越大分解的块数越多"), PropSplitXProbability.floatValue, 0.0f, 1.0f);
                if (EditorGUI.EndChangeCheck()) bProbabilityXChanged = true;
                EditorGUI.BeginChangeCheck();
                PropSplitYProbability.floatValue = EditorGUILayout.Slider(new GUIContent("Y分割", "Y方向的分解能力,值越大分解的块数越多"), PropSplitYProbability.floatValue, 0.0f, 1.0f);
                if (EditorGUI.EndChangeCheck()) bProbabilityYChanged = true;
                EditorGUI.BeginChangeCheck();
                PropSplitZProbability.floatValue = EditorGUILayout.Slider(new GUIContent("Z分割", "Z方向的分解能力,值越大分解的块数越多"), PropSplitZProbability.floatValue, 0.0f, 1.0f);
                if (EditorGUI.EndChangeCheck()) bProbabilityZChanged = true;
                GUI.enabled = true;

                PropSplitSizeVariation.floatValue = EditorGUILayout.Slider(new GUIContent("分块大小", "值越小,每个分块的大小越接近"), PropSplitSizeVariation.floatValue, 0.0f, 1.0f);
                PropSplitXVariation.floatValue = EditorGUILayout.Slider(new GUIContent("X角度", "X方向切割后的切割面的最大倾斜程度"), PropSplitXVariation.floatValue, 0.0f, 1.0f);
                PropSplitYVariation.floatValue = EditorGUILayout.Slider(new GUIContent("Y角度", "Y方向切割后的切割面的最大倾斜程度"), PropSplitYVariation.floatValue, 0.0f, 1.0f);
                PropSplitZVariation.floatValue = EditorGUILayout.Slider(new GUIContent("Z角度", "Z方向切割后的切割面的最大倾斜程度"), PropSplitZVariation.floatValue, 0.0f, 1.0f);
            }

            EditorGUILayout.Space();

            float fNewSplitXProbability = PropSplitXProbability.floatValue;
            float fNewSplitYProbability = PropSplitYProbability.floatValue;
            float fNewSplitZProbability = PropSplitZProbability.floatValue;

            if (bProbabilityXChanged) ChangeProbability(PropSplitXProbability.floatValue, fProbabilityXBefore, ref fNewSplitYProbability, ref fNewSplitZProbability);
            if (bProbabilityYChanged) ChangeProbability(PropSplitYProbability.floatValue, fProbabilityYBefore, ref fNewSplitXProbability, ref fNewSplitZProbability);
            if (bProbabilityZChanged) ChangeProbability(PropSplitZProbability.floatValue, fProbabilityZBefore, ref fNewSplitXProbability, ref fNewSplitYProbability);

            PropSplitXProbability.floatValue = fNewSplitXProbability;
            PropSplitYProbability.floatValue = fNewSplitYProbability;
            PropSplitZProbability.floatValue = fNewSplitZProbability;
        }

        EditorGUILayout.Space();
        //fracturedComponent.GUIExpandEvents = EditorGUILayout.Foldout(fracturedComponent.GUIExpandEvents, new GUIContent("Events", "These parameters control the behavior of the object on some events."));

        //if (fracturedComponent.GUIExpandEvents)
        //{
        //    EditorGUILayout.LabelField("Chunk Detach From Object Due To Physics Collision:");
        //    EditorGUI.indentLevel += nIndentationJump;
        //    EditorGUILayout.PropertyField(PropEventDetachMinMass, new GUIContent("Min Impact Mass", "The minimum mass an object needs to have to detach a chunk from this object on impact."));
        //    EditorGUILayout.PropertyField(PropEventDetachMinVelocity, new GUIContent("Min Impact Velocity", "The minimum velocity an object needs to impact with to detach a chunk from this object."));
        //    EditorGUILayout.PropertyField(PropEventDetachExitForce, new GUIContent("Exit Force", "If a chunk is detached due to an impact, it will have this value applied to it."));
        //    EditorGUILayout.PropertyField(PropEventDetachUpwardsModifier, new GUIContent("Upwards Modifier", "Adds an upwards explosion effect to the chunks that have exit force. A value of 0.0 won't add any effect, while 2.0 means it will apply the force from a distance of 2 below the chunk."));
              EditorGUILayout.PropertyField(PropEventDetachSound, new GUIContent("破碎音效", "Will play this sound on the collision point when a chunk is detached due to an impact."));
        //    EditorGUILayout.PropertyField(PropEventDetachPrefabsArray, new GUIContent("Instance Prefab List (1 Randomly Spawned On Detach)", "A list of prefabs. When a chunk is detached due to an impact, a random prefab will be picked from this list and instanced on the collision point. Use this for particles/explosions."), true);
        //    EditorGUILayout.PropertyField(PropEventDetachCollisionCallMethod, new GUIContent("Call Method Name", "The method name that will be called on an impact-triggered detach chunk event."));
        //    EditorGUILayout.PropertyField(PropEventDetachCollisionCallGameObject, new GUIContent("Call GameObject", "The GameObject whose method will be called."));
        //    EditorGUI.indentLevel -= nIndentationJump;
        //    EditorGUILayout.LabelField("Free (Detached) Chunks:");
        //    EditorGUI.indentLevel += nIndentationJump;
              EditorGUILayout.PropertyField(PropEventDetachedMinLifeTime, new GUIContent("最小生存时间", "The minimum lifetime of a free chunk. When the life of a chunk expires it will be deleted."));
              EditorGUILayout.PropertyField(PropEventDetachedMaxLifeTime, new GUIContent("最大生存时间", "The maximum lifetime of a free chunk. When the life of a chunk expires it will be deleted."));
        //    EditorGUILayout.PropertyField(PropEventDetachedOffscreenLifeTime, new GUIContent("Offscreen LifeTime", "If a free chunk is outside the visible screen for more than this seconds, it will be deleted."));
        //    EditorGUILayout.PropertyField(PropEventDetachedMinMass, new GUIContent("Min Impact Mass", "The minimum mass a free chunk need to impact with in order to trigger a collision event."));
        //    EditorGUILayout.PropertyField(PropEventDetachedMinVelocity, new GUIContent("Min Impact Velocity", "The minimum velocity a free chunk need to impact with in order to trigger a collision event."));
        //    EditorGUILayout.PropertyField(PropEventDetachedMaxSounds, new GUIContent("Max Simult. Sounds", "The maximum collision sounds that will be played at the same time."));
        //    EditorGUILayout.PropertyField(PropEventDetachedSoundArray, new GUIContent("Collision Sound List (1 Randomly Played On Collision)", "A list of sounds. On a free chunk collision a random sound will be picked from this list and played on the collision point."), true);
        //    EditorGUILayout.PropertyField(PropEventDetachedMaxPrefabs, new GUIContent("Max Simult. Prefabs", "The maximum number of collision prefabs present at the same time."));
        //    EditorGUILayout.PropertyField(PropEventDetachedPrefabsArray, new GUIContent("Collision Prefab List (1 Randomly Spawned On Collision)", "A list of prefabs. On a free chunk collision a random prefab will be picked from this list and instanced on the collision point. Use this for particles/explosions."), true);
        //    EditorGUILayout.PropertyField(PropEventDetachedCollisionCallMethod, new GUIContent("Call Method Name", "The method name that will be called on a free chunk collision."));
        //    EditorGUILayout.PropertyField(PropEventDetachedCollisionCallGameObject, new GUIContent("Call GameObject", "The GameObject whose method will be called."));
        //    EditorGUI.indentLevel -= nIndentationJump;
        //    EditorGUILayout.LabelField("When Explode() Is Called Through Scripting (Explosions):");
        //    EditorGUI.indentLevel += nIndentationJump;
        //    EditorGUILayout.PropertyField(PropEventExplosionSound, new GUIContent("Explosion Sound", "The sound that will be played when Explode() is called on this object."));
        //    EditorGUILayout.PropertyField(PropEventExplosionPrefabInstanceCount, new GUIContent("Random Prefabs", "The number of prefabs to instance on random positions of the object."));
        //    EditorGUILayout.PropertyField(PropEventExplosionPrefabsArray, new GUIContent("Instance Prefab List (Spawned Randomly Around)", "A list of prefabs. When Explode() is called a random number of them will be instanced on random positions of the object. Use this for particles/explosions."), true);
        //    EditorGUI.indentLevel -= nIndentationJump;
        //    EditorGUILayout.LabelField("When Impact() Is Called Through Scripting (f.e. Missiles):");
        //    EditorGUI.indentLevel += nIndentationJump;
              EditorGUILayout.PropertyField(PropEventImpactSound, new GUIContent("受力音效", "The sound that will be played when Impact() is called on this object."));
        //    EditorGUILayout.PropertyField(PropEventImpactPrefabsArray, new GUIContent("Impact Prefab List (1 Randomly Spawned On Impact)", "A list of prefabs. When Impact() is called a random prefab will be instanced on the impact point. Use this for particles/explosions."), true);
        //    EditorGUI.indentLevel -= nIndentationJump;
        //}

        EditorGUILayout.Space();
        fracturedComponent.GUIExpandSupportPlanes = EditorGUILayout.Foldout(fracturedComponent.GUIExpandSupportPlanes, new GUIContent("支撑平面", "支撑平面用于控制物件破碎时是否整体全部破碎。支撑平面以下的永远不会破碎,以上的会在受力时根据支撑程度破碎,支撑平面以外的只要受力就会全部破碎"));

        if (fracturedComponent.GUIExpandSupportPlanes)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(" ");

            if (GUILayout.Button(new GUIContent("添加支撑平面", "Adds a new support plane to this object. Support planes control which chunks are tagged as support. Chunks that act as support can't be destroyed and will hold the object together. A chunk needs to be connected to a support chunk (directly or through other chunks) or otherwise it will fall. This prevents chunks from staying static in the air and enables realistic collapsing behavior."), GUILayout.Width(200)))
            {
                fracturedComponent.AddSupportPlane();
                fracturedComponent.ComputeSupportPlaneIntersections();
                fracturedComponent.MarkNonSupportedChunks();
            }

            GUILayout.Label(" ");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            int nDeletePlane = -1;
            int nPlaneIndex = 0;

            EditorGUI.indentLevel += nIndentationJump;

            foreach (UltimateFracturing.SupportPlane supportPlane in fracturedComponent.ListSupportPlanes)
            {
                supportPlane.GUIExpanded = EditorGUILayout.Foldout(supportPlane.GUIExpanded, new GUIContent(supportPlane.GUIName, "Support plane parameters."));

                if (supportPlane.GUIExpanded)
                {
                    EditorGUI.indentLevel += nIndentationJump;

                    EditorGUILayout.Space();
                    EditorGUI.BeginChangeCheck();

                    supportPlane.GUIShowInScene = EditorGUILayout.Toggle(new GUIContent("Show In Scene", "Controls if the plane is drawn or not in the scene view."), supportPlane.GUIShowInScene);
                    supportPlane.GUIName = EditorGUILayout.TextField(new GUIContent("Plane Name", "The name that will be shown on the scene view."), supportPlane.GUIName);
                    supportPlane.v3PlanePosition = EditorGUILayout.Vector3Field("Local Position", supportPlane.v3PlanePosition);
                    supportPlane.qPlaneRotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Local Rotation", supportPlane.qPlaneRotation.eulerAngles));
                    supportPlane.v3PlaneScale = EditorGUILayout.Vector3Field("Local Scale", supportPlane.v3PlaneScale);

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    if (GUILayout.Button(new GUIContent("Delete", "Deletes this support plane."), GUILayout.Width(100)))
                    {
                        nDeletePlane = nPlaneIndex;
                        bRecomputePlanes = true;
                    }
                    GUILayout.Label(" ");
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        bRecomputePlanes = true;
                    }

                    EditorGUI.indentLevel -= nIndentationJump;
                }

                nPlaneIndex++;
            }

            EditorGUI.indentLevel -= nIndentationJump;

            if (nDeletePlane != -1)
            {
                fracturedComponent.ListSupportPlanes.RemoveAt(nDeletePlane);
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        PropRandomSeed.intValue = EditorGUILayout.IntField(new GUIContent("随机种子", "用于控制分块生成的随机性,种子一致那么生成的分块也是一致的"), PropRandomSeed.intValue);
        if (GUILayout.Button(new GUIContent("New Seed"), GUILayout.Width(100)))
        {
            PropRandomSeed.intValue = Mathf.RoundToInt(Random.value * 1000000.0f);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        PropDecomposePreview.floatValue = EditorGUILayout.Slider(new GUIContent("预览分块", "Use this slider to preview the chunks that were generated."), PropDecomposePreview.floatValue, 0.0f, fracturedComponent.DecomposeRadius);
        if (EditorGUI.EndChangeCheck())
        {
            bDecomposePreviewUpdate = true;
        }

        //PropAlwaysComputeColliders.boolValue = EditorGUILayout.Toggle(new GUIContent("Always Gen. Colliders", "Will also generate colliders each time the chunks are computed."), PropAlwaysComputeColliders.boolValue);
        //PropShowChunkConnectionLines.boolValue = EditorGUILayout.Toggle(new GUIContent("Show Chunk Connections", "Will draw lines on the scene view to show how chunks are connected between each other."), PropShowChunkConnectionLines.boolValue);
        //PropShowChunkColoredState.boolValue = EditorGUILayout.Toggle(new GUIContent("Color Chunk State", "Will color chunks in the scene view to show which ones are support chunks and which ones aren't connected to any support chunks directly or indirectly and will fall down when checking for structure integrity."), PropShowChunkColoredState.boolValue);
        //PropShowChunkColoredRandomly.boolValue = EditorGUILayout.Toggle(new GUIContent("Color Chunks", "Will color chunks randomly in the scene window to see them better."), PropShowChunkColoredRandomly.boolValue);

        EditorGUI.BeginChangeCheck();
        PropSaveMeshDataToAsset.boolValue = EditorGUILayout.Toggle(new GUIContent("Save Mesh Data To Asset", "Will save the chunk and collider meshes to an asset file on disk when they are computed. Use this if you want to add this object to a prefab, otherwise the meshes and colliders won't be instanced properly."), PropSaveMeshDataToAsset.boolValue);
        if (EditorGUI.EndChangeCheck())
        {
            if (PropSaveMeshDataToAsset.boolValue)
            {
                if (System.IO.File.Exists(fracturedComponent.MeshAssetDataFile) == false)
                {
                    PropMeshAssetDataFile.stringValue = UnityEditor.EditorUtility.SaveFilePanelInProject("Save mesh asset", "mesh_" + fracturedComponent.name + this.GetInstanceID().ToString() + ".asset", "asset", "Please enter a file name to save the mesh asset to");
                }

                if (PropMeshAssetDataFile.stringValue.Length == 0)
                {
                    PropSaveMeshDataToAsset.boolValue = false;
                }
            }
        }

        //PropVerbose.boolValue = EditorGUILayout.Toggle(new GUIContent("Output Console Info", "Outputs messages and warnings to the console window."), PropVerbose.boolValue);

        // Fracture?

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        GUI.enabled = PropSourceObject.objectReferenceValue != null;
        bComputeFracturing = GUILayout.Button(new GUIContent("Compute Chunks", "Computes the fractured chunks"));
        GUI.enabled = true;

        bool bHasChunks = fracturedComponent.ListFracturedChunks.Count > 0;

        GUI.enabled = bHasChunks;
        if (GUILayout.Button(new GUIContent("Delete Chunks", "Deletes the fractured chunks")))
        {
            Undo.RegisterSceneUndo("Delete Chunks");
            fracturedComponent.DeleteChunks();
        }
        GUI.enabled = true;

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        if(GUILayout.Button("ExportXml",GUILayout.Height(30)))
        {
            ExportXml();
        }
        if (GUILayout.Button("ImportXml", GUILayout.Height(30)))
        {
            ImportXml();
        }

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        PropIntegrateWithConcaveCollider.boolValue = EditorGUILayout.Toggle(new GUIContent("Use Concave Collider", "Use the external Concave Collider utility to have more control over how mesh colliders are generated."), PropIntegrateWithConcaveCollider.boolValue);
        if (EditorGUI.EndChangeCheck())
        {
            if (PropIntegrateWithConcaveCollider.boolValue)
            {
                if (System.IO.File.Exists("Assets/Plugins/x86_64/ConvexDecompositionDll.dll") == false)
                {
                    if (EditorUtility.DisplayDialog("Concave Collider not found", "Concave Collider is a utility for Unity that allows the automatic generation of compound colliders for dynamic objects.\n\nThe Ultimate Fracturing tool can use it to avoid generating hulls bigger than 255 triangles. It will also allow to specify the maximum number of vertices to generate for each collider to optimize collision calculations.\n\nNote: The Concave Collider requires a PRO license of Unity3D.", "Show Asset", "Cancel"))
                    {
                        Application.OpenURL("https://www.assetstore.unity3d.com/#/content/4596");
                    }

                    PropIntegrateWithConcaveCollider.boolValue = false;
                }
            }
        }

        GUI.enabled = PropIntegrateWithConcaveCollider.boolValue;
        PropConcaveColliderMaxHullVertices.intValue = EditorGUILayout.IntSlider(new GUIContent("Max Collider Vertices", "Limits the maximum vertices a collider hull can have."), PropConcaveColliderMaxHullVertices.intValue, 4, 1024);
        GUI.enabled = true;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUI.enabled = fracturedComponent.transform.GetChildCount() > 0;

        if (GUILayout.Button(new GUIContent("Compute Colliders", "Computes the chunk colliders.")))
        {
            bComputeColliders = true;
        }

        if (GUILayout.Button(new GUIContent("Delete Colliders", "Deletes the chunk colliders")))
        {
            bDeleteColliders = true;
        }

        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        // Apply changes to the serializedProperty

        serializedObject.ApplyModifiedProperties();

        // Perform actions

        bool bProgressBarCreated = false;
        m_bProgressCancelled = false;

        if (bComputeFracturing)
        {
            bProgressBarCreated = true;

            GameObject goSource = PropSourceObject.objectReferenceValue as GameObject;
            if (goSource.GetComponent<MeshFilter>() == null || goSource.transform.childCount != 0)
            {
                EditorUtility.DisplayDialog("Error", "Source object has no mesh assigned,please combine target first", "OK");
            }
            else
            {
                bool bPositionOnSourceAndHideOriginal = false;

                Undo.RegisterSceneUndo("Compute Chunks");

#if UNITY_4_0
				bool bIsActive = goSource.activeSelf;
#else
                bool bIsActive = goSource.active;
#endif

                if (bIsActive && bHasChunks == false)
                {
                    if (EditorUtility.DisplayDialog("Hide old and position new?", "Do you want to hide the original object and place the new fractured object on its position?", "Yes", "No"))
                    {
                        bPositionOnSourceAndHideOriginal = true;
                    }
                }

                List<GameObject> listGameObjects;

                bool bError = false;

                float fStartTime = Time.realtimeSinceStartup;

                try
                {
                    fracturedComponent.GenerateIslands = false;
                    UltimateFracturing.Fracturer.FractureToChunks(fracturedComponent, bPositionOnSourceAndHideOriginal, out listGameObjects, Progress);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(string.Format("Exception computing chunks ({0}):\n{1}", e.Message, e.StackTrace));
                    bError = true;
                }

                float fEndTime = Time.realtimeSinceStartup;

                EditorUtility.ClearProgressBar();

                if (bError == false && m_bProgressCancelled == false)
                {
                    if (fracturedComponent.Verbose)
                    {
                        Debug.Log("Compute time = " + (fEndTime - fStartTime) + "seconds");
                    }
                }

                if (m_bProgressCancelled)
                {
                    fracturedComponent.DeleteChunks();
                }
            }
        }

        if (bComputeColliders && m_bProgressCancelled == false)
        {
            Undo.RegisterSceneUndo("Compute Colliders");
            bProgressBarCreated = true;
            UltimateFracturing.Fracturer.ComputeChunkColliders(fracturedComponent, Progress);
        }

        if (bDeleteColliders)
        {
            Undo.RegisterSceneUndo("Delete Colliders");
            UltimateFracturing.Fracturer.DeleteChunkColliders(fracturedComponent);
        }

        foreach (FracturedChunk fracturedChunk in fracturedComponent.ListFracturedChunks)
        {
            if (fracturedChunk != null)
            {
                if (bDecomposePreviewUpdate || bComputeFracturing)
                {
                    fracturedChunk.PreviewDecompositionValue = PropDecomposePreview.floatValue;
                    fracturedChunk.UpdatePreviewDecompositionPosition();
                }

                if (bReassignSplitMaterial)
                {
                    Undo.RegisterUndo(fracturedChunk.GetComponent<Renderer>(), "Assign Inside Material");

                    if (fracturedChunk.SplitSubMeshIndex != -1)
                    {
                        Material[] aMaterials = new Material[fracturedChunk.GetComponent<Renderer>().sharedMaterials.Length];

                        for (int nMaterial = 0; nMaterial < aMaterials.Length; nMaterial++)
                        {
                            aMaterials[nMaterial] = nMaterial == fracturedChunk.SplitSubMeshIndex ? PropSplitMaterial.objectReferenceValue as Material : fracturedChunk.GetComponent<Renderer>().sharedMaterials[nMaterial];
                        }

                        fracturedChunk.GetComponent<Renderer>().sharedMaterials = aMaterials;
                    }
                }
            }
        }

        if (bRecomputePlanes)
        {
            SceneView.RepaintAll();
            fracturedComponent.ComputeSupportPlaneIntersections();
        }

        if (bMarkNonSupportedChunks || bComputeFracturing || bRecomputePlanes)
        {
            fracturedComponent.MarkNonSupportedChunks();
        }

        if (fracturedComponent.SaveMeshDataToAsset && (bComputeFracturing || bComputeColliders) && (m_bProgressCancelled == false))
        {
            bProgressBarCreated = true;

            if (fracturedComponent.MeshAssetDataFile.Length > 0)
            {
                UnityEditor.AssetDatabase.DeleteAsset(fracturedComponent.MeshAssetDataFile);

                bool bFirstAdded = false;

                for (int nChunk = 0; nChunk < fracturedComponent.ListFracturedChunks.Count; nChunk++)
                {
                    FracturedChunk chunk = fracturedComponent.ListFracturedChunks[nChunk];

                    Progress("Saving mesh assets to disk", string.Format("Chunk {0}/{1}", nChunk + 1, fracturedComponent.ListFracturedChunks.Count), (float)nChunk / (float)fracturedComponent.ListFracturedChunks.Count);

                    MeshFilter meshFilter = chunk.GetComponent<MeshFilter>();

                    // Save mesh

                    if (bFirstAdded == false && meshFilter != null)
                    {
                        UnityEditor.AssetDatabase.CreateAsset(meshFilter.sharedMesh, fracturedComponent.MeshAssetDataFile);
                        bFirstAdded = true;
                    }
                    else if (meshFilter != null)
                    {
                        UnityEditor.AssetDatabase.AddObjectToAsset(meshFilter.sharedMesh, fracturedComponent.MeshAssetDataFile);
                        UnityEditor.AssetDatabase.ImportAsset(UnityEditor.AssetDatabase.GetAssetPath(meshFilter.sharedMesh));
                    }

                    // Save collider mesh if it was generated by the concave collider

                    if (chunk.HasConcaveCollider)
                    {
                        MeshCollider meshCollider = chunk.GetComponent<MeshCollider>();

                        if (bFirstAdded == false && meshCollider != null)
                        {
                            UnityEditor.AssetDatabase.CreateAsset(meshCollider.sharedMesh, fracturedComponent.MeshAssetDataFile);
                            bFirstAdded = true;
                        }
                        else if (meshCollider != null)
                        {
                            UnityEditor.AssetDatabase.AddObjectToAsset(meshCollider.sharedMesh, fracturedComponent.MeshAssetDataFile);
                            UnityEditor.AssetDatabase.ImportAsset(UnityEditor.AssetDatabase.GetAssetPath(meshCollider.sharedMesh));
                        }
                    }
                }
            }

            UnityEditor.AssetDatabase.Refresh();
        }

        if (bProgressBarCreated)
        {
            EditorUtility.ClearProgressBar();
        }
        fracturedComponent.mForce = EditorGUILayout.Vector3Field("Force", fracturedComponent.mForce);
        fracturedComponent.mRadius = EditorGUILayout.FloatField("Radius", fracturedComponent.mRadius);
        if (GUILayout.Button("Destroy In EditorMode",GUILayout.Height(30)))
        {
            if(Application.isPlaying && fracturedComponent.transform.childCount > 0)
            {
                FracturedChunk chunk = fracturedComponent.transform.GetChild(0).GetComponent<FracturedChunk>();
                if(chunk != null)
                    chunk.Impact(fracturedComponent.mForce, chunk.transform.position, fracturedComponent.mRadius);
            }
        }
    }

    void ChangeProbability(float fNewValue, float fOldValue, ref float fOtherValue1Ref, ref float fOtherValue2Ref)
    {
        float fChange = fNewValue - fOldValue;

        if (Mathf.Approximately(fOtherValue1Ref, 0.0f) && Mathf.Approximately(fOtherValue2Ref, 0.0f) == false)
        {
            fOtherValue1Ref = 0.0f;
            fOtherValue2Ref = 1.0f - fNewValue;
        }
        else if (Mathf.Approximately(fOtherValue2Ref, 0.0f) && Mathf.Approximately(fOtherValue1Ref, 0.0f) == false)
        {
            fOtherValue1Ref = 1.0f - fNewValue;
            fOtherValue2Ref = 0.0f;
        }
        else
        {
            fOtherValue1Ref = Mathf.Clamp01(fOtherValue1Ref - (fChange * 0.5f));
            fOtherValue2Ref = 1.0f - (fNewValue + fOtherValue1Ref);
        }
    }

    void ExportXml()
    {
        string filePrefix = "FractureInfo";
        if (PropSourceObject.objectReferenceValue == null)
        {
            EditorUtility.DisplayDialog("ERROR", "需要先选择要进行处理的目标物体", "OK");
            return;
        }
        string xmlPath = System.IO.Path.GetFullPath(Application.dataPath + "/../../../../../doc/Product_Doc/90_数据表格/XML");
        string filePath = EditorUtility.SaveFilePanel("导出XML配置", xmlPath, filePrefix + "_" + PropSourceObject.objectReferenceValue.name, "xml");
        if (!string.IsNullOrEmpty(filePath))
        {
            if (!System.IO.Path.GetFileNameWithoutExtension(filePath).StartsWith(filePrefix + "_"))
            {
                EditorUtility.DisplayDialog("ERROR", "导出的XML前缀必须是" + filePrefix + "_", "OK");
                return;
            }
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            System.IO.StreamWriter fs = new System.IO.StreamWriter(filePath);
            fs.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            fs.WriteLine("<FractureDatas" + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            fs.WriteLine("\t<FractureData>");
            fs.WriteLine("\t\t<fileName>" + System.IO.Path.GetFileNameWithoutExtension(filePath) + "</fileName>");
            fs.WriteLine("\t\t<totalMass>" + PropTotalMass.floatValue.ToString() + "</totalMass>");

            if(PropSplitMaterial.objectReferenceValue != null)
                fs.WriteLine("\t\t<splitMaterial>" +  PropSplitMaterial.objectReferenceValue.name + "</splitMaterial>");

            fs.WriteLine("\t\t<generateNumChunks>" + PropNumChunks.intValue.ToString() + "</generateNumChunks>");
            fs.WriteLine("\t\t<splitRegularly>" + PropSplitRegularly.boolValue.ToString() + "</splitRegularly>");
            fs.WriteLine("\t\t<splitXProbability>" + PropSplitXProbability.floatValue.ToString() + "</splitXProbability>");
            fs.WriteLine("\t\t<splitYProbability>" + PropSplitYProbability.floatValue.ToString() + "</splitYProbability>");
            fs.WriteLine("\t\t<splitZProbability>" + PropSplitZProbability.floatValue.ToString() + "</splitZProbability>");

            fs.WriteLine("\t\t<splitSizeVariation>" + PropSplitSizeVariation.floatValue.ToString() + "</splitSizeVariation>");
            fs.WriteLine("\t\t<splitXVariation>" + PropSplitXVariation.floatValue.ToString() + "</splitXVariation>");
            fs.WriteLine("\t\t<splitYVariation>" + PropSplitYVariation.floatValue.ToString() + "</splitYVariation>");
            fs.WriteLine("\t\t<splitZVariation>" + PropSplitZVariation.floatValue.ToString() + "</splitZVariation>");

            fs.WriteLine("\t\t<eventDetachedMinLifeTime>" + PropEventDetachedMinLifeTime.floatValue.ToString() + "</eventDetachedMinLifeTime>");
            fs.WriteLine("\t\t<eventDetachedMaxLifeTime>" + PropEventDetachedMaxLifeTime.floatValue.ToString() + "</eventDetachedMaxLifeTime>");

            fs.WriteLine("\t\t<randomSeed>" + PropRandomSeed.intValue.ToString() + "</randomSeed>");

            SimpleFractureObject fracturedComponent = serializedObject.targetObject as SimpleFractureObject;
            for (int i = 0; i < fracturedComponent.ListSupportPlanes.Count; i++)
            {
                UltimateFracturing.SupportPlane supportPlane = fracturedComponent.ListSupportPlanes[i];
                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlaneNames>" + supportPlane.GUIName + "</supportPlanes-" + i.ToString() + ".supportPlaneNames>");

                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlanePossX>" + supportPlane.v3PlanePosition.x.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlanePossX>");
                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlanePossY>" + supportPlane.v3PlanePosition.y.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlanePossY>");
                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlanePossZ>" + supportPlane.v3PlanePosition.z.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlanePossZ>");

                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlaneQuaternionsX>" + supportPlane.qPlaneRotation.x.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlaneQuaternionsX>");
                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlaneQuaternionsY>" + supportPlane.qPlaneRotation.y.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlaneQuaternionsY>");
                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlaneQuaternionsZ>" + supportPlane.qPlaneRotation.z.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlaneQuaternionsZ>");
                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlaneQuaternionsW>" + supportPlane.qPlaneRotation.w.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlaneQuaternionsW>");

                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlaneScalesX>" + supportPlane.v3PlaneScale.x.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlaneScalesX>");
                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlaneScalesY>" + supportPlane.v3PlaneScale.y.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlaneScalesY>");
                fs.WriteLine("\t\t<supportPlanes-" + i.ToString() + ".supportPlaneScalesZ>" + supportPlane.v3PlaneScale.z.ToString() + "</supportPlanes-" + i.ToString() + ".supportPlaneScalesZ>");
            }

            fs.WriteLine("\t</FractureData>");
            fs.WriteLine("</FractureDatas>");

            fs.Close();

            string targetFilePath = xmlPath + "/" + filePrefix + ".xml";
            if (!System.IO.File.Exists(targetFilePath))
            {
                System.IO.File.Create(targetFilePath).Close();
            }
            string[] files = System.IO.Directory.GetFiles(xmlPath, filePrefix + "_*.xml");
            string merge = filePrefix;
            for(int i = 0;i < files.Length;i++)
            {
                merge += ";" + System.IO.Path.GetFileNameWithoutExtension(files[i]);
            }

            string tool_path = Application.dataPath + "/../../../../../src/tools/ProtoGen/export.py";
            System.Diagnostics.Process.Start(tool_path, xmlPath + " " + merge);
        }
    }

    void ImportXml()
    {
        if(PropSourceObject.objectReferenceValue == null)
        {
            EditorUtility.DisplayDialog("ERROR", "需要先选择要进行处理的目标物体", "OK");
            return;
        }
        string xmlPath = System.IO.Path.GetFullPath(Application.dataPath + "/../../../../../doc/Product_Doc/90_数据表格/XML/" + PropSourceObject.objectReferenceValue.name);
        string filePath = EditorUtility.OpenFilePanel("读取XML配置", xmlPath,"xml");
        if (!string.IsNullOrEmpty(filePath))
        {
            System.IO.StreamReader fs = new System.IO.StreamReader(filePath);
            string[] strs = fs.ReadToEnd().Split('\n');
            fs.Close();
            List<string[]> fields = new List<string[]>(); 
            for(int i = 0;i<strs.Length;i++)
            {
                string[] tmp = strs[i].Split(new char[] { '<','>' });
                if(tmp.Length > 1)
                {
                    fields.Add(tmp);
                }
            }

//             if(!string.Equals(PropSourceObject.objectReferenceValue.name, GetXmlField(fields,"fileName")))
//             {
//                 EditorUtility.DisplayDialog("", "filename must equal to source object name", "OK");
//             }
//             else
            {
                PropTotalMass.floatValue = float.Parse(GetXmlField(fields, "totalMass"));

                string inerMatName = GetXmlField(fields, "splitMaterial");
                if(!string.IsNullOrEmpty(inerMatName))
                {
                    string[] files = System.IO.Directory.GetFiles(Application.dataPath, inerMatName + ".mat", System.IO.SearchOption.AllDirectories);

                    if (files.Length != 0)
                    {
                        Object obj = AssetDatabase.LoadAssetAtPath<Material>(files[0].Replace(Application.dataPath,"Assets"));
                        PropSplitMaterial.objectReferenceValue = obj;
                    }
                }

                PropNumChunks.intValue = int.Parse(GetXmlField(fields, "generateNumChunks"));
                PropSplitRegularly.boolValue = GetXmlField(fields, "splitRegularly") == "true";
                PropSplitXProbability.floatValue = float.Parse(GetXmlField(fields, "splitXProbability"));
                PropSplitYProbability.floatValue = float.Parse(GetXmlField(fields, "splitYProbability"));
                PropSplitZProbability.floatValue = float.Parse(GetXmlField(fields, "splitZProbability"));

                PropSplitSizeVariation.floatValue = float.Parse(GetXmlField(fields, "splitSizeVariation"));
                PropSplitXVariation.floatValue = float.Parse(GetXmlField(fields, "splitXVariation"));
                PropSplitYVariation.floatValue = float.Parse(GetXmlField(fields, "splitYVariation"));
                PropSplitZVariation.floatValue = float.Parse(GetXmlField(fields, "splitZVariation"));

                PropEventDetachedMinLifeTime.floatValue = float.Parse(GetXmlField(fields, "eventDetachedMinLifeTime"));
                PropEventDetachedMaxLifeTime.floatValue = float.Parse(GetXmlField(fields, "eventDetachedMaxLifeTime"));

                PropRandomSeed.intValue = int.Parse(GetXmlField(fields, "randomSeed"));

                int i = 0;
                SimpleFractureObject fracturedComponent = serializedObject.targetObject as SimpleFractureObject;
                while (true)
                {
                    string guiName = GetXmlField(fields, string.Format("supportPlanes-{0}.supportPlaneNames", i));
                    if (string.IsNullOrEmpty(guiName))
                    {
                        break;
                    }
                    else
                    {
                        fracturedComponent.AddSupportPlane();
                        UltimateFracturing.SupportPlane supportPlane = fracturedComponent.ListSupportPlanes[i];
                        supportPlane.GUIName = guiName;
                        supportPlane.v3PlanePosition = new Vector3(float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlanePossX" + i.ToString())),
                                                                   float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlanePossY" + i.ToString())),
                                                                   float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlanePossZ" + i.ToString())));
                        supportPlane.qPlaneRotation = new Quaternion(float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlaneQuaternionsX" + i.ToString())),
                                                                     float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlaneQuaternionsY" + i.ToString())),
                                                                     float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlaneQuaternionsZ" + i.ToString())),
                                                                     float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlaneQuaternionsW" + i.ToString())));
                        supportPlane.v3PlaneScale = new Vector3(float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlaneScalesX" + i.ToString())),
                                                                float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlaneScalesY" + i.ToString())),
                                                                float.Parse(GetXmlField(fields, "supportPlanes-{0}.supportPlaneScalesZ" + i.ToString())));
                    }
                    i++;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

    string GetXmlField(List<string[]> fields, string field)
    {
        for(int i = 0;i < fields.Count;i++)
        {
            for(int j = 0; j < fields[i].Length;j++)
            {
                if(fields[i][j].Equals(field))
                {
                    return fields[i][j + 1];
                }
            }
        }
        return string.Empty;
    }
}
