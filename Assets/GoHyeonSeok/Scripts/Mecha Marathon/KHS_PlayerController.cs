using Photon.Pun;
using UnityEngine;

public class KHS_PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int _totalInputCount;  // 플레이어가 'J'키를 입력한 총 횟수
    public int TotalInputCount { get { return _totalInputCount; } set { _totalInputCount = value; } }

    [SerializeField] private KHS_MechaMarathonGameManager _mechaMarathonGameManager;    // 게임 매니저를 참조하기 위한 변수

    [SerializeField] private Renderer _renderer;    // 게임오브젝트의 컬러를 변경해주기 위한 랜더 변수
    public Renderer Renderer { get { return _renderer; } set { _renderer = value; } }

    [SerializeField] private KHS_PlayerUI _playerUI;    // 플레이어의 식별을 위한 UI 변수


    private void Awake()
    {
        _mechaMarathonGameManager = FindAnyObjectByType<KHS_MechaMarathonGameManager>();    // 게임매니저를 탐색해서 참조시키기
        Renderer = GetComponent<Renderer>();    // 랜더 컴포넌트를 넣어주기
    }

    private void Start()
    {
        switch (PhotonNetwork.LocalPlayer.ActorNumber)  // 현재 포톤 네트워크상의 본인의 넘버링을 판별
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

        Ready(); // 플레이어 오브젝트가 생성되면 스크립트를 참조한다.
        _mechaMarathonGameManager.PlayerReady();    // 게임 매니저에 선언되어있는 PlayerReady함수를 호출하여 준비가 되었다고 알린다.

        _playerUI.NickName = photonView.Owner.NickName; // 현재 이 오브젝트의 소유자의 닉네임을 UI로 출력
        Debug.Log($"레디한 플레이어 : {photonView.Owner.ActorNumber}");

    }

    private void Update()
    {
        if (_mechaMarathonGameManager.IsStarted == false)   // 시작하지 않은 상태면 return
            return;

        if (photonView.IsMine)  // 이 오브젝트가 내 소유권이라면
        {
            if (Input.GetKeyDown(KeyCode.J))    // 'J'를 눌렀을 때
            {
                if (TotalInputCount <= 60)   // (임시) 매크로 방지를 위한 입력횟수 제한
                {
                    TotalInputCount++;
                    Debug.Log("J키 눌러짐");
                }
            }
        }
        else return;
    }

    private void Ready()    // 스크립트를 참조시키는 RPC 함수를 호출하는 함수
    {
        photonView.RPC("ReadyRPC", RpcTarget.AllBuffered);  
    }


    [PunRPC]
    private void ReadyRPC() // 스크립트를 참조시키는 RPC 함수
    {
        _mechaMarathonGameManager.PlayerController[photonView.Owner.ActorNumber] = this;    // 게임매니저의 플레이어 배열에 자신의 넘버링에 참조시키기
        Debug.Log($"{photonView.Owner.ActorNumber}번째 플레이어 스크립트 참조 됨");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_totalInputCount);  // 플레이어에서 입력한 총 입력횟수 동기화
            stream.SendNext(Renderer.material.color.r);  // 컬러 r
            stream.SendNext(Renderer.material.color.g);  // 컬러 g
            stream.SendNext(Renderer.material.color.b);  // 컬러 b


        }
        else if (stream.IsReading)
        {
            Color color = new Color();
            _totalInputCount = (int)stream.ReceiveNext();   // 플레이어에서 입력한 총 입력횟수 동기화
            color.r = (float)stream.ReceiveNext();  // 컬러 r
            color.g = (float)stream.ReceiveNext();  // 컬러 g
            color.b = (float)stream.ReceiveNext();  // 컬러 b

            Renderer.material.color = color;    // 오브젝트의 색상을 받아온 색상으로 변경시켜주기

        }
    }
}

//  TODO : 무승부 판단하는 로직 구현해보기