using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_Button : MonoBehaviour
{
    public bool deadBombButton;

    GameObject Bomb;

    private void Update()
    {
        // 플레이어 위치가 n번 버튼 앞자리에 있으면 n번 버튼이 선택되어 있음
        // if (인풋매니저.플레이어위치 == n번버튼 앞자리) 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Boom();
        }
    }

    public void Boom()
    {
        if (deadBombButton)
        {
            // Comment : 폭탄 터짐
            Bomb.SetActive(false);
        }
        else
        {
            // Commnet : 폭탄 안터짐
            return;
        }
            //ToDo : 폭탄 터지는 내용 구현해야함
    }


}
