using System.Text;
using TMPro;
using UnityEngine.UI;

public class HJS_MatchView : HJS_BaseUI
{

    private StringBuilder sb = new StringBuilder();

    private void Awake()
    {
        BindAll();
    }

    private void Start()
    {
        #region 랜덤매칭 UI

        // 일단 화면을 비활성화
        GetUI("RandomMatchPanel").SetActive(false);
        // 이벤트 메소드 연결
        GetUI<Button>("RandomMatchStartButton").onClick.AddListener(ShowRandomMatchStopUI);
        GetUI<Button>("RandomMatchStopButton").onClick.AddListener(HideRandomMatchStopUI);
        GetUI<Button>("RandomMatchCloseButton").onClick.AddListener(CloseRandomMatchPanel);
        #endregion

        #region 방 참가 UI
        // 일단 화면을 비활성화
        GetUI("JoinLobbyPanel").SetActive(false);
        // 이벤트 메소드 연결
        GetUI<Button>("JoinLobbyCloseButton").onClick.AddListener(CloseJoinLobbyPanel);
        GetUI<Button>("ShowCreateJoinRoomPanelButton").onClick.AddListener(ShowCreateRoomPanel);
        #endregion

        #region 방 생성 UI
        GetUI("CreateRoomPanel").SetActive(false);
        GetUI<Slider>("PlayerCountSlider").onValueChanged.AddListener(UpdatePlayerCount);
        GetUI<Button>("CloseJoinRoomButton").onClick.AddListener(CloseCreateRoomPanel);
        #endregion

        #region 방 수정 UI
        GetUI("CreateRoomPanel").SetActive(false);
        GetUI<Button>("EditButton").onClick.AddListener(ShowEditSetting);
        GetUI<Button>("ApplyEditRoomButton").onClick.AddListener(CloseEditSetting);
        GetUI<Button>("CloseEditRoomButton").onClick.AddListener(CloseEditSetting);
        #endregion

        #region 옵션 UI

        GetUI("OptionPanel").SetActive(false);
        GetUI<Button>("CloseOptionButton").onClick.AddListener(CloseOptionPanel);


        #endregion
    }

    private void ShowRandomMatchStopUI()
    {
        GetUI("RandomMatchStopButton").SetActive(true);
        GetUI("RandomMatchPlayerCount").SetActive(true);
    }
    private void HideRandomMatchStopUI()
    {
        GetUI("RandomMatchStopButton").SetActive(false);
        GetUI("RandomMatchPlayerCount").SetActive(false);
    }
    private void CloseRandomMatchPanel() => GetUI("RandomMatchPanel").SetActive(false);

    private void ShowCreateRoomPanel()
    {
        GetUI("JoinLobbyPanel").SetActive(false);
        GetUI("CreateRoomPanel").SetActive(true);
    }

    private void CloseJoinLobbyPanel() => GetUI("JoinLobbyPanel").SetActive(false);
    private void CloseCreateRoomPanel()
    {
        GetUI("CreateRoomPanel").SetActive(false);
        GetUI("JoinLobbyPanel").SetActive(true);
    }

    private void UpdatePlayerCount(float value)
    {
        sb.Clear();
        int playerCount = (int)value;
        sb.Append($"Player: {playerCount}");
        GetUI<TMP_Text>("PlayerCount").SetText(sb);
    }

    private void ShowEditSetting()
    {
        GetUI<TMP_InputField>("RoomNameInputField").interactable = false;
        GetUI("CloseEditRoomButton").SetActive(true);
        GetUI("ApplyEditRoomButton").SetActive(true);
        GetUI("CreateRoomPanel").SetActive(true);
        GetUI("JoinRoomPanel").SetActive(false);
    }
    private void CloseEditSetting()
    {
        GetUI<TMP_InputField>("RoomNameInputField").interactable = true;
        GetUI("CloseEditRoomButton").SetActive(false);
        GetUI("ApplyEditRoomButton").SetActive(false);
        GetUI("CreateRoomPanel").SetActive(false);
        GetUI("JoinRoomPanel").SetActive(true);
    }

    private void CloseOptionPanel()
    {
        GetUI("OptionPanel").SetActive(false);
    }
}
