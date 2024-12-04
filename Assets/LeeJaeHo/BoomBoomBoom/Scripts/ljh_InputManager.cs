using Cinemachine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;




public class ljh_InputManager : MonoBehaviourPun
{
    int curUserNum;

    [SerializeField] ljh_SpotLight _spotlight;
    [SerializeField] ljh_ButtonParent buttonParent;

    [SerializeField] GameObject player;


    [Header("n명 남은 상황의 버튼 부모 오브젝트")]
    [SerializeField] GameObject buttonParents;

    [Header("n명 남은 상황의 자리 부모 오브젝트")]
    [SerializeField] GameObject posParents;

    [SerializeField] GameObject boom;

    public int defaultIndex;
    public int minus;
    [SerializeField] public int index;


    public ljh_Button[] buttonObj;
    public ljh_Pos[] _pos;
    public Coroutine _boomCoroutine;

    private void Start()
    {
        defaultIndex = ljh_GameManager.instance.defaultIndex ;// 4인일땐 2 3인일땐 2로 
        minus = 0;

        buttonObj = buttonParents.GetComponent<ljh_ButtonParent>().buttonArray;
        _pos = posParents.GetComponent<ljh_PosParent>().posArray;
    }

    private void Update()
    {
        buttonObj = buttonParents.GetComponent<ljh_ButtonParent>().buttonArray;
        _pos = posParents.GetComponent<ljh_PosParent>().posArray;

        curUserNum = ljh_GameManager.instance.curUserNum;
    }

    //Comment : 현재 플레이어 찾아주는 함수
    public GameObject FindPlayer(GameObject _player)
    {
        if (player == null)
        {
            if ((int)ljh_GameManager.instance.myTurn == (int)_player.GetComponent<ljh_Player>().playerNumber)
            {
                return _player;
            }
            return null;
        }
        return null;
    }

    // comment : 초이스 상태에서 A 또는 D 키로 이동 함수
    public GameObject ChoiceAnswer()
    {
        curUserNum = ljh_GameManager.instance.curUserNum;

        index = defaultIndex - minus;

        if (Input.GetKeyDown(KeyCode.A) && index >= 1)
        {
            minus++;
        }
        else if (Input.GetKeyDown(KeyCode.D) && index < buttonObj.Length-1)
        {
            minus--;
        }

        _spotlight.MovingSpotlight(buttonObj, index);

        return _pos[index].gameObject;

    }

    //Comment : 현재 선택하려는 버튼 지정 함수
    public void SelectButton(Vector3 curPos)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (curPos == _pos[index].transform.position)
            {
                buttonObj[index].GetComponent<ljh_Button>().PushedButton(buttonObj[index]);
                if (_boomCoroutine == null)
                    // 선택한 버튼의 붐코루틴을 발동함
                    _boomCoroutine = StartCoroutine(BoomCoroutine(buttonObj[index]));
            }
        }
    }

    //선택한 버튼에서 붐코루틴 발동
    // 버튼패런트에서 셀렉티드 버튼 액션 발동 > 선택한 버튼을 넣어서
    IEnumerator BoomCoroutine(ljh_Button _button)
    {
        boom.GetComponent<ljh_Boom>().Vibe();

        yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f,2.5f));
        
        buttonParent.SelectedButtonAction(_button);

    }


    
}
