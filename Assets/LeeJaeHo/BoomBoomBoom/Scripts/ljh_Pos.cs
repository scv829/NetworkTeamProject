using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ljh_Pos : MonoBehaviour
{
    [SerializeField] GameObject cart;
    [SerializeField] GameObject[] _cartArray;



    private void Start()
    {
        _cartArray = new GameObject[4];

        _cartArray[0] = GameObject.Find("Cart1");
        _cartArray[1] = GameObject.Find("Cart2");
        _cartArray[2] = GameObject.Find("Cart3");
        _cartArray[3] = GameObject.Find("Cart4");

    }


    public void EndPoint()
    {
        Debug.Log("엔드포인트호출");
        if (_cartArray[0].GetComponent<CinemachineDollyCart>().m_Position == 1)
        {
            if (ljh_GameManager.instance.curState == State.enter)
            {
                Debug.Log("초이스로바뀜");
                ljh_GameManager.instance.curState = State.choice;
                
            }
            return;
        }
        if (_cartArray[1].GetComponent<CinemachineDollyCart>().m_Position == 1)
        {
            if (ljh_GameManager.instance.curState == State.enter)
            {
                ljh_GameManager.instance.curState = State.choice;

            }
            return;
        }
        if (_cartArray[2].GetComponent<CinemachineDollyCart>().m_Position == 1)
        {
            if (ljh_GameManager.instance.curState == State.enter)
            {
                ljh_GameManager.instance.curState = State.choice;

            }
            return;
        }
        if (_cartArray[3].GetComponent<CinemachineDollyCart>().m_Position == 1)
        {
            if (ljh_GameManager.instance.curState == State.enter)
            {
                ljh_GameManager.instance.curState = State.choice;

            }
            return;
        }

    }

    

}
