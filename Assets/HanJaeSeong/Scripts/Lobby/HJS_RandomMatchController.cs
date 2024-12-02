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
using Random = UnityEngine.Random;

/// <summary>
/// 랜덤 매칭의 시작, 중단을 담당하는 컴포넌트
/// </summary>
public class HJS_RandomMatchController : MonoBehaviourPunCallbacks
{
    [SerializeField] HJS_MatchView matchView;

    [Header("ConnectPlayer")]
    [SerializeField] HJS_FusionPlayerController player;

    [Header("SceneName")]
    [SerializeField] string sceneName;


    private const string MAP_NAME = "mn";
    private StringBuilder sb = new StringBuilder();

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어의 충돌만 확인할 건데
        if (other.transform.CompareTag("Player") && player == null)
        {
            // 움직인 캐릭터의 소유자가 아닐 경우 보여줄 필요가 없다.
            if (other.transform.GetComponent<NetworkBehaviour>().HasStateAuthority == false) return;

            player = other.GetComponent<HJS_FusionPlayerController>();
            matchView.GetUI("RandomMatchPanel").SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어의 충돌만 확인하는데 
        if (other.transform.CompareTag("Player"))
        {
            // 움직인 캐릭터의 소유자가 아닐 경우 보여줄 필요가 없다.
            if (other.transform.GetComponent<NetworkBehaviour>().HasStateAuthority == false) return;

            if(player != null && player.Equals(other.transform.GetComponent<HJS_FusionPlayerController>()))
                player = null;
        }
    }

    private void Start()
    {
        // 플레이어 넘버링 체인지는 어디서 호출하는게 좋냐
        // 시작 패널이 활성화 되었을 때가 좋다
        // 그럼 거기에 넣는게 좋지 않냐? 그게 추가하는 시점이다

        // 버튼에 이벤트를 연결
        matchView.GetUI<Button>("RandomMatchStartButton").onClick.AddListener(StartMatch);
        matchView.GetUI<Button>("RandomMatchStopButton").onClick.AddListener(StopMatch);
    }

    public void StartMatch()
    {
        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;

        Debug.Log("StartMatch");

        if (PhotonNetwork.InRoom) return;

        PhotonHastable properties = new PhotonHastable { { MAP_NAME, sceneName } };

        // 방 설정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomProperties = new PhotonHastable { { MAP_NAME, sceneName} };

        sb.Clear();
        sb.Append($"RandomRoom {DateTime.Now}");

        PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties: properties, roomName: sb.ToString(), roomOptions: roomOptions);
        Debug.Log($"connectRoom {PhotonNetwork.LocalPlayer.NickName}");
    }

    public void StopMatch()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayers;

        Debug.Log("StopMatch");
        
        if (!PhotonNetwork.InRoom) return;
        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayers()
    {
        // 로딩이 안되어 있으면 넘어가기 금지
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetPlayerNumber() == -1) return;
        }

        sb.Clear();
        // 현재 이 방에 참여한 플레이어의 수
        sb.Append($"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}");
        // UI 업데이트
        matchView.GetUI<TMP_Text>("RandomMatchPlayerCount").SetText(sb);

        if (PhotonNetwork.CurrentRoom.PlayerCount.Equals(PhotonNetwork.CurrentRoom.MaxPlayers))
        {
            PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayers;
            player.LeaveRoom();
            PhotonNetwork.LoadLevel(sceneName);
        }
    }
    
}
