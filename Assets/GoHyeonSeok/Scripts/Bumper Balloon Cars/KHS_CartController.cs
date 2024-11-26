using Photon.Pun;
using UnityEngine;

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



    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("부딪히는 중입니다.");
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(transform.rotation);  // transform 이용으로 회전 구현해보기
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(rb.velocity);
        }
        else if(stream.IsReading)
        {
            rb.position = (Vector3) stream.ReceiveNext();
            rb.rotation = (Quaternion) stream.ReceiveNext();
            rb.velocity = (Vector3) stream.ReceiveNext();

            float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.timestamp));
            rb.position += rb.velocity * lag;

            //transform.rotation = (Quaternion)stream.ReceiveNext();    // transform 이용으로 회전 구현해보기
        }

    }
}
