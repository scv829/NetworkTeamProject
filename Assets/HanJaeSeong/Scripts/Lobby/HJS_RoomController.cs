using Fusion;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어의 충돌만 확인할 건데
        if (other.transform.CompareTag("Player") && player == null)
        {
            // 움직인 캐릭터의 소유자가 아닐 경우 보여줄 필요가 없다.
            if (other.transform.GetComponent<NetworkBehaviour>().HasStateAuthority == false) return;

            player = other.GetComponent<HJS_FusionPlayerController>();

            if (!PhotonNetwork.InLobby) 
            {
                PhotonNetwork.JoinLobby();
                matchView.GetUI("JoinLobbyPanel").SetActive(true);
                Debug.Log("RoomController의 Ontrigger의 Inlobby");
            }

            if(PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE))
            {
                Debug.Log("RoomController의 Ontrigger의 inROOM");
                PhotonNetwork.LeaveLobby();
                matchView.GetUI("JoinLobbyPanel").SetActive(false);
                matchView.GetUI("JoinRoomPanel").SetActive(true);
                UpdatePlayers();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어의 충돌만 확인하는데 
        if (other.transform.CompareTag("Player"))
        {
            // 움직인 캐릭터의 소유자가 아닐 경우 보여줄 필요가 없다.
            if (other.transform.GetComponent<NetworkBehaviour>().HasStateAuthority == false) return;

            if (player != null && player.Equals(other.transform.GetComponent<HJS_FusionPlayerController>()))
            {
                player = null;
            }
        }
    }


    // 방에 들어왔을 때
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE))
        {
            // 넘버링 동기화
            Debug.Log("호출!");
            PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;

            matchView.GetUI<TMP_Text>("JoinRoomTitle").text = PhotonNetwork.CurrentRoom.Name;

            matchView.GetUI("CreateRoomPanel").SetActive(false);
            matchView.GetUI("JoinLobbyPanel").SetActive(false);
            matchView.GetUI("JoinRoomPanel").SetActive(true);
        }
    }
    
    
    public override void OnLeftRoom() 
    {
        Debug.Log("RoomController의 OnLeftRoom");
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayers;
        HJS_GameMap.instance.ResetList();
    }

    public void UpdatePlayers()
    {
        // TODO: 모두 제거했는데 계속 작동 왜?
        if(!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE)) return;

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
            matchView.GetUI("StartButton").SetActive(true);
            matchView.GetUI("EditButton").SetActive(true);
        }
        else
        {
            matchView.GetUI("StartButton").SetActive(false);
            matchView.GetUI("EditButton").SetActive(false);
        }
    }

    // 방을 시작하는 옵션
    private void StartGame()
    {
        player.LeaveScene();
        // 게임을 시작하는 옵션
        PhotonNetwork.LoadLevel(HJS_GameMap.instance.NextMap());
    }

    // 방을 나가는 설정
    private void LeaveRoom()
    {
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
        if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ROOM_TYPE))
        {
            Debug.Log("RommController의 OnRoomProperties");
            UpdatePlayers();
        }
    }

}
