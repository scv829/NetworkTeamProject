using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ljh_Boom : MonoBehaviourPun
{
    [SerializeField] ljh_BoomTestGameScene scene;
    public GameObject bomb;
    public ljh_Player player;


    public void Vibe()
    {
        for (int i = 0; i < 10; i++)
        {
            bomb.transform.position += new Vector3(1f, 0, 0);
            bomb.transform.position += new Vector3(-1f, 0, 0);
        }
    }

    public void Boom()
    {
        photonView.RPC("BoomRPC", RpcTarget.All);
    }

    [PunRPC]
    public void BoomRPC()
    {
        bomb.SetActive(false);
        if ((int)player.playerNumber == (int)ljh_GameManager.instance.myTurn)
            player.gameObject.SetActive(false);
        // 사운드
        // 터지는 효과
        // 플레이어 탈락
        Invoke("BoomReset", 1f);
    }

    public void BoomReset()
    {
        bomb.SetActive(true);
    }

    public void NoBoom()
    {
        photonView.RPC("NoBoomRPC", RpcTarget.All);
    }

    [PunRPC]
    public void NoBoomRPC()
    {
        bomb.SetActive(false);
    }

    
}
