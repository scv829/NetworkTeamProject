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

        transform.Translate(new Vector3(x, 0, z)* moveSpeed * Time.deltaTime);
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
