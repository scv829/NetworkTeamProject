using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    phase0, phase1, phase2, phase3
}
public class ljh_AvoidGameManager : MonoBehaviour
{
    Phase curPhase;

    //타이머
    float timer;

    private void Update()
    {
    
        switch(curPhase)
        {
            case Phase.phase0:
                대기시간();
                break;

                case Phase.phase1:
                타이머시작();
                페이즈1();
                break;

                case Phase.phase2:
                페이즈2();
                break;

                case Phase.phase3:
                페이즈3();
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

    public void 페이즈1()
    {
        //Todo: 3초마다 하나씩 떨어짐
    }

    public void 페이즈2()
    {
        //Todo: 3초마다 하나씩 떨어짐 페이크 시작
    }

    public void 페이즈3()
    {
        //Todo: 2초마다 하나씩 떨어짐 페이크 있음
    }
}
