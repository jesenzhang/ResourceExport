using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EditorAppUtil
{
    public static void ClearConsoleLog()
    {
        System.Type log = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
        log.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Invoke(null, null);
    }
}
