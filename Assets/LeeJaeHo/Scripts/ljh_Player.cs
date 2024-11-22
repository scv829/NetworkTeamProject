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
    [SerializeField] ljh_inputTTTTT inputManagerScript;
    [SerializeField] GameObject inputManager;

    Vector3 buttonPos1;
    Vector3 buttonPos2;
    Vector3 buttonPos3;
    Vector3 buttonPos4;
    Vector3 buttonPos5;

    [SerializeField] GameObject buttonObj1;
    [SerializeField] GameObject buttonObj2;
    [SerializeField] GameObject buttonObj3;
    [SerializeField] GameObject buttonObj4;
    [SerializeField] GameObject buttonObj5;


    private void Update()
    {
        if(!photonView.IsMine)
            return;

        Vector3 vec = inputManager.GetComponent<ljh_inputTTTTT>()._curPos;

        if (vec != Vector3.zero)
        {
            MovePlayer(vec);
        }
        else return;
    }
    private void Start()
    {
       // inputManagerScript = GameObject.FindWithTag("GameController").GetComponent<ljh_InputManager>();
       inputManager = GameObject.FindWithTag("GameController");
       //
       // buttonObj1 = inputManager.GetComponent<ljh_InputManager>().buttonObj1;
       // buttonObj2 = inputManager.GetComponent<ljh_InputManager>().buttonObj2;
       // buttonObj3 = inputManager.GetComponent<ljh_InputManager>().buttonObj3;
       // buttonObj4 = inputManager.GetComponent<ljh_InputManager>().buttonObj4;
       // buttonObj5 = inputManager.GetComponent<ljh_InputManager>().buttonObj5;
    }

    public void MovePlayer(Vector3 vector)
    {
       inputManagerScript = GameObject.FindWithTag("GameController").GetComponent<ljh_inputTTTTT>();
       inputManager = GameObject.FindWithTag("GameController");
     
       buttonObj1 = inputManager.GetComponent<ljh_inputTTTTT>().buttonObj1;
       buttonObj2 = inputManager.GetComponent<ljh_inputTTTTT>().buttonObj2;
       buttonObj3 = inputManager.GetComponent<ljh_inputTTTTT>().buttonObj3;
       buttonObj4 = inputManager.GetComponent<ljh_inputTTTTT>().buttonObj4;
       buttonObj5 = inputManager.GetComponent<ljh_inputTTTTT>().buttonObj5;
     
       Vector3[] buttonPos = { buttonPos1, buttonPos2, buttonPos3, buttonPos4, buttonPos5 };
       GameObject[] buttonObj = { buttonObj1, buttonObj2, buttonObj3, buttonObj4, buttonObj5 };
     
       
        Vector3 playerVec = vector;

        transform.position = vector;
        //if(vector == buttonPos[0])
         //   gameObject.transform.position = ;

       // gameObject.transform.position = ;

        // 캐릭터의 위치를 vector로 이동시킴

    }


}
