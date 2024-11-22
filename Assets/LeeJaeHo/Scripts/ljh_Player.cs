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
        
    }
    private void Start()
    {
        testSpawner = GameObject.Find("Button3 Pos");

        inputManagerScript = GameObject.FindWithTag("GameController").GetComponent<ljh_InputManager>();
        transform.position = testSpawner.transform.position;
    }

    public void MovePlayer()
    {
        transform.position = inputManagerScript._curPos;//Todo: 버그 수정해야함 널레퍼런스뜸
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
