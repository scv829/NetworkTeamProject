using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum Phase
{
    phase0, phase1, phase2, phase3, endPhase
}
public class ljh_AvoidGameManager : MonoBehaviourPun
{
    [SerializeField] public Phase curPhase;

    [SerializeField] public ljh_AvoidUIManager uiManager;

    [SerializeField] public ljh_AvoidStone[] stoneArray;

    [SerializeField] public ljh_AvoidStone stone;


    //타이머
    public float timer;
    bool isStarted;

    float stoneCooldown;
    float atkCooldown;
    public Coroutine attackRoutine;
    public Coroutine timerRoutine;


    private void Start()
    {
        curPhase = Phase.phase0;
        isStarted = false;
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

        if (attackRoutine == null) // 이거 널아니라서 막혀있음 1바퀴만 도는중 문제 x
            attackRoutine = StartCoroutine(AttackRoutine());


        TimerCalc();
        PhaseCalc();



    }

    IEnumerator AttackRoutine()
    {
        Debug.Log("코루틴실행");
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

        if (timer <= 0)
            curPhase = Phase.phase1;
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

    public void Phase1()
    {
        atkCooldown = 2;

        // if (attackRoutine != null)
        //     StopCoroutine(attackRoutine);

        
        //Todo: 2초마다 하나씩 떨어짐
    }


    public void Phase2()
    {
       // if (attackRoutine != null)
       //     StopCoroutine(attackRoutine);
       //
       //
       // attackRoutine = StartCoroutine(AttackRoutine());
        //if (attackRoutine == null)
        //    attackRoutine = StartCoroutine(AttackRoutine());
        //Todo: 2초마다 하나씩 떨어짐 페이크 시작
    }

    public void Phase3()
    {
       // atkCooldown = 1;
       // if (attackRoutine != null)
       //     StopCoroutine(attackRoutine);
       //
       //
       // attackRoutine = StartCoroutine(AttackRoutine());
        // if (attackRoutine == null)
        //     attackRoutine = StartCoroutine(AttackRoutine());
        //Todo: 1초마다 하나씩 떨어짐 페이크 있음
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

            case Phase.phase1:
                TimerStart();
                Phase1();

                break;

            case Phase.phase2:
                Phase2();
                break;

            case Phase.phase3:
                Phase3();
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
                    curPhase = Phase.phase1;

                else if (curPhase == Phase.phase3)
                    curPhase = Phase.endPhase;
                break;

            case 25:
                curPhase = Phase.phase2;
                break;

            case 15:
                curPhase = Phase.phase3;
                break;
        }
    }


}
