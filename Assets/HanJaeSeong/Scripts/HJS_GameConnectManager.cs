using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 게임 입장에 관련된 기능을 담당해주는 스크립트
/// </summary>
public class HJS_GameConnectManager : MonoBehaviourPunCallbacks
{
    [Header("GameInit"), Tooltip("게임 시작하기 전 초기 세팅 부분")]
    [SerializeField] UnityEvent gameInitEvent;
    [Header("GameStart"), Tooltip("실질적인 게임을 시작 요청을 하는 부분")]
    [SerializeField] UnityEvent gameStartEvent;

    [Header("InitOver"), Tooltip("사전 작업이 끝났는지 확인")]
    [SerializeField] bool isOver;

    [Header("Player")] 
    [SerializeField] Color[] playerColors;  // 플레이어의 색상

    [Header("Fade")]
    [SerializeField] HJS_FadeController fadeController;

    // 씬 로드 확인
    private void Start()
    {
        Vector3 vectorColor = new Vector3(playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].r, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].g, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].b);
        PhotonNetwork.LocalPlayer.SetPlayerColor(vectorColor);
        PhotonNetwork.LocalPlayer.SetLoad(true);
    }

    // 접속 동기화
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(HJS_CustomProperty.LOAD))
        {
            Debug.Log($"{targetPlayer.NickName} 이 로딩이 완료되었습니다. ");
            bool allLoaded = CheckAllLoad();
            Debug.Log($"모든 플레이어 로딩 완료 여부 : {allLoaded} ");
            if (allLoaded)
            {
                // 게임 시작 전 초기 설정
                StartCoroutine(InitRoutine());
                // 페이드 부분
                StartCoroutine(FadeRoutine());
                /* 게임 동작 로직 */
                StartCoroutine(StartCoutine());
            }
        }
    }

    // 동기화 완료 시 작동
    private bool CheckAllLoad()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false)
                return false;
        }
        return true;
    }
    
    // 화면에 안보이게 작업해야할 내용들
    private IEnumerator InitRoutine()
    {
        yield return null;
        gameInitEvent?.Invoke();

        isOver = true;
    }

    // 페이드 진행
    private IEnumerator FadeRoutine()
    {
        // 앞의 사전 작업을 다 끝내면
        yield return new WaitUntil(() => { return isOver; });

        fadeController.FadeIn();
    }

    // 게임 시작 로직
    private IEnumerator StartCoutine()
    {
        // 페이드 애니메이션이 끝났을 때
        yield return new WaitUntil(() => {
            return fadeController.isFadeOver.Equals(true);
        });

        // 재사용을 위해 false로 설정
        fadeController.isFadeOver = false;

        // 게임 동작 로직 작동
        gameStartEvent?.Invoke();
    }

}
