using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private UnityEvent _startTiming;

    public float FinishTime { get { return _finishTime; } set { _finishTime = value; } }


    private void Awake()
    {
        // 게임 메니저 찾기
        _mechaMarathonGameManager = FindAnyObjectByType<KHS_MechaMarathonGameManager>();
    }

    private void Start()
    {
        // 헤이호가 움직이기 시작하면 실행시킬 이벤트 추가
        _startTiming.AddListener(_mechaMarathonGameManager.MovingHeyHo);

        // 게임메니저에 스크립트를 참조시키는 함수 선언
        ReadyHeyHo();
    }

    private void Update()
    {
        // 게임메니저에서 입력이 끝났을때 && 현재 움직이고 있지 않을때 && 이 오브젝트가 내 소유일때
        if (_mechaMarathonGameManager.IsInputFinished == true && _isMoved == false && photonView.IsMine)
        {
            // 플레이어 컨트롤러의 TotalInputCount를 매개변수로 사용하여 해당 변수의 값만큼 이동하는 함수 선언
            MoveHeyHo(_playerController.TotalInputCount);
            Debug.Log("헤이호 움직이는중");
        }
        else if (_isMoved == true)  // 움직였다면
        {
            return;
        }
    }

    public void MoveHeyHo(int count)
    {
        if (_isTargeted == false)   // 타겟이 정해지지 않았을때
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + count);
            Debug.Log($"현재 타겟 위치 {targetPos}");

            // 헤이호와 타겟 위치 사이의 거리
            float _distance = Vector3.Distance(transform.position, targetPos);

            // 목표위치까지 걸리는 시간
            FinishTime = _distance / _moveSpeed;
            Debug.Log($"헤이호 컨트롤러에서 걸리는 시간 계산 완료됨 {FinishTime}");

            _isTargeted = true; // 타겟 설정 완료
            _mechaMarathonGameManager._heyHoFinished++;
            _startTiming?.Invoke(); 

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
        Debug.Log($"{photonView.Owner.ActorNumber}번째 헤이호 스크립트 참조 성공");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_finishTime);   // 이동할때 걸리는 시간

        }
        if (stream.IsReading)
        {
            _finishTime = (float)stream.ReceiveNext();    // 이동할때 걸리는 시간
        }
    }
}