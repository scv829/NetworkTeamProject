using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ljh_PlayerController : MonoBehaviourPun
{
    [SerializeField] ljh_AvoidGameManager gameManager;
    [SerializeField] ljh_AvoidStone[] stone;

    float moveSpeed = 2;
    //Comment : 죽음 상태
    public bool died;
    //Comment : 점수
    public float score;

    Rigidbody rigid;

    private void Start()
    {
        died = false;
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {

        if (!photonView.IsMine)
            return;

        if(!died)
        Move();

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("ExitWay"))
        {
            transform.localScale = new Vector3(1, 0.35f, 1);
            died = true;
            transform.tag = "Untagged";
            rigid.constraints = RigidbodyConstraints.FreezeAll;

            //score = 100 - gameManager.timer;

        }
    }


    public void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z !=0)
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
}
