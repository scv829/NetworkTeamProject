using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
public class HYJ2_GameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";
    [SerializeField] Color[] playerColors;
    void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";


        PhotonNetwork.ConnectUsingSettings(); //게임 씬 테스트용

        

        //Vector3 vectorColor = new Vector3(playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].r, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].g, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].b);
        //PhotonNetwork.LocalPlayer.SetPlayerColor(vectorColor);
        //PhotonNetwork.LocalPlayer.SetLoad(true);

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(HJS_CustomProperty.LOAD))
        {
            Debug.Log($"{targetPlayer.NickName} 이 로딩이 완료되었습니다. ");
            bool allLoaded = CheckAllLoad();
            Debug.Log($"모든 플레이어 로딩 완료 여부 : {allLoaded} ");
            if (allLoaded)
            {
                //TODO : 게임 진행코드
            }
        }
    }

    private bool CheckAllLoad()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false)
                return false;
        }
        return true;
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName,options,TypedLobby.Default);
    }

    public void GameStart()
    {
        // TODO : 필드 스폰(필드가 스폰되면 필드에서 플레이어 스폰)
        // TODO : 게임 시작
        // 
    }
}
