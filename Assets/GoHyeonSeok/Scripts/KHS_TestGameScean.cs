using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class KHS_TestGameScean : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";

    [SerializeField] private KHS_PlayerController[] _playerController;
    public KHS_PlayerController[] PlayerController {  get { return _playerController; } set { _playerController = value; } }

    [SerializeField] private bool _isStarted;
    public bool IsStarted { get { return _isStarted; } set { _isStarted = value; } }

    [SerializeField] private Player[] _sortedPlayers;
    public Player[] SortedPlayer {  get { return _sortedPlayers; } set {_sortedPlayers = value; } }

    private float _gameTimer;
    public int[] _totalCount;


    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
        IsStarted = false;
    }

    private void Update()
    {
        // 게임 진행중이 아니면 return
        if(IsStarted == false) 
            return;

        // 게임 시간 측정
        _gameTimer += Time.deltaTime;
        //Debug.Log($"현재 진행중인 시간 : {_gameTimer}");

        if (_gameTimer >= 5f) // 5초가 지나가면
        {
            IsStarted = false;
            _gameTimer = 0;
            Debug.Log("게임 종료!");

            for (int i = 0; i < _playerController.Length; i++)
            {
                _totalCount[i] = _playerController[i].TotalInputCount;
                Debug.Log($"Player {i + 1} 총 입력 횟수 : {_totalCount[i]}");
            }

        }

    }

    // 마스터 서버에 연결
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    // 방에 참가했을때 호출되는 함수
    public override void OnJoinedRoom()
    {
        // 플레이어 리스트를 넣어주고
        SortedPlayer = PhotonNetwork.PlayerList;
        StartCoroutine(StartDelayRoutine());
    }

    // 네트워크에 진입 후 준비에 필요한 시간 살짝 주기
    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);    // 네트워크 준비에 필요한 시간 살짝 주기
        TestGameStart();
    }

    // 테스트 게임씬 시작
    public void TestGameStart()
    {
        Debug.Log("게임 시작");
        // TODO : 테스트용 게임 시작 부분
        PlayerSpawn();
        _playerController[0] = FindAnyObjectByType<KHS_PlayerController>();

        if (PhotonNetwork.IsMasterClient == false)
            return;
        HeyHoSpawn();

        // 플레이어, 헤이호 생성된 후 게임시작 카운트 다운 들어가기
        StartCoroutine(DelayGameStart());
    }

    // 플레이어 스폰
    private void PlayerSpawn()
    {
        //Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        Vector3 spawnPosition = Vector3.zero;

        switch(PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                spawnPosition = new Vector3(-7f, 1f, -8f);
                break;
            case 2:
                spawnPosition = new Vector3(-3f, 1f, -8f);
                break;
            case 3:
                spawnPosition = new Vector3(3f, 1f, -8f);
                break;
            case 4:
                spawnPosition = new Vector3(7f, 1f, -8f);
                break;
        }

        //Color color = Random.ColorHSV();
        //object[] data = { color.r, color.g, color.b };

        PhotonNetwork.Instantiate("KHS/KHS_Player", spawnPosition, Quaternion.identity/*, data : data*/);
    }

    // 헤이호 스폰
    private void HeyHoSpawn()
    {

        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        //Color color = Random.ColorHSV();
        //object[] data = { color.r, color.g, color.b };

        PhotonNetwork.Instantiate("KHS/KHS_Hey-Ho", randomPos, Quaternion.identity/*, data : data*/);

    }

    private void GameStart()
    {

    }

    WaitForSeconds _delay = new WaitForSeconds(1f); 
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
}
