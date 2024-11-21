using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_Player : MonoBehaviour
{
    [SerializeField] ljh_InputManager inputManagerScript;
    [SerializeField] Vector3 playerPos;
    [SerializeField] GameObject testSpawner;
    
    private void OnEnable()
    {
        inputManagerScript = GameObject.FindWithTag("GameController").GetComponent<ljh_InputManager>();
        
        Debug.Log(inputManagerScript.buttonPos3);
        Debug.Log("는 플레이어꺼");
        transform.position = testSpawner.transform.position;
    }
    private void Start()
    {
       
    }

    public void Update()
    {
        //playerPos = inputManagerScript.curPos;

       // if (Input.GetKeyDown(KeyCode.Space))
       // {
       //     transform.position = playerPos;
       // }
          
    }

}
