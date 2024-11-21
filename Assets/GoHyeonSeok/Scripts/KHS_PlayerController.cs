using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_PlayerController : MonoBehaviourPun
{
    [SerializeField] private int _totalInputCount;
    public int TotalInputCount { get { return _totalInputCount; } set { _totalInputCount = value; } }

    [SerializeField] private KHS_MechaMarathonGameManager _mechaMarathonGameManager;
    [SerializeField] private KHS_HeyHoController _heyHoController;


    private void Awake()
    {
        // (임시) 해당 오브젝트가 생성되었을때 게임매니저를 찾는 경우
        _mechaMarathonGameManager = FindAnyObjectByType<KHS_MechaMarathonGameManager>();

        // (임시) 해당 오브젝트가 생성되었을때 헤이호를 찾는 경우
        //_heyHoController = FindAnyObjectByType<KHS_HeyHoController>();
    }

    private void Start()
    {
        // 현재 접속된 자신의 ActorNumber대로 이 스크립트를 참조시켜준다.
        _mechaMarathonGameManager.PlayerController[PhotonNetwork.LocalPlayer.ActorNumber] = this;
        Debug.Log($"{PhotonNetwork.LocalPlayer.ActorNumber}번째 플레이어");
    }

    private void Update()
    {
        // 시작하지 않은 상태면 return
        if (_mechaMarathonGameManager.IsStarted == false)
            return;

        if(photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                TotalInputCount++;
            }
        }
        else return;
    }
}
