using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Threading;
using UnityEngine.Events;
using Photon.Pun.Demo.Cockpit;

public class ljh_PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] ljh_AvoidGameManager gameManager;
    [SerializeField] ljh_AvoidStone[] stone;
    ljh_AvoidUIManager uiManager;
    


    float moveSpeed = 3;
    //Comment : 죽음 상태
    public bool died;
    //Comment : 점수
    public float score;

    Rigidbody rigid;

    public UnityEvent onPlayerDead;

    public string myName;

    private void Start()
    {
        died = false;
        rigid = GetComponent<Rigidbody>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<ljh_AvoidGameManager>();
        uiManager = GameObject.FindWithTag("Finish").GetComponent<ljh_AvoidUIManager>();

        //myName = PhotonNetwork.LocalPlayer.NickName;
    }
    void Update()
    {

        if (!photonView.IsMine)
            return;

        if (!died)
            Move();



    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("ExitWay"))
            {
                transform.localScale = new Vector3(1, 0.35f, 1);
                died = true;
                transform.tag = "Untagged";
                rigid.constraints = RigidbodyConstraints.FreezeAll;
                Dead();
                
            }
        }
    }

    

    public void DeletePlayer()
    {
        gameManager.playerCount--;
    }

    private void Dead()
    {
        onPlayerDead.Invoke();
    }


    public void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
            transform.forward = new Vector3(x, 0, z);

        transform.Translate(new Vector3(x, 0, z).normalized * moveSpeed * Time.deltaTime, Space.World);
        //photonView.RPC("RPCMove", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPCMove()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(x, 0, z));


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
