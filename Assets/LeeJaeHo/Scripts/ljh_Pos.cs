using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ljh_Pos : MonoBehaviour
{
    [SerializeField] GameObject cart;

    public void Update()
    {
        EndPoint();
    }

    public void EndPoint()
    {

        if (cart.GetComponent<CinemachineDollyCart>().m_Position == 1)
        {
            if (ljh_GameManager.instance.curState != State.choice)
            {
                ljh_GameManager.instance.curState = State.choice;
            }
            return;
        }
    }

}
