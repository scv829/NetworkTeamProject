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
        // 연결 준비가 안되어 있을 때 입장을 못하게
        if (!PhotonNetwork.IsConnectedAndReady) return;

        // 방에 참가를 하는데 만약 내가 방이 있으면
        if (PhotonNetwork.InRoom)
        {
            // 이방이 내 방이다 그냥 넘어가고
            if (PhotonNetwork.CurrentRoom.Name.Equals(roomName.name))
            {
                return;
            }
            // 아니다 -> 방을 나간다
            else
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        // 새로운 방으로 들어간다
        PhotonNetwork.JoinRoom(roomName.text);
    }
}
