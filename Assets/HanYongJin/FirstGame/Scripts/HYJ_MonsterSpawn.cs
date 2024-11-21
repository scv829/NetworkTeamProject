using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HYJ_MonsterSpawn : MonoBehaviour
{
    [SerializeField] GameObject playerPoint;
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] GameObject headPrefab;

    private void Start()
    {
        if(playerPoint.transform.childCount > 0)    // 플레이어가 위치했을때만 몬스터를 생성
        {
            MonsterSpawn(bodyPrefab, headPrefab);
        }
    }

    private void MonsterSpawn(GameObject bodyPrefab, GameObject headPrefab)
    {
        //몬스터 바디 프리팹 생성, 자식 오브젝트로 생성
        for(int i = 0; i < 9; i++)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f + (float)i, transform.position.z);
            GameObject body = Instantiate(bodyPrefab, pos, Quaternion.identity);
            body.transform.parent = this.transform;
        }

        //몬스터 머리 프리팹 생성, 자식 오브젝트로 생성
        GameObject head = Instantiate(headPrefab, new Vector3(transform.position.x, transform.position.y + 9.5f, transform.position.z), Quaternion.identity);
        head.transform.parent = this.transform;
        
    }
}
