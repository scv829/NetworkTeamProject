using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 게임 맵을 담고 있는 스크립트
/// </summary>
public class HJS_GameMap : MonoBehaviour
{
    public static HJS_GameMap instance;

    [Header("GameMap")]
    [SerializeField] string[] gameScenes;

    [Header("SelectMap")]
    [SerializeField] List<string> selectSceneList;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetList();
    }

    public string NextMap()
    {
        if(selectSceneList.Count == 0)
        {
            return null;
        }
        int index = Random.Range(0, selectSceneList.Count);
        string name = selectSceneList[index];
        selectSceneList.RemoveAt(index);

        return name;
    }

    public int SceneLength => gameScenes.Length;

    public string GetName(int i) => gameScenes[i];

    public void SetSelectMap(List<string> list)
    {
        ResetList();
        foreach(string s in list)
        {
            selectSceneList.Add(s);
        }
    }

    public bool SceneEmpty()
    {
        return selectSceneList.Count >= 1 ? false : true;
    }

    public bool ContainSelectScene(string name) => selectSceneList.Contains(name);

    public void ResetList() => selectSceneList.Clear();

}
