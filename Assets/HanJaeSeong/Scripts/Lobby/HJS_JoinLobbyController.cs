using Fusion;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PhotonHastable = ExitGames.Client.Photon.Hashtable;

public class HJS_JoinLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] HJS_MatchView matchView;

    [Header("ConnectPlayer")]
    [SerializeField] HJS_FusionPlayerController player;

    [Header("RoomList")]
    [SerializeField] HJS_RoomEntry roomEntryPrefab;
    private Dictionary<string, HJS_RoomEntry> roomDictionary = new Dictionary<string, HJS_RoomEntry>();

    private const string ROOM_TYPE = "rt";

    private void Start()
    {
        matchView.GetUI<Button>("JoinLobbyCloseButton").onClick.AddListener(LeaveLobby);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어의 충돌만 확인할 건데
        if (other.transform.CompareTag("Player") && player == null)
        {
            // 움직인 캐릭터의 소유자가 아닐 경우 보여줄 필요가 없다.
            if (other.transform.GetComponent<NetworkBehaviour>().HasStateAuthority == false) return;

            player = other.GetComponent<HJS_FusionPlayerController>();
            PhotonNetwork.JoinLobby();

            matchView.GetUI("JoinLobbyPanel").SetActive(true);
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

    private void LeaveLobby()
    {
        Debug.Log("로비에서 나갔습니다");
        PhotonNetwork.LeaveLobby();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // 방이 사라진 경우 + 방이 비공개인 경우 + 입장이 불가능한 방인 경우 + 랜덤 매칭 방인경우
            if (info.RemovedFromList == true || info.IsVisible == false || info.IsOpen == false ||
                !info.CustomProperties.ContainsKey(ROOM_TYPE))
            {
                // 예외 상황 : 로비 들어가자마자 사라지는 방인 경우
                if (roomDictionary.ContainsKey(info.Name) == false) continue;

                Destroy(roomDictionary[info.Name].gameObject);
                roomDictionary.Remove(info.Name);
            }

            // 새로운 방이 생성된 경우
            else if (roomDictionary.ContainsKey(info.Name) == false)
            {
                HJS_RoomEntry roomEntry = Instantiate(roomEntryPrefab, matchView.GetUI<RectTransform>("LobbyContent"));
                roomDictionary.Add(info.Name, roomEntry);
                roomEntry.SetRoomInfo(info);
            }

            // 방의 정보가 변경된 경우
            else if (roomDictionary.ContainsKey(info.Name) == true)
            {
                HJS_RoomEntry roomEntry = roomDictionary[info.Name];
                roomEntry.SetRoomInfo(info);
            }
        }
    }

    public void ClearRoomEntries()
    {
        foreach (string name in roomDictionary.Keys)
        {
            Destroy(roomDictionary[name].gameObject);
        }
        roomDictionary.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    public override void OnLeftLobby()
    {
        Debug.Log("로비 퇴장 성공");
        ClearRoomEntries();
    }

}