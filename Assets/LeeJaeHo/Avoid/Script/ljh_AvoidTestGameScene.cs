using Photon.Chat.UtilityScripts;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class ljh_AvoidTestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "Testroom";

    [SerializeField] GameObject playerPrefab;
    [SerializeField] ljh_AvoidGameManager gameManager;


    private void Start()
    {

        PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(0000,9999)}";
        PhotonNetwork.ConnectUsingSettings(); // 이거 지우고 실전

        PhotonNetwork.LocalPlayer.SetLoad(true);
    }

    //테스트용
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;
   
        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        TestGameStart();
    }
    //실전용
  // public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
  // {
  //     if (changedProps.ContainsKey(HJS_CustomProperty.LOAD))
  //     {
  //         Debug.Log($"{targetPlayer.NickName} 이 로딩이 완료되었습니다. ");
  //         bool allLoaded = CheckAllLoad();
  //         Debug.Log($"모든 플레이어 로딩 완료 여부 : {allLoaded} ");
  //         if (allLoaded)
  //         {
  //             StartCoroutine(StartDelayRoutine());
  //         }
  //     }
  // }

    //실전용
  //  private bool CheckAllLoad()
  //  {
  //      foreach (Player player in PhotonNetwork.PlayerList)
  //      {
  //          if (player.GetLoad() == false)
  //              return false;
  //      }
  //      return true;
  //  }
    public void TestGameStart()
    { 
        PlayerSpawn();


        if (!PhotonNetwork.IsMasterClient)
            return;

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (newMasterClient.IsLocal)
        {
            
        }
    }

    private void PlayerSpawn()
    {

        Vector3 playerPos = new Vector3(Random.Range(-5,5), 0, Random.Range(-5, 5));
        GameObject player = PhotonNetwork.Instantiate("ljh_AvoidPlayer", playerPos, Quaternion.identity);
        gameManager.playerCount++;


        //Color[] vectorColor = { playerColor1, playerColor2, playerColor3, playerColor4 };
        //playerColor = new Color(vectorColor[index].r, vectorColor[index].g, vectorColor[index].b, 1);

        //player.GetComponentInChildren<Renderer>().material.color = playerColor;


        

    }


    










}
