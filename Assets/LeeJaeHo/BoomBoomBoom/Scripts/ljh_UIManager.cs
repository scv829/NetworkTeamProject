using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ljh_UIManager : MonoBehaviourPun,IPunObservable
{
    [SerializeField] ljh_GameManager gameManager;

    [SerializeField] TMP_Text turnText;
    [SerializeField] TMP_Text winnerText;

    State curState;
    private void Update()
    {
        curState = gameManager.curState;

        ShowWhosTurn();
        ShowUiEnd();
        UIOnOff();

        
        

    }
    void UIOnOff()
    {
        photonView.RPC("RPCUI", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void RPCUI()
    {
        if (curState == State.idle || curState == State.die || curState == State.end)
        {
            turnText.gameObject.SetActive(false);
        }

        if (curState == State.enter)
        {
            turnText.gameObject.SetActive(true);
        }

        if (curState == State.end)
        {
            winnerText.gameObject.SetActive(true);
        }
    }

    public void ShowWhosTurn()
    {
        turnText.text = gameManager.myTurn.ToString();
    }
    

    public void ShowUiEnd()
    {
        winnerText.text = gameManager.curPhotonList[(int)gameManager.myTurn].NickName;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) 
        {
            stream.SendNext(turnText.text);
            stream.SendNext(winnerText.text);
        }
        else
        {
            turnText.text = (string)stream.ReceiveNext();
            winnerText.text = (string)stream.ReceiveNext();
        }
    }
}
