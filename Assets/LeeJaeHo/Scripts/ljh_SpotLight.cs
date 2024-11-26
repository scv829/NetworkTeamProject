using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ljh_SpotLight : MonoBehaviourPun, IPunObservable
{
    [SerializeField] ljh_InputManager inputManager;
    
    public void MovingSpotlight(ljh_Button[] buttons, int index)
    {
            transform.LookAt(buttons[index].transform.position);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
