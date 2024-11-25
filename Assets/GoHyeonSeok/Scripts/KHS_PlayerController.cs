using Photon.Pun;
using UnityEngine;

public class KHS_PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int _totalInputCount;
    public int TotalInputCount { get { return _totalInputCount; } set { _totalInputCount = value; } }

    [SerializeField] private KHS_MechaMarathonGameManager _mechaMarathonGameManager;


    private void Awake()
    {
        // (임시) 해당 오브젝트가 생성되었을때 게임매니저를 찾는 경우
        _mechaMarathonGameManager = FindAnyObjectByType<KHS_MechaMarathonGameManager>();

    }

    private void Start()
    {
        // 플레이어 오브젝트가 생성되면 준비완료했다고 신호를 보내준다.
        Ready();
        _mechaMarathonGameManager.PlayerReady();
        Debug.Log($"레디한 플레이어 : {photonView.Owner.ActorNumber}");

    }

    private void Update()
    {
        // 시작하지 않은 상태면 return
        if (_mechaMarathonGameManager.IsStarted == false)
            return;

        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                TotalInputCount++;
                Debug.Log("J키 눌러짐");
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
            stream.SendNext(_totalInputCount);

        }
        else if (stream.IsReading)
        {
            _totalInputCount = (int)stream.ReceiveNext();
        }
    }
}