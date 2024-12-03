using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class KHS_BumperBalloonCarsGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static KHS_BumperBalloonCarsGameManager Instance { get; private set; }   // 임시로 선언한 게임매니저 싱글톤

    [SerializeField] private KHS_CartController[] _cartController;  // 플레이어(카트)를 담아놓는 변수
    public KHS_CartController[] CartController { get { return _cartController; } set { _cartController = value; } }

    [SerializeField] private int _playersLoaded;    // 현재 로딩된 플레이어 수
    public int PlayerLoaded { get { return _playersLoaded; } set { _playersLoaded = value; } }

    [SerializeField] private KHS_BumperUIManager _uiManager; // UI관리를 위한 UI매니저 참조
    [SerializeField] private int _curLivePlayer;  // 현재 살아있는 플레이어 인원 수
    public int CurLivePlayer { get { return _curLivePlayer; } set { _curLivePlayer = value; } }

    [SerializeField] private bool _isGameStarted;   // 현재 게임 시작했는지 확인하는 변수
    public bool IsGameStarted { get { return _isGameStarted; } set { _isGameStarted = value; } }

    [SerializeField] private Player[] _curPhotonPlayer; // 현재 포톤 네트워크에 접속된 플레이어의 리스트를 받는 변수
    public Player[] CurPhotonPlayer { get { return _curPhotonPlayer; } set { _curPhotonPlayer = value; } }
    private int _winnerIndex = 0;   // 우승자의 인덱스를 위한 변수

    private void Awake()
    {
        // 싱글톤 패턴 사용으로 인해 예외처리 진행
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurLivePlayer = PhotonNetwork.CurrentRoom.PlayerCount;  // 게임 시작했을때 현재 접속되어있는 인원수 만큼 설정
                                                                
        CurPhotonPlayer = PhotonNetwork.PlayerList; // 현재 포톤네트워크에 접속된 플레이어 담아주기
        Debug.Log($"{CurPhotonPlayer.Length}");

        PhotonNetwork.LocalPlayer.SetLoad(true);    // 로드되었을시
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
                PlayerSpawn();
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


    public override void OnJoinedRoom() // 방에 접속되었을 때
    {
        PlayerSpawn();
    }

    private Vector3 SetPosition()   // 플레이어(카트) 위치를 위한 함수
    {
        // 현재 자신의 ActorNumber 대로 위치 설정
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                return new Vector3(-7f, 0f, -8f);
            case 2:
                return new Vector3(-7f, 0f, 8f);
            case 3:
                return new Vector3(7f, 0f, -8f);
            case 4:
                return new Vector3(7f, 0f, 8f);
        }
        return Vector3.zero;
    }

    // 플레이어(카트) 스폰 함수
    private GameObject PlayerSpawn()
    {
        Vector3 spawnPosition = SetPosition();  // 함수 내에서 미리 설정한 위치로 초기화
        Quaternion spawnRotation = PlayerRotate();  // 함수 내에세 미리 설정한 회전으로 초기화

        switch (PhotonNetwork.LocalPlayer.ActorNumber)  // 현재 플레이어 넘버링에 따라서 소환되는 위치 설정
        {
            case 1:
                return PhotonNetwork.Instantiate("KHS/KHS_Cart1", spawnPosition, spawnRotation);
            case 2:
                return PhotonNetwork.Instantiate("KHS/KHS_Cart2", spawnPosition, spawnRotation);
            case 3:
                return PhotonNetwork.Instantiate("KHS/KHS_Cart3", spawnPosition, spawnRotation);
            case 4:
                return PhotonNetwork.Instantiate("KHS/KHS_Cart4", spawnPosition, spawnRotation);
        }
        return null;
    }

    private Quaternion PlayerRotate()   // 플레이어의 회전 값을 반환시켜주는 함수
    {
        // 현재 자신의 ActorNumber 대로 위치 설정
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                return Quaternion.identity;
            case 2:
                return Quaternion.Euler(0f, 180f, 0f);
            case 3:
                return Quaternion.identity;
            case 4:
                return Quaternion.Euler(0f, 180f, 0f);
        }
        return Quaternion.identity;
    }

    public void GameOverPlayer()    // 플레이어가 게임오버되었을때 호출되는 함수
    {
        CurLivePlayer--;    // 현재 살아있는 플레이어 인원 1명 줄여주기
        Debug.Log($"현재 남은 플레이어 {CurLivePlayer} 명");

        if( CurLivePlayer == 1) // 현재 살아있는 플레이어가 1명이라면,
        {
            StartCoroutine(DelayGameOver());    // 게임을 마무리하고 결과창을 출력하는 코루틴 함수 호출
        }
    }

    private IEnumerator DelayGameOver() // 게임을 마무리하고 결과창을 출력하는 코루틴 함수
    {
        yield return new WaitForSeconds(0.5f);  // TODO : 우선은 임시방편으로 데이터를 불러오는 시간이 조금 딜레이가 있는 것으로 확인되어서 넣은 코루틴
        string nickName = "";

        if (CurLivePlayer == 1) // 남은 인원이 1명일때
        {
            for (int i = 1; i < CartController.Length; i++)
            {
                if (CartController[i].IsGameOver == false)
                // 카트 컨트롤러 안에 선언되어있는 IsGameOver가 false일때
                {
                    _winnerIndex = i;    // 해당 인덱스가 승자를 나타내기에 설정
                    Debug.Log($"{i} 승자 인덱스");

                    nickName = CurPhotonPlayer[i - 1].NickName;
                    Player winner = CurPhotonPlayer[i - 1];
                    HJS_GameSaveManager.Instance.GameOver(new Player[] { winner });
                    break;
                }
            }
            _uiManager.ResultGame(nickName); // 결과 창 출력
            Debug.Log($"!게임 종료!");

        }
    }

    public void PlayerReady()
    {
        _playersLoaded++;   // 로딩된 플레이어의 변수 + 1

        Debug.Log($"현재 로딩된 플레이어 : {_playersLoaded}");

        if (_playersLoaded == PhotonNetwork.CurrentRoom.PlayerCount)    // TODO : 네트워크로 합칠때 인원 수에 관한 조정이 필요한 상태
        {
            photonView.RPC("KHS_BumperCartGameStart", RpcTarget.AllViaServer, PhotonNetwork.Time);  // 모두에게 게임을 시작한다는 RPC함수를 호출하겠다고 신호를 보냄
        }
    }

    [PunRPC]
    private void KHS_BumperCartGameStart(double sentTime, PhotonMessageInfo info)
    {
        // 서버에서 RPC를 보낸 시간과 현재 시간의 격차를 계산
        float lag = Mathf.Abs((float)(sentTime - info.SentServerTime));

        // 시간의 격차만큼 감소된 시간만큼 카운트를 진행
        // ex. 지연 시간이 0.1초 발생한 경우 카운트 다운을 2.9초 진행
        StartCoroutine(GameTimer(3f - lag));
        _uiManager.CountDownGameStart();
    }

    IEnumerator GameTimer(float timer)
    {
        yield return new WaitForSeconds(timer); // 렉 지연보상을 적용한 시간만큼 코루틴을 사용해줌
        Debug.Log("게임 스타트!");
        IsGameStarted = true;   // 현재 게임 시작했음을 알림
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsGameStarted); // 현재 게임 시작했는지 확인하는 변수
            stream.SendNext(CurLivePlayer); // 현재 살아있는 플레이어 인원 변수
            stream.SendNext(_winnerIndex);  // 우승자 인덱스 변수
        }
        else if (stream.IsReading)
        {
            IsGameStarted = (bool)stream.ReceiveNext(); // 현재 게임 시작했는지 확인하는 변수
            CurLivePlayer = (int)stream.ReceiveNext();  // 현재 살아있는 플레이어 인원 변수
            _winnerIndex = (int)stream.ReceiveNext();   // 우승자 인덱스 변수
        }
    }
}
