using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_MechaMarathonGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private KHS_PlayerController[] _playerController;  // TODO : 배열 초기화 해야함. 매치메이킹 이후에 이루어지는 작업진행
    public KHS_PlayerController[] PlayerController { get { return _playerController; } set { _playerController = value; } }

    [SerializeField] private KHS_HeyHoController[] _heyHoController;    // TODO : 배열 초기화 해야함. 매치메이킹 이후에 이루어지는 작업진행
    public KHS_HeyHoController[] HeyHoController { get { return _heyHoController; } set { _heyHoController = value; } }

    [SerializeField] private bool _isStarted;
    public bool IsStarted { get { return _isStarted; } set { _isStarted = value; } }

    [SerializeField] private bool _isFinished;
    public bool IsFinished { get { return _isFinished; ; } set { _isFinished = value; } }

    //[SerializeField] private Player[] _sortedPlayers;
    //public Player[] SortedPlayer { get { return _sortedPlayers; } set { _sortedPlayers = value; } }

    private float _gameTimer;
    public int[] _totalCount;   // TODO : 배열 초기화 해야함. 매치메이킹 이후에 이루어지는 작업진행

    public int _playersLoaded = 0;
    private int _totalplayers;

    // Start 함수
    private void Start()
    {
        // 게임씬에 진입했을시에는 false
        IsStarted = false;

        // 마스터 클라이언트만 진행
        if(PhotonNetwork.IsMasterClient)
        {
            _totalplayers = PhotonNetwork.CurrentRoom.PlayerCount;  // 총 플레이어 수 저장
        }
    }

    // Update 함수
    private void Update()
    {
        // 게임 진행중이 아니면 return
        if (IsStarted == false)
            return;

        // 게임 시간 측정
        _gameTimer += Time.deltaTime;
        //Debug.Log($"현재 진행중인 시간 : {_gameTimer}");

        if (_gameTimer >= 5f) // 5초가 지나가면
        {
            // 게임 종료
            IsStarted = false;

            // 헤이호 출발을 위한 true
            IsFinished = true;

            // 타이머는 0으로
            //_gameTimer = 0;

            Debug.Log("게임 종료!");

            // 플레이어 컨트롤러 배열의 길이만큼 반복
            for (int i = 1; i < _playerController.Length; i++)
            {
                // 현재 플레이어 컨트롤러가 배열에 할당되지 않았으면 break.
                if(_playerController[i] == null)
                    break;

                // 플레이어들의 총합 입력수를 입력
                _totalCount[i] = _playerController[i].TotalInputCount;
                Debug.Log($"Player {i} 총 입력 횟수 : {_totalCount[i]}");
            }

        }

    }

    // 방에 참가했을때 호출되는 함수
    public override void OnJoinedRoom()
    {
        // 준비시간 조금 가지는 코루틴 실행
        photonView.RPC("PlayerReady", RpcTarget.MasterClient);
        //StartCoroutine(StartDelayRoutine());
    }

    // 네트워크에 진입 후 준비에 필요한 시간 살짝 주는 함수
    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);    // 네트워크 준비에 필요한 시간 살짝 주기
        TestGameStart();
    }

    // 테스트 게임씬 시작 함수
    public void TestGameStart()
    {
        Debug.Log("게임 시작");
        // TODO : 테스트용 게임 시작 부분
        PlayerSpawn();

        //if (PhotonNetwork.IsMasterClient == false)
        //    return;

        //헤이호 생성
        HeyHoSpawn();

        // 플레이어, 헤이호 생성된 후 게임시작 카운트 다운 들어가기
        StartCoroutine(DelayGameStart());
    }

    // 플레이어, 헤이호의 위치를 위한 함수
    private Vector3 SetPosition()
    {
        // 현재 자신의 ActorNumber 대로 위치 설정
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                return new Vector3(-7f, 1f, -8f);
            case 2:
                return new Vector3(-3f, 1f, -8f);
            case 3:
                return new Vector3(3f, 1f, -8f);  
            case 4:
                return new Vector3(7f, 1f, -8f);
        }
        return Vector3.zero;
    }

    // 플레이어 스폰 함수
    private void PlayerSpawn()
    {
        Vector3 spawnPosition = SetPosition();

        //Color color = Random.ColorHSV();
        //object[] data = { color.r, color.g, color.b };

        PhotonNetwork.Instantiate("KHS/KHS_Player", spawnPosition, Quaternion.identity/*, data : data*/);
    }

    // 헤이호 스폰 함수
    private void HeyHoSpawn()
    {
        Vector3 spawnPosition = SetPosition();
        spawnPosition.z += 2f;

        //Color color = Random.ColorHSV();
        //object[] data = { color.r, color.g, color.b };

        PhotonNetwork.Instantiate("KHS/KHS_Hey-Ho", spawnPosition, Quaternion.identity/*, data : data*/);

    }

    WaitForSeconds _delay = new WaitForSeconds(1f);

    // 카운트다운 후 게임 시작 함수
    private IEnumerator DelayGameStart()
    {

        Debug.Log("3!");
        yield return _delay;

        Debug.Log("2!");
        yield return _delay;

        Debug.Log("1!");
        yield return _delay;

        Debug.Log("Start!!!");
        IsStarted = true; // 게임 시작 상태로 변경
    }

    [PunRPC]
    public void PlayerReady()
    {
        {
            _playersLoaded++;
            Debug.Log($"현재 로딩된 플레이어 : {_playersLoaded}");

            if (_playersLoaded == 2)
            {
                StartCoroutine(StartDelayRoutine());
            }
        }
    }


}
