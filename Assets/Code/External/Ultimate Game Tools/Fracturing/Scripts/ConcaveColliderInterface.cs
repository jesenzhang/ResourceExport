using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace UltimateFracturing
{
    public static class ConcaveColliderInterface
    {
        [StructLayout(LayoutKind.Sequential)]
        struct SConvexDecompositionInfoInOut
        {
       	    public uint     uMaxHullVertices;
	        public uint     uMaxHulls;
	        public float    fPrecision;
            public float    fBackFaceDistanceFactor;
            public uint     uLegacyDepth;
            public uint     uNormalizeInputMesh;
            public uint     uUseFastVersion;

            public uint     uTriangleCount;
            public uint     uVertexCount;

	        // Out parameters

	        public int      nHullsOut;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SConvexDecompositionHullInfo
        {
	        public int      nVertexCount;
	        public int      nTriangleCount;
        };

        [DllImport("ConvexDecompositionDll")]
        private static extern void DllInit(bool bUseMultithreading);

        [DllImport("ConvexDecompositionDll")]
        private static extern void DllClose();

        [DllImport("ConvexDecompositionDll")]
        private static extern void SetLogFunctionPointer(IntPtr pfnUnity3DLog);

        [DllImport("ConvexDecompositionDll")]
        private static extern void SetProgressFunctionPointer(IntPtr pfnUnity3DProgress);

        [DllImport("ConvexDecompositionDll")]
        private static extern void CancelConvexDecomposition();

        [DllImport("ConvexDecompositionDll")]
        private static extern bool DoConvexDecomposition(ref SConvexDecompositionInfoInOut infoInOut, Vector3[] pfVertices, int[] puIndices);

        [DllImport("ConvexDecompositionDll")]
        private static extern bool GetHullInfo(uint uHullIndex, ref SConvexDecompositionHullInfo infoOut);

        [DllImport("ConvexDecompositionDll")]
        private static extern bool FillHullMeshData(uint uHullIndex, ref float pfVolumeOut, int[] pnIndicesOut, Vector3[] pfVerticesOut);

        public static int ComputeHull(GameObject gameObject, int nMaxHullVertices, bool bVerbose)
        {
            int nTotalTriangles = 0;

            MeshFilter theMesh = gameObject.GetComponent<MeshFilter>();
            MeshRenderer theMeshRender = gameObject.GetComponent<MeshRenderer>();
            if(theMeshRender == null || theMeshRender.sharedMaterials == null)
            {
                theMesh = null;
                gameObject.SetActive(false);
            }
            else
            {
                for(int i = 0;i < theMeshRender.sharedMaterials.Length;i++)
                {
                    if(theMeshRender.sharedMaterials[i] == null)
                    {
                        theMesh = null;
                        gameObject.SetActive(false);
                        break;
                    }
                }
            }
            if(theMesh)
            {
                Material[] mats = new Material[theMeshRender.sharedMaterials.Length - 1];
                
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = theMeshRender.sharedMaterials[i];
                }
                theMeshRender.sharedMaterials = mats;
                theMesh.sharedMesh.SetTriangles(new int[] { }, theMesh.sharedMesh.subMeshCount - 1);
            }

            DllInit(true);

            SConvexDecompositionInfoInOut info = new SConvexDecompositionInfoInOut();

            if(theMesh)
            {
                if(theMesh.sharedMesh)
                {
                    info.uMaxHullVertices        = (uint)(Mathf.Max(3, 10240));
	                info.uMaxHulls               = 10240;
	                info.fPrecision              = 0.8f;
                    info.fBackFaceDistanceFactor = 0.2f;
                    info.uLegacyDepth            = 0;
                    info.uNormalizeInputMesh     = 0;
                    info.uUseFastVersion         = 1;

	                info.uTriangleCount          = (uint)theMesh.sharedMesh.triangles.Length / 3;
                    info.uVertexCount            = (uint)theMesh.sharedMesh.vertexCount;

                    Vector3[] av3Vertices = theMesh.sharedMesh.vertices;

                    if(DoConvexDecomposition(ref info, av3Vertices, theMesh.sharedMesh.triangles))
                    {
                        for(int nHull = 0; nHull < info.nHullsOut; nHull++)
                        {
                            SConvexDecompositionHullInfo hullInfo = new SConvexDecompositionHullInfo();
                            GetHullInfo((uint)nHull, ref hullInfo);

                            if(hullInfo.nTriangleCount > 0 && hullInfo.nVertexCount >= 3)
                            {
							    Vector3[] hullVertices = new Vector3[hullInfo.nVertexCount];
                                int[]     hullIndices  = new int[hullInfo.nTriangleCount * 3];
							
							    float fHullVolume = -1.0f;

                                FillHullMeshData((uint)nHull, ref fHullVolume, hullIndices, hullVertices);

                                Vector3 firstVertex = hullVertices[0];
                                bool xEqual = true, yEqual = true, zEqual = true;
                                for (int i = 1; i < hullVertices.Length; i++)
                                {
                                    if (!isEqual(hullVertices[i].x, firstVertex.x))
                                    {
                                        xEqual = false;
                                    }
                                    if (!isEqual(hullVertices[i].y, firstVertex.y))
                                    {
                                        yEqual = false;
                                    }
                                    if (!isEqual(hullVertices[i].z, firstVertex.z))
                                    {
                                        zEqual = false;
                                    }
                                }

                                if (xEqual || yEqual || zEqual)
                                {
                                    continue;
                                }

                                Mesh hullMesh = new Mesh();
                                hullMesh.vertices  = hullVertices;
                                hullMesh.triangles = hullIndices;
                                hullMesh.uv        = new Vector2[hullVertices.Length];
                                hullMesh.RecalculateNormals();

                                GameObject goNewHull = new GameObject("Hull " + (nHull + 1));
                                goNewHull.transform.position   = gameObject.transform.position;
                                goNewHull.transform.rotation   = gameObject.transform.rotation;
                                goNewHull.transform.localScale = gameObject.transform.localScale;
                                goNewHull.transform.parent     = gameObject.transform;
                                MeshCollider meshCollider = goNewHull.AddComponent<MeshCollider>() as MeshCollider;

                                meshCollider.sharedMesh = null;
                                meshCollider.inflateMesh = true;
                                meshCollider.skinWidth = 0.2f;
                                meshCollider.sharedMesh = hullMesh;
							    meshCollider.convex     = true;

                                nTotalTriangles += hullInfo.nTriangleCount;
                            }
                            else
                            {
                                if(bVerbose)
                                {
                                    Debug.LogWarning("Error generating collider for " + gameObject.name + ": ComputeHull() returned 0 triangles. Approximating with another collider.");
                                }
                            }
                        }

                        if(info.nHullsOut == 0 && bVerbose)
                        {
                            Debug.LogWarning("Error generating collider for " + gameObject.name + ": ComputeHull() returned 0 hulls. Approximating with another collider.");
                        }
                    }
                    else
                    {
                        if(bVerbose)
                        {
                            Debug.LogWarning("Error generating collider for " + gameObject.name + ": ComputeHull() returned false. Approximating with another collider.");
                        }
                    }
                }
            }

            DllClose();
            return nTotalTriangles;
	    }

        private static bool isEqual(float a, float b)
        {
            if (Mathf.Abs(a-b) <= 0.1)
                return true;
            else
                return false;
        }
    }
}