using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

enum State
{ 
    idle,
    move,
    choice
};


public class ljh_InputManager : MonoBehaviour
{
    PhotonView photonView;
    [SerializeField] GameObject player;
    [SerializeField] GameObject button;

    public Vector3 curPos;

    Vector3 buttonPos1;
    Vector3 buttonPos2;
    public Vector3 buttonPos3;
    Vector3 buttonPos4;
    Vector3 buttonPos5;

    [SerializeField] GameObject buttonObj1;
    [SerializeField] GameObject buttonObj2;
    [SerializeField] GameObject buttonObj3;
    [SerializeField] GameObject buttonObj4;
    [SerializeField] GameObject buttonObj5;

    State curState;

    int defaultIndex;
    int minus;
    [SerializeField] int index;

    private void OnEnable()
    {
        curState = State.idle;
    }
    private void Start()
    {
        Vector3[] buttonPos = { buttonPos1, buttonPos2, buttonPos3, buttonPos4, buttonPos5 };
        GameObject[] buttonObj = { buttonObj1, buttonObj2, buttonObj3, buttonObj4, buttonObj5 };
        

        for (int i = 0; i < buttonPos.Length - 1; i++)
        {
            buttonPos[i] = buttonObj[i].transform.position;
        }
        Debug.Log(buttonPos[3]);
        Debug.Log("는 인풋매니저꺼");

        defaultIndex = 2;
        minus = 0;
    }

    private void Update()
    {
        // Comment : 테스트용도
        if (Input.GetKeyDown(KeyCode.R))
            curState = State.choice;

        if (Input.GetKeyDown(KeyCode.T))
            curState = State.idle;


        switch(curState)
        {
            case State.idle:
                FindPlayer();
                break;

            case State.move:
                break;
            
            case State.choice:
                ChoiceAnswer();
                SelectButton(curPos);
                break;

        }
        

    }

    public void FindPlayer()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

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

        curPos = buttonPos[index];
    }

    public void SelectButton(Vector3 pos)
    {
        Vector3[] buttonPos = { buttonPos1, buttonPos2, buttonPos3, buttonPos4, buttonPos5 };
        GameObject[] buttonObj = { buttonObj1, buttonObj2, buttonObj3, buttonObj4, buttonObj5 };

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pos == buttonPos[index])
                button.GetComponent<ljh_Button>().PushedButton(buttonObj[index]);
        }
    }
}
