using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GameUtils;

#if UNITY_EDITOR
[InitializeOnLoad]
public class HierarchyCustom
{
    private static bool initialized = false;
    private static HierarchyData data;
    
    static HierarchyCustom()
    {
        Init();
    }

    public static void Init()
    {
        data = LoadData();

        if (initialized)
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyGUI;
        }

        initialized = true;
       

        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static HierarchyData LoadData()
    {
        HierarchyData result = EditorGUIUtility.Load("HierarchyData.asset") as HierarchyData;

        return result;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        GameTag gameTag;

        if (obj != null)
        {
            if (Enum.TryParse<GameTag>(obj.transform.root.gameObject.tag, out gameTag))
            {
                EditorGUI.DrawRect(selectionRect, data.backgroundColor[(int)gameTag]);
            }
        }
    }
}
#endif
