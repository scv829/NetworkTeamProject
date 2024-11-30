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
public class ljh_AvoidGameManager : MonoBehaviourPun, IPunObservable
{   //Todo: 오프닝(후순위), 점수 표기, 1등 가리기, 엔딩




    [SerializeField] public Phase curPhase;

    [SerializeField] public ljh_AvoidUIManager uiManager;

    [SerializeField] public ljh_AvoidStone[] stoneArray;

    [SerializeField] public ljh_AvoidStone stone;

    public int playerCount;
    public Queue<ljh_PlayerController> playerQ;

    //타이머
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
        timer = 3;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;


        if (curPhase != Phase.endPhase)
        {
            if (timerRoutine == null)
            {
                timerRoutine = StartCoroutine(TimerCo());
            }
        }



        if (timer == 0)
        {
            StopCoroutine(attackRoutine);
            curPhase = Phase.endPhase;
        }
        PhaseCalc();
        //FindPlayer();


    }

    void FindPlayer()
    {
        if (playerCount == 1)
        {
            curPhase = Phase.endPhase;

        }
    }

    IEnumerator AttackCo()
    {
        while ((int)curPhase > 0 || (int)curPhase < 2)
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
            Debug.Log("와일문도는중");
            yield return new WaitForSeconds(2);
        }

        Debug.Log("호출");
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
        Debug.Log("타이머스타트");

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

        PhaseChange();
        Debug.Log("호출됨");
    }

    public void PhaseChange()
    {
        photonView.RPC("RPCPhaseChange", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPCPhaseChange()
    {
        if (timer <= 0)
        {
            Debug.Log("엔드페이즈로넘어감");
            curPhase = Phase.endPhase;
        }
    }


    public void EndPhase()
    {
        //StopCoroutine(attackRoutine);

        // Todo: 게임 끝 살아남은 사람 줌인 / 우선순위 낮음
        // Todo: 시간 비례해서 순위
    }

    // public void PhaseCalc()
    // {
    //     photonView.RPC("RPCPhaseCalc", RpcTarget.AllViaServer);
    // }

    //[PunRPC]
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
                break;

            case Phase.endPhase:
                EndPhase();
                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        int phaseInt = (int)curPhase;
        if (stream.IsWriting)
        {
            stream.SendNext(playerCount);
            stream.SendNext(phaseInt);
        }
        else
        {
            playerCount = (int)stream.ReceiveNext();
            phaseInt = (int)stream.ReceiveNext();
        }

        curPhase = (Phase)phaseInt;
    }
}
