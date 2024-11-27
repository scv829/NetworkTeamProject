using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_AviodStone : MonoBehaviour
{
    ljh_AvoidGameManager gameManager;
    
    bool 떨어질대상;
    public void 돌에게명령()
    {

        if (떨어질대상)
        {
            흔들거림();
            돌넘어짐();
        }
        else if(!떨어질대상)
        {
            흔들거림(); // Todo : 코루틴으로 지연 시간 넣어줘야함
        }
    }

    public void 흔들거림()
    {
        //Todo: 좌우로 흔들거리는 효과 줘야함 > 불빛으로 대체 가능
    }

    public void 돌넘어짐()
    {
        //Todo: 돌이 넘어져서 바닥에 붙어야함
        //Todo: 플레이어가 닿으면 플레이어가 장애물로 변함 > 이건 플레이어에서? 스톤에서?
    }
}
