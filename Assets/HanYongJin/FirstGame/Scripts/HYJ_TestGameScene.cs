using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class HYJ_TestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";
    [SerializeField] HYJ_PlayerSpawn playerSpawnPoint;
    [SerializeField] GameObject timer;
    [SerializeField] TMP_Text gameStartCountText;

    void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        //PhotonNetwork.ConnectUsingSettings();
        //Debug
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
        yield return new WaitForSeconds(2f); // 로딩기다리기
        
        gameStartCountText.text = "3";
        yield return new WaitForSeconds(1f);
        gameStartCountText.text = "2";
        yield return new WaitForSeconds(1f);
        gameStartCountText.text = "1";
        yield return new WaitForSeconds(1f);
        gameStartCountText.fontSize = 85;
        gameStartCountText.text = "Start!";
        yield return new WaitForSeconds(0.3f);
        TestGameStart();
    }

    public void TestGameStart()
    {
        Debug.Log("게임 시작");
        gameStartCountText.gameObject.SetActive(false);
        timer.gameObject.SetActive(true);
        timer.gameObject.GetComponent<HYJ_GameTimer>().TimerStart();
        playerSpawnPoint.GetComponent<HYJ_PlayerSpawn>().PlayerSpawn();
    }
}
