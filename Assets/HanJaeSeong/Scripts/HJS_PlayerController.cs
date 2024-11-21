using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

{
    [SerializeField] HJS_RandomSlot.AnswerDirection answer; // 플레이어의 입력한 값

    private Coroutine coroutine;

    private void Start()
    {
        answer = HJS_RandomSlot.AnswerDirection.None;
        coroutine = StartCoroutine(InputRoutine());                  // 입력하기
    }

    public void StopInput()
    {
        StopCoroutine(coroutine);
        coroutine = null;
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

        // 선택한 내용 및 걸린 시간 전송
        PhotonNetwork.LocalPlayer.SetAnswer(answer, Mathf.Abs((float)(time - PhotonNetwork.Time)));
    }
}
