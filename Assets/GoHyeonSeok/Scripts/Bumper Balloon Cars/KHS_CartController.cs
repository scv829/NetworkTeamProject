using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class KHS_CartController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] float _moveSpeed;          // 카트의 이동속도 변수
    [SerializeField] float _bodyRotateSpeed;    // 카트의 회전속도 변수

    [SerializeField] bool _isGameOver;          // 현재 플레이어가 게임오버되었는지 확인하는 변수
    public bool IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }

    private Rigidbody rb;   // 카트의 리지드바디 변수
    public Rigidbody Rb { get { return rb; } set { rb = value; } }

    [SerializeField] Animator _animator;    // 애니메이션 재생을 위한 애니메이터 변수
    public Animator Animator { get { return _animator; } set { _animator = value; } }

    [SerializeField] private bool _canMove; // 현재 움직일 수 있는 상황인지 판단하기 위한 변수
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }



    private Vector3 networkPosition;    // 동기화를 위한 네트워크 포지션 변수
    private float deltaPosition;        // 지연보상을 적용한 포지션 변수

    private Quaternion networkRotation; // 동기화를 위한 네트워크 회전 변수
    private float deltaRotation;        // 지연보상을 적용한 회전 변수

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PhotonNetwork.SendRate = 60;    // 초당 패키지를 몇번 보내는지 횟수 설정
        PhotonNetwork.SerializationRate = 60;   // 초당 OnPhotonSerialize가 몇번 실행하는지 횟수
    }

    private void Start()
    {
        CanMove = true; // 시작시엔 움직일 수 있기에 true
        Ready();    //  오브젝트가 생성되고 준비가 완료되었다고 알리기위해 호출
        KHS_BumperBalloonCarsGameManager.Instance.PlayerReady();    // 로드되었다고 게임매니저에 선언되어있는 함수 사용
        Debug.Log($"레디한 플레이어 : {photonView.Owner.ActorNumber}");
    }

    private void Update()
    {
        if (photonView.IsMine && KHS_BumperBalloonCarsGameManager.Instance.IsGameStarted == true && CanMove == true)
            // 소유권인 나에게 있고 && 현재 게임 매니저에서 게임을 시작했다고 판단한다면
        {
            BodyMove(); // 카트를 움직일 수 있다.
        }

        if (photonView.IsMine == false)
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, deltaPosition * Time.deltaTime * PhotonNetwork.SerializationRate);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, deltaRotation * Time.deltaTime * PhotonNetwork.SerializationRate);
        }
    }

    private void BodyMove()
    {
        // 리지드바디를 사용한 움직임 구현
        //float zMove = Input.GetAxis("Vertical");

        //Vector3 moveDirection = transform.forward * zMove * moveSpeed;
        //rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        //float xRotate = Input.GetAxis("Horizontal");
        //Quaternion deltaRotation = Quaternion.Euler(Vector3.up * xRotate * bodyRotateSpeed * Time.deltaTime);
        //rb.MoveRotation(rb.rotation * deltaRotation);





        // 카트 몸체 이동
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);
        }

        // 카트 몸체 회전
        float x = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * x * _bodyRotateSpeed * Time.deltaTime);
    }

    private void Ready()    // 현재 카트 생성 완료되었다는 RPC 선언
    {
        photonView.RPC("KHS_CartReadyRPC", RpcTarget.AllBuffered);
    }


    [PunRPC]
    private void KHS_CartReadyRPC()
    {
        KHS_BumperBalloonCarsGameManager.Instance.CartController[photonView.Owner.ActorNumber] = this;  // 게임 매니저 카트배열에 this 참조시켜주기
        Debug.Log($"{photonView.Owner.ActorNumber}번째 카트 스크립트 참조 됨");
    }


    private void OnCollisionStay(Collision collision) // 부딪히는중일때
    {
        //Debug.Log("부딪히는 중입니다.");
        rb.velocity = Vector3.zero; // 혹시 모를 속도 줄여주기
        rb.angularVelocity = Vector3.zero; // 혹시 모를 속도 줄여주기

        Quaternion currentRotate = transform.rotation;  
        transform.rotation = Quaternion.Euler(0, currentRotate.eulerAngles.y, 0);   // 다른 방향으로 비틀어지지 않게 초기화
    }

    private void OnCollisionExit(Collision collision)   // 부딪히다가 떼졌을때ㅐ
    {
        rb.velocity = Vector3.zero; // 혹시 모를 속도 줄여주기
        rb.angularVelocity = Vector3.zero;  // 혹시 모를 속도 줄여주기

        Quaternion currentRotate = transform.rotation;
        transform.rotation = Quaternion.Euler(0, currentRotate.eulerAngles.y, 0);   // 다른 방향으로 비틀어지지 않게 초기화
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);    // 위치
            stream.SendNext(transform.rotation);    // 회전
            stream.SendNext(CanMove);   // 움직일 수 있는지 여부
            stream.SendNext(PhotonNetwork.Time);    // 현재 네트워크 시간을 전송
        }
        else if (stream.IsReading)
        {
            networkPosition = (Vector3)stream.ReceiveNext();     // 위치
            networkRotation = (Quaternion)stream.ReceiveNext();  // 회전
            CanMove = (bool)stream.ReceiveNext();   // 움직일 수 있는지 여부
            double sentTime = (double)stream.ReceiveNext(); // 현재 네트워크 시간을 전송

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));

            //transform.position = Vector3.Lerp(transform.position, networkPosition, lag );
            //transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, lag );

            deltaPosition = Vector3.Distance(transform.position, networkPosition);
            deltaRotation = Quaternion.Angle(transform.rotation, networkRotation);
        }
    }
}
