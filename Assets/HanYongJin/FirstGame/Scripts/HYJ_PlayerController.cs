using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HYJ_PlayerController : MonoBehaviourPun
{
    [SerializeField] Canvas timerCanvas;
    [SerializeField] TMP_Text playerRankText;

    private HYJ_MonsterSearch monster;
    private float time;
    private int[] playerRanks;
    private void Start()
    {
        monster = transform.GetComponentInParent<HYJ_MonsterSearch>();
        time = 0;
        playerRanks = new int[4];
        for(int i = 0; i < playerRanks.Length; i++)
        {
            playerRanks[i] = 0; // 랭크를 0으로 초기화
        }
    }

    private void Update()
    {
        if (photonView.IsMine == false)
        {
            return;
        }
        
        
        if (monster.monsterCount > 0)
        {
            Debug.Log("공격가능");
            time += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Attack());
            }
        }
        if (monster.monsterCount <= 0 && time > 0)
        {
            photonView.RPC("PlayerRecord", RpcTarget.All);
        }
    }

    IEnumerator Attack()
    {
        transform.Rotate(new Vector3(0, -90, 0));
        monster.GetComponent<HYJ_MonsterSearch>().MonsterBringHit();
        yield return new WaitForSeconds(0.1f);
        transform.Rotate(new Vector3(0, 90, 0));
    }

    [PunRPC]
    public void PlayerRecord()
    {
        for(int i = 0; i< playerRanks.Length; i++)
        {
            if(playerRanks[i] == 0)
            {
                playerRanks[i] = photonView.ViewID;
                timerCanvas.gameObject.SetActive(true);
                Debug.Log(i+1);
                playerRankText.text = i+1.ToString() +"등";
                if(i == 3)
                {
                    //TODO : 게임 종료 결과창 보여주기
                    Debug.Log("게임 끝");
                }
            }
        }
    }
}
