using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Phase
{
    phase0, phase1, phase2, phase3, endPhase
}
public class ljh_AvoidGameManager : MonoBehaviourPun
{
    Phase curPhase;

    [SerializeField] public ljh_AviodStone[] stoneArray;

    [SerializeField] public ljh_AviodStone stone;

    //타이머
    float timer;

    float stoneCooldown;
    public Coroutine attackRoutine;

    private void Start()
    {

    }

    private void Update()
    {


        if (attackRoutine == null)
            attackRoutine = StartCoroutine(AttackRoutine());

        CooldownCalc();

        switch (curPhase)
        {
            case Phase.phase0:
                Wait();
                break;

            case Phase.phase1:
                TimerStart();
                Phase1(stoneCooldown);

                break;

            case Phase.phase2:
                Phase2(stoneCooldown);
                break;

            case Phase.phase3:
                Phase3(stoneCooldown);
                break;
        }
    }
    IEnumerator AttackRoutine()
    {
        Debug.Log("어택루틴시작");
        yield return new WaitForSeconds(3); // 쿨다운으로 바꿔줘야함
        Debug.Log("때린다!!");
        AttackStone();
    }

    public void AttackStone()
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
        //Todo : 진동 효과 넣어줘야함
    }


    public void Wait()
    {
        // Todo: 타이머 3초 대기시간
    }

    public void TimerStart()
    {
        timer = 35f;
    }

    public void Phase1(float cooldown)
    {
        //Todo: 2초마다 하나씩 떨어짐
    }


    public void Phase2(float cooldown)
    {
        //Todo: 2초마다 하나씩 떨어짐 페이크 시작
    }

    public void Phase3(float cooldown)
    {
        //Todo: 1초마다 하나씩 떨어짐 페이크 있음
    }

    public void EndPhase()
    {
        // Todo: 게임 끝 살아남은 사람 줌인 / 우선순위 낮음
        // Todo: 시간 비례해서 순위
    }

    public void CooldownCalc()
    {
        switch (curPhase)
        {
            case Phase.phase1:
            case Phase.phase2:
                stoneCooldown = 2;

                break;

            case Phase.phase3:
                stoneCooldown = 1;

                break;
        }
    }
}
