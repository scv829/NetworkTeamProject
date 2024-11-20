using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HJS_PlayerController : MonoBehaviour
{
    [SerializeField] HJS_RandomSlot.AnswerDirection answer; // 플레이어의 입력한 값
    [SerializeField] HJS_RandomSlot slotMaster;

    private Coroutine coroutine;
    private Coroutine delayCoroutine;

    private void Start()
    {
        answer = HJS_RandomSlot.AnswerDirection.None;
        coroutine = StartCoroutine(InputRoutine());                  // 입력하기
        delayCoroutine = StartCoroutine(delayRoutine());             // 5초 이후에 입력이 없으면 멈추기
    }

    IEnumerator delayRoutine()
    {
        yield return new WaitForSeconds(5f);
        StopCoroutine(coroutine);
        Debug.Log(slotMaster.Answer.Equals(answer));
    }

    IEnumerator InputRoutine()
    {
        // 입력 대기
        yield return new WaitUntil(() => 
        {
            // 입력 
            if (Input.GetKeyDown(KeyCode.W))
            {
                answer = HJS_RandomSlot.AnswerDirection.Top;
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                answer = HJS_RandomSlot.AnswerDirection.Botton;
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                answer = HJS_RandomSlot.AnswerDirection.Right;
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                answer = HJS_RandomSlot.AnswerDirection.Left;
                return true;
            }
            return false;
        });

        // 입력에 대한 연산
        Debug.Log(slotMaster.Answer.Equals(answer));
        StopCoroutine(delayCoroutine);
    }
}
