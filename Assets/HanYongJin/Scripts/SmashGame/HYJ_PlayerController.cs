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

    //색상
    [SerializeField] Renderer bodyRenderer;
    [SerializeField] Color color;

    private void Start()
    {
        monster = gameObject.transform.GetComponentInParent<HYJ_MonsterSearch>();
        time = 0;
        playerRanks = new int[4];
        for(int i = 0; i < playerRanks.Length; i++)
        {
            playerRanks[i] = 0; // 랭크를 0으로 초기화
        }

        Vector3 vectorColor = photonView.Owner.GetPlayerColor();
        color.r = vectorColor.x; color.g = vectorColor.y; color.b = vectorColor.z;
        bodyRenderer.material.color = color;
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
            //TODO : 전원 멈추고 승자가 누군지 알려주기
            GameObject.FindWithTag("GameController").GetComponent<HYJ_TestGameScene>().GameEnd(PhotonNetwork.LocalPlayer.ActorNumber);
            
        }
    }

    IEnumerator Attack()
    {
        transform.Rotate(new Vector3(0, -90, 0));
        monster.GetComponent<HYJ_MonsterSearch>().MonsterBringHit();
        yield return new WaitForSeconds(0.1f);
        transform.Rotate(new Vector3(0, 90, 0));
    }

    
}
