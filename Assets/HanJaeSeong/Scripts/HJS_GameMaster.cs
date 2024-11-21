using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class HJS_GameMaster : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] HJS_RandomSlot slotMaster;     // 슬롯 마스터
    [SerializeField] float delayTime = 3f;          // 정답을 맞출 수 있는 대기시간
    [SerializeField] bool isOver;                   // 입력받을 수 있는 시간을 초과했는지 여부
    [SerializeField] UnityEvent inputStopEvent;     // 초과했을 때 실행할 이벤트 함수

    private Dictionary<Player, int> scoreDictionary = new Dictionary<Player, int>();     // 플레이어와 점수
    private List<(Player, float)> selectResult = new List<(Player, float)>();   // 플레이어의 걸린시간
    private WaitForSeconds delay;

    private void Start()
    {
        Init();   

        if (PhotonNetwork.IsMasterClient == false) return;
        StartCoroutine(SlotSettingRoutine());
    }

    private IEnumerator SlotSettingRoutine()
    {
        slotMaster.Setting();       // slotMaster에게 슬롯의 심볼 세팅 요청

        yield return delay;         // 입력 받을 수 있는 대기 시간동안 지연

        isOver = true;              // 시간이 지나면 끝났다고 설정하고
        inputStopEvent?.Invoke();   // 입력 중단 이벤트 함수 실행

        CalculateResult();          // 결과 계산 시작
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
        foreach( (Player, float) playerResult in selectResult.OrderBy(s => s.Item2))
        {
            scoreDictionary[playerResult.Item1] += rank;                    // 순위에 따른 점수 저장
            rank -= 2;                                                      // 점수는 순위가 늦어짐에 따라 계속 내려간다 , 7 -> 5 -> 3 -> 1
        }
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
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            scoreDictionary[player] = 0;
        }
    }

    /// <summary>
    /// 점수를 동기화 하는 과정
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 변수 데이터를 보내는 경우
        if (stream.IsWriting)
        {
            foreach( int score in scoreDictionary.Values )
            {
                stream.SendNext(score);
            }
        }
        // 변수 데이터를 받는 경우
        else if (stream.IsWriting)
        {
            foreach (Player player in scoreDictionary.Keys)
            {
                scoreDictionary[player] = (int)stream.ReceiveNext();
            }
        }
    }


    // 프로퍼티가 변경되었을 때 동작하는 콜백함수
    // 역할: 플레이어가 입력한 값을 판단해서 정답이면 저장해주는 역할
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        // 1. 선택한 방향이 일치하는 지 확인
        if(!isOver && slotMaster.Answer.Equals(targetPlayer.GetAnswer().Item1))
        {
            // 2. 일치하면 정답 리스트에 유저와 시간입력
            selectResult.Add((targetPlayer, targetPlayer.GetAnswer().Item2));
        }
    }

}
