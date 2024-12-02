using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class HYJ2_PlayerController : MonoBehaviourPun
{
    [SerializeField] float moveSpeed; // 플레이어의 속도 조절
    [SerializeField] float playerCurMoveSpeed; // 플레이어의 현재 이동속도
    [SerializeField] Animator playerAnimator;
    [SerializeField] BoxCollider touchArea;
    [SerializeField] Rigidbody playerRigidBody;


    private Vector3 lastPosition; // 플레이어의 이전 지점 > 현재 이동속도를 구하기 위한 변수
    private bool isAttack = false; // 플레이어 공격 판별 변수

    private void Update()
    {
        if(photonView.IsMine == false)
        {
            return;
        }

        if (!isAttack) // 플레이어가 공격 중이 아닐 때
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                photonView.RPC("HYJ2_TouchRPC", RpcTarget.All);
            }
        }
    }

    private void FixedUpdate()
    {
        //PlayerRunningAnimator();
    }

    private void PlayerRunningAnimator() // 플레이어의 속도에 따라 애니메이션 실행
    {
        playerCurMoveSpeed = GetPlayerSpeed(); // 플레이어의 현재 속도
        if (playerCurMoveSpeed > 0)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else if (playerCurMoveSpeed <= 0)
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }

    private void Move() // 플레이어 이동
    {
        
        //position 기반 이동
        Vector3 moveDir = Vector3.zero;
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.z = Input.GetAxisRaw("Vertical");
        

        if (moveDir == Vector3.zero)
        {
            playerAnimator.SetBool("isRunning", false);
            return;
        }
        playerAnimator.SetBool("isRunning", true);
        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);
        transform.forward = moveDir.normalized;
        

        //RigidBody 기반 이동

    }


    float GetPlayerSpeed() // 플레이어의 현재 이동 속도
    {
        float speed = (((transform.position-lastPosition).magnitude)/Time.deltaTime);
        lastPosition = transform.position;

        return speed;
    }

    IEnumerator PlayerTouch() // 플레이어 터치 애니메이션 플레이 & 기능
    {
        isAttack = true;
        playerAnimator.SetTrigger("Attack");
        touchArea.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        touchArea.gameObject.SetActive(false);
        isAttack = false;
    }

    [PunRPC]
    private void HYJ2_TouchRPC() // 터치를 RPC로 사용하기 위함
    {
        StartCoroutine(PlayerTouch());
    }
}