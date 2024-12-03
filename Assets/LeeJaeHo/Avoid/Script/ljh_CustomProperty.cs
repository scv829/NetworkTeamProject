using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_CustomProperty : MonoBehaviourPunCallbacks
{
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        
        int Index = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        
        Color color = ljh_BoomTestGameScene.playerColors[Index % ljh_BoomTestGameScene.playerColors.Length];

        // Custom Properties에 색 저장
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "playerColor", color }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }
}
