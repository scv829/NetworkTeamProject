using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ljh_ButtonParent : MonoBehaviourPun
{
    [SerializeField] public ljh_Button[] buttonArray;
    [SerializeField] public GameObject player;

    [SerializeField] ljh_Boom bomb;

    GameObject button;
    bool buttonOnOff;

    private void Start()
    {
    }
    public void OnEnable()
    {
        buttonArray = GetComponentsInChildren<ljh_Button>();

        //Comment : 랜덤한 버튼에 윈버튼 넣어줌
        int WinNum = Random.Range(0, buttonArray.Length - 1);
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
        //ToDo : 폭탄 터지는 내용 구현해야함

    }

}
