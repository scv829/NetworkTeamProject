using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HYJ_PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject playerPoint1;
    [SerializeField] GameObject playerPoint2;
    [SerializeField] GameObject playerPoint3;
    [SerializeField] GameObject playerPoint4;

    private void Start()
    {
        

    }

    void ObjectChildTest()
    {
        Debug.Log(playerPoint1.transform.GetChild(0).name); // 자식 오브젝트 이름 가져오기
        if (playerPoint2.transform.childCount < 1)  // 자식 오브젝트 갯수
        {
            Debug.Log($"자식 오브젝트가 없습니다.");
        }
    }
}
