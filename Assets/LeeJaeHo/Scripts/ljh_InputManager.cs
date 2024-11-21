using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_InputManager : MonoBehaviour
{
    PhotonView photonView;
    [SerializeField] GameObject player;

    public Vector3 curPos;

    [SerializeField] Vector3 buttonPos1;
    [SerializeField] Vector3 buttonPos2;
    [SerializeField] Vector3 buttonPos3;
    [SerializeField] Vector3 buttonPos4;
    [SerializeField] Vector3 buttonPos5;

    [SerializeField] GameObject buttonObj1;
    [SerializeField] GameObject buttonObj2;
    [SerializeField] GameObject buttonObj3;
    [SerializeField] GameObject buttonObj4;
    [SerializeField] GameObject buttonObj5;

    int defaultIndex;
    int minus;
    [SerializeField] int index;

    private void Start()
    {

        buttonPos1 = buttonObj1.transform.position;
        buttonPos2 = buttonObj2.transform.position;
        buttonPos3 = buttonObj3.transform.position;
        buttonPos4 = buttonObj4.transform.position;
        buttonPos5 = buttonObj5.transform.position;

        defaultIndex = 2;
        minus = 0;
    }

    private void Update()
    {
        FindPlayer();
        
    }

    public void FindPlayer()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

        //else if (player != null)
           // ChoiceAnswer();
        
    }

    public void ChoiceAnswer()
    {
        index = defaultIndex - minus;
        if (Input.GetKeyDown(KeyCode.A) && index >= 1)
        {
            minus++;
        }
        else if (Input.GetKeyDown(KeyCode.D) && index <= 3)
        {
            minus--;
        }

        Vector3[] buttonPos = { buttonPos1, buttonPos2, buttonPos3, buttonPos4, buttonPos5 };

        //curPos = new Vector3(buttonPos[index].x, 0, buttonPos[index].z);
        curPos = buttonPos[index];

        //curPos = player.transform.position;
        Debug.Log($"인풋매니저에서 계산한 위치값{curPos}");
    }
}
