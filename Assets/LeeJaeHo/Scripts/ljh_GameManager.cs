using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum State
{
    idle,
    move,
    choice,
    end
};
public class ljh_GameManager : MonoBehaviour
{
    [SerializeField] public ljh_UIManager uiManager;
    [SerializeField] public ljh_InputManager inputManager;
    [SerializeField] public ljh_Player player;
    [SerializeField] public ljh_TestGameScene scene;
    [SerializeField] public ljh_CartManager cartManager;


    public int curUserNum;
    public int defaultIndex;
    public int minus;
    [SerializeField] public int index;


    [SerializeField] public State curState;

    [Header("기타 오브젝트")]
    [SerializeField] public GameObject boom;
    [SerializeField] public GameObject door;
    [SerializeField] public GameObject sunLight;


    public static ljh_GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        curUserNum = 0;

        if(curState != State.idle)
            curState = State.idle;
    }

    private void Start()
    {
        UserNumCalculate(curUserNum);
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
                //4인플
                break;

            case 3:

                defaultIndex = 2;
                //3인플
                break;

            case 2:

                defaultIndex = 1;
                //2인플
                break;
        }
    }

    public void Playing()
    {
        Vector3 _curPos = inputManager._curPos;

        switch (ljh_GameManager.instance.curState)
        {
            case State.idle:
                uiManager.ShowUiIdle();
                inputManager.FindPlayer();
                break;

            case State.move:
                uiManager.ShowUiMove();
                cartManager.CartMove();
                break;

            case State.choice:
                uiManager.ShowUiChoice();
                _curPos = inputManager.ChoiceAnswer();
                inputManager.SelectButton(_curPos);
                break;

            case State.end:
                GameEnd();
                break;

        }
    }



    void GameEnd()
    {
        door.transform.rotation = Quaternion.Euler(0, 90, 0);
        sunLight.transform.rotation = Quaternion.Euler(130, 48, 0);
    }


}
