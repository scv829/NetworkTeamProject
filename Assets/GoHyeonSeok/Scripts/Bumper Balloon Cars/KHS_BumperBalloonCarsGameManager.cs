using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

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
    private int _winnerIndex = 0;

    private void Awake()
    {
        if(Instance == null)
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
        CurLivePlayer = 4;  // 게임 시작했을때 현재 접속되어있는 인원수 만큼 설정 (임시 2명 설정)
                            // TODO : 나중에 PhotonNetwork.CurrentRoom.PlayerCount; 현재 게임에 접속된 플레이어 카운트 새야함
        PhotonNetwork.LocalPlayer.SetLoad(true);
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
                return new Vector3(-7f, 1f, -8f);
            case 2:
                return new Vector3(-7f, 1f, 8f);
            case 3:
                return new Vector3(7f, 1f, -8f);
            case 4:
                return new Vector3(7f, 1f, 8f);
        }
        return Vector3.zero;
    }

    // 플레이어(카트) 스폰 함수
    private GameObject PlayerSpawn()
    {
        Vector3 spawnPosition = SetPosition();  // 함수내에서 미리 설정한 위치로 초기화

        return PhotonNetwork.Instantiate("KHS/KHS_Cart", spawnPosition, Quaternion.identity);
    }

    public void GameOverPlayer()
    {
        CurLivePlayer--;    // 현재 살아있는 플레이어 인원
        Debug.Log($"현재 남은 플레이어 {CurLivePlayer} 명");

        StartCoroutine(DelayGameOver());
    }

    private IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(0.2f);  // TODO : 우선은 임시방편으로 데이터를 불러오는 시간이 조금 딜레이가 있는 것으로 확인되어서 넣은 코루틴

        if (CurLivePlayer == 1) // 남은 인원이 1명일때
        {
            for (int i = 1; i < CartController.Length; i++)
            {
                if (CartController[i].IsGameOver == false)
                // 카트 컨트롤러 안에 선언되어있는 IsGameOver가 false일때
                {
                    _winnerIndex = i;    // 해당 인덱스가 승자를 나타내기에 설정
                    Debug.Log($"{i} 승자 인덱스");
                    break;
                }
            }
        }

        _uiManager.ResultGame(_winnerIndex); // 결과 창 출력
        Debug.Log($"!게임 종료!");
    }

    public void PlayerReady()
    {
        _playersLoaded++;   // 로딩된 플레이어의 변수 + 1

        Debug.Log($"현재 로딩된 플레이어 : {_playersLoaded}");

        if (_playersLoaded == 4)    // TODO : 네트워크로 합칠때 인원 수에 관한 조정이 필요한 상태
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
            stream.SendNext(_winnerIndex);
        }
        else if (stream.IsReading)
        {
            IsGameStarted = (bool)stream.ReceiveNext(); // 현재 게임 시작했는지 확인하는 변수
            CurLivePlayer = (int)stream.ReceiveNext();  // 현재 살아있는 플레이어 인원 변수
            _winnerIndex = (int)stream.ReceiveNext();
        }
    }
}
