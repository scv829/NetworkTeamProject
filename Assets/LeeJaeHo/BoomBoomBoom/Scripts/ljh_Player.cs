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
    //[SerializeField] ljh_InputManager inputManagerScript;
    [SerializeField] ljh_InputManager inputManagerScript;
    [SerializeField] ljh_BoomTestGameScene testGameScene;
    [SerializeField] GameObject inputManager;
    [SerializeField] GameObject cartManager;

    GameObject[] buttonPos;
    public PlayerNumber playerNumber;

    [SerializeField] GameObject cart;

    [SerializeField] GameObject exitCart;

    public Vector3 _curPos;
    public int myNum;

    public int i = 1;

    private void Start()
    {
        testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_BoomTestGameScene>();
        inputManager = GameObject.FindWithTag("GameController");
        //buttonPos = inputManagerScript. 나중에 유저 4 > 3번 포즈 3명 > 3번포즈 2명 2번 포즈
      


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
        
        if((int)ljh_GameManager.instance.myTurn != (int)this.playerNumber )
        {
            transform.position = testGameScene.vectorPlayerSpawn[testGameScene.index];
        }


        Debug.Log($"현재 플레이어 : { playerNumber}"); 
        Debug.Log($"지금 턴인 플레이어 :{ljh_GameManager.instance.myTurn}"); // 이놈이 문제 마이턴 2
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
                ExitCart();
                
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

    public void ExitCart()
    {
        ljh_BoomTestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_BoomTestGameScene>();
        GameObject player = testGameScene.player;

        exitCart = ljh_GameManager.instance.cartManagerEnter.exitCart;

        player.transform.parent = exitCart.transform;

        exitCart.GetComponent<CinemachineDollyCart>().enabled = true;
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
