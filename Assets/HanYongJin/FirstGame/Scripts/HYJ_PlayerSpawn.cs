using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HYJ_PlayerSpawn : MonoBehaviourPun
{
    [SerializeField] GameObject playerPoint1;
    [SerializeField] GameObject monsterPoint1;
    [SerializeField] GameObject playerPoint2;
    [SerializeField] GameObject monsterPoint2;
    [SerializeField] GameObject playerPoint3;
    [SerializeField] GameObject monsterPoint3;
    [SerializeField] GameObject playerPoint4;
    [SerializeField] GameObject monsterPoint4;
    [SerializeField] GameObject playerPrefab;

    public void PlayerSpawn()
    {
        photonView.RPC("PlayerSpawnRPC", RpcTarget.All);
    }

    [PunRPC]
    public void PlayerSpawnRPC()
    {
        Vector3 playerSpawnPoint = SetPosition();
        GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerSpawnPoint, Quaternion.identity);
        //if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        //{
        //    GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerPoint1.transform.position, Quaternion.identity);
        //    player.transform.parent = playerPoint1.transform;
        //    monsterPoint1.GetComponent<HYJ_MonsterSpawn>().MonsterSpawn();
        //}
        //else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        //{
        //    GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerPoint2.transform.position, Quaternion.identity);
        //    player.transform.parent = playerPoint2.transform;
        //    monsterPoint2.GetComponent<HYJ_MonsterSpawn>().MonsterSpawn();
        //}
        //else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        //{
        //    GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerPoint3.transform.position, Quaternion.identity);
        //    player.transform.parent = playerPoint3.transform;
        //    monsterPoint3.GetComponent<HYJ_MonsterSpawn>().MonsterSpawn();
        //}
        //else if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
        //{
        //    GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerPoint4.transform.position, Quaternion.identity);
        //    player.transform.parent = playerPoint4.transform;
        //    monsterPoint4.GetComponent<HYJ_MonsterSpawn>().MonsterSpawn();
        //}
    }

    private Vector3 SetPosition()
    {
        // 현재 자신의 ActorNumber 대로 위치 설정
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                return playerPoint1.transform.position;
            case 2:
                return playerPoint2.transform.position;
            case 3:
                return playerPoint3.transform.position;
            case 4:
                return playerPoint4.transform.position;
        }
        return Vector3.zero;
    }
}
