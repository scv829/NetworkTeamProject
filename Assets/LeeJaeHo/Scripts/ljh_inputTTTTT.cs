using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;




public class ljh_inputTTTTT : MonoBehaviourPun
{
    int curUserNum;
    
    [SerializeField] ljh_UIManager uiManager;
    [SerializeField] GameObject buttonParent; // 인원수에 맞게 초기화해줘야함
    [SerializeField] GameObject posParent;  // 인원수에 맞게 초기화 해줘야함

    [SerializeField] GameObject player;
    [SerializeField] GameObject button;
    [SerializeField] GameObject spotlight;

    [Header("유저가 서는 자리 좌표")]
    [SerializeField] Vector3 Pos1;
    [SerializeField] Vector3 Pos2;
    [SerializeField] Vector3 Pos3;
    [SerializeField] Vector3 Pos4;
    [SerializeField] Vector3 Pos5;

    [Header("유저가 서는 자리 오브젝트")]
    [SerializeField] public GameObject PosObj1;
    [SerializeField] public GameObject PosObj2;
    [SerializeField] public GameObject PosObj3;
    [SerializeField] public GameObject PosObj4;
    [SerializeField] public GameObject PosObj5;

    [Header("버튼 오브젝트")]
    [SerializeField] public GameObject buttonObj1;
    [SerializeField] public GameObject buttonObj2;
    [SerializeField] public GameObject buttonObj3;
    [SerializeField] public GameObject buttonObj4;
    [SerializeField] public GameObject buttonObj5;

    [Header("n명 남은 상황의 버튼 부모 오브젝트")]
    [SerializeField] public ljh_Button[] buttonParent5;
    [SerializeField] public ljh_Button[] buttonParent4;
    [SerializeField] public ljh_Button[] buttonParent3;
    [SerializeField] public ljh_Button[] buttonParent2;

    [Header("n명 남은 상황의 자리 부모 오브젝트")]
    [SerializeField] public ljh_Pos[] PosParent5;
    [SerializeField] public ljh_Pos[] PosParent4;
    [SerializeField] public ljh_Pos[] PosParent3;
    [SerializeField] public ljh_Pos[] PosParent2;

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
        defaultIndex = 2;// 4인일땐 2 3인일땐 2로 
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
        {
            curState = State.idle;
        }

        UserNumCalculate(curUserNum);

        Playing();



    }


    public void UserNumCalculate(int curUserNum)
    {
        switch (curUserNum)
        {
            case 4:
                //4인플
                break;

            case 3:
                //3인플
                break;

            case 2:
                //2인플
                break;
        }
    }

    public void Playing()
    {
        switch (curState)
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

        ljh_Button[] buttonObj = buttonParent.GetComponent<ljh_ButtonParent>().buttonArray;
        ljh_Pos[] pos = posParent.GetComponent<ljh_PosParent>().posArray;


        spotlight.transform.LookAt(buttonObj[index].transform.position);

        Vector3 curPos;
        return curPos = pos[index].transform.position;

    }

    public void SelectButton(Vector3 pos)
    {
        Vector3[] buttonPos = { Pos1, Pos2, Pos3, Pos4, Pos5 };
        GameObject[] buttonObj = { buttonObj1, buttonObj2, buttonObj3, buttonObj4, buttonObj5 };

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pos == buttonPos[index])
                button.GetComponent<ljh_Button>().PushedButton(buttonObj[index]);
        }
    }

    public ljh_Button[] MakeButtonArray(int curUser)
    {
        if(curUser == 5)
        {
            buttonParent5 = new ljh_Button[] { };
            return buttonParent5 = GetComponentsInChildren<ljh_Button>();
        }
        else if (curUser == 4)
        {
            buttonParent4 = new ljh_Button[] { };
            return buttonParent4 = GetComponentsInChildren<ljh_Button>();
        }
        else if (curUser == 3)
        {
            buttonParent3 = new ljh_Button[] { };
            return buttonParent3 = GetComponentsInChildren<ljh_Button>();
        }
        else if (curUser == 2)
        {
            buttonParent2 = new ljh_Button[] { };
            return buttonParent2 = GetComponentsInChildren<ljh_Button>();
        }
        else
            return null;
    }

    public ljh_Pos[] MakePosArray(int curUser)
    {
        if(curUser == 5)
        {
            PosParent5 = new ljh_Pos[] { };
            return PosParent5 = GetComponentsInChildren<ljh_Pos>();
        }
        else if (curUser == 4)
        {
            PosParent4 = new ljh_Pos[] { };
            return PosParent4 = GetComponentsInChildren<ljh_Pos>();
        }
        else if (curUser == 3)
        {
            PosParent3 = new ljh_Pos[] { };
            return PosParent3 = GetComponentsInChildren<ljh_Pos>();
        }
        else if (curUser == 2)
        {
            PosParent2 = new ljh_Pos[] { };
            return PosParent2 = GetComponentsInChildren<ljh_Pos>();
        }
        else
            return null;
    }
}
