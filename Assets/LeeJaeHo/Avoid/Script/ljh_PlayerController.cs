using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ljh_PlayerController : MonoBehaviourPun
{
    float moveSpeed = 2;
    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        Move();
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
