using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HYJ_MonsterSpawn : MonoBehaviourPun
{
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] GameObject headPrefab;
    [SerializeField] GameObject monsterPoint1;
    [SerializeField] GameObject monsterPoint2;
    [SerializeField] GameObject monsterPoint3;
    [SerializeField] GameObject monsterPoint4;

    public void MonsterSpawn()
    {
        //몬스터 바디 프리팹 생성, 자식 오브젝트로 이동
        Vector3 monsterSpawnPoint = SetMonsterPoint();
        for (int i = 0; i < 9; i++)
        {
            Vector3 pos = new Vector3(0,0.5f + (float)i,0);
            GameObject body = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_MonsterBody", monsterSpawnPoint+pos, Quaternion.identity);
            photonView.RPC("MonsterParentSetRPC",RpcTarget.All,body.GetComponent<PhotonView>().ViewID);
        }

        //몬스터 머리 프리팹 생성, 자식 오브젝트로 이동
        GameObject head = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_MonsterHead", monsterSpawnPoint+new Vector3(0,9.5f,0), Quaternion.identity);
        photonView.RPC("MonsterParentSetRPC", RpcTarget.All, head.GetComponent<PhotonView>().ViewID);
    }

    private Vector3 SetMonsterPoint()
    {
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                return monsterPoint1.transform.position;
            case 2:
                return monsterPoint2.transform.position;
            case 3:
                return monsterPoint3.transform.position;
            case 4:
                return monsterPoint4.transform.position;
        }
        return Vector3.zero;
    }

    [PunRPC]
    private void MonsterParentSetRPC(int playerID)
    {
        PhotonView monsterView = PhotonView.Find(playerID);
        Debug.Log(monsterView);
        GameObject monsterParent = null;
        switch (monsterView.Owner.ActorNumber)
        {
            case 1:
                monsterParent = monsterPoint1;
                break;
            case 2:
                monsterParent = monsterPoint2;
                break;
            case 3:
                monsterParent = monsterPoint3;
                break;
            case 4:
                monsterParent = monsterPoint4;
                break;
        }
        monsterView.transform.parent = monsterParent.transform;
    }
}
