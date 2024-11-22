using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HJS_PlayerController : MonoBehaviourPun
{
    [SerializeField] HJS_RandomSlot.AnswerDirection answer; // 플레이어의 입력한 값
    [SerializeField] HJS_GameMaster gameMaster;

    private Coroutine coroutine;

    public void StartInput()
    {
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} startInput");
        answer = HJS_RandomSlot.AnswerDirection.None;
        coroutine = StartCoroutine(InputRoutine());                  // 입력하기
    }

    public void StopInput()
    {
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} stopInput");
        StopCoroutine(coroutine);
        coroutine = null;
    }

    IEnumerator InputRoutine()
    {
        double time = PhotonNetwork.Time;

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
        // PhotonNetwork.LocalPlayer.SetAnswer(answer, Mathf.Abs((float)(time - PhotonNetwork.Time)));

        photonView.RPC("CheckAnswerPun", RpcTarget.MasterClient, answer);
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} {answer}");
    }

    [PunRPC]
    public void SendAnswerRPC(HJS_RandomSlot.AnswerDirection answer, PhotonMessageInfo messageInfo)
    {
        gameMaster.AddPlayerAnswer(answer, messageInfo);
    }

}
