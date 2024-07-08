using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;
using UnityEngine.UI;


public class UIBase : MonoBehaviour
{
    Dictionary<Type, List<UnityEngine.Object>> UI_Objects = new Dictionary<Type, List<UnityEngine.Object>>();

    [Tooltip("T�� Button�̳� Text ���� Ÿ��, type�� �ش� Ÿ���� �̸��� ������ Enum�� ���")]
    protected void Bind<T>(Type type, bool recursive = true) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        List<UnityEngine.Object> objects = new List<UnityEngine.Object>();

        foreach (string str in names)
        {
            objects.Add(Utils.FindChild<T>(gameObject, str, recursive));
        }

        UI_Objects.Add(typeof(T), objects);
    }

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        List<UnityEngine.Object> obj;

        if (UI_Objects.TryGetValue(typeof(T), out obj))
        {
            return (T)obj[index];
        }
        else
        {
            return null;
        }
    }
}

