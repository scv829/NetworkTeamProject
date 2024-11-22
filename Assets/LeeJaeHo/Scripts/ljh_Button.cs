using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_Button : MonoBehaviour
{
    public bool deadBombButton;

    GameObject selectedButton;
    [SerializeField] GameObject Bomb;
    [SerializeField] GameObject inputManager;
    [SerializeField] GameObject myPos;

    [SerializeField] GameObject player;

    private void Update()
    {
        Vector3 vec = inputManager.GetComponent<ljh_InputManager>()._curPos;

        float distance = vec.x - myPos.transform.position.x;

        if (myPos.transform.position.x != vec.x)
            return;

            if (Input.GetKeyDown(KeyCode.Space))
            {

                PushedButton(gameObject);
                Invoke("TurnOffButton", 1f);
                Invoke("Boom", 1f);
        }

    }

    public void PushedButton(GameObject button)
    {
        button.transform.position = button.transform.position + new Vector3(0, 1, 0);
        //빼야함
        
    }

    public void Boom()
    {
        if (deadBombButton)
        {
            // Comment : 폭탄 터짐
            Bomb.SetActive(false);

            // Todo :
            //애니메이션 추가
            //소리 추가
            //플레이어 사망 추가
            //상태 Idle로 바꾸고
            //버튼 갯수 -1 해서 다시 생성
            //혼자 남은 경우 게임 승리
        }
        else
        {
            // Comment : 폭탄 안터짐
            // Todo :
            //폭탄 안터짐
            TurnOffButton(gameObject);
            //반대 쪽으로 내려감 (상태 무빙으로 변경)
            return;
        }
        //ToDo : 폭탄 터지는 내용 구현해야함
    }

    public void TurnOffButton(GameObject button)
    {
        button.SetActive(false);
    }



}
