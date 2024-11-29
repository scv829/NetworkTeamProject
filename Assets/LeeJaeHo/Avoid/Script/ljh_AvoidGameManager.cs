using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum Phase
{
    phase0, GamePhase, endPhase
}
public class ljh_AvoidGameManager : MonoBehaviourPun
{   //Todo: 오프닝(후순위), 점수 표기, 1등 가리기, 엔딩




    [SerializeField] public Phase curPhase;

    [SerializeField] public ljh_AvoidUIManager uiManager;

    [SerializeField] public ljh_AvoidStone[] stoneArray;

    [SerializeField] public ljh_AvoidStone stone;

    public int playerCount;

    //타이머
    public float timer;
    bool isStarted;

    float stoneCooldown;
    float atkCooldown;
    public Coroutine attackRoutine;
    public Coroutine timerRoutine;
    public Coroutine waitRoutine;


    private void Start()
    {
        curPhase = Phase.phase0;
        isStarted = false;
        playerCount = 0;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;


        if (curPhase != Phase.endPhase)
        {
            if (timerRoutine == null)
            {
                timerRoutine = StartCoroutine(TimerCo());
            }
        }

        TimerCalc();
        PhaseCalc();
        FindPlayer();


    }

    void FindPlayer()
    {
        List<ljh_PlayerController> players = new List<ljh_PlayerController>();
        ljh_PlayerController curPlayer = GameObject.FindWithTag("Player").GetComponent<ljh_PlayerController>();
        if (curPlayer.died == false)
        {
            players.Add(curPlayer);
        }

        if(playerCount - players.Count == 1) // 주말에 질문하자..
            curPhase = Phase.endPhase;
    }

    IEnumerator AttackRoutine()
    {
        while ((int)curPhase > 0 || (int)curPhase < 4)
        {
                stoneArray[Random.Range(0, stoneArray.Length)].real = true;

                for (int i = 0; i < stoneArray.Length; i++)
                {
                    if (stoneArray[i].real == true)
                    {
                        stone = stoneArray[i];
                        stone.Smash();
                        stone.real = false;
                    }
                }
            yield return new WaitForSeconds(atkCooldown);
        }
        
    }


    public void Wait()
    {
        // Todo: 타이머 3초 대기시간
        timer = 3f;

        if (waitRoutine == null)
            waitRoutine = StartCoroutine(WaitCo());

        if (timer <= 0)
            curPhase = Phase.GamePhase;
    }

    IEnumerator WaitCo()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            uiManager.readyTimerText.text = $"{timer}";
        }
    }

    public void TimerStart()
    {
        if (!isStarted)
        {
            timer = 35f;
            isStarted = true;
        }
    }

    IEnumerator TimerCo()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            uiManager.timerText.text = $"Time : {timer}";
        }
    }

    public void GamePhase()
    {
        atkCooldown = 2;

        if (attackRoutine != null) return;
        attackRoutine = StartCoroutine(AttackRoutine());
    }


    public void EndPhase()
    {
        StopCoroutine(attackRoutine);

        // Todo: 게임 끝 살아남은 사람 줌인 / 우선순위 낮음
        // Todo: 시간 비례해서 순위
    }

    public void PhaseCalc()
    {
        switch (curPhase)
        {
            case Phase.phase0:
                Wait();
                break;

            case Phase.GamePhase:
                TimerStart();
                GamePhase();
                break;

            case Phase.endPhase:
                EndPhase();
                break;
        }
    }

    public void TimerCalc()
    {


        switch (timer)
        {
            case 0:
                if (curPhase == Phase.phase0)
                    curPhase = Phase.GamePhase;

                else if (curPhase == Phase.GamePhase)
                    curPhase = Phase.endPhase;
                break;
        }
    }


}
