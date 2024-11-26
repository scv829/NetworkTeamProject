using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ljh_Pos : MonoBehaviour
{
    [SerializeField] GameObject cart;
    [SerializeField] GameObject[] cartArray;


    
    

    public void EndPoint()
    {

        if (cartArray[0].GetComponent<CinemachineDollyCart>().m_Position == 1)
        {
            if (ljh_GameManager.instance.curState == State.enter)
            {
                ljh_GameManager.instance.curState = State.choice;
                
            }
            return;
        }
        if (cartArray[1].GetComponent<CinemachineDollyCart>().m_Position == 1)
        {
            if (ljh_GameManager.instance.curState == State.enter)
            {
                ljh_GameManager.instance.curState = State.choice;

            }
            return;
        }
        if (cartArray[2].GetComponent<CinemachineDollyCart>().m_Position == 1)
        {
            if (ljh_GameManager.instance.curState == State.enter)
            {
                ljh_GameManager.instance.curState = State.choice;

            }
            return;
        }
        if (cartArray[3].GetComponent<CinemachineDollyCart>().m_Position == 1)
        {
            if (ljh_GameManager.instance.curState == State.enter)
            {
                ljh_GameManager.instance.curState = State.choice;

            }
            return;
        }

    }

    

}
