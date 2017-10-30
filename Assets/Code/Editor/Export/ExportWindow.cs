using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


public class ExportWindow : EditorWindow {

    public static Vector2 minWindowSize = new Vector2(750.0f, 250.0f);

    [MenuItem("Export/Open ExportSetting",false,1)]
    public static void OpenExportSetting()
    {
        var window = GetWindow<ExportWindow>();
        window.Show();
        AssetUtility.LoadSetting();
    }

    [MenuItem("Export/Export All", false, 2)]
    public static void ExportAll()
    {
        ExportAudio();
        Debug.Log("ExportAudio Done!");
        ExportCharactor();
        Debug.Log("ExportCharactor Done!");
        ExportPlayer();
        Debug.Log("ExportPlayer Done!");
        ExportNpc();
        Debug.Log("ExportNpc Done!");
        ExportEffect();
        Debug.Log("ExportEffect Done!");
        ExportRide();
        Debug.Log("ExportRide Done!");
    }


    [MenuItem("Export/Export All Audio", false, 3)]
    public static void ExportAudio()
    {
        Object[] objs = AssetUtility.GetAllAudio().ToArray();
        //objs = ObjVersionChecker.FilterWithDependencies(objs);
        if (objs == null || objs.Length == 0)
        {
            Debug.LogError("there is no object selected!");
            return;
        }
        OBJExportor.ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs);
    }

    [MenuItem("Export/Export All Charactor", false, 3)]
    public static void ExportCharactor()
    {
        List<Object>[] list = AssetUtility.GetAllCharactor().ToArray();
        Object[] objs0 = list[0].ToArray();
        Object[] objs1 = list[1].ToArray();
        //objs = ObjVersionChecker.FilterWithDependencies(objs);
        if (objs0 == null || objs0.Length == 0)
        {
            Debug.LogError("there is no object selected!");
            return;
        }
        CharacterExport.ExecuteUnPack(objs0, EditorUserBuildSettings.activeBuildTarget);
        OBJExportor.ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs1);
    }

    [MenuItem("Export/Export All Player", false, 3)]
    public static void ExportPlayer()
    {
        List<Object>[] list = AssetUtility.GetAllPlayer().ToArray();
        Object[] objs0 = list[0].ToArray();
        Object[] objs1 = list[1].ToArray();
        //objs = ObjVersionChecker.FilterWithDependencies(objs);
        if (objs0 == null || objs0.Length == 0)
        {
            Debug.LogError("there is no object selected!");
            return;
        }
        OBJExportor.ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs0);
        OBJExportor.ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs1);
    }

    [MenuItem("Export/Export All Npc", false, 3)]
    public static void ExportNpc()
    {
        List<Object>[] list = AssetUtility.GetAllNpc().ToArray();
        Object[] objs0 = list[0].ToArray();
        Object[] objs1 = list[1].ToArray();
        if (objs0 == null || objs0.Length == 0)
        {
            Debug.LogError("there is no object selected!");
            return;
        }
        OBJExportor.ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs0);
        OBJExportor.ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs1);
    }

    [MenuItem("Export/Export All Effect", false, 3)]
    public static void ExportEffect()
    {
        Object[] objs = AssetUtility.GetAllEffect().ToArray();
        if (objs == null || objs.Length == 0)
        {
            Debug.LogError("there is no object selected!");
            return;
        }
        OBJExportor.ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs);
    }
    [MenuItem("Export/Export All Ride", false, 3)]
    public static void ExportRide()
    {
        List<Object>[] list = AssetUtility.GetAllRide().ToArray();
        Object[] objs0 = list[0].ToArray();
        Object[] objs1 = list[1].ToArray();
        if (objs0 == null || objs0.Length == 0)
        {
            Debug.LogError("there is no object selected!");
            return;
        }
        OBJExportor.ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs0);
        OBJExportor.ExportOBJ(EditorUserBuildSettings.activeBuildTarget, objs1);
    }



    Vector2 regScrollPos = new Vector2(0, 0);
    ReorderableList regsList;

    Vector2 NpcregScrollPos = new Vector2(0, 0);
    ReorderableList NpcregsList;

    bool Reload = false;
    private void OnGUI()
    {
        EditorGUILayout.Separator();
        EditorGUILayout.BeginVertical("Box");
        regScrollPos = EditorGUILayout.BeginScrollView(regScrollPos, GUILayout.Width(position.width - 12), GUILayout.Height(200));
        // 没有初始化过list
        if (regsList == null  || Reload )
        {
            regsList = new ReorderableList(AssetUtility.exportSetting.CharactorPaths, typeof(string), false, false, false, true);
            regsList.displayAdd = true;
        }
        // 绘制表头
        regsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "CharactorPaths");
        };
        // 渲染element
        regsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            string ps = regsList.list[index] as string;
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 2, 40, EditorGUIUtility.singleLineHeight), "Path:");
            EditorGUI.TextField(new Rect(rect.x + 45, rect.y + 2, rect.width - 45 - 60, EditorGUIUtility.singleLineHeight), ps);
           };
        // 删除
        regsList.onRemoveCallback = (ReorderableList l) => {
            l.list.RemoveAt(l.index);
            l.index = -1;
        };
        //添加
        regsList.onAddCallback = (ReorderableList l) => {
            l.list.Add("");
        };
 
        regsList.DoLayoutList();
        DropProc(regsList);

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Separator();
        //Npc List
        NpcregScrollPos = EditorGUILayout.BeginScrollView(NpcregScrollPos, GUILayout.Width(position.width - 12), GUILayout.Height(200));
        // 没有初始化过list
        if (NpcregsList == null || Reload)
        {
            NpcregsList = new ReorderableList(AssetUtility.exportSetting.IgnoreList, typeof(string), false, false, false, true);
            NpcregsList.displayAdd = true;
            Reload = false;
        }
        // 绘制表头
        NpcregsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "IgnoreList");
        };
        // 渲染element
        NpcregsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            string ps = NpcregsList.list[index] as string;
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 2, 40, EditorGUIUtility.singleLineHeight), "Path:");
            EditorGUI.TextField(new Rect(rect.x + 45, rect.y + 2, rect.width - 45 - 60, EditorGUIUtility.singleLineHeight), ps);
        };
        // 删除
        NpcregsList.onRemoveCallback = (ReorderableList l) => {
            l.list.RemoveAt(l.index);
            l.index = -1;
        };
        //添加
        NpcregsList.onAddCallback = (ReorderableList l) => {
            l.list.Add("");
        };

        NpcregsList.DoLayoutList();
        DropProc(NpcregsList);

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("LoadSetting", EditorStyles.toolbarButton))
        {
            AssetUtility.LoadSetting();
            Reload = true;
        }

        if (GUILayout.Button("SaveSetting", EditorStyles.toolbarButton))
        {
            AssetUtility.SaveSetting();
            Reload = true;
        }
        if (GUILayout.Button("ExportAll", EditorStyles.toolbarButton))
        {
            ExportAll();
        }

        EditorGUILayout.EndVertical();
    }
    /**
       * 拖拽
       * */
    private void DropProc(ReorderableList regsList)
    {
        var evt = Event.current;
        var dropArea = GUILayoutUtility.GetLastRect();
        int id = GUIUtility.GetControlID(FocusType.Passive);
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition)) break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                DragAndDrop.activeControlID = id;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                    {
                        for (int i = 0; i < DragAndDrop.paths.Length; i++)
                        {
                            string path = DragAndDrop.paths[i];
                            regsList.list.Add(path);
                        }
                    }
                    DragAndDrop.activeControlID = 0;
                }
                Event.current.Use();
                break;
        }
    }

}
