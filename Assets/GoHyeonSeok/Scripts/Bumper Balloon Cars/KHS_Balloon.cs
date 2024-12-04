using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_Balloon : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private KHS_CartController _cartController;    // 플레이어 변수

    [SerializeField] private Renderer _renderer;    // 컬러 변경을 위한 렌더 변수
    public Renderer Renderer { get { return _renderer; } set { _renderer = value; } }

    [SerializeField] private bool _isTouched;   // 풍선이 터졌는지 확인하는 변수

    public bool IsTouched { get { return _isTouched; } set { _isTouched = value; } }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
        switch (photonView.Owner.ActorNumber)   // 현재 소유자의 넘버링에 따른 색상 변경
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

    private void Start()
    {
        IsTouched = false;  // 해당 스크립트가 실행되었을때는 false로 진행
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.collider.tag}");
        if (collision.collider.CompareTag("Player") && IsTouched == false)    // 카트 앞에 달려있는 가시에 부딪혔을때 && 아직 풍선이 터지지 않았을때
        {
            Debug.Log($"{collision.gameObject.name} 감지됨 !");
            photonView.RPC("KHS_DistroyBallon", RpcTarget.AllBufferedViaServer); // 풍선을 터트리는 RPC 함수 호출
            IsTouched = true;   // 풍선이 터졌으니 true

        }
    }


    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("풍선 지금 부딪히고 있어요");    
    }

    [PunRPC]
    public void KHS_DistroyBallon() // 풍선이 터졌다는 것을 알리기 위한 RPC함수
    {
        KHS_BumperBalloonCarsGameManager.Instance.GameOverPlayer(); // 현재 남아있는 인원수를 위해 함수 호출
        Debug.Log("삭제 진행됨");
        _cartController.IsGameOver = true;  // 해당 플레이어가 게임 오버됐음을 알리기 위한 bool변수
        _cartController.gameObject.SetActive(false);    // 해당 플레이어가 게임오버 되었으니 비활성화 진행


        //if(photonView.IsMine)
        //{
        //    PhotonNetwork.Destroy(gameObject);    // 풍선은 삭제
        //}

        // 삭제 진행시에 문제가 생기는 것으로 판단하여 우선 주석처리

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Renderer.material.color.r); // 컬러 R
            stream.SendNext(Renderer.material.color.g); // 컬러 G
            stream.SendNext(Renderer.material.color.b); // 컬러 B
        }
        else if (stream.IsReading)
        {
            Color color = new Color();
            color.r = (float)stream.ReceiveNext();  // 컬러 R
            color.g = (float)stream.ReceiveNext();  // 컬러 G
            color.b = (float)stream.ReceiveNext();  // 컬러 B

            Renderer.material.color = color;    // 현재 색상 받아온 컬러로 변경해주기
        }
    }
}
