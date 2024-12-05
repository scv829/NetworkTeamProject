using Fusion;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHastable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 방에 대한 스크립트
/// 방의 내용에 대한 업데이트, 게임 시작, 게임 이탈 등
/// </summary>
public class HJS_RoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] HJS_MatchView matchView;

    [Header("ConnectPlayer")]
    [SerializeField] HJS_FusionPlayerController player;
    public HJS_FusionPlayerController Player { get { return player; } set { player = value; } }


    [Header("PlayerEntry")]
    [SerializeField] HJS_PlayerEntry[] playerEntries;   // 플레이어 엔트리의 수

    private const string MAP_PROPS = "maps";
    private const string ROOM_TYPE = "rt";

    private StringBuilder sb = new StringBuilder();

    private void Start()
    {
        // 시작 버튼에 이벤트 장착
        matchView.GetUI<Button>("StartButton").onClick.AddListener(StartGame);
        // 나가는 버튼에 이벤트 장착
        matchView.GetUI<Button>("LeaveButton").onClick.AddListener(LeaveRoom);

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom) PhotonNetwork.CurrentRoom.IsVisible = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어의 충돌만 확인할 건데
        if (other.transform.CompareTag("Player"))
        {
            // 움직인 캐릭터의 소유자가 아닐 경우 보여줄 필요가 없다.
            if (other.transform.GetComponent<NetworkBehaviour>().HasStateAuthority == false) return;

            // 포톤이 준비가 되었을 때만 아래 기능을 수행
            if (PhotonNetwork.IsConnectedAndReady)
            {
                // 로비에 있어서 들어갔는데 <- 현재 내가 방안에 있을 때(게임 씬 -> 광장 씬 넘어올 때)
                if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE))
                {
                    // Debug.Log("RoomController의 Ontrigger의 inROOM");
                    // PhotonNetwork.LeaveLobby();

                    // 방의 이름 설정
                    matchView.GetUI<TMP_Text>("JoinRoomTitle").text = PhotonNetwork.CurrentRoom.Name;

                    matchView.GetUI("JoinLobbyPanel").SetActive(false);
                    matchView.GetUI("JoinRoomPanel").SetActive(true);

                    UpdatePlayers();
                }
                else if (!PhotonNetwork.InLobby) // 로비에 접속하고
                {
                    // PhotonNetwork.JoinLobby();
                    matchView.GetUI("JoinLobbyPanel").SetActive(true);
                    Debug.Log("RoomController의 Ontrigger의 Inlobby");
                }
            }
        }
    }

    // 방에 들어왔을 때
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE))
        {
            // 넘버링 동기화
            PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;
            // 방의 이름 설정
            matchView.GetUI<TMP_Text>("JoinRoomTitle").text = PhotonNetwork.CurrentRoom.Name;

            matchView.GetUI("CreateRoomPanel").SetActive(false);
            matchView.GetUI("JoinLobbyPanel").SetActive(false);
            matchView.GetUI("JoinRoomPanel").SetActive(true);
        }

        if (PhotonNetwork.InLobby == false)
        {
            PhotonNetwork.LeaveLobby();
        }
    }

    // 방에서 나갔을 때
    public override void OnLeftRoom()
    {
        Debug.Log("RoomController의 OnLeftRoom");
        HJS_GameMap.instance.ResetList();
    }

    /// <summary>
    /// 참가하는 방의 플레이어에 따라 UI를 업데이트를 하는 부분
    /// </summary>
    public void UpdatePlayers()
    {
        // TODO: 모두 제거했는데 계속 작동 왜?
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE)) return;

        try
        {
            foreach (HJS_PlayerEntry entry in playerEntries)        // 일단 방의 UI를 초기화 시키기
            {
                entry.SetEmpty();
                entry.gameObject.SetActive(false);
            }

            // 칸은 현재 룸의 최대만 가능
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                sb.Clear();
                sb.Append($"Player_{i}");
                matchView.GetUI(sb.ToString()).SetActive(true);
            }

            foreach (Player player in PhotonNetwork.PlayerList)     // 방에 있는 플레이어의 수만큼
            {
                if (player.GetPlayerNumber() == -1) continue;       // 아직 할당을 안받았으면 넘어가기

                int number = player.GetPlayerNumber();              // 플레이어의 번호를 가져와서
                playerEntries[number].SetPlayer(player);            // 그 위치에 있다고 설정
            }

            if (PhotonNetwork.LocalPlayer.IsMasterClient)           // 게임 시작 버튼과 방 수정은 방장만 가능
            {
                matchView.GetUI("StartButton").SetActive(true);     // 게임 시작 버튼은 리스트가 있을 때에 본다
                matchView.GetUI("EditButton").SetActive(true);
            }
            else
            {
                matchView.GetUI("StartButton").SetActive(false);
                matchView.GetUI("EditButton").SetActive(false);
            }
        }
        catch (Exception e)
        {
            Debug.Log($"에러 메시지! {e.Message}");
        }
    }

    // 방을 시작하는 옵션
    private void StartGame()
    {
        if (HJS_GameMap.instance.SceneEmpty())
        {
            matchView.GetUI<HJS_PopupPanel>("PopupPanel").ShowPopup("하나 이상의 맵을 선택해야 합니다.");
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            matchView.GetUI<HJS_PopupPanel>("PopupPanel").ShowPopup("혼자서는 시작할 수 없습니다.");
            return;
        }

        // 모두에게 보내주기
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            photonView.RPC("LeaveSceneRPC", player);
        }

        if (PhotonNetwork.IsMasterClient == false) return;

        PhotonNetwork.CurrentRoom.IsVisible = false;
        // 게임을 시작하는 옵션
        PhotonNetwork.LoadLevel(HJS_GameMap.instance.NextMap());
    }

    [PunRPC]
    public void LeaveSceneRPC()
    {
        Debug.Log($"{HJS_FirebaseManager.Auth.CurrentUser.DisplayName} is Leave!");
        player.LeaveScene();
    }

    // 방을 나가는 설정
    private void LeaveRoom()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayers;

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE))
        {
            matchView.GetUI("JoinRoomPanel").SetActive(false);
            matchView.GetUI("JoinLobbyPanel").SetActive(true);
        }

        PhotonNetwork.LeaveRoom();
    }

    // 방의 정보가 업데이트 되었을 때 작동하는 함수
    public override void OnRoomPropertiesUpdate(PhotonHastable propertiesThatChanged)
    {
        // 여기다
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE))
        {
            Debug.Log("RommController의 OnRoomProperties");
            UpdatePlayers();
        }
    }

    // 새로운 플레이어가 방에 들어왔을 때
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE))
        {
            UpdatePlayers();
        }
    }

    // 플레이어가 방에서 나갔을 때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE))
        {
            UpdatePlayers();
        }
    }

    // 방 입장에 실패했을 때
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        matchView.GetUI<HJS_PopupPanel>("PopupPanel").ShowPopup("방 입장 실패!\n 로비를 새로고침 해주세요.");
        Debug.Log($"방 입장 실패 ! : {message}");
    }

}
