using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HYJ_PlayerSpawn : MonoBehaviourPun
{
    [SerializeField] GameObject playerPoint1;
    [SerializeField] GameObject playerPoint2;
    [SerializeField] GameObject playerPoint3;
    [SerializeField] GameObject playerPoint4;
    [SerializeField] GameObject playerPrefab;

    public void PlayerSpawn()
    {
        
        if(PhotonNetwork.LocalPlayer.ActorNumber ==1)
        {
            Debug.Log("p1 없음");
            GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerPoint1.transform.position, Quaternion.identity);
            player.transform.parent = playerPoint1.transform;
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerPoint2.transform.position, Quaternion.identity);
            player.transform.parent = playerPoint2.transform;
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {
            GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerPoint3.transform.position, Quaternion.identity);
            player.transform.parent = playerPoint3.transform;
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
        {
            GameObject player = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_Player", playerPoint4.transform.position, Quaternion.identity);
            player.transform.parent = playerPoint4.transform;
        }
    }
}
