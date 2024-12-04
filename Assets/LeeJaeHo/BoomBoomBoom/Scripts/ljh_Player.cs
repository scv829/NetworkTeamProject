using Cinemachine;
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
    [SerializeField] ljh_BoomTestGameScene testGameScene;
    [SerializeField] GameObject inputManager;
    [SerializeField] GameObject cartManager;

    [SerializeField] public Animator anime;

    public PlayerNumber playerNumber;

    [SerializeField] GameObject exitCart;

    public Vector3 _curPos;

    public bool winnerCheck;

    private void Start()
    {
        testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_BoomTestGameScene>();
        inputManager = GameObject.FindWithTag("GameController");
        //buttonPos = inputManagerScript. 나중에 유저 4 > 3번 포즈 3명 > 3번포즈 2명 2번 포즈
        cartManager = GameObject.FindWithTag("EditorOnly");

        _curPos = testGameScene.playerPos;
        winnerCheck = false;
        exitCart = ljh_GameManager.instance.cartManagerEnter.exitCart;

    }


    private void Update()
    {


        if (!photonView.IsMine)
            return;

        //Comment : 플레이어 위치 카트 위치에 고정
        if(ljh_GameManager.instance.curState == State.idle || ljh_GameManager.instance.curState == State.enter)
        {
            transform.position = testGameScene.cartArray[testGameScene.index].transform.position;
        }
        //Comment : 플레이어 위치 엑싯카트 위치에 고정
        if(ljh_GameManager.instance.curState == State.end)
        {
            transform.position = exitCart.transform.position;
        }

        // Comment 내 턴일때만 플레이어 이동
        if ((int)ljh_GameManager.instance.myTurn == (int)this.playerNumber)
        {
            if (ljh_GameManager.instance.curState == State.choice)
            {
                MovePlayer(_curPos);
            }
        }

        //Comment : 내 턴일 때만 플레이 가능
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
                _curPos = inputManager.GetComponent<ljh_InputManager>().ChoiceAnswer().transform.position;
                ljh_GameManager.instance.inputManager.SelectButton(_curPos);

                break;

            case State.die:
                break;

            case State.end:
                //ExitCart();
                cartManager.GetComponent<ljh_CartManager>().CartMoveExit();

                break;

        }
    }

    public void MovePlayer(Vector3 vector)
    {
        transform.position = vector;
    }


    public void UnRideCart()
    {
        ljh_BoomTestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_BoomTestGameScene>();
        GameObject player = testGameScene.player;

        player.transform.parent = null;

    }



    public void PlayerDied()
    {
        photonView.RPC("RPCPlayerDied", RpcTarget.All);
    }

    [PunRPC]
    public void RPCPlayerDied()
    {
        gameObject.SetActive(false);
        inputManager.GetComponent<ljh_InputManager>().ChoiceAnswer().SetActive(false);

    }

    



}
