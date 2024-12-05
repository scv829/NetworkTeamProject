using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class ljh_UIManager : MonoBehaviourPun, IPunObservable
{
    [SerializeField] ljh_GameManager gameManager;
    [SerializeField] ljh_BoomTestGameScene testScene;
    [SerializeField] ljh_Player player;

    [SerializeField] TMP_Text turnText;
    [SerializeField] TMP_Text winnerText;

    [SerializeField] TMP_Text guideText;
    [SerializeField] Image[] keyImages;

    State curState;
    private void Update()
    {
        curState = gameManager.curState;

        ShowWhosTurn();

        UIOnOff();
        SelfUIOnOFf();



    }
    void SelfUIOnOFf()
    {
        if ((int)ljh_GameManager.instance.myTurn == PhotonNetwork.LocalPlayer.ActorNumber - 1)
        {

            if (curState == State.choice)
            {
                guideText.gameObject.SetActive(true);
                keyImages[0].gameObject.SetActive(true);
                keyImages[1].gameObject.SetActive(true);
            }

            if (curState == State.end)
            {
                guideText.gameObject.SetActive(false);
                keyImages[0].gameObject.SetActive(false);
                keyImages[1].gameObject.SetActive(false);
            }
        }

        else
        {
            guideText.gameObject.SetActive(false);
            keyImages[0].gameObject.SetActive(false);
            keyImages[1].gameObject.SetActive(false);
        }

    }
    void UIOnOff()
    {
        photonView.RPC("RPCUI", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void RPCUI()
    {
        if (curState == State.die || curState == State.end)
        {
            turnText.gameObject.SetActive(false);
        }

        if (curState == State.idle || curState == State.enter)
        {
            turnText.gameObject.SetActive(true);
        }

        if (curState == State.end)
        {
            winnerText.gameObject.SetActive(true);
        }
    }

    void END()
    {
        photonView.RPC("RPCEND", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPCEND()
    {
        winnerText.text = $"승자는... {gameManager.curPhotonList[(int)gameManager.myTurn].NickName}입니다!!!";
    }

    public void ShowWhosTurn()
    {
        turnText.text = $"{gameManager.myTurn.ToString()}의 차례입니다.";
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
