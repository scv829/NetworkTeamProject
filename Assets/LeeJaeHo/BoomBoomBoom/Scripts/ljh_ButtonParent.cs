using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ljh_ButtonParent : MonoBehaviourPun,IPunObservable
{
    [SerializeField] public ljh_Button[] buttonArray;

    [SerializeField] ljh_Boom bomb;

    int WinNum;

    public void OnEnable()
    {
        buttonArray = GetComponentsInChildren<ljh_Button>();

        //Comment : 랜덤한 버튼에 윈버튼 넣어줌
        if (PhotonNetwork.IsMasterClient)
        {
            WinNum = Random.Range(0, buttonArray.Length - 1);
        }
            buttonArray[WinNum].GetComponent<ljh_Button>().WinButton = true;
        
    }

    public void SelectedButtonAction(ljh_Button _button)
    {
        ljh_BoomTestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_BoomTestGameScene>();
        GameObject player = testGameScene.player;

        if (_button.WinButton)
        {
            // Comment : 생존
            bomb.NoBoom();
            player.GetComponent<ljh_Player>().winnerCheck = true;
            ljh_GameManager.instance.curState = State.end;

        }
        else
        {
            // Comment : 사망
            // Todo :
            bomb.Boom();
            _button.TurnOffButton();
            player.GetComponent<ljh_Player>().PlayerDied();

            ljh_GameManager.instance.curState = State.die;


            return;
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(WinNum);
        }
        else
        {
            WinNum = (int)stream.ReceiveNext();
        }
    }
}
