using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ljh_Player : MonoBehaviourPun
{
    //[SerializeField] ljh_InputManager inputManagerScript;
    [SerializeField] ljh_InputManager inputManagerScript;
    [SerializeField] GameObject testGameScene;
    [SerializeField] GameObject inputManager;
    [SerializeField] GameObject cartManager;

    GameObject[] buttonPos;
    Vector3 myPos; // 테스트용

    [SerializeField] GameObject cart;

    private void Start()
    {
        inputManager = GameObject.FindWithTag("GameController");
        //buttonPos = inputManagerScript. 나중에 유저 4 > 3번 포즈 3명 > 3번포즈 2명 2번 포즈
        myPos = new Vector3(-2, 0, 0.7f);
        
    }


    private void Update()
    {
        if(!photonView.IsMine)
            return;

        Vector3 vec = ljh_GameManager.instance._curPos;
        if (vec != Vector3.zero)
        {
            MovePlayer(vec);
        }
        else return;
    }
    
    public void MovePlayer(Vector3 vector)
    {
        transform.position = vector;
    }

    public void RideEnterCart()
    {
        cartManager = ljh_GameManager.instance.cartManagerEnter.gameObject;

        ljh_TestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_TestGameScene>();
        int index = testGameScene.index;

        cart = cartManager.GetComponent<ljh_CartManager>().cartArrayEnter[index];
        transform.parent = cart.transform;
    }

    public void RideExitCart()
    {
        cartManager = ljh_GameManager.instance.cartManagerExit.gameObject;

        ljh_TestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_TestGameScene>();
        int index = testGameScene.index;

        cart = cartManager.GetComponent<ljh_CartManager>().cartArrayExit[index];
        
        GameObject player = testGameScene.player;

        player.transform.parent = cart.transform;
    }

    public void UnRideCart()
    {
        ljh_TestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_TestGameScene>();
        GameObject player = testGameScene.player;

        player.transform.parent = null;
        player.transform.position = myPos;
    }

    public void PlayerEnterdChoice()
    {
        //transform.position = button[ljh_GameManager.instance.defaultIndex].transform.position;
    }


}
