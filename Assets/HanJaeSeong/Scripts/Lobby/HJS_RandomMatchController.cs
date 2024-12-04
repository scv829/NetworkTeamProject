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
    public HJS_FusionPlayerController Player { get { return player; } set { player = value; } }

    [Header("SceneName")]
    [SerializeField] string sceneName;


    private const string MAP_NAME = "mn";
    private StringBuilder sb = new StringBuilder();

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어의 충돌만 확인할 건데
        if (other.transform.CompareTag("Player"))
        {
            // 움직인 캐릭터의 소유자가 아닐 경우 보여줄 필요가 없다.
            if (other.transform.GetComponent<NetworkBehaviour>().HasStateAuthority == false) return;

            matchView.GetUI("RandomMatchPanel").SetActive(true);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MAP_NAME))
        {
            PlayerNumbering.OnPlayerNumberingChanged -= UpdateRandomPlayers;
            Debug.Log("RandomMacth의 Start");
            PhotonNetwork.LeaveRoom();
        }

        // 버튼에 이벤트를 연결
        matchView.GetUI<Button>("RandomMatchStartButton").onClick.AddListener(StartMatch);
        matchView.GetUI<Button>("RandomMatchStopButton").onClick.AddListener(StopMatch);
    }

    public void StartMatch()
    {
        PlayerNumbering.OnPlayerNumberingChanged += UpdateRandomPlayers;

        Debug.Log("StartMatch");

        if (PhotonNetwork.InRoom) return;


        PhotonHastable properties = new PhotonHastable { { MAP_NAME, sceneName } };

        // 방 설정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomProperties = new PhotonHastable { { MAP_NAME, sceneName } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { MAP_NAME };

        sb.Clear();
        sb.Append($"RandomRoom {DateTime.Now}");
        Debug.Log("RandomMatchController의 StartMatch");

        PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties: properties, roomName: sb.ToString(), roomOptions: roomOptions);
        Debug.Log($"connectRoom {PhotonNetwork.LocalPlayer.NickName}");
    }

    public void StopMatch()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdateRandomPlayers;

        Debug.Log("StopMatch");
        
        if (!PhotonNetwork.InRoom) return;
        PhotonNetwork.LeaveRoom();
    }

    private void UpdateRandomPlayers()
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
            // 넘버링 갱신을 제거하고
            PlayerNumbering.OnPlayerNumberingChanged -= UpdateRandomPlayers;
            // 플레이어가 광장에서 나간다.
            player.LeaveScene();
            // 혹시 모를 선택한 리스트를 제거하고
            HJS_GameMap.instance.ResetList();
            
            // 방장만 맵 옮기기 요청
            if(PhotonNetwork.IsMasterClient)
            {
                // 맵들 중 랜덤으로 하나로 들어가기
                PhotonNetwork.LoadLevel(HJS_GameMap.instance.GetName(Random.Range(0 ,HJS_GameMap.instance.SceneLength)));
            }
        }
    }
    
}
