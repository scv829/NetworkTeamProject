using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_Balloon : MonoBehaviourPun
{
    [SerializeField] private KHS_CartController _cartController;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.collider.tag}");
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name} 감지됨 !");
            photonView.RPC("KHS_DistroyBallon", RpcTarget.AllBuffered);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("풍선 지금 부딪히고 있어요");    
    }

    [PunRPC]
    public void KHS_DistroyBallon()
    {
        Debug.Log("삭제 진행됨");
        KHS_BumperBalloonCarsGameManager.Instance.GameOverPlayer();
        _cartController.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
