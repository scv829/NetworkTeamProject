using Photon.Pun;
using UnityEngine;

public class KHS_HeyHoController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private KHS_MechaMarathonGameManager _mechaMarathonGameManager;
    public KHS_PlayerController _playerController;  // 테스트를 위한 변수 선언
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isMoved;
    [SerializeField] private bool _isTargeted;
    [SerializeField] private Vector3 targetPos;

    [SerializeField] private float _finishTime;
    [SerializeField] private float _onlineFinishTime;

    public float FinishTime {  get { return _finishTime; } set { _finishTime = value; } }


    private void Awake()
    {
        _mechaMarathonGameManager = FindAnyObjectByType<KHS_MechaMarathonGameManager>();
    }

    private void Start()
    {
        ReadyHeyHo();

    }

    private void Update()
    {
        if (_mechaMarathonGameManager.IsFinished == true && _isMoved == false && photonView.IsMine)
        {
            MoveHeyHo(_playerController.TotalInputCount);
            Debug.Log("헤이호 움직이는중");
        }
        else if (_isMoved == true)
        {
            return;
        }
    }

    public void MoveHeyHo(int count)
    {
        if (_isTargeted == false)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + count);
            Debug.Log($"현재 타겟 위치 {targetPos}");
            _isTargeted = true;


            // 헤이호와 타겟 위치 사이의 거리
            float _distance = Vector3.Distance(transform.position, targetPos);

            // 목표위치까지 걸리는 시간
            FinishTime = _distance / _moveSpeed;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);

        if (transform.position.z >= targetPos.z && _isMoved == false)
        {
            _isMoved = true; // 목표 위치에 도달했음을 표시
        }
    }

    private void ReadyHeyHo()
    {
        photonView.RPC("ReadyHeyHoRPC", RpcTarget.AllBuffered);
    }


    [PunRPC]
    private void ReadyHeyHoRPC()
    {
        _mechaMarathonGameManager.HeyHoController[photonView.Owner.ActorNumber] = this;
        Debug.Log($"{PhotonNetwork.LocalPlayer.ActorNumber}번째 헤이호 스크립트 참조 성공");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_finishTime);   // 이동할때 걸리는 시간

        }
        if(stream.IsReading) 
        {
            _finishTime = (float)stream.ReceiveNext();    // 이동할때 걸리는 시간
        }
    }
}
