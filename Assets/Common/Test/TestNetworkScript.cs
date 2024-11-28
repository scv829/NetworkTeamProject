using Firebase.Auth;
using Firebase.Database;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 테스트용 TestNetworkScript
/// 실습의 LobbyScene + RoomPanel + TestGameScene 스크립트 참조
/// </summary>
public class TestNetworkScript : MonoBehaviourPunCallbacks
{
    [Header("Room")]
    [SerializeField] string RoomName = "TestRoom";      // 포톤 방의 이름
    [SerializeField] Button startButton;                // 게임 시작 버튼
    [Header("Scene")]
    [SerializeField] string[] sceneNames;               // 다음으로 넘어갈 씬 이름
    [SerializeField] GameObject sceneSelectCanvas;      // 씬 선택 캔버스
    [SerializeField] RectTransform selectContent;       // 플레이 가능한 씬들이 나올 위치
    [SerializeField] TestSceneEntry sceneEntryPrefab;   // 씬 선택 프리팹
    [Header("Nickname")]
    [SerializeField] TMP_InputField inputNicknameField; // 닉네임 변경 입력창

    [Header("PlayerEntry")]
    [SerializeField] TestPlayerEntry[] playerEntries;   // 플레이어 엔트리의 수

    [Header("Login")]
    [SerializeField] HJS_Test_Login login;

    private void Start()
    {
        CreateSceneLoadList();

        // 이미 연결되어있으면 연결 작업 실행 x
        if (PhotonNetwork.IsConnected) {
            UpdatePlayers();
            return;  
        }

        PhotonNetwork.ConnectUsingSettings();                                       // 설정한 값으로 연결 요청
        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;                  // 플레이어의 번호가 변경되었을 시 UpdatePlayers 실행
    }

    public override void OnConnectedToMaster()
    {
        login.Login();

        inputNicknameField.text = PhotonNetwork.LocalPlayer.NickName;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);      // 방이 없으면 방을 만들고 입장, 있으면 그냥 입장

        // 마스터 클라이언트와 항상 똑같은 씬으로 로딩여부
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    private void CreateSceneLoadList()
    {
        // 씬 선택 프리팹 생성
        // 이미 저장한 배열 만큼 생성한다. 배열은 씬의 이름으로 이루어져 있다.
        foreach (string name in sceneNames)
        {
            TestSceneEntry sceneEntry = Instantiate(sceneEntryPrefab, selectContent);
            sceneEntry.SetInfo(name);
        }
    }

    // 닉네임이 변경되었을 때 알려주는 콜백함수
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        UpdatePlayers();
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

    // 닉네임 변경 로직
    public void ChangeNickname()
    {
        FirebaseUser user = HJS_FirebaseManager.Auth.CurrentUser;

        if (user == null) return;

        string uid = user.UserId;

        PhotonNetwork.LocalPlayer.NickName = inputNicknameField.text;

        DatabaseReference userNameRef = HJS_FirebaseManager.Database.RootReference.Child("UserData").Child(uid);
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary["/name"] = inputNicknameField.text;

        userNameRef.UpdateChildrenAsync(dictionary);
    }

    // 게임 씬 선택 로직
    public void SelectGame()             
    {
        sceneSelectCanvas.SetActive(true);          // 게임 씬 선택 캔버스를 활성화
        startButton.gameObject.SetActive(false);    // 게임 시작 버튼을 비활성화
    }

    // 게임 씬 선택 취소 로직
    public void CancelSelectGame()
    {
        sceneSelectCanvas.SetActive(false);         // 게임 씬 선택 캔버스를 비활성화
        startButton.gameObject.SetActive(true);     // 게임 시작 버튼을 활성화
    }   
}
