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


    

    [SerializeField] GameObject cart;



    private void Update()
    {
        if(!photonView.IsMine)
            return;

        Vector3 vec = inputManager.GetComponent<ljh_InputManager>()._curPos;

        if (vec != Vector3.zero)
        {
            MovePlayer(vec);
        }
        else return;
    }
    private void Start()
    {
       inputManager = GameObject.FindWithTag("GameController");
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
    }

    public void PlayerEnterdChoice()
    {
        //transform.position = buttonObj3.transform.position;
    }


}
