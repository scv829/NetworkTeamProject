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
        // 시작시 일단 리스트 초기화
        ResetList();
    }

    /// <summary>
    /// 다음 맵을 반환해주는 함수
    /// </summary>
    /// <returns>다음 맵의 이름 또는 null</returns>
    public string NextMap()
    {
        // 만약 다음 맵이 없으면
        if(selectSceneList.Count == 0)
        {
            // null 반환
            return null;
        }
        // 현재 리스트에 있는 맵들 중 하나를 가져오고
        int index = Random.Range(0, selectSceneList.Count);
        // 해당 인덱스의 이름을 가져오고
        string name = selectSceneList[index];
        // 가져온 인덱스를 삭제하고
        selectSceneList.RemoveAt(index);
        // 해당 이름을 반환한다.
        return name;
    }

    /// <summary>
    /// 전체 미니 게임의 수
    /// </summary>
    public int SceneLength => gameScenes.Length;

    /// <summary>
    /// 해당하는 인덱스의 미니 게임 이름 가져오기
    /// </summary>
    /// <param name="i">인덱스</param>
    /// <returns>인덱스의 이름</returns>
    public string GetName(int i) => gameScenes[i];

    /// <summary>
    /// 방을 만들었을 때 선택한 맵을 담아두는 함수
    /// </summary>
    /// <param name="list">맵들이 있는 리스트</param>
    public void SetSelectMap(List<string> list)
    {
        // 일단 리스트를 비우고
        ResetList();
        // 매개변수에 있는 리스트에 담는다
        foreach(string s in list)
        {
            selectSceneList.Add(s);
        }
    }

    /// <summary>
    /// 다음 맵이 남아있는지 확인하는 함수
    /// </summary>
    /// <returns>남아 있는지 여부</returns>
    public bool SceneEmpty()
    {
        return selectSceneList.Count >= 1 ? false : true;
    }

    /// <summary>
    /// 담아있는 맵들을 초기화 시키는 함수
    /// </summary>
    public void ResetList() => selectSceneList.Clear();

}
