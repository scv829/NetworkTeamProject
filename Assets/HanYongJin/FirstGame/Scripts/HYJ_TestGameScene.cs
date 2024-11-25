using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HYJ_TestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";
    [SerializeField] HYJ_PlayerSpawn playerSpawnPoint;

    void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName,options,TypedLobby.Default);
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

    public void TestGameStart()
    {
        Debug.Log("게임 시작");
        playerSpawnPoint.GetComponent<HYJ_PlayerSpawn>();
        playerSpawnPoint.PlayerSpawn();
    }
}
