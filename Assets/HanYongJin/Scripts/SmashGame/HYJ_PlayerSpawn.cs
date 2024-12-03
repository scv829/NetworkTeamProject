using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HYJ_PlayerSpawn : MonoBehaviourPun
{
    //플레이어 생성 지점 및 
    [SerializeField] GameObject playerPoint1;
    [SerializeField] GameObject monsterPoint1;
    [SerializeField] GameObject playerPoint2;
    [SerializeField] GameObject monsterPoint2;
    [SerializeField] GameObject playerPoint3;
    [SerializeField] GameObject monsterPoint3;
    [SerializeField] GameObject playerPoint4;
    [SerializeField] GameObject monsterPoint4;
    [SerializeField] GameObject playerPrefab;

    public void PlayerSpawn() //플레이어 생성
    {
        Vector3 playerSpawnPoint = SetPosition(); // 플레이어 생성 지점 설정
        GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerSpawnPoint, Quaternion.identity);
        photonView.RPC("PlayerParentSetRPC",RpcTarget.All,player.GetComponent<PhotonView>().ViewID);
    }

    private Vector3 SetPosition() // 플레이어 생성 지점을 설정하는 함수
    {
        switch (PhotonNetwork.LocalPlayer.ActorNumber) // 플레이어의 ActorNumber에 따라 생성 지점을 설정
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

    [PunRPC]
    private void PlayerParentSetRPC(int playerID) // 플레이어의 ID에 따라 어느 지점의 자식으로 갈지 결정하는 RPC 함수
    {
        PhotonView playerView = PhotonView.Find(playerID); // 플레이어 ID에 해당하는 포톤뷰를 기준
        GameObject playerParent = null; // 부모 초기화
        switch (playerView.Owner.ActorNumber) // 플레이어의 ActorNumber에 따라 부모를 결정
        {
            case 1:
                playerParent = playerPoint1;
                break;
            case 2:
                playerParent = playerPoint2;
                break;
            case 3:
                playerParent = playerPoint3;
                break;
            case 4:
                playerParent = playerPoint4;
                break;
        }
        playerView.transform.parent = playerParent.transform; // 플레이어 이동
        playerView.transform.localPosition = Vector3.zero;
    }
}
