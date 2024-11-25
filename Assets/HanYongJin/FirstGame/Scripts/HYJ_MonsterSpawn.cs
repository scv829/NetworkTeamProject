using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HYJ_MonsterSpawn : MonoBehaviourPun
{
    [SerializeField] GameObject playerPoint;
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] GameObject headPrefab;

    public void MonsterSpawn()
    {
        photonView.RPC("MonsterSpawnRPc", RpcTarget.All);
    }
    [PunRPC]
    public void MonsterSpawnRPC()
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
