using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum PlayerNumber
{
    player1,
    player2,
    player3,
    player4
};
public class ljh_Player : MonoBehaviourPun
{
    //[SerializeField] ljh_InputManager inputManagerScript;
    [SerializeField] ljh_InputManager inputManagerScript;
    [SerializeField] ljh_TestGameScene testGameScene;
    [SerializeField] GameObject inputManager;
    [SerializeField] GameObject cartManager;

    GameObject[] buttonPos;
    Vector3 myPos;
    public PlayerNumber playerNumber;

    [SerializeField] GameObject cart;

    public Vector3 _curPos;
    public int myNum;

    public int i = 1;

    private void Start()
    {
        testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_TestGameScene>();
        inputManager = GameObject.FindWithTag("GameController");
        //buttonPos = inputManagerScript. 나중에 유저 4 > 3번 포즈 3명 > 3번포즈 2명 2번 포즈
        defaultPos();


    }


    private void Update()
    {
        if (!photonView.IsMine)
            return;

        Vector3 vec = _curPos;
        if (ljh_GameManager.instance.curState == State.choice)
        {
                MovePlayer(vec);
        }

        if ((int)playerNumber == (int)ljh_GameManager.instance.myTurn)
        {
            PlayingPlayer();
        }
    }

    public void PlayingPlayer()
    {
        switch (ljh_GameManager.instance.curState)
        {
            case State.idle:
                ljh_GameManager.instance.MoveStart();
                break;

            case State.enter:
                ljh_GameManager.instance.cartManagerEnter.CartMoveEnter();
                break;

            case State.choice:
                UnRideCart();
                _curPos = inputManager.GetComponent<ljh_InputManager>().ChoiceAnswer();
                ljh_GameManager.instance.inputManager.SelectButton(_curPos);

                break;

            case State.exit:
                ljh_GameManager.instance.cartManagerEnter.CartMoveExit();
                break;

            case State.end:
                break;

        }
    }

    public void MovePlayer(Vector3 vector)
    {
        transform.position = vector;
    }


    public void UnRideCart()
    {
        ljh_TestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_TestGameScene>();
        GameObject player = testGameScene.player;

        player.transform.parent = null;

    }

    public void defaultPos()
    {
        switch(ljh_GameManager.instance.curUserNum)
        {
            case 4:
                myPos = new Vector3(-2, 0, 0.7f);
                break;

            case 3:
                myPos = new Vector3(-1.3f, 0, 0.7f);
                break;

            case 2:
                myPos = new Vector3(-2, 0, 0.7f);
                break;

        }
    }

    
}
