using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using UnityEngine;

public class HJS_PlayerController : MonoBehaviourPun
{
    [SerializeField] Camera playerCamera;
    [SerializeField] RenderTexture[] playerRenderTextures;

    [SerializeField] Animator animator;

    [SerializeField] HJS_RandomSlot.AnswerDirection answer; // 플레이어의 입력한 값
    [SerializeField] HJS_GameMaster gameMaster;

    /* 색상 */
    [SerializeField] Renderer bodyRenderer;
    [SerializeField] Color color;

    private Coroutine coroutine;

    private void Start()
    {
        // 색상 설정
        Vector3 vectorColor = photonView.Owner.GetPlayerColor();
        color.r = vectorColor.x; color.g = vectorColor.y; color.b = vectorColor.z;
        bodyRenderer.material.color = color;

        // 랜터 텍스쳐 설정
        //playerCamera.targetTexture = playerRenderTextures[PhotonNetwork.LocalPlayer.GetPlayerNumber()];
        photonView.RPC("SetRenderTextureRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.GetPlayerNumber());

        // 애니메이터 설정
        animator = GetComponent<Animator>();

        // GameMaster 할당
        gameMaster = GameObject.FindWithTag("GameController").GetComponent<HJS_GameMaster>();
        gameMaster.inputStartEvent.AddListener(StartInput);
        gameMaster.inputStopEvent.AddListener(StopInput);
    }

    public void SetRenderTexture(int number)
    {
        playerCamera.targetTexture = playerRenderTextures[PhotonNetwork.LocalPlayer.GetPlayerNumber()];
    }

    public void StartInput()
    {
        if(coroutine == null)
        {
            answer = HJS_RandomSlot.AnswerDirection.None;
            coroutine = StartCoroutine(InputRoutine());                  // 입력하기
        }
    }

    public void StopInput()
    {
        if(coroutine != null)
        {
            animator.SetTrigger("NoneTrigger");
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    IEnumerator InputRoutine()
    {
        if (photonView.IsMine == false) yield break;

        // 입력 대기
        yield return new WaitUntil(() =>
        {
            // 입력 
            if (Input.GetKeyDown(KeyCode.W))
            {
                answer = HJS_RandomSlot.AnswerDirection.Top;
                animator.SetTrigger("UpTrigger");
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                answer = HJS_RandomSlot.AnswerDirection.Botton;
                animator.SetTrigger("DownTrigger");
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                answer = HJS_RandomSlot.AnswerDirection.Right;
                animator.SetTrigger("RIghtTrigger");
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                answer = HJS_RandomSlot.AnswerDirection.Left;
                animator.SetTrigger("LeftTrigger");
                return true;
            }
            return false;
        });

        // 방장에게 선택한 내용 및 걸린 시간 전송
        photonView.RPC("SendAnswerRPC", RpcTarget.MasterClient, answer);
    }

    [PunRPC]
    public void SendAnswerRPC(HJS_RandomSlot.AnswerDirection answer, PhotonMessageInfo messageInfo)
    {
        gameMaster.AddPlayerAnswer(answer, messageInfo);
    }

    [PunRPC]
    private void SetRenderTextureRPC(int number)
    {
        playerCamera.targetTexture = playerRenderTextures[number];
    }

}
