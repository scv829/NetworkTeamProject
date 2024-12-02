using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_CartManager : MonoBehaviourPun, IPunObservable
{
    public GameObject[] cartArrayEnter;
    public GameObject cart1Enter;
    public GameObject cart2Enter;
    public GameObject cart3Enter;
    public GameObject cart4Enter;

    [SerializeField] public GameObject exitCart;
    [SerializeField] public GameObject player;

    public void CartMoveEnter()
    {
        ljh_BoomTestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_BoomTestGameScene>();
        int index = testGameScene.index;

        cartArrayEnter[index].GetComponent<CinemachineDollyCart>().enabled = true;

    }

    public void CartReset()
    {
        ljh_BoomTestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_BoomTestGameScene>();
        int index = testGameScene.index;

        cartArrayEnter[index].GetComponent<CinemachineDollyCart>().m_Position = 0;
        cartArrayEnter[index].GetComponent<CinemachineDollyCart>().enabled = false;
        
    }

    public void CartMoveExit()
    {
        exitCart.GetComponent<CinemachineDollyCart>().enabled = true;
        player.transform.position = exitCart.transform.position;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
           // stream.SendNext(transform.position);
        }
        else
        {
           // transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}

