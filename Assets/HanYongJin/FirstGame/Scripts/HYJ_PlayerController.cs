using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HYJ_PlayerController : MonoBehaviourPun
{
    [SerializeField] Canvas timerCanvas;
    [SerializeField] TMP_Text playerLeftTimerText;
    [SerializeField] TMP_Text playerRightTimerText;

    private HYJ_MonsterSearch monster;
    private float time;
    private void Start()
    {
        monster = transform.GetComponentInParent<HYJ_MonsterSearch>();
        time = 0;
    }

    private void Update()
    {
        if (photonView.IsMine == false)
        {
            return;
        }
        
        
        if (monster.monsterCount > 0)
        {
            time += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Attack());
            }
        }
        if (monster.monsterCount <= 0 && time > 0)
        {
            timerCanvas.gameObject.SetActive(true);
            PlayerTimeRecord();
        }
    }

    IEnumerator Attack()
    {
        transform.Rotate(new Vector3(0, -90, 0));
        monster.GetComponent<HYJ_MonsterSearch>().MonsterBringHit();
        yield return new WaitForSeconds(0.1f);
        transform.Rotate(new Vector3(0, 90, 0));
    }

    public void PlayerTimeRecord()
    {
        //time => 게임 타이머에서 기록 가져오기
        playerLeftTimerText.text = $"{(int)time}";
        playerRightTimerText.text = $"{(int)((time % 1) * 100)}";
    }
}
