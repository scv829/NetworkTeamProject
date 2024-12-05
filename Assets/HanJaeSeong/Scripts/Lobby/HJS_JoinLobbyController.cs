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

    [Header("RoomList")]
    [SerializeField] HJS_RoomEntry roomEntryPrefab;
    private Dictionary<string, HJS_RoomEntry> roomDictionary = new Dictionary<string, HJS_RoomEntry>();

    private const string ROOM_TYPE = "rt";

    private void Start()
    {
        matchView.GetUI<Button>("JoinLobbyCloseButton").onClick.AddListener(LeaveLobby);
        matchView.GetUI<Button>("LobbyRefreshButton").onClick.AddListener(RefreshLobby);
    }

    /// <summary>
    /// 로비에서 나가는 함수
    /// </summary>
    private void LeaveLobby()
    {
        Debug.Log("로비에서 나갔습니다");
        if(PhotonNetwork.InLobby) PhotonNetwork.LeaveLobby();
    }

    /// <summary>
    /// 방 리스트가 업데이트 되는 부분
    /// </summary>
    /// <param name="roomList">갱신된 방 리스트</param>
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

    /// <summary>
    /// 모든 방을 삭제
    /// </summary>
    public void ClearRoomEntries()
    {
        foreach (string name in roomDictionary.Keys)
        {
            Destroy(roomDictionary[name].gameObject);
        }
        roomDictionary.Clear();
    }

    // 방 업데이트 하는 콜백함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("JoinLobby의 RommListUpdate");
        UpdateRoomList(roomList);
    }

    // 로비에서 나갔을 때 콜백함수
    public override void OnLeftLobby()
    {
        Debug.Log("로비 퇴장 성공");
        ClearRoomEntries();
    }

    // 로비를 갱신하는 함수 -> 로비에 재접속
    private void RefreshLobby()
    {
        if (!PhotonNetwork.InLobby && !PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinLobby();
        }
    }

}
