using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HJS_BaseUI : MonoBehaviour
{
    private Dictionary<string, GameObject> gameObjectDic;
    private Dictionary<(string, Type), Component> componentDic;

    // 시간에 좀 예민하면 사용
    protected void Bind()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);

        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);
        
        foreach (Transform t in transforms)
        {
            gameObjectDic.TryAdd(t.gameObject.name, t.gameObject);
        }

        componentDic = new Dictionary<(string, Type), Component>();
    }

    // 시간이 좀 걸릴거 같아도 상관 없으면 사용
    protected void BindAll()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);

        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);

        foreach (Transform t in transforms)
        {
            gameObjectDic.TryAdd(t.gameObject.name, t.gameObject);
        }

        Component[] components = GetComponentsInChildren<Component>(true);
        componentDic = new Dictionary<(string, Type), Component>(components.Length << 4);

        foreach (Component component in components)
        {
            componentDic.TryAdd((component.gameObject.name, component.GetType()), component);
        }
    }

    public GameObject GetUI(in string name)
    {
        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        return gameObject;
    }

    public T GetUI<T>(in string name) where T : Component
    {
        (string, Type) key = (name, typeof(T));

        componentDic.TryGetValue(key, out Component component);
        if (component != null) return component as T;

        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        if(gameObject.Equals(null)) return null;

        component = gameObject.GetComponent<T>();
        if (component.Equals(null)) return null;

        componentDic.TryAdd(key, component);
        return component as T;
    }
}
