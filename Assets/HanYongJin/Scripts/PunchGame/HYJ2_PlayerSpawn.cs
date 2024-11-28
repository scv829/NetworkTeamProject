using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HYJ2_PlayerSpawn : MonoBehaviourPun
{
    [SerializeField] GameObject playerPrefab;
    public void PlayerSpawn()
    {
        //PhotonNetwork.Instantiate("HYJ2_GameObject/HYJ2_Player",new Vector3(0,0,0),Quaternion.identity);
        Instantiate(playerPrefab,new Vector3(0,0,0),Quaternion.identity); 
    }
}
