using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] bool isStart;

    private Coroutine coroutine;

    private void Start()
    {
        // 색상 설정
        Vector3 vectorColor = photonView.Owner.GetPlayerColor();
        color.r = vectorColor.x; color.g = vectorColor.y; color.b = vectorColor.z;
        bodyRenderer.material.color = color;

        // 랜터 텍스쳐 설정
        playerCamera.targetTexture = playerRenderTextures[photonView.Owner.GetPlayerNumber()]; // 소유자의 랜더 텍스쳐로 설정
        
        Debug.LogWarning($"{photonView.Owner.NickName} spawn time {PhotonNetwork.Time}");

        // 애니메이터 설정
        animator = GetComponent<Animator>();

        // GameMaster 할당
        gameMaster = GameObject.FindWithTag("GameController").GetComponent<HJS_GameMaster>();
        gameMaster.inputStartEvent.AddListener(StartInput);
        gameMaster.inputStopEvent.AddListener(StopInput);
        gameMaster.changeShaderEvent.AddListener(ChangeShader);
        
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
            photonView.RPC("SetAnimationRPC", RpcTarget.All, "NoneTrigger");
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
                photonView.RPC("SetAnimationRPC", RpcTarget.All, "UpTrigger");
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                answer = HJS_RandomSlot.AnswerDirection.Botton;
                photonView.RPC("SetAnimationRPC", RpcTarget.All, "DownTrigger");
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                answer = HJS_RandomSlot.AnswerDirection.Right;
                photonView.RPC("SetAnimationRPC", RpcTarget.All, "RightTrigger");
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                answer = HJS_RandomSlot.AnswerDirection.Left;
                photonView.RPC("SetAnimationRPC", RpcTarget.All, "LeftTrigger");
                return true;
            }
            return false;
        });

        // 방장에게 선택한 내용 및 걸린 시간 전송
        photonView.RPC("SendAnswerRPC", RpcTarget.MasterClient, answer);
    }

    /// <summary>
    /// 셰이더 변경 로직
    /// 그림자를 받으면 잘 안보여서 color로 변경
    /// </summary>
    public void ChangeShader()
    {
        isStart = !isStart;
        bodyRenderer.material.shader = Shader.Find( (isStart) ? "Unlit/Color" : "Standard");
    }

    [PunRPC]
    public void SendAnswerRPC(HJS_RandomSlot.AnswerDirection answer, PhotonMessageInfo messageInfo)
    {
        gameMaster.AddPlayerAnswer(answer, messageInfo);
    }

    [PunRPC]
    private void SetAnimationRPC(string dir, PhotonMessageInfo messageInfo)
    {
        animator.SetTrigger(dir);
    }

}
