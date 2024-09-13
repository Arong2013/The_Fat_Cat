using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public static class Utils
{
    public static T GetUI<T>(string _name = null) where T : MonoBehaviour
    {
        T component = null;
        if (component == null)
        {
            component = FindInCanvasChildren<T>();
        }
        else if (component == null)
        {
            Debug.Log(component.name + " found in the current scene.");

        }
        return component;
    }
    private static T FindInCanvasChildren<T>() where T : MonoBehaviour
    {
        T component = null;
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            component = canvas.GetComponentInChildren<T>(true);
        }

        return component;
    }
    public static GameObject FindGameObjectField<T>(T gameObjectClass)
    {
        Type type = gameObjectClass.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            // 필드가 GameObject 형식인지 확인
            if (field.FieldType == typeof(GameObject))
            {
                // 필드의 값 가져오기
                GameObject gameObject = field.GetValue(gameObjectClass) as GameObject;
                return gameObject;
            }
        }
        return null;
    }
}
