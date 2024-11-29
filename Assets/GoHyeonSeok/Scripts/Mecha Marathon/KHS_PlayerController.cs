using Photon.Pun;
using UnityEngine;

public class KHS_PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int _totalInputCount;
    public int TotalInputCount { get { return _totalInputCount; } set { _totalInputCount = value; } }

    [SerializeField] private KHS_MechaMarathonGameManager _mechaMarathonGameManager;

    [SerializeField] private Renderer _renderer;
    public Renderer Renderer { get { return _renderer; } set { _renderer = value; } }

    [SerializeField] private KHS_PlayerUI _playerUI;


    private void Awake()
    {
        _mechaMarathonGameManager = FindAnyObjectByType<KHS_MechaMarathonGameManager>();
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

        Ready(); // 플레이어 오브젝트가 생성되면 스크립트를 참조한다.
        _mechaMarathonGameManager.PlayerReady();    // 게임 매니저에 선언되어있는 PlayerReady함수를 호출하여 준비가 되었다고 알린다.

        _playerUI.NickName = photonView.Owner.NickName;
        Debug.Log($"레디한 플레이어 : {photonView.Owner.ActorNumber}");

    }

    private void Update()
    {
        if (_mechaMarathonGameManager.IsStarted == false)   // 시작하지 않은 상태면 return
            return;

        if (photonView.IsMine)  // 이 오브젝트가 내 소유권이라면
        {
            if (Input.GetKeyDown(KeyCode.J))    // J를 눌렀을 때
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

    private void Ready()
    {
        photonView.RPC("ReadyRPC", RpcTarget.AllBuffered);
    }


    [PunRPC]
    private void ReadyRPC()
    {
        _mechaMarathonGameManager.PlayerController[photonView.Owner.ActorNumber] = this;
        Debug.Log($"{photonView.Owner.ActorNumber}번째 플레이어 스크립트 참조 됨");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_totalInputCount);  // 플레이어에서 입력한 총 입력횟수 동기화
            stream.SendNext(Renderer.material.color.r);  
            stream.SendNext(Renderer.material.color.g);  
            stream.SendNext(Renderer.material.color.b);  


        }
        else if (stream.IsReading)
        {
            Color color = new Color();
            _totalInputCount = (int)stream.ReceiveNext();   // 플레이어에서 입력한 총 입력횟수 동기화
            color.r = (float)stream.ReceiveNext();
            color.g = (float)stream.ReceiveNext();
            color.b = (float)stream.ReceiveNext();

            Renderer.material.color = color;

        }
    }
}