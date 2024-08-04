using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Mesh;

[CreateAssetMenu(fileName = "HierarchyData", menuName = "ScriptableObjectAsset")]
public class HierarchyData : ScriptableObject
{
    public Color[] backgroundColor;
   
    private HierarchyData()
    {
        backgroundColor = new[]
        {
            new Color(1f, 0.44f, 0.97f),
            new Color(0.56f, 0.44f, 1f),
            new Color(0.44f, 0.71f, 1f),
            new Color(0.19f, 0.53f, 0.78f)
        };
    }

    private void OnValidate()
    {
        HierarchyCustom.Init();
    }
}