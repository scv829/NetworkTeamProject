using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHastable = ExitGames.Client.Photon.Hashtable;
/// <summary>
/// 방 생성과 수정 패널의 작업들
/// </summary>
public class HJS_CreateRoomController : MonoBehaviour
{
    [SerializeField] HJS_MatchView matchView;
    [Header("MapSlot")]
    [SerializeField] HJS_SceneEntry mapPrefab;

    [SerializeField] List<(string, HJS_SceneEntry)> sceneList;

    private const string ROOM_TYPE = "rt";

    private void Start()
    {
        sceneList = new List<(string, HJS_SceneEntry)>(HJS_GameMap.instance.SceneLength << 2);

        for(int i = 0; i < HJS_GameMap.instance.SceneLength; i++)
        {
            HJS_SceneEntry instance = Instantiate(mapPrefab, matchView.GetUI<RectTransform>("GridMap"));
            instance.SetInfo(HJS_GameMap.instance.GetName(i));
            sceneList.Add((HJS_GameMap.instance.GetName(i), instance));
        }
        // 생성 버튼에 방 생성 추가
        matchView.GetUI<Button>("CreateJoinRoomButton").onClick.AddListener(CreateRoom);
        // 방을 떠났을 때 선택한 맵 초기화
        matchView.GetUI<Button>("LeaveButton").onClick.AddListener(ResetSceneEntry);
        // 수정 화면 집입하기 전 데이터 초기화
        matchView.GetUI<Button>("EditButton").onClick.AddListener(ResetEditRoom);
        // 수정하는 버튼에 이벤트 장착
        matchView.GetUI<Button>("ApplyEditRoomButton").onClick.AddListener(EditRoom);
    }

    /// <summary>
    /// 선택한 맵의 리스트 초기화 함수
    /// </summary>
    private void ResetSceneEntry()
    {
        for (int i = 0; i < sceneList.Count; i++)
        {
            sceneList[i].Item2.ResetSetting();
        }
    }

    /// <summary>
    /// 방을 만들었을 때
    /// </summary>
    private void CreateRoom()
    {
        // 방 이름
        string roomName = matchView.GetUI<TMP_InputField>("RoomNameInputField").text;

        if (roomName == "")
        {
            matchView.GetUI<HJS_PopupPanel>("PopupPanel").ShowPopup("방 이름을 입력해주세요.");
            return;
        }

        // 최대 플레이어 수
        float maxPlayer = matchView.GetUI<Slider>("PlayerCountSlider").value;

        // 선택한 방의 맵들
        List<string> scenes = new List<string> ();

        // 선택한 맵을 리스트로 가져온다
        foreach((string, HJS_SceneEntry) value in sceneList)
        {
            if(value.Item2.isCheck) scenes.Add(value.Item1);
        }

        // 맵은 하나라도 선택을 해야한다
        if(scenes.Count < 1)
        {
            matchView.GetUI<HJS_PopupPanel>("PopupPanel").ShowPopup("하나 이상의 맵을 선택해야 합니다.");
            return;
        }

        HJS_GameMap.instance.SetSelectMap(scenes);

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (int)maxPlayer;
        options.CustomRoomPropertiesForLobby = new string[] { ROOM_TYPE };
        options.CustomRoomProperties = new PhotonHastable { { ROOM_TYPE, 1} };

        PhotonNetwork.CreateRoom(roomName, options);
    }

    /// <summary>
    /// 수정 화면에 들어갔을 때 UI 데이터 갱신
    /// </summary>
    private void ResetEditRoom()
    {
        matchView.GetUI<TMP_InputField>("RoomNameInputField").text = PhotonNetwork.CurrentRoom.Name;
        matchView.GetUI<Slider>("PlayerCountSlider").value = PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    /// <summary>
    /// 방을 수정하는 함수
    /// </summary>
    private void EditRoom()
    {
        // 최대 플레이어 수
        float maxPlayer = matchView.GetUI<Slider>("PlayerCountSlider").value;

        if(maxPlayer < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            matchView.GetUI<HJS_PopupPanel>("PopupPanel").ShowPopup("설정한 방의 크기가\n 현재 플레이어의 수보다 많습니다.");
            return;
        }

        // 선택한 방의 맵들
        List<string> scenes = new List<string>();

        // 선택한 맵을 리스트로 가져온다
        foreach ((string, HJS_SceneEntry) value in sceneList)
        {
            if (value.Item2.isCheck) scenes.Add(value.Item1);
        }

        // 맵은 하나라도 선택을 해야한다
        if (scenes.Count < 1)
        {
            matchView.GetUI<HJS_PopupPanel>("PopupPanel").ShowPopup("하나 이상의 맵을 선택해야 합니다.");
            return;
        }

        HJS_GameMap.instance.SetSelectMap(scenes);
        PhotonNetwork.CurrentRoom.MaxPlayers = (int)maxPlayer;

        int index = 0;

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            PlayerNumberingExtensions.SetPlayerNumber(player, index++);
        }

        matchView.CloseEditSetting();
    }
    

}
