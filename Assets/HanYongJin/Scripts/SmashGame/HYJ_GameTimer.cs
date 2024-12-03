using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class HYJ_GameTimer : MonoBehaviourPun
{
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
        if(time <= 10f) //10초를 넘기지 않았을 때
        {
            timerLeftText.text = $"{(int)time}"; // 타이머의 초 단위
            timerRightText.text = $"{(int)((time % 1) * 100)}";  // 초의 하위 소수 단위
        }
        else  // 10초가 넘어가게 되었을 때
        {
            mid.gameObject.SetActive(false);
            timerLeftText.color = new Color(0.8f, 0.0f, 0.0f);
            timerLeftText.text = "Time";
            timerRightText.color = new Color(0.8f, 0.0f, 0.0f);
            timerRightText.text = "Over";
        }
    }

    public void TimerStart() // 타이머를 시작시키기 위한 함수
    {
        timerStart = true;
    }
}
