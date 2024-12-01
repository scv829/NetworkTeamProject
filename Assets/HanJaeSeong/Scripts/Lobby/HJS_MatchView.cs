using Photon.Pun;
using System.Text;
using TMPro;
using UnityEngine.UI;

public class HJS_MatchView : HJS_BaseUI
{

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

}
