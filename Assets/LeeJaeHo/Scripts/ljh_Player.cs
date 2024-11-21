using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_Player : MonoBehaviour
{
    [SerializeField] ljh_InputManager inputManagerScript;
    [SerializeField] Vector3 playerPos;
    
    private void OnEnable()
    {
        inputManagerScript = GameObject.FindWithTag("GameController").GetComponent<ljh_InputManager>();
        inputManagerScript.enabled = true;
    }
    private void Start()
    {
       
    }

    public void Update()
    {
        playerPos = inputManagerScript.curPos;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = playerPos;
        }
          
    }

}
