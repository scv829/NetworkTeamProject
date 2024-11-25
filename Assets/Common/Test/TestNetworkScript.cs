using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 테스트용 TestNetworkScript
/// 실습의 LobbyScene + RoomPanel + TestGameScene 스크립트 참조
/// </summary>
public class TestNetworkScript : MonoBehaviourPunCallbacks
{
    [Header("Room")]
    public const string RoomName = "TestRoom";      // 포톤 방의 이름
    [SerializeField] Button startButton;            // 게임 시작 버튼
    [Header("Scene")]
    [SerializeField] string[] sceneNames;              // 다음으로 넘어갈 씬 이름
    [SerializeField] GameObject sceneSelectCanvas;  
    [SerializeField] RectTransform selectContent;
    [SerializeField] TestSceneEntry sceneEntryPrefab;

    [Header("PlayerEntry")]
    [SerializeField] TestPlayerEntry[] playerEntries;   // 플레이어 엔트리의 수

    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}"; // 초기 생성시 이름 랜덤으로 지정
        PhotonNetwork.ConnectUsingSettings();                                       // 설정한 값으로 연결 요청
            
        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;                  // 플레이어의 번호가 변경되었을 시 UpdatePlayers 실행
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);      // 방이 없으면 방을 만들고 입장, 있으면 그냥 입장

        // 마스터 클라이언트와 항상 똑같은 씬으로 로딩여부
        PhotonNetwork.AutomaticallySyncScene = true;

        foreach(string name in sceneNames)
        {
            TestSceneEntry sceneEntry = Instantiate(sceneEntryPrefab, selectContent);
            sceneEntry.SetInfo(name);
        }
    }

    public void UpdatePlayers()
    {
        foreach (TestPlayerEntry entry in playerEntries)        // 일단 방의 UI를 초기화 시키기
        {
            entry.SetEmpty();
        }

        foreach (Player player in PhotonNetwork.PlayerList)     // 방에 있는 플레이어의 수만큼
        {
            if (player.GetPlayerNumber() == -1) continue;       // 아직 할당을 안받았으면 넘어가기

            int number = player.GetPlayerNumber();              // 플레이어의 번호를 가져와서
            playerEntries[number].SetPlayer(player);            // 그 위치에 있다고 설정
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)           // 게임 시작 버튼은 방장만 가능
        {
            startButton.gameObject.SetActive(true);             
        }
        else
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void SelectGame()             
    {
        sceneSelectCanvas.SetActive(true);
        startButton.gameObject.SetActive(false);
    }

    public void CancelSelectGame()
    {
        sceneSelectCanvas.SetActive(false);
        startButton.gameObject.SetActive(true);
    }
}
