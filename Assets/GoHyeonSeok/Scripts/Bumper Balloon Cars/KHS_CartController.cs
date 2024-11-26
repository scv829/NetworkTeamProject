using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class KHS_CartController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] float moveSpeed;
    [SerializeField] float bodyRotateSpeed;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            BodyMove();

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





        // 탱크 몸체 이동
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        // 탱크 몸체 회전
        float x = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * x * bodyRotateSpeed * Time.deltaTime);
    }


    private void OnCollisionStay(Collision collision) // 부딪히는중일때
    {
        Debug.Log("부딪히는 중입니다.");
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
            //stream.SendNext(transform.rotation);  // transform 이용으로 회전 구현해보기
        }
        else if (stream.IsReading)
        {
            //transform.rotation = (Quaternion)stream.ReceiveNext();    // transform 이용으로 회전 구현해보기
        }
    }
}
