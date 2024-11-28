using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_Balloon : MonoBehaviourPun
{
    [SerializeField] private KHS_CartController _cartController;    // 플레이어 변수

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.collider.tag}");
        if (collision.collider.CompareTag("Player"))    // 카트 앞에 달려있는 가시에 부딪혔을때
        {
            Debug.Log($"{collision.gameObject.name} 감지됨 !");
            photonView.RPC("KHS_DistroyBallon", RpcTarget.AllBuffered); // RPC
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        photonView.RPC("KHS_DistroyBallon", RpcTarget.AllBuffered); // RPC
    //    }
    //}


    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("풍선 지금 부딪히고 있어요");    
    }

    [PunRPC]
    public void KHS_DistroyBallon()
    {
        Debug.Log("삭제 진행됨");
        KHS_BumperBalloonCarsGameManager.Instance.GameOverPlayer(); // 현재 남아있는 인원수를 위해 함수 호출
        _cartController.IsGameOver = true;  // 해당 플레이어가 게임 오버됐음을 알리기 위한 bool변수
        _cartController.gameObject.SetActive(false);    // 해당 플레이어가 게임오버 되었으니 비활성화 진행

        if(photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);    // 풍선은 삭제
        }

    }
}
