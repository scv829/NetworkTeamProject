using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HJS_RoomEntry : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text currentPlayer;
    [SerializeField] Button joinRoomButton;

    /// <summary>
    /// 방의 엔트리를 생성할 때 세팅하는 저장
    /// </summary>
    /// <param name="info">방의 정보</param>
    public void SetRoomInfo(RoomInfo info)
    {
        roomName.text = info.Name;
        currentPlayer.text = $"{info.PlayerCount} / {info.MaxPlayers}";
        joinRoomButton.interactable = info.PlayerCount < info.MaxPlayers;
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    private void JoinRoom()
    {
        // 다음과 같은 상황이면 시작하지 않는다
        // 1. 연결 준비가 안되어 있을 때
        // 2. 이미 들어있는 방이 현재 들어가 있는 방
        if (!PhotonNetwork.IsConnectedAndReady || PhotonNetwork.CurrentRoom.Name.Equals(roomName.name)) return;

        // 방에 참가를 하는데 만약 내가 방이 있으면 해당 방을 나오고 선택한 방으로 들어간다.
        if(PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();

        // 새로운 방으로 들어간다
        PhotonNetwork.JoinRoom(roomName.text);
    }
}
