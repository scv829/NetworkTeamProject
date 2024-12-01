using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum Phase
{
    phase0, GamePhase, endPhase
}
public class ljh_AvoidGameManager : MonoBehaviourPun
{   //Todo: 오프닝(후순위), 점수 표기, 1등 가리기, 엔딩




    [SerializeField] public Phase curPhase;

    [SerializeField] public ljh_AvoidUIManager uiManager;

    [SerializeField] public ljh_AvoidStone[] stoneArray;

    [SerializeField] public ljh_AvoidStone stone;

    public int playerCount;
    public Queue<ljh_PlayerController> playerQ;

    //타이머
    public float readyTimer;
    public float timer;
    bool isStarted;

    float stoneCooldown;
    public Coroutine attackRoutine;
    public Coroutine timerRoutine;
    public Coroutine waitRoutine;


    private void Start()
    {
        curPhase = Phase.phase0;
        isStarted = false;
        playerCount = 0;
        readyTimer = 3;
        timer = 35;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;


        if (curPhase == Phase.GamePhase)
        {
            if (timerRoutine == null)
            {
                timerRoutine = StartCoroutine(TimerCo());
            }
        }

        PhaseCalc();
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

    void AlivePlayer()
    {
        photonView.RPC("RPCAP", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void RPCAP()
    {
        uiManager.alivePlayer = GameObject.FindWithTag("Player").GetComponent<ljh_PlayerController>();
        Debug.Log($"이름 {uiManager.alivePlayer.myName}");
    }

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
            timer = 35f;
            isStarted = true;
        }
    }


    IEnumerator TimerCo()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            uiManager.timerText.text = $"Time : {timer}";
        }

    }


    public void GamePhase()
    {

        StopCoroutine(waitRoutine);

        if (attackRoutine != null) return;
        attackRoutine = StartCoroutine(AttackCo());

    }
    

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



    public void EndPhase()
    {
        //StopCoroutine(attackRoutine);

        // Todo: 게임 끝 살아남은 사람 줌인 / 우선순위 낮음
        // Todo: 시간 비례해서 순위
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
                EndPhase();
                break;
        }
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerCount);
            stream.SendNext(timer);
        }
        else
        {
            playerCount = (int)stream.ReceiveNext();
            timer = (float)stream.ReceiveNext();
        }
    }*/
}
