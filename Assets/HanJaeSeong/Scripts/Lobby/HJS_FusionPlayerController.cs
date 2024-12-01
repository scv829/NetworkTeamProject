using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HJS_FusionPlayerController : NetworkBehaviour
{
    private CharacterController _controller;

    public float PlayerSpeed = 2f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        // Only move own player and not every other player. Each player controls its own player object.
        if (HasStateAuthority == false)
        {
            return;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

        if (move == Vector3.zero) return;

        _controller.Move(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }

    public void LeaveRoom()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        // 현재 위치를 기억하고
        HJS_PlayerPosition.Instance.playerPos = transform.position;

        // 연결을 종료한다 -> 내가 사라진다
        Runner.Shutdown();
    }

}
