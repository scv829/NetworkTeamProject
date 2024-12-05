using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public enum Phase
{
    phase0, GamePhase, endPhase
}
public class ljh_AvoidGameManager : MonoBehaviourPun, IPunObservable
{   //Todo: 오프닝(후순위), 점수 표기, 1등 가리기, 엔딩

    [SerializeField] public ljh_Player player;


    [SerializeField] public Phase curPhase;

    [SerializeField] public ljh_AvoidUIManager uiManager;

    [SerializeField] public ljh_AvoidStone[] stoneArray;

    [SerializeField] public ljh_AvoidStone stone;

    [SerializeField] public ljh_AvoidTestGameScene testScene;

    public int playerCount;

    //타이머
    public float readyTimer;
    public float timer;
    bool isStarted;

    float stoneCooldown;
    public Coroutine attackRoutine;
    public Coroutine timerRoutine;
    public Coroutine waitRoutine;

    public Player[] curPhotonList;

    public ljh_PlayerController _alivePlayer;
    bool isEnd;

    int index;

    bool textOn;

    private void Start()
    {
        curPhase = Phase.phase0;
        isStarted = false;
        playerCount = 0;
        readyTimer = 3;
        timer = 15;

        curPhotonList = PhotonNetwork.PlayerList;
        isEnd = false;
        textOn = false;
    }

    private void Update()
    {
        if(textOn)
        Invoke("winnerText", 0.3f);

        if (!PhotonNetwork.IsMasterClient)
            return;

        //Comment 게임페이즈에 돌입 시 시간초 흘러가기 시작
        if (curPhase == Phase.GamePhase)
        {
            if (timerRoutine == null)
            {
                timerRoutine = StartCoroutine(TimerCo());
            }
        }

        //Comment : 페이즈 에 따라 게임 진행 처리
        PhaseCalc();
        //Commnet : 우승자 찾기 위한 함수
        FindPlayer();
    }

    void FindPlayer()
    {
        if (curPhase == Phase.GamePhase)
        {
            if (playerCount == 1)
            {
                curPhase = Phase.endPhase;
                AlivePlayer();
            }
        }
    }

    // Comment 생존자 찾기 함수
    void AlivePlayer()
    {
        photonView.RPC("RPCAP", RpcTarget.AllViaServer);
            

    }

    [PunRPC]
    void RPCAP()
    {
        _alivePlayer = GameObject.FindWithTag("Player").GetComponent<ljh_PlayerController>();
        textOn = true;
    }

    void winnerText()
    {
        uiManager.winnerText.text = $"살아남은 생존자는.... {_alivePlayer.myName}입니다!!!";
    }

    //Comment : 비석 공격 코루틴
    IEnumerator AttackCo()
    {
        while (curPhase == Phase.GamePhase)
        {
            stoneArray[Random.Range(0, stoneArray.Length)].real = true;

            for (int i = 0; i < stoneArray.Length; i++)
            {
                if (stoneArray[i].real == true)
                {
                    stone = stoneArray[i];
                    stone.Smash();
                    stone.real = false;
                }
            }
            yield return new WaitForSeconds(2);
        }

    }

    //Comment : 게임 시작시 로딩 대기시간
    public void Wait()
    {
        // Todo: 타이머 3초 대기시간

        if (waitRoutine == null)
            waitRoutine = StartCoroutine(WaitCo());
    }

    IEnumerator WaitCo()
    {
        yield return new WaitForSeconds(3f);
        curPhase = Phase.GamePhase;
    }

    public void TimerStart()
    {

        if (!isStarted)
        {
            timer = 15f;
            isStarted = true;
        }
    }

    
    IEnumerator TimerCo()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            uiManager.timerText.text = $"남은 시간 : {timer}초..";
        }

    }

    // 게임 페이즈 실행 함수
    public void GamePhase()
    {

        StopCoroutine(waitRoutine);

        if (attackRoutine != null) return;
        attackRoutine = StartCoroutine(AttackCo());
    }

    // 페이즈 변경 함수
    public void PhaseChange()
    {
        if (curPhase == Phase.GamePhase)
        {
            if (timer == 0)
            {
                curPhase = Phase.endPhase;
            }
        }
    }


    //엔드 페이즈 함수
    public void EndPhase()
    {
        isEnd = true;

        photonView.RPC("RPCEndPhase", RpcTarget.AllViaServer);

    }

    [PunRPC]
    public void RPCEndPhase()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_alivePlayer.playerNum == i)
            {
                Player winner = curPhotonList[i];
                HJS_GameSaveManager.Instance.GameOver(new Player[] { winner });
            }
        }
    }

    //Comment 승자 계산 함수
    public int WinnerCalc()
    {
        for (int i = 0; i < curPhotonList.Length; i++)
        {
            if (testScene.playerList[i].died == false)
            {
                return i;
            }
        }
        return 0;
    }
    public void PhaseCalc()
    {
        switch (curPhase)
        {
            case Phase.phase0:
                Wait();
                break;

            case Phase.GamePhase:
                TimerStart();
                GamePhase();
                PhaseChange();
                break;

            case Phase.endPhase:
                if (!isEnd)
                    EndPhase();
                break;
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(timer);
            stream.SendNext(uiManager.timerText.text);
        }
        else
        {
            timer = (float)stream.ReceiveNext();
            uiManager.timerText.text = (string)stream.ReceiveNext();
        }
    }
}
