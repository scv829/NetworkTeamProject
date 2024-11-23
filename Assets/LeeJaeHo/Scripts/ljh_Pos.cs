using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_Pos : MonoBehaviour
{
    ljh_InputManager inputManager;



    public void OnCollisionEnter(Collision collision)
    {
        if (ljh_GameManager.instance.curState == State.move)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ljh_GameManager.instance.curState = State.choice;
            }
        }
    }
}
