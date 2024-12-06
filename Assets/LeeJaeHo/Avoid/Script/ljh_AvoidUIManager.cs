using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ljh_AvoidUIManager : MonoBehaviourPun, IPunObservable
{
    [SerializeField] ljh_AvoidGameManager gameManager;
    [SerializeField] ljh_PlayerController player;


    
    [SerializeField] public TMP_Text timerText;
    [SerializeField] public TMP_Text winnerText;
    [SerializeField] public TMP_Text guideText;
    [SerializeField] Image image;


    private void Start()
    {
        timerText.enabled = false;
        winnerText.enabled = false;

    }

    private void Update()
    {
        TextOnOff();
    }

   // [PunRPC]
   // public void RPCWinner(string name)
   // {
   //     winnerText.text = $"살아남은 생존자는.... {name}입니다!!!";
   // }


    public void TextOnOff()
    {
        if (gameManager.curPhase == Phase.GamePhase)
        {
            image.gameObject.SetActive(true);
            guideText.gameObject.SetActive(true);
            timerText.enabled = false; // 타이머 폐기
        }

        else if (gameManager.curPhase == Phase.endPhase)
        {
            image.gameObject.SetActive(false);
            guideText.gameObject.SetActive(false);
            timerText.enabled = false;
            winnerText.enabled = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) 
        {
            stream.SendNext(timerText.enabled);
            stream.SendNext(winnerText.enabled);
        }
        else
        {
            timerText.enabled = (bool)stream.ReceiveNext();
            winnerText.enabled = (bool)stream.ReceiveNext();

        }
    }
}
