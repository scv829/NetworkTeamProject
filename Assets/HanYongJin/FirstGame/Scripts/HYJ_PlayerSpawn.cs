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
        Vector3 playerSpawnPoint = SetPosition();
        GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerSpawnPoint, Quaternion.identity);
        photonView.RPC("PlayerParentSetRPC",RpcTarget.All,player.GetComponent<PhotonView>().ViewID);
    }

    private Vector3 SetPosition()
    {
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

    [PunRPC]
    private void PlayerParentSetRPC(int playerID)
    {
        PhotonView playerView = PhotonView.Find(playerID);
        GameObject playerParent = null;
        switch (playerView.Owner.ActorNumber)
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
        playerView.transform.parent = playerParent.transform;
        playerView.transform.localPosition = Vector3.zero;
    }
}
