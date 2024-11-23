/*using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;




public class ljh_InputManager_Old : MonoBehaviourPun
{

    [SerializeField] ljh_UIManager uiManager;

    [SerializeField] GameObject player;
    [SerializeField] GameObject button;
    [SerializeField] GameObject spotlight;


    [SerializeField] Vector3 Pos1;
    [SerializeField] Vector3 Pos2;
    [SerializeField] Vector3 Pos3;
    [SerializeField] Vector3 Pos4;
    [SerializeField] Vector3 Pos5;

    [SerializeField] public GameObject PosObj1;
    [SerializeField] public GameObject PosObj2;
    [SerializeField] public GameObject PosObj3;
    [SerializeField] public GameObject PosObj4;
    [SerializeField] public GameObject PosObj5;

    [SerializeField] public GameObject buttonObj1;
    [SerializeField] public GameObject buttonObj2;
    [SerializeField] public GameObject buttonObj3;
    [SerializeField] public GameObject buttonObj4;
    [SerializeField] public GameObject buttonObj5;

    Vector3[] buttonPos;

    State curState;
    public Vector3 _curPos;

    int defaultIndex;
    int minus;
    [SerializeField] int index;

    private void OnEnable()
    {
        curState = State.idle;
    }
    private void Start()
    {
        defaultIndex = 2;
        minus = 0;
    }

    private void Update()
    {
        
         //Comment : 테스트용도
        if (Input.GetKeyDown(KeyCode.R))
        {
            curState = State.choice;
        }
        if (Input.GetKeyDown(KeyCode.T))
            curState = State.idle;


        switch(curState)
        {
            case State.idle:
                uiManager.ShowUiIdle();
                FindPlayer();
                break;

            case State.move:
                uiManager.ShowUiMove();
                break;
            
            case State.choice:
                uiManager.ShowUiChoice();
                _curPos = ChoiceAnswer();
                SelectButton(_curPos);
                break;

        }
        

    }

    public void FindPlayer()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

    }

    public Vector3 ChoiceAnswer()
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

        Vector3[] Pos= 
        { 
            PosObj1.transform.position,
            PosObj2.transform.position,
            PosObj3.transform.position,
            PosObj4.transform.position,
            PosObj5.transform.position
        } ;
        GameObject[] buttonObj = { buttonObj1, buttonObj2, buttonObj3, buttonObj4, buttonObj5 };

        spotlight.transform.LookAt(buttonObj[index].transform.position);
        
        Vector3 curPos;
        return curPos = Pos[index];

    }

    public void SelectButton(Vector3 pos)
    {
        Vector3[] buttonPos = { Pos1, Pos2, Pos3, Pos4, Pos5 };
        GameObject[] buttonObj = { buttonObj1, buttonObj2, buttonObj3, buttonObj4, buttonObj5 };

        if (Input.GetKeyDown(KeyCode.Space))
        {
           // if (pos == buttonPos[index])
               // button.GetComponent<ljh_Button>().PushedButton(buttonObj[index]);
        }
    }
}
*/