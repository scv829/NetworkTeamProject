using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

public class HYJ_GameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] Button goPartyRoomButton;
    void Start()
    {
        
    }

    
    public void GoPartyRoom()
    {
        // TODO
        //PhotonNetwork.LoadLevel("LobbyScene"); -> 씬 이름 수정하기
    }
}
