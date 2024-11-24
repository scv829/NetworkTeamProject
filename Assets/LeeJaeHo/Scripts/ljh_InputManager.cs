using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;




public class ljh_InputManager : MonoBehaviourPun
{
    int curUserNum;
    
    [SerializeField] ljh_UIManager uiManager;
    [SerializeField] ljh_CartManager cartManager;

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
    [SerializeField] GameObject buttonParent4;
    [SerializeField] GameObject buttonParent3;
    [SerializeField] GameObject buttonParent2;

    [Header("n명 남은 상황의 자리 부모 오브젝트")]
    [SerializeField] GameObject posParent4;
    [SerializeField] GameObject posParent3;
    [SerializeField] GameObject posParent2;

    public Vector3 _curPos;

    GameObject boom;

    public int defaultIndex;
    public int minus;
    [SerializeField] public int index;





    public Coroutine _boomCoroutine;

    private void OnEnable()
    {
    }
    private void Start()
    {
        defaultIndex = ljh_GameManager.instance.defaultIndex ;// 4인일땐 2 3인일땐 2로 
        minus = 0;
    }

    private void Update()
    {

        //Comment : 테스트용도
        if (Input.GetKeyDown(KeyCode.R))
        {
            ljh_GameManager.instance.curState = State.choice;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ljh_GameManager.instance.curState = State.idle;
        }




    }

    public void FindPlayer()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

    }

    public Vector3 ChoiceAnswer()
    {
        curUserNum = ljh_GameManager.instance.curUserNum;
        Debug.Log(curUserNum);
        ljh_Button[] buttonObj = MakeButtonArray(curUserNum); // curUserNum으로 바꿔야함
        ljh_Pos[] pos = MakePosArray(4);

        index = defaultIndex - minus;

        if (Input.GetKeyDown(KeyCode.A) && index >= 1)
        {
            minus++;
        }
        else if (Input.GetKeyDown(KeyCode.D) && index < buttonObj.Length-1)
        {
            minus--;
        }


        spotlight.transform.LookAt(buttonObj[index].transform.position);

        return pos[index].transform.position;

    }

    public void SelectButton(Vector3 curPos)
    {
        ljh_Button[] buttonObj = MakeButtonArray(curUserNum);
        ljh_Pos[] pos = MakePosArray(curUserNum);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (curPos == pos[index].transform.position)
            {
                buttonObj[index].GetComponent<ljh_Button>().PushedButton(buttonObj[index]);
                if (_boomCoroutine == null)
                    _boomCoroutine = StartCoroutine(BoomCoroutine(buttonObj[index]));
            }
        }
    }

    IEnumerator BoomCoroutine(ljh_Button _button)
    {
        boom.GetComponent<ljh_Boom>().Vibe();
        yield return new WaitForSeconds(Random.Range(1.5f,2.5f));
        _button.SelectedButtonAction();

    }

    public ljh_Button[] MakeButtonArray(int curUser)
    {
        if (curUser == 4)
        {
            ljh_Button[] buttonParents = buttonParent4.GetComponent<ljh_ButtonParent>().buttonArray;
            return buttonParents;
        }
        else if (curUser == 3)
        {
            ljh_Button[] buttonParents = buttonParent3.GetComponent<ljh_ButtonParent>().buttonArray;
            return buttonParents;
        }
        else if (curUser == 2)
        {
            ljh_Button[] buttonParents = buttonParent2.GetComponent<ljh_ButtonParent>().buttonArray;
            return buttonParents;
        }
        else
            return null;
    }

    public ljh_Pos[] MakePosArray(int curUser)
    {
        if (curUser == 4)
        {
            ljh_Pos[] PosParents = posParent4.GetComponent<ljh_PosParent>().posArray;
            return PosParents;
        }
        else if (curUser == 3)
        {
            ljh_Pos[] PosParents = posParent3.GetComponent<ljh_PosParent>().posArray;
            return PosParents;
        }
        else if (curUser == 2)
        {
            ljh_Pos[] PosParents = posParent2.GetComponent<ljh_PosParent>().posArray;
            return PosParents;
        }
        else
            return null;
    }

    
    



    
}
