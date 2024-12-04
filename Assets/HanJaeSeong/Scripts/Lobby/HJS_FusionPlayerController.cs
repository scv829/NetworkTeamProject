using Cinemachine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Fusion;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HJS_FusionPlayerController : NetworkBehaviour, IAfterSpawned
{
    private CharacterController _controller;

    [Header("Child")]
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] TMP_Text nickname;
    [SerializeField] GameObject frontCanvas;
    [SerializeField] GameObject backCanvas;
    [Header("Spec")]
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float rotateSpeed = 90f;
    private Vector3 moveDir;

    // 네트워크 상에서의 위치
    [Networked] string name { get; set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public override void Spawned()
    {
       if(HasStateAuthority)
       {
            camera.Priority = 15;
            name = PhotonNetwork.LocalPlayer.NickName;
            gameObject.GetComponent<NetworkTransform>().Teleport(HJS_PlayerPosition.Instance.PlayerPos);
            frontCanvas.layer = 26;
            backCanvas.layer = 26;
            SetPlayer();
        }
       nickname.text = name;
    }

    // 접속이 끊어질 때 <- 파괴가 된다 <- 그때 데이터에 저장aa
    private void OnDestroy()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        HJS_PlayerPosition.Instance.PlayerPos = transform.position;
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false)
        {
            return;
        }
        Vector3 move = Vector3.forward * moveDir.z * Runner.DeltaTime * playerSpeed;
        float rotate = moveDir.x * rotateSpeed * Runner.DeltaTime;

        // 위치를 동기화
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + rotate, 0);
        if(!move.Equals(Vector3.zero))
        transform.position += transform.TransformDirection(move);
    }

    private void Update()
    {
        // 소유권이 아니면 안하기
        if (!HasStateAuthority) return;

        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");
    }
    public void LeaveScene()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        Debug.Log("SavePosition");
        // 현재 위치를 기억하고
        HJS_PlayerPosition.Instance.PlayerPos = transform.position;

        Debug.Log("PauseServer");
        // 연결을 멈춘다 -> 내가 사라진다
        Runner.Shutdown();
    }

    public void AfterSpawned()
    {
        _controller.enabled = true;
    }

    private void SetPlayer()
    {
        HJS_RoomController roomController = FindFirstObjectByType<HJS_RoomController>();
        HJS_RandomMatchController matchController = FindFirstObjectByType<HJS_RandomMatchController>();

        roomController.Player = this;
        matchController.Player = this;
    }
}
