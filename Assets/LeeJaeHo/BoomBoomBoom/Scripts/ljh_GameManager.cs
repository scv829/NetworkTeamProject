using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum State
{
    idle,
    enter,
    die,
    choice,
    end
};
public enum MyTurn
{
    player1,
    player2,
    player3,
    player4
};
public class ljh_GameManager : MonoBehaviourPun, IPunObservable
{
    [SerializeField] public ljh_Boom bomb;
    [SerializeField] public ljh_UIManager uiManager;
    [SerializeField] public ljh_InputManager inputManager;
    [SerializeField] public ljh_Player player;
    [SerializeField] public ljh_BoomTestGameScene scene;
    [SerializeField] public ljh_CartManager cartManagerEnter;
    [SerializeField] public ljh_Pos posManager;

    public int curUserNum;
    public int deathCount = 0;
    public int defaultIndex;
    public int minus;
    [SerializeField] public int index;


    [SerializeField] public State curState;

    [SerializeField] public GameObject Player4;
    [SerializeField] public GameObject Player3;
    [SerializeField] public GameObject Player2;

    [Header("기타 오브젝트")]
    [SerializeField] public GameObject boom;
    [SerializeField] public GameObject door;
    [SerializeField] public GameObject sunLight;

    public Coroutine moveCo;

    public PlayerNumber playerNumber;

    public MyTurn myTurn;

    public static ljh_GameManager instance = null;

    [SerializeField] public bool playingCheck;

    Coroutine turnCoroutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        curUserNum = 4; // ToDo : 테스트용도로 4로 둔 상태 추후에 0으로 교체 및 테스트게임신에서 주석 해제

        if (curState != State.idle)
            curState = State.idle;

        

    }

    private void Start()
    {
        UserNumCalculate(curUserNum - deathCount);


        if (player != null)
        {
            for (int i = 0; i < scene.playerArray.Length - 1; i++)
            {
                scene.playerArray[i].GetComponent<ljh_Player>().playerNumber = (PlayerNumber)i;
                
                // 인트인 리스트에 
            }
        }
        //플레이어의 갯수를 세줌 > playerNum
        //curUserNum은 포톤뷰의 숫자를 세줌?
        // 둘이 동일하면 첫번째 플레이어 상태 무브로 변경
        // 버튼 돌림
        // 생존 : 첫번째 플레이어 상태 퇴장무브로 변경
        // 사망 : -1 상태로 씬 리로드
        // 두번째 플레이어부터는 반복
        // 혼자 남으면 : 폭탄 재생 안되고 Win 문구 뜨면서 문열리고 빛밝아지고 탈출
    }


    private void Update()
    {
        Debug.Log($"현재 마이턴 {myTurn}");
            Playing();
        
        

    }
    public void UserNumCalculate(int curUserNum)
    {
        switch (curUserNum)
        {
            case 4:

                defaultIndex = 2;
                // 4개 중 1개만 생존
                //4인플
                break;

            case 3:

                defaultIndex = 2;
                // 3개 중 1개만 생존
                //3인플
                break;

            case 2:

                defaultIndex = 1;
                // 2개 중 1개만 생존
                //2인플
                break;

            case 1:
                defaultIndex = 4;
                //테스트용 1인플
                break;
        }
    }

    public void Playing()
    {
        switch (curState)
        {
            case State.idle:
                //uiManager.ShowUiIdle();
                inputManager.FindPlayer(player.gameObject);
                playingCheck = true;
                break;

            case State.enter:
                //uiManager.ShowUiEnterMove();
                posManager.EndPoint();
                break;

            case State.choice:
                //uiManager.ShowUiChoice();
                cartManagerEnter.CartReset();

                break;

            case State.die:
                //uiManager.ShowUiExitMove();
                if (playingCheck)
                {
                    photonView.RPC("RPCNextTurn", RpcTarget.All);
                    playingCheck = false;
                }
                break;

            case State.end:
                GameEnd();
                break;

        }
        return;
    }

    

    public void MoveStart()
    {
        if (moveCo == null)
            moveCo = StartCoroutine(ComoveStart());
    }

    IEnumerator ComoveStart()
    {
        yield return new WaitForSeconds(5f);
        curState = State.enter;
        StopCoroutine(moveCo);
    }

    public void PlayerDie()
    {
        curState = State.die;
    }


    public void GameEnd()
    {
        door.transform.rotation = Quaternion.Euler(0, 90, 0);
        sunLight.transform.rotation = Quaternion.Euler(130, 48, 0);
    }

    [PunRPC]
    public void RPCNextTurn()
    {
        switch (myTurn)
        {
            case MyTurn.player1:
                myTurn = MyTurn.player2;
                curState = State.idle;
                break;

            case MyTurn.player2:
                myTurn = MyTurn.player3;
                curState = State.idle;
                Debug.Log("2에서 3로 진행했음");
                break;
           
            case MyTurn.player3:
                myTurn = MyTurn.player4;
                curState = State.idle;
                break;
           
        }
    }

    public void ButtonReset()
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(curState);
            stream.SendNext(myTurn);
        }
        else
        {
            curState = (State)stream.ReceiveNext();
            myTurn = (MyTurn)stream.ReceiveNext();
        }
    }
}
