using Photon.Pun;
using System.Collections;
using UnityEngine;

public class KHS_MechaMarathonGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private KHS_PlayerController[] _playerController;  
    public KHS_PlayerController[] PlayerController { get { return _playerController; } set { _playerController = value; } }

    [SerializeField] private KHS_HeyHoController[] _heyHoController;    
    public KHS_HeyHoController[] HeyHoController { get { return _heyHoController; } set { _heyHoController = value; } }

    [SerializeField] private KHS_UIManager _uiManager;
    public KHS_UIManager UIManager { get { return _uiManager; } set { _uiManager = value; } }

    [SerializeField] private bool _isStarted;
    public bool IsStarted { get { return _isStarted; } set { _isStarted = value; } }

    [SerializeField] private bool _isInputFinished;
    public bool IsInputFinished { get { return _isInputFinished; ; } set { _isInputFinished = value; } }

    private float _gameTimer;
    public int[] _totalCount;   // TODO : 배열 초기화 해야함. 매치메이킹 이후에 이루어지는 작업진행

    public int _playersLoaded = 0;
    public int _heyHoFinished = 0;
    private int _totalplayers;
    private float _finishTimer;

    WaitForSeconds _delay = new WaitForSeconds(1f);
    WaitForSeconds _delayHeyHo;

    // Start 함수
    private void Start()
    {
        // 게임씬에 진입했을시에는 false
        IsStarted = false;

        // 마스터 클라이언트만 진행
        if (PhotonNetwork.IsMasterClient)
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
            IsInputFinished = true;
            Debug.Log("게임 종료!");


        }

    }

    public void MovingHeyHo()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("mmmmmmm마스터 클라이언트로 확인.mmmmmmm");
            photonView.RPC("FinishGame", RpcTarget.AllBuffered);
            //FinishGame();
        }
        else
        {
            Debug.Log("mmmmm일반 클라이언트롤 확인.mmmmm");
            photonView.RPC("FinishGame", RpcTarget.AllBuffered);
            //FinishGame();
        }
    }


    private int FindWinnerHeyHo()
    {
        int _maxCount = 0;
        int _heyHoIndex = 0;

        Debug.Log($"===============플레이어 인풋카운트 비교 시작=================");
        for (int i = 1; i < _playerController.Length; i++)
        {
            if (_playerController[i] == null)
            {
                continue;
            }
            Debug.Log($"{i}번째 인덱스는 {_playerController[i].TotalInputCount}");
            if (_playerController[i].TotalInputCount > _maxCount)
            {
                _maxCount = _playerController[i].TotalInputCount;
                _heyHoIndex = i;
            }
        }
        Debug.Log($"###########플레이어 인풋카운트 비교 끝###########{_heyHoIndex}, {_maxCount}");
    
        return _heyHoIndex;
    }

    // 방에 참가했을때 호출되는 함수
    public override void OnJoinedRoom()
    {
        
        GameObject player = PlayerSpawn();  // 방에 들어왔을때 플레이어 생성
        GameObject heyho = HeyHoSpawn();   // 방에 들어왔을때 헤이호 생성
        heyho.GetComponent<KHS_HeyHoController>()._playerController = player.GetComponent<KHS_PlayerController>();
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
    private GameObject PlayerSpawn()
    {
        Vector3 spawnPosition = SetPosition();

        return PhotonNetwork.Instantiate("KHS/KHS_Player", spawnPosition, Quaternion.identity/*, data : data*/);
    }

    // 헤이호 스폰 함수
    private GameObject HeyHoSpawn()
    {
        Vector3 spawnPosition = SetPosition();
        spawnPosition.z += 2f;

        return PhotonNetwork.Instantiate("KHS/KHS_Hey-Ho", spawnPosition, Quaternion.identity/*, data : data*/);

    }


    // 카운트다운 후 게임 시작 함수
    private IEnumerator DelayGameStart()
    {
        _uiManager.CountDownGame();

        Debug.Log("3!");
        yield return _delay;

        Debug.Log("2!");
        yield return _delay;

        Debug.Log("1!");
        yield return _delay;

        Debug.Log("Start!!!");
        IsStarted = true; // 게임 시작 상태로 변경
    }

    // 입력받은 카운트 중 가장 높은 카운트가 눌린 헤이호의 이동시간만큼 딜레이
    private IEnumerator MovedHeyHo()
    {
        int find = FindWinnerHeyHo();
        KHS_HeyHoController heyho = _heyHoController[find];

        if( heyho == null )
        {
            Debug.Log($"@@@@@@@@@@@@@@@@{find}@@@@@@@@@@@@@@@@@");
        }
        // 제일 멀리 날아가는 헤이호의 이동 시간 초기화
        _finishTimer = heyho.FinishTime;


        // 해당 시간만큼 딜레이 초기화
        _delayHeyHo = new WaitForSeconds(_finishTimer);
        Debug.Log($"{_finishTimer}만큼 걸릴 예정");

        // 딜레이
        yield return _delayHeyHo;
        ResultGame();
    }

    /// <summary>
    /// 플레이어가 레디가 되었는지 확인하기 위한 함수
    /// </summary>
    public void PlayerReady()
    {
        _playersLoaded++;

        Debug.Log($"현재 로딩된 플레이어 : {_playersLoaded}");

        if (_playersLoaded == 2)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                //RPC
                photonView.RPC("StartRPC", RpcTarget.All);
            }
        }
    }

    private void ResultGame()
    {
        Debug.Log($"승자는 {FindWinnerHeyHo()} 번 플레이어 입니다!");
        _uiManager.OnWinner(FindWinnerHeyHo());
    }

    [PunRPC]
    private void FinishGame()
    {
       
        Debug.Log("_heyHoFinished 값 올림");
        if (_heyHoFinished >= 1)
        {
            Debug.Log("헤이호 날아가는 코루틴 시작 RPC");
            StartCoroutine(MovedHeyHo());
        }
        else
        {
            Debug.Log("코루틴 아직 시작안함");
        }

    }

    [PunRPC]
    private void StartRPC()
    {
        StartCoroutine(StartDelayRoutine());
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_playersLoaded);    // 플레이어 로드된 인원 변수
            stream.SendNext(_finishTimer);    // 제일 많이 날아가는 헤이호가 걸리는 시간 변수
            stream.SendNext(_totalCount[1]);    // 1번 플레이어 총 카운트 횟수
            stream.SendNext(_totalCount[2]);    // 2번 플레이어 총 카운트 횟수
            stream.SendNext(_totalCount[3]);    // 3번 플레이어 총 카운트 횟수
            stream.SendNext(_totalCount[4]);    // 4번 플레이어 총 카운트 횟수
            stream.SendNext(_heyHoFinished);
        }
        else if (stream.IsReading)
        {
            _playersLoaded = (int)stream.ReceiveNext(); // 플레이어 로드된 인원 변수
            _finishTimer = (float)stream.ReceiveNext(); // 제일 많이 날아가는 헤이호가 걸리는 시간 변수
            _totalCount[1] = (int)stream.ReceiveNext(); // 1번 플레이어 총 카운트 횟수
            _totalCount[2] = (int)stream.ReceiveNext(); // 2번 플레이어 총 카운트 횟수
            _totalCount[3] = (int)stream.ReceiveNext(); // 3번 플레이어 총 카운트 횟수
            _totalCount[4] = (int)stream.ReceiveNext(); // 4번 플레이어 총 카운트 횟수
            _heyHoFinished = (int)stream.ReceiveNext();
        }
    }
}