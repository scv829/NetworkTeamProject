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
    [SerializeField] ljh_InputManager inputManager;
    [SerializeField] ljh_CartManager cartManager;


    public PlayerNumber playerNumber;

    [SerializeField] GameObject exitCart;

    public Vector3 _curPos;

    public bool winnerCheck;

    public Color[] playerColors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.yellow,
        Color.green
    };

    [SerializeField] GameObject[] cartArray;


    private void OnEnable()
    {
        playerNumber = (PlayerNumber)PhotonNetwork.LocalPlayer.ActorNumber - 1;
        ColorChange();
        Debug.Log(playerNumber);
    }

    public void ColorChange()
    {

        Color color = playerColors[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        GetComponentInChildren<Renderer>().material.color = color;
        photonView.RPC("RPCColor", RpcTarget.AllViaServer, color.r, color.g, color.b);


    }

    [PunRPC]
    public void RPCColor(float r, float g, float b)
    {
        GetComponentInChildren<Renderer>().material.color = new(r, g, b);
    }
    private void Start()
    {
        cartArray = new GameObject[4];
        cartArray[0] = GameObject.Find("Cart1");
        cartArray[1] = GameObject.Find("Cart2");
        cartArray[2] = GameObject.Find("Cart3");
        cartArray[3] = GameObject.Find("Cart4");


        testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_BoomTestGameScene>();
        inputManager = GameObject.Find("InputManager").GetComponent<ljh_InputManager>();
        //buttonPos = inputManagerScript. 나중에 유저 4 > 3번 포즈 3명 > 3번포즈 2명 2번 포즈
        cartManager = GameObject.FindWithTag("CartManager").GetComponent<ljh_CartManager>();

        _curPos = testGameScene.playerPos;
        winnerCheck = false;

        exitCart = cartManager.exitCart;
    }


    private void Update()
    {


        if (!photonView.IsMine)
            return;

        if ((int)ljh_GameManager.instance.myTurn == (int)playerNumber)
        {
            //Comment : 플레이어 위치 카트 위치에 고정

            if (ljh_GameManager.instance.curState == State.idle || ljh_GameManager.instance.curState == State.enter)
            {
                transform.position = cartArray[PhotonNetwork.LocalPlayer.ActorNumber-1].transform.position;

            }

            //Comment : 플레이어 위치 엑싯카트 위치에 고정
            if (ljh_GameManager.instance.curState == State.end)
            {
                if (winnerCheck)
                    transform.position = exitCart.transform.position;
            }

            // Comment 내 턴일때만 플레이어 이동

            if (ljh_GameManager.instance.curState == State.choice)
            {
                MovePlayer(_curPos);
            }


            //Comment : 내 턴일 때만 플레이 가능

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
                break;

            case State.choice:
                _curPos = inputManager.ChoiceAnswer().transform.position;
                ljh_GameManager.instance.inputManager.SelectButton(_curPos);

                break;

            case State.die:
                break;

            case State.end:
                //ExitCart();
                CartMoveExit();

                break;

        }
    }
    public void CartMoveExit()
    {
        if ((int)ljh_GameManager.instance.myTurn == PhotonNetwork.LocalPlayer.ActorNumber - 1)
            transform.position = exitCart.transform.position;
    }

    public void MovePlayer(Vector3 vector)
    {
        transform.position = vector;
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
