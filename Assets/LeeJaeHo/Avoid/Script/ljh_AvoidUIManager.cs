using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ljh_AvoidUIManager : MonoBehaviourPun, IPunObservable
{
    [SerializeField] ljh_AvoidGameManager gameManager;
    [SerializeField] ljh_PlayerController player;


    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public TMP_Text myScoreText;
    [SerializeField] public TMP_Text timerText;
    [SerializeField] public TMP_Text winnerText;


    private void Start()
    {
        scoreText.enabled = false;
        myScoreText.enabled = false;
        timerText.enabled = false;
        winnerText.enabled = false;

    }

    private void Update()
    {
        if(gameManager._alivePlayer != null)
        winnerText.text = $"Winner is {gameManager._alivePlayer.myName}!!!"; // Todo : 수정해야함

        TextOnOff();

    }


    /*public void TextOnOff()
    {
        photonView.RPC("RPCTextOnOff", RpcTarget.AllBufferedViaServer);
    }*/

    public void TextOnOff()
    {
        if (gameManager.curPhase == Phase.GamePhase)
        {
            timerText.enabled = true;
        }

        else if (gameManager.curPhase == Phase.endPhase)
        {
            scoreText.enabled = true;
            myScoreText.enabled = true;
            timerText.enabled = false;
            winnerText.enabled = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) 
        {
            stream.SendNext(scoreText.enabled);
            stream.SendNext(myScoreText.enabled);
            stream.SendNext(timerText.enabled);
            stream.SendNext(winnerText.enabled);
            stream.SendNext(gameManager._alivePlayer?.myName);
        }
        else
        {
            scoreText.enabled = (bool)stream.ReceiveNext();
            myScoreText.enabled = (bool)stream.ReceiveNext();
            timerText.enabled = (bool)stream.ReceiveNext();
            winnerText.enabled = (bool)stream.ReceiveNext();
            if(gameManager._alivePlayer != null)
                gameManager._alivePlayer.myName = (string)stream.ReceiveNext();

        }
    }
}
