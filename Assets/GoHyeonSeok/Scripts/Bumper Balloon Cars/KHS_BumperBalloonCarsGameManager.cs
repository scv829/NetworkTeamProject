using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_BumperBalloonCarsGameManager : MonoBehaviourPunCallbacks
{

    public override void OnJoinedRoom()
    {
        PlayerSpawn();
    }

    // 플레이어(카트) 위치를 위한 함수
    private Vector3 SetPosition()
    {
        // 현재 자신의 ActorNumber 대로 위치 설정
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                return new Vector3(-7f, 1f, -8f);
            case 2:
                return new Vector3(-3f, 1f, -8f);
            case 3:
                return new Vector3(3f, 1f, -8f);
            case 4:
                return new Vector3(7f, 1f, -8f);
        }
        return Vector3.zero;
    }

    // 플레이어(카트) 스폰 함수
    private GameObject PlayerSpawn()
    {
        Vector3 spawnPosition = SetPosition();  // 함수내에서 미리 설정한 위치로 초기화

        return PhotonNetwork.Instantiate("KHS/KHS_Cart", spawnPosition, Quaternion.identity);
    }
}
