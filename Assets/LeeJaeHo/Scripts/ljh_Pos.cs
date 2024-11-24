using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_Pos : MonoBehaviour
{
    ljh_InputManager inputManager;
    ljh_Player player;


    public void OnCollisionEnter(Collision collision)
    {
        if (ljh_GameManager.instance.curState == State.enter)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                player.PlayerEnterdChoice();
                ljh_GameManager.instance.curState = State.choice;
            }
        }
    }
}
