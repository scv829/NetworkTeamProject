using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KHS_HeyHoController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private KHS_MechaMarathonGameManager _mechaMarathonGameManager;
    public KHS_PlayerController _playerController;  // 테스트를 위한 변수 
    [SerializeField] private float _moveSpeed;  // 헤이호의 이동속도 변수
    [SerializeField] private bool _isMoved;     // 헤이호가 움직였는가 변수
    [SerializeField] private bool _isTargeted;  // 헤이호가 목적지를 설정했는가 변수
    [SerializeField] private Vector3 targetPos; // 헤이호가 이동해야할 목적지 변수

    [SerializeField] private UnityEvent _startTiming;   // 헤이호가 움직이기 시작했다는 이벤트

    [SerializeField] private float _finishTime; // 헤이호가 목적지까지 걸리는 시간 변수
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
            // 매개변수로 받은 총 입력횟수만큼 position.z 값을 늘려주기
            targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + count / 2 ); // TODO : 거리조절 필요?
            Debug.Log($"현재 타겟 위치 {targetPos}");

            if(count == 0)  // 플레이어가 한번도 입력하지 않았다면
            {
                targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z); // TODO : 거리조절 필요?
                Debug.Log($"현재 타겟 위치 {targetPos}");
            }

            float _distance = Vector3.Distance(transform.position, targetPos);  // 헤이호와 타겟 위치 사이의 거리

            FinishTime = _distance / _moveSpeed;    // 목적지까지 걸리는 시간 구하기
            Debug.Log($"헤이호 컨트롤러에서 걸리는 시간 계산 완료됨 {FinishTime}");    // 목표위치까지 걸리는 시간

            _isTargeted = true; // 타겟 설정 완료

            photonView.RPC("FinishedHeyHo", RpcTarget.MasterClient);    // 헤이호가 출발했다는 변수의 값을 늘려주기 위한 RPC 호출

            StartCoroutine(StartTimingCoroutine()); // 헤이호가 출발했다는 타이밍 선언


        }

        // 설정해준 목적지로 설정한 속도로 헤이호가 날아감
        transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);

        if (transform.position.z >= targetPos.z && _isMoved == false)   // 타겟위치보다 현재위치가 더 높아지면 && 아직 도착을 하지 않았을때
        {
            _isMoved = true; // 목표 위치에 도달했음을 표시
  
        }
    }

    private void ReadyHeyHo()   // 스크립트 참조를 위한 RPC 함수
    {
        photonView.RPC("ReadyHeyHoRPC", RpcTarget.AllBuffered);
    }


    [PunRPC]
    private void ReadyHeyHoRPC()
    {
        _mechaMarathonGameManager.HeyHoController[photonView.Owner.ActorNumber] = this;
        Debug.Log($"{photonView.Owner.ActorNumber}번째 헤이호 스크립트 참조 성공");
    }

    [PunRPC]
    private void FinishedHeyHo()
    {
        _mechaMarathonGameManager._heyHoFinished++;
    }

    private IEnumerator StartTimingCoroutine()  // 헤이호가 움직이기 시작하는 타이밍 함수
    {
        yield return new WaitForSeconds(1f);    // (임시) _heyHoFinished변수가 제대로 반영이 될때까지 조금의 틈 주기
        _startTiming?.Invoke(); // 이벤트로 추가해놓은 함수 실행
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