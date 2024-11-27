using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    phase0, phase1, phase2, phase3, endPhase
}
public class ljh_AvoidGameManager : MonoBehaviour
{
    Phase curPhase;

    [SerializeField] public ljh_AviodStone stone1; // 돌
    [SerializeField] public ljh_AviodStone stone2;
    [SerializeField] public ljh_AviodStone stone3;
    [SerializeField] public ljh_AviodStone stone4;

    [SerializeField] public ljh_AviodStone[] stoneArray;

    //타이머
    float timer;

    float stoneCooldown;

    private void Update()
    {

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
