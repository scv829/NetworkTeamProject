using Photon.Pun;
using UnityEngine;

public class KHS_PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int _totalInputCount;
    public int TotalInputCount { get { return _totalInputCount; } set { _totalInputCount = value; } }

    [SerializeField] private KHS_MechaMarathonGameManager _mechaMarathonGameManager;


    private void Awake()
    {
        _mechaMarathonGameManager = FindAnyObjectByType<KHS_MechaMarathonGameManager>();
    }

    private void Start()
    {
        Ready(); // 플레이어 오브젝트가 생성되면 스크립트를 참조한다.
        _mechaMarathonGameManager.PlayerReady();    // 게임 매니저에 선언되어있는 PlayerReady함수를 호출하여 준비가 되었다고 알린다.
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
                if(TotalInputCount <= 60)   // (임시) 매크로 방지를 위한 입력횟수 제한
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

        }
        else if (stream.IsReading)
        {
            _totalInputCount = (int)stream.ReceiveNext();   // 플레이어에서 입력한 총 입력횟수 동기화
        }
    }
}