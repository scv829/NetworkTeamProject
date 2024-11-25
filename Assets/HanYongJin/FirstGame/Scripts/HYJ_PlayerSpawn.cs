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
