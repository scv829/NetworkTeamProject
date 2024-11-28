using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun.UtilityScripts;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

/// <summary>
/// 게임 플레이를 담당하는 클래스
/// </summary>
public class HJS_GameMaster : MonoBehaviourPunCallbacks
{
    [Header("Game")]
    [SerializeField] HJS_RandomSlot slotMaster;     // 슬롯 마스터
    [SerializeField] float delayTime = 3f;          // 정답을 맞출 수 있는 대기시간
    [SerializeField] bool isOver;                   // 입력받을 수 있는 시간을 초과했는지 여부
    [SerializeField] public UnityEvent inputStartEvent;    // 입력을 시작할 때 실행할 이벤트 함수
    [SerializeField] public UnityEvent inputStopEvent;     // 입력 시간을 초과했을 때 실행할 이벤트 함수
    [SerializeField] public UnityEvent changeShaderEvent;  // 메테리얼의 셰이더를 변경할 이벤트 함수
    [SerializeField] int refeatTime;               // 게임의 반복횟수
    [SerializeField] bool isExplain;
    [Header("UI")]
    [SerializeField] GameObject explainUI;
    [SerializeField] HJS_scoreUI[] scoreUIs;
    [SerializeField] GameObject[] inputUIs;
    [SerializeField] HJS_TitleUI titleUI;
    [Header("GameScene")]
    [SerializeField] HJS_GameScene gameScene;   // 게임의 연출을 담당하는 매니저

    private Dictionary<Player, int> scoreDictionary = new Dictionary<Player, int>();     // 플레이어와 점수
    private List<(Player, double)> selectResult = new List<(Player, double)>();   // 플레이어의 걸린시간
    private WaitForSeconds delay;

    private Coroutine coroutine;

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        Init();
        isExplain = true;

        if (PhotonNetwork.IsMasterClient == false) return;

        photonView.RPC("ChangeShaderRPC", RpcTarget.All);   // 셰이더 변경

        coroutine = StartCoroutine(SlotSettingRoutine());
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void GameEnd()
    {
        photonView.RPC("ChangeShaderRPC", RpcTarget.All);

        slotMaster.SlotClear();     // 슬롯을 초기화 하고

        // TODO: 같은 점수에 대해서 예외처리
        Player winner = scoreDictionary.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

        titleUI.UpdateUI(winner.NickName);
        HJS_GameSaveManager.Instance.GameOver(new Player[] { winner });
    }

    private IEnumerator SlotSettingRoutine()
    {
        // 한번만 설명을 보여주는 로직
        if (isExplain)
        {
            photonView.RPC("ShowExplainRPC", RpcTarget.All);
            yield return new WaitForSeconds(2f);
            photonView.RPC("ShowExplainRPC", RpcTarget.All);
            isExplain = false;
        }

        int count = 1;
        while (count <= refeatTime)
        {
            slotMaster.SlotClear();
            titleUI.UpdateUI(count, refeatTime);

            yield return new WaitForSeconds(1f);  // 게임 시작을 위해 1초 대기

            slotMaster.SlotSetting();           // slotMaster에게 슬롯의 심볼 세팅 요청
            selectResult.Clear();               // 플레이어의 걸린 시간 리스트 초기화
            photonView.RPC("StartInputRPC", RpcTarget.All);  // 입력 시작
            isOver = false;                     // 초기화

            yield return delay;                  // 입력 받을 수 있는 대기 시간동안 지연

            isOver = true;                      // 시간이 지나면 끝났다고 설정하고
            photonView.RPC("StopInputRPC", RpcTarget.All);   // 입력 중단

            CalculateResult();                  // 결과 계산 시작

            yield return new WaitForSeconds(1f);  // 게임 재시작을 위해 1초 대기

            count++;
        }

        GameEnd();
    }

    /// <summary>
    /// 점수를 계산해주는 함수
    /// </summary>
    private void CalculateResult()
    {
        // 점수
        // 7 5 3 1
        int rank = 7;
        // 걸린 시간 순으로 정렬
        foreach ((Player, double) playerResult in selectResult.OrderBy(s => s.Item2))
        {
            scoreDictionary[playerResult.Item1] += rank;                    // 순위에 따른 점수 저장
            rank -= 2;                                                      // 점수는 순위가 늦어짐에 따라 계속 내려간다 , 7 -> 5 -> 3 -> 1
            Debug.Log($"{playerResult.Item1} time : {playerResult.Item2}");
        }

        UpdateUI();
    }

    /// <summary>
    /// 초기화 작업
    /// </summary>
    private void Init()
    {
        scoreDictionary.Clear(); // 초기화

        delay = new WaitForSeconds(delayTime);
        isOver = false;

        // 플레이어의 인원 수 만큼 플레이어 사전에 추가
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            scoreDictionary[player] = 0;

            int number = player.GetPlayerNumber();
            scoreUIs[number].SetProfile(player.GetPlayerColor());
            scoreUIs[number].gameObject.SetActive(true);
            inputUIs[number].SetActive(true);
        }

        titleUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// 점수 UI를 업데이트 해주는 함수
    /// </summary>
    private void UpdateUI()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetPlayerNumber() == -1) continue;

            int number = player.GetPlayerNumber();
            scoreUIs[number].SetScore(scoreDictionary[player]);
        }
    }

    /// <summary>
    /// 플레이어가 입력한 값을 판단해서 정답이면 정답 리스트에 저장하는 함수
    /// </summary>
    /// <param name="answer">플레이어의 선택한 답</param>
    /// <param name="messageInfo">플레이어의 정보</param>
    public void AddPlayerAnswer(HJS_RandomSlot.AnswerDirection answer, PhotonMessageInfo messageInfo)
    {

        // 1. 선택한 방향이 일치하는 지 확인
        if (!isOver && slotMaster.Answer.Equals(answer))
        {
            // 2. 일치하면 정답 리스트에 유저와 시간입력
            selectResult.Add((messageInfo.Sender, messageInfo.SentServerTime));
        }
        // TODO: 3. 틀린사람 화면에 카메라 배경을 검은색으로
    }

    [PunRPC]
    public void StartInputRPC() => inputStartEvent?.Invoke();   // 입력을 시작하게 해주는 이벤트 함수 실행

    [PunRPC]
    public void StopInputRPC() => inputStopEvent?.Invoke();     // 입력을 중단하게 해주는 이벤트 함수 실행

    [PunRPC]
    public void ChangeShaderRPC() => changeShaderEvent?.Invoke();     // 셰이더를 변경해주는 이벤트 함수 실행

    [PunRPC]
    public void ShowExplainRPC() => explainUI.SetActive(!explainUI.activeSelf); // 설명을 보여주는 이벤트 함수 실행

}
