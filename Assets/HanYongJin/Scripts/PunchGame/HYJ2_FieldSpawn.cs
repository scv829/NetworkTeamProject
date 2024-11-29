using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HYJ2_FieldSpawn : MonoBehaviourPun
{
    [SerializeField] HYJ2_PlayerSpawn HYJ2_PlayerSpawn;
    [SerializeField] HYJ2_ObjectManager HYJ2_ObjectManager;
    public void FieldSpawn()
    {
        Vector3 spawnPoint = SetPosition();
        GameObject Field = PhotonNetwork.Instantiate("HYJ2_GameObject/HYJ2_Field",spawnPoint,Quaternion.identity);
        //TODO : 플레이어 스폰
        HYJ2_PlayerSpawn.GetComponent<HYJ2_PlayerSpawn>().PlayerSpawn(spawnPoint);
    }

    private Vector3 SetPosition()
    {
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                return new Vector3(-10, -0.001f, 10);
            case 2:
                return new Vector3(10, -0.001f, 10);
            case 3:
                return new Vector3(-10, -0.001f, -10);
            case 4:
                return new Vector3(10, -0.001f, -10);
        }
        return Vector3.zero;
    }

    public void ObjectManagerOn()
    {
        HYJ2_ObjectManager.GetComponent<HYJ2_ObjectManager>().ManagerOn();
    }
}
