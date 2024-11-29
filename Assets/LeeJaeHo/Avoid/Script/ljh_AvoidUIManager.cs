using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ljh_AvoidUIManager : MonoBehaviour
{
    [SerializeField] ljh_AvoidGameManager gameManager;
    [SerializeField] ljh_PlayerController player;



    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public TMP_Text myScoreText;
    [SerializeField] public TMP_Text readyTimerText;
    [SerializeField] public TMP_Text timerText;
    [SerializeField] public TMP_Text winnerText;


    private void Start()
    {


        scoreText.enabled = false;
        myScoreText.enabled = false;
        readyTimerText.enabled = false;
        timerText.enabled = false;
        winnerText.enabled = false;

    }

    private void Update()
    {
        scoreText.text = player.score.ToString();
        readyTimerText.text = $"{gameManager.timer}";
        //timerText.text = gameManager.timer.ToString();
        winnerText.text = $"Winner is{player}!!!"; // Todo : 수정해야함

        if (gameManager.curPhase == Phase.phase0)
            readyTimerText.enabled = true;

        else if (gameManager.curPhase == Phase.phase1)
        {
            readyTimerText.enabled = false;
            timerText.enabled = true;
        }
        
        else if ( gameManager.curPhase == Phase.endPhase)
        {
            scoreText.enabled = true;
            myScoreText.enabled= true;
            timerText.enabled = false;
            winnerText.enabled = true;
        }


    }



}
