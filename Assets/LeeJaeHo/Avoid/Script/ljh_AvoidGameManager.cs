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

    private void Start()
    {
        
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackStone();
            stone.Smash();

        }
        CooldownCalc();
        
        switch(curPhase)
        {
            case Phase.phase0:
                대기시간();
                break;

                case Phase.phase1:
                타이머시작();
                페이즈1(stoneCooldown);

                break;

                case Phase.phase2:
                페이즈2(stoneCooldown);
                break;

                case Phase.phase3:
                페이즈3(stoneCooldown);
                break;
        }
    }

    public void AttackStone()
    {
        stoneArray[Random.Range(0, stoneArray.Length)].real = true;

        for (int i = 0; i < stoneArray.Length; i++)
        {
            if (stoneArray[i].real == true)
            {
                stone = stoneArray[i];
                stone.real = false;
            }
        }
      //  if (stone.real)
      //  {
      //      Vibe();
      //      // Todo: 코루틴으로 바꿔줘야함
      //      Invoke("Smash", 0.5f);
      //  }
      //  else if (!real)
      //  {
      //      Vibe(); // Todo : 코루틴으로 지연 시간 넣어줘야함
      //  }
    }


    public void 대기시간()
    {
        // Todo: 타이머 3초 대기시간
    }

    public void 타이머시작()
    {
        timer = 35f;
    }

    public void 페이즈1(float cooldown)
    {
        //Todo: 2초마다 하나씩 떨어짐
    }

    IEnumerator 쿵코루틴()
    {
        yield return new WaitForSeconds(stoneCooldown);
    }

    public void 페이즈2(float cooldown)
    {
        //Todo: 2초마다 하나씩 떨어짐 페이크 시작
    }

    public void 페이즈3(float cooldown)
    {
        //Todo: 1초마다 하나씩 떨어짐 페이크 있음
    }

    public void 엔드페이즈()
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
