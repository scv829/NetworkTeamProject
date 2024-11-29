using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_Balloon : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private KHS_CartController _cartController;    // 플레이어 변수

    [SerializeField] private Renderer _renderer;
    public Renderer Renderer { get { return _renderer; } set { _renderer = value; } }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                Renderer.material.color = Color.red; break;
            case 2:
                Renderer.material.color = Color.yellow; break;
            case 3:
                Renderer.material.color = Color.green; break;
            case 4:
                Renderer.material.color = Color.blue; break;
        }
    }

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Renderer.material.color.r);
            stream.SendNext(Renderer.material.color.g);
            stream.SendNext(Renderer.material.color.b);
        }
        else if (stream.IsReading)
        {
            Color color = new Color();
            color.r = (float)stream.ReceiveNext();
            color.g = (float)stream.ReceiveNext();
            color.b = (float)stream.ReceiveNext();

            Renderer.material.color = color;
        }
    }
}
