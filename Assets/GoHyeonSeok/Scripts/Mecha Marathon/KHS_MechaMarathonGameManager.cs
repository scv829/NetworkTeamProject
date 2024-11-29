using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class KHS_MechaMarathonGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private KHS_PlayerController[] _playerController;  // 플레이어를 담아두는 배열
    public KHS_PlayerController[] PlayerController { get { return _playerController; } set { _playerController = value; } }

    [SerializeField] private KHS_HeyHoController[] _heyHoController;    // 헤이호를 담아두는 배열
    public KHS_HeyHoController[] HeyHoController { get { return _heyHoController; } set { _heyHoController = value; } }

    [SerializeField] private KHS_UIManager _uiManager;  // UImanger
    public KHS_UIManager UIManager { get { return _uiManager; } set { _uiManager = value; } }

    [SerializeField] private bool _isStarted;   // 게임이 시작했는지 여부 bool
    public bool IsStarted { get { return _isStarted; } set { _isStarted = value; } }

    [SerializeField] private bool _isInputFinished; // 입력하는 시간이 끝났늕지 여부 bool
    public bool IsInputFinished { get { return _isInputFinished; ; } set { _isInputFinished = value; } }

    [SerializeField] private Player[] _curPhotonPlayer;
    public Player[] CurPhotonPlayer { get { return _curPhotonPlayer; } set { _curPhotonPlayer = value; } }

    private float _gameTimer;   // 현재 게임이 시작하고 얼마만큼의 시간이 지났는지
    public int[] _totalCount;   // TODO : 배열 초기화 해야함. 매치메이킹 이후에 이루어지는 작업진행

    public int _playersLoaded = 0;  // 현재 로드된 플레이어 수
    public int _heyHoFinished = 0;  // 현재 헤이호의 움직임에 필요한 계산이 끝난 헤이호의 수
    private int _totalplayers;  // 총 플레이어 수
    private float _finishTimer; // 제일 많이 입력한 플레이어의 헤이호의 목적지까지 걸리는 시간 

    WaitForSeconds _countDownDelay = new WaitForSeconds(1f); // 코루틴에서 사용하는 카운트다운을 위한 변수
    WaitForSeconds _delayHeyHo; // 코루틴에서 사용하는 1등 헤이호가 목적지까지 걸리는 시간을 위한 변수

    // Start 함수
    private void Start()
    {
        IsStarted = false;  // 게임씬에 진입했을시에는 false
        PhotonNetwork.LocalPlayer.SetLoad(true);

        //if (PhotonNetwork.IsMasterClient)   // 마스터 클라이언트만 진행
        //{
        //    _totalplayers = PhotonNetwork.CurrentRoom.PlayerCount;  // 총 플레이어 수 저장
        //}

        CurPhotonPlayer = PhotonNetwork.PlayerList;
    }

    // Update 함수
    private void Update()
    {

        if (IsStarted == false) // 게임 진행중이 아니면 return
            return;

        _gameTimer += Time.deltaTime; // 게임 시간 측정

        if (_gameTimer >= 5f) // 5초가 지나가면
        {

            IsStarted = false;  // 게임 종료

            IsInputFinished = true; // 헤이호 출발을 위한 true
            Debug.Log("게임 종료!");

        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(HJS_CustomProperty.LOAD))
        {
            Debug.Log($"{targetPlayer.NickName} 이 로딩이 완료되었습니다. ");
            bool allLoaded = CheckAllLoad();
            Debug.Log($"모든 플레이어 로딩 완료 여부 : {allLoaded} ");
            if (allLoaded)
            {
                GameObject player = PlayerSpawn();  // 방에 들어왔을때 플레이어 생성
                GameObject heyho = HeyHoSpawn();   // 방에 들어왔을때 헤이호 생성
                                                   // 생성한 heyho에 플레이어 컨트롤러 변수에 생성한 플레이어의 스크립트 참조시켜주기
                heyho.GetComponent<KHS_HeyHoController>()._playerController = player.GetComponent<KHS_PlayerController>();
            }
        }
    }

    private bool CheckAllLoad()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false)
                return false;
        }
        return true;
    }

    // 헤이호 컨트롤러에서 움직이는 상황이라고 판단할때 이벤트를 호출하게 되는데,
    // 그때 사용되는 함수
    public void MovingHeyHo()
    {

        if (_heyHoFinished >= PhotonNetwork.CurrentRoom.PlayerCount)    // 현재 움직이기 시작한 헤이호가 접속된 플레이어만큼 확인 되었을시
                                    // TODO : 네트워크로 합칠때 인원 수에 관한 조정이 필요한 상태
        {
            if (PhotonNetwork.IsMasterClient)   // 마스터 클라이언트만 진행
            {
                Debug.Log("mmmmmmm마스터 클라이언트로 확인.mmmmmmm");
                photonView.RPC("FinishGame", RpcTarget.AllBuffered);    // RPC함수를 사용해서 모두에게 FinisgGame 함수를 실행시킴
            }
            else
            {
                Debug.Log("mmmmm일반 클라이언트롤 확인.mmmmm");
            }
        }


    }

    // 플레이어가 입력한 횟수를 비교하여 어떤 헤이호가 가장 멀리 갔는지를 도출하는 함수
    private int FindWinnerHeyHo()
    {
        int _maxCount = 0;      // 가장 많이 누른 횟수
        int _heyHoIndex = 0;    // 가장 멀리 날아가는 헤이호의 인덱스 

        Debug.Log($"===============플레이어 인풋카운트 비교 시작=================");
        for (int i = 1; i < _playerController.Length; i++)  // 캐릭터 컨트롤러 배열의 길이만큼 반복
        {
            if (_playerController[i] == null)   // i 인덱스 속 내용이 비어있다면 continue로 스킵
            {
                continue;
            }
            Debug.Log($"{i}번째 인덱스는 {_playerController[i].TotalInputCount}");
            if (_playerController[i].TotalInputCount > _maxCount)   // i번째 컨트롤러 속 총 입력횟수가 현재까지 확인된 가장 많이 누른 횟수보다 크다면
            {
                _maxCount = _playerController[i].TotalInputCount;   // 가장 많이 누른 횟수에 i번째 컨트롤러가 입력한 총 입력횟수 초기화
                _heyHoIndex = i;    // 가장 많이 날아가는 헤이호를 위한 현재 i 인덱스 초기화
            }
            if(_playerController[i].TotalInputCount == _maxCount)
            {
                _heyHoIndex = 5;
            }
        }
        Debug.Log($"###########플레이어 인풋카운트 비교 끝###########{_heyHoIndex}, {_maxCount}");  // 비교를 완료했다는 디버그
        //if(_heyHoIndex == 0)
        //{
        //    return 5;
        //}

        return _heyHoIndex; // 가장 많이 날아가는 헤이호의 인덱스 반환
    }

    // 방에 참가했을때 호출되는 함수
    //public override void OnJoinedRoom()
    //{

    //    GameObject player = PlayerSpawn();  // 방에 들어왔을때 플레이어 생성
    //    GameObject heyho = HeyHoSpawn();   // 방에 들어왔을때 헤이호 생성
    //    // 생성한 heyho에 플레이어 컨트롤러 변수에 생성한 플레이어의 스크립트 참조시켜주기
    //    heyho.GetComponent<KHS_HeyHoController>()._playerController = player.GetComponent<KHS_PlayerController>();
    //}


    // 네트워크에 진입 후 준비에 필요한 시간 살짝 주는 함수
    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);    // 네트워크 준비에 필요한 시간 살짝 주기
        TestGameStart();    // 게임을 시작하는 함수
    }

    // 테스트 게임씬 시작 함수
    private void TestGameStart()
    {
        Debug.Log("게임 시작");
        StartCoroutine(DelayGameStart());   // 게임 시작을 위해 카운트다운을 진행하는 코루틴 선언
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
        Vector3 spawnPosition = SetPosition();  // 함수내에서 미리 설정한 위치로 초기화

        return PhotonNetwork.Instantiate("KHS/KHS_Player", spawnPosition, Quaternion.identity);
    }

    // 헤이호 스폰 함수
    private GameObject HeyHoSpawn()
    {
        Vector3 spawnPosition = SetPosition(); // 함수내에서 미리 설정한 위치로 초기화
        spawnPosition.z += 2f;  // 지정한 포지션값보다 살짝 앞에서 생성되어야하기 때문에 +2

        return PhotonNetwork.Instantiate("KHS/KHS_Hey-Ho", spawnPosition, Quaternion.identity);

    }


    // 카운트다운 후 게임 시작 함수
    private IEnumerator DelayGameStart()
    {
        _uiManager.CountDownGame(); // UI상에서도 현재 카운트다운을 진행하는 화면을 출력하기 위한 함수 호출

        Debug.Log("3!");
        yield return _countDownDelay;

        Debug.Log("2!");
        yield return _countDownDelay;

        Debug.Log("1!");
        yield return _countDownDelay;

        Debug.Log("Start!!!");
        IsStarted = true; // 게임 시작 상태로 변경
    }

    // 입력받은 카운트 중 가장 높은 카운트가 눌린 헤이호의 이동시간만큼 딜레이
    private IEnumerator MovedHeyHo()
    {
        int find = FindWinnerHeyHo();   // 가장 많이 날아가는 헤이호를 함수 내에서 찾고, 해당 헤이호의 인덱스를 find에 초기화
        KHS_HeyHoController heyho = _heyHoController[find]; // find 인덱스를 가지고 있는 헤이호 컨트롤러를 새로운 변수에 초기화

        if(find == 5)
        {
            _uiManager.NoWinner();
        }

        if (heyho == null)  // 해당 헤이호 컨트롤러가 혹시 비어있다면
        {
            Debug.Log($"@@@@@@@@@@@@@@@@{find}@@@@@@@@@@@@@@@@@");
            _uiManager.NoWinner();
        }
        else
        {
            _finishTimer = heyho.FinishTime;    // 제일 멀리 날아가는 헤이호의 이동 시간 초기화

            _delayHeyHo = new WaitForSeconds(_finishTimer);  // 해당 시간만큼 딜레이 초기화
            Debug.Log($"{_finishTimer}만큼 걸릴 예정");

            yield return _delayHeyHo;   // 초기화 시킨 시간 만큼 딜레이
            ResultGame();   // 게임의 우승자를 출력해주는 함수 호출
        }

    }

    /// <summary>
    /// KHS_PlayerController 스크립트에서 호출하는 플레이어가 레디가 되었는지 확인하기 위한 함수
    /// </summary>
    public void PlayerReady()
    {
        _playersLoaded++;   // 로딩된 플레이어의 변수 + 1

        Debug.Log($"현재 로딩된 플레이어 : {_playersLoaded}");

        if (_playersLoaded == PhotonNetwork.CurrentRoom.PlayerCount)    // TODO : 네트워크로 합칠때 인원 수에 관한 조정이 필요한 상태
        {
            if (PhotonNetwork.IsMasterClient)   // 마스터 클라이언트라면
            {
                photonView.RPC("StartRPC", RpcTarget.All);  // 모두에게 게임을 시작한다는 RPC함수를 호출하겠다고 신호를 보냄
            }
        }
    }

    // 결과를 출력하는 함수
    private void ResultGame()
    {
        Debug.Log($"승자는 {FindWinnerHeyHo()} 번 플레이어 입니다!");
        _uiManager.OnWinner(CurPhotonPlayer[FindWinnerHeyHo() - 1].NickName); // UI매니저 속 함수를 사용해서 화면 출력
    }

    [PunRPC]
    private void FinishGame()
    {
        if (_heyHoFinished >= PhotonNetwork.CurrentRoom.PlayerCount)    // TODO : 네트워크로 합칠때 인원 수에 관한 조정이 필요한 상태
        {
            Debug.Log($"헤이호 날아가는 코루틴 시작 RPC {_heyHoFinished}");
            StartCoroutine(MovedHeyHo());   // 1등 헤이호가 걸리는 시간, 결과를 출력하기 위한 코루틴선언
        }
        else
        {
            Debug.Log($"코루틴 아직 시작안함{_heyHoFinished}");
        }

    }

    [PunRPC]
    private void StartRPC() // 마스터 클라이언트가 게임을 시작한다고 신호를 주는 함수
    {
        StartCoroutine(StartDelayRoutine());    // 잠깐의 딜레이 후 게임을 진행시키는 코루틴 선언
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
            stream.SendNext(_heyHoFinished);    // 헤이호가 움직이는지 확인된 변수
        }
        else if (stream.IsReading)
        {
            _playersLoaded = (int)stream.ReceiveNext(); // 플레이어 로드된 인원 변수
            _finishTimer = (float)stream.ReceiveNext(); // 제일 많이 날아가는 헤이호가 걸리는 시간 변수
            _totalCount[1] = (int)stream.ReceiveNext(); // 1번 플레이어 총 카운트 횟수
            _totalCount[2] = (int)stream.ReceiveNext(); // 2번 플레이어 총 카운트 횟수
            _totalCount[3] = (int)stream.ReceiveNext(); // 3번 플레이어 총 카운트 횟수
            _totalCount[4] = (int)stream.ReceiveNext(); // 4번 플레이어 총 카운트 횟수
            _heyHoFinished = (int)stream.ReceiveNext(); // 헤이호가 움직이는지 확인된 변수
        }
    }
}