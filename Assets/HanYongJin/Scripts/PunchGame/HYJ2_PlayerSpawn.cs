using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HYJ2_PlayerSpawn : MonoBehaviourPun
{
    [SerializeField] GameObject playerPrefab;
    public void PlayerSpawn(Vector3 playerSpawnPoint)
    {
        PhotonNetwork.Instantiate("HYJ2_GameObject/HYJ2_Player",playerSpawnPoint,Quaternion.identity);
    }
}
