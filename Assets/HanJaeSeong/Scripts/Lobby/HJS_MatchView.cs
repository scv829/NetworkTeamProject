using Fusion;
using Photon.Pun;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        #region 로비 UI
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
        GetUI<Button>("CloseEditRoomButton").onClick.AddListener(CloseEditSetting);
        #endregion

        #region 옵션 UI

        GetUI("OptionPanel").SetActive(false);
        GetUI<Button>("CloseOptionButton").onClick.AddListener(CloseOptionPanel);
        GetUI<Button>("UpdateProfileButton").onClick.AddListener(
            GetUI<HJS_UserProfile>("OptionPanel").UpdateProfile);

        #endregion

        #region 게임 종료

        GetUI<Button>("LogoutButton").onClick.AddListener(Logout);
        GetUI<Button>("ExitButton").onClick.AddListener(ExitGame);

        #endregion

        #region InputTextField 글자 수 제한
        GetUI<TMP_InputField>("RoomNameInputField").characterLimit = 20;
        GetUI<TMP_InputField>("PlayerNicknameInputField").characterLimit = 12;
        GetUI<TMP_InputField>("PlayerDescriptionInputField").characterLimit = 50;
        #endregion
    }

    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape))
        {
            if(!GetUI("OptionPanel").activeSelf) GetUI<HJS_UserProfile>("OptionPanel").GetUserProfile();
            GetUI("OptionPanel").SetActive(!GetUI("OptionPanel").activeSelf);
        }
    }

    /// <summary>
    /// 랜덤 매칭 시작 시 중단 버튼 보이게 해주는 함수
    /// </summary>
    private void ShowRandomMatchStopUI()
    {
        GetUI("RandomMatchStopButton").SetActive(true);
        GetUI("RandomMatchPlayerCount").SetActive(true);
    }
    /// <summary>
    /// 랜덤 매칭 중단 시 시작 버튼 보이게 해주는 함수
    /// </summary>
    private void HideRandomMatchStopUI()
    {
        GetUI("RandomMatchStopButton").SetActive(false);
        GetUI("RandomMatchPlayerCount").SetActive(false);
    }
    /// <summary>
    /// 랜덤 매칭 UI를 닫는 함수
    /// </summary>
    private void CloseRandomMatchPanel() => GetUI("RandomMatchPanel").SetActive(false);

    /// <summary>
    /// 로비에서 나갔을 때 로비 UI를 닫아주는 함수
    /// </summary>
    private void CloseJoinLobbyPanel() => GetUI("JoinLobbyPanel").SetActive(false);

    /// <summary>
    /// 방 생성 UI를 보여주는 함수
    /// </summary>
    private void ShowCreateRoomPanel()
    {
        GetUI("JoinLobbyPanel").SetActive(false);
        GetUI("CreateRoomPanel").SetActive(true);
    }
    /// <summary>
    /// 방 생성 UI를 닫아주는 함수
    /// </summary>
    private void CloseCreateRoomPanel()
    {
        GetUI("CreateRoomPanel").SetActive(false);
        GetUI("JoinLobbyPanel").SetActive(true);
    }


    /// <summary>
    /// 방의 인원 수를 갱신해주는 함수
    /// </summary>
    /// <param name="value">인원 수</param>
    private void UpdatePlayerCount(float value)
    {
        sb.Clear();
        int playerCount = (int)value;
        sb.Append($"Player: {playerCount}");
        GetUI<TMP_Text>("PlayerCount").SetText(sb);
    }

    /// <summary>
    /// 방 수정 UI를 보여주는 함수
    /// </summary>
    private void ShowEditSetting()
    {
        GetUI<TMP_InputField>("RoomNameInputField").interactable = false;
        GetUI("CloseEditRoomButton").SetActive(true);
        GetUI("ApplyEditRoomButton").SetActive(true);
        GetUI("CreateRoomPanel").SetActive(true);
        GetUI("JoinRoomPanel").SetActive(false);
    }
    /// <summary>
    /// 방 수정 UI를 닫아주는 함수
    /// </summary>
    public void CloseEditSetting()
    {
        GetUI<TMP_InputField>("RoomNameInputField").interactable = true;
        GetUI("CloseEditRoomButton").SetActive(false);
        GetUI("ApplyEditRoomButton").SetActive(false);
        GetUI("CreateRoomPanel").SetActive(false);
        GetUI("JoinRoomPanel").SetActive(true);
    }

    /// <summary>
    /// 설정 창을 닫아주는 함수
    /// </summary>
    private void CloseOptionPanel()
    {
        GetUI("OptionPanel").SetActive(false);
    }

    /// <summary>
    /// 게임 종료 함수
    /// </summary>
    private void ExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    /// <summary>
    /// 게임 로그아웃 함수
    /// </summary>
    private void Logout()
    {
        PhotonNetwork.Disconnect();
        HJS_FirebaseManager.Auth.CurrentUser.Dispose();
        SceneManager.LoadScene("HJS_LoginScene");
    }
}
