using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class KHS_CartController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _bodyRotateSpeed;

    [SerializeField] bool _isGameOver;
    public bool IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }

    private Rigidbody rb;
    public Rigidbody Rb { get { return rb; } set { rb = value; } }

    [SerializeField] Animator _animator;
    public Animator Animator { get { return _animator; } set { _animator = value; } }

    [SerializeField] private bool _canMove;
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        CanMove = true;
        Ready();
        KHS_BumperBalloonCarsGameManager.Instance.PlayerReady();
        Debug.Log($"레디한 플레이어 : {photonView.Owner.ActorNumber}");
    }

    private void Update()
    {
        if (photonView.IsMine && KHS_BumperBalloonCarsGameManager.Instance.IsGameStarted == true && CanMove == true)
            // 소유권인 나에게 있고 && 현재 게임 매니저에서 게임을 시작했다고 판단한다면
        {
            BodyMove(); // 카트를 움직일 수 있다.
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
            stream.SendNext(CanMove);   // 움직일 수 있는지 여부
        }
        else if (stream.IsReading)
        {
            CanMove = (bool)stream.ReceiveNext();   // 움직일 수 있는지 여부
        }
    }
}
