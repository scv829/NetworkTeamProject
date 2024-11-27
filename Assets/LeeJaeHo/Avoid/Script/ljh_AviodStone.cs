using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_AviodStone : MonoBehaviour
{
    ljh_AvoidGameManager gameManager;
    
    bool real;
    public void 돌에게명령()
    {
        gameManager.stoneArray[Random.Range(0, gameManager.stoneArray.Length - 1)].real = true;

        if (real)
        {
            Vibe();
            // Todo: 코루틴으로 바꿔줘야함
            Invoke("Smash", 0.5f);
        }
        else if(!real)
        {
            Vibe(); // Todo : 코루틴으로 지연 시간 넣어줘야함
        }
    }

    public void Vibe()
    {
        //Todo: 좌우로 흔들거리는 효과 줘야함 > 불빛으로 대체 가능
    }

    public void Smash()
    {
        //Todo: 돌이 넘어져서 바닥에 붙어야함
        //Todo: 플레이어가 닿으면 플레이어가 장애물로 변함 > 이건 플레이어에서? 스톤에서?
    }
}
