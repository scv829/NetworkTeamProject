using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class HYJ_GameTimer : MonoBehaviourPun
{
    /*  TODO : 게임 타이머 만들기
     *  1. UI로 타이머를 만들기
     *  2. 타이머는 두가지다. 개인타이머 / 전체 타이머
     *  3. 개인 타이머 저장 기능 만들기 > 몬스터컨트롤러.몬스터다이 함수를 통해 기능 사용할 수 있도록 하기 > 기록/점수 저장용
     *  4. 전체 타이머는 플레이어 전원이 
     */
    [Header("전체 타이머")]
    [SerializeField] TMP_Text timerLeftText;
    [SerializeField] TMP_Text mid;
    [SerializeField] TMP_Text timerRightText;

    private float time = 0f;
    private float playerTime = 0f;
    private bool timerStart = false;

    private void Update()
    {
        if (timerStart)
        {
            TimerUpdate();
        }
    }

    public void TimerUpdate()
    {
        time += Time.deltaTime;
        if(time <= 10f)
        {
            timerLeftText.text = $"{(int)time}";
            timerRightText.text = $"{(int)((time % 1) * 100)}";
        }
        else
        {
            mid.gameObject.SetActive(false);
            timerLeftText.color = new Color(0.8f, 0.0f, 0.0f);
            timerLeftText.text = "Time";
            timerRightText.color = new Color(0.8f, 0.0f, 0.0f);
            timerRightText.text = "Over";
        }
    }

    public void TimerStart()
    {
        timerStart = true;
    }
}
