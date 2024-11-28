using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ljh_AvoidUIManager : MonoBehaviour
{
    ljh_AvoidGameManager gameManager;
    ljh_PlayerController player;



    TextMeshPro scoreText;
    TextMeshPro readyTimerText;
    TextMeshPro TimerText;
    TextMeshPro winnerText;


    private void Start()
    {
        scoreText.text = player.score.ToString();
        //readyTimerText.text = 



        scoreText.enabled = false;
        readyTimerText.enabled = false;
        TimerText.enabled = false;
        winnerText.enabled = false;
    }

    

}
