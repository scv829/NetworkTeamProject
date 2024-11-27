using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HYJ2_PlayerSpawn : MonoBehaviourPun
{
    public void PlayerSpawn()
    {
        PhotonNetwork.Instantiate("HYJ2_GameObject/HYJ2_Player",new Vector3(0,0,0),Quaternion.identity);
    }
}
