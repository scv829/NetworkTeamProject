using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum State
{
    idle,
    enter,
    exit,
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
public class ljh_GameManager : MonoBehaviour
{
    [SerializeField] public ljh_UIManager uiManager;
    [SerializeField] public ljh_InputManager inputManager;
    [SerializeField] public ljh_Player player;
    [SerializeField] public ljh_TestGameScene scene;
    [SerializeField] public ljh_CartManager cartManagerEnter;
    [SerializeField] public ljh_CartManager cartManagerExit;

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

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        curUserNum = 2; // ToDo : 테스트용도로 4로 둔 상태 추후에 0으로 교체 및 테스트게임신에서 주석 해제

        if (curState != State.idle)
            curState = State.idle;

        myTurn = 0;

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
        Playing();

        

    }
    public void UserNumCalculate(int curUserNum)
    {
        switch (curUserNum)
        {
            case 4:

                defaultIndex = 2;
                Player4.SetActive(true);
                Player3.SetActive(false);
                Player2.SetActive(false);
                //4인플
                break;

            case 3:

                defaultIndex = 2;
                Player4.SetActive(false);
                Player3.SetActive(true);
                Player2.SetActive(false);
                //3인플
                break;

            case 2:

                defaultIndex = 1;
                Player4.SetActive(false);
                Player3.SetActive(false);
                Player2.SetActive(true);
                //2인플
                break;

            case 1:
                defaultIndex = 2;
                //테스트용 1인플
                break;
        }
    }

    public void Playing()
    {

        switch (curState)
        {
            case State.idle:
                uiManager.ShowUiIdle();
                inputManager.FindPlayer();
                MoveStart();
                break;

            case State.enter:
                uiManager.ShowUiEnterMove();
                //cartManagerEnter.CartMoveEnter();
                break;

            case State.choice:
                uiManager.ShowUiChoice();
                //player.UnRideCart();
                //_curPos = inputManager.ChoiceAnswer();
                //inputManager.SelectButton(_curPos);

                break;

            case State.exit:
                uiManager.ShowUiExitMove();
                //player.RideExitCart();
                //cartManagerEnter.CartMoveExit();
                //player.NextTurn(player.i);
                break;

            case State.end:
                GameEnd();
                break;

        }
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

    public void PlayerExit()
    {
        curState = State.exit;
    }


    public void GameEnd()
    {
        door.transform.rotation = Quaternion.Euler(0, 90, 0);
        sunLight.transform.rotation = Quaternion.Euler(130, 48, 0);
    }

    public void ButtonOn4P()
    {

    }


}
