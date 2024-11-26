using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_Button : MonoBehaviourPun
{
    public bool WinButton;

    public ljh_ButtonParent buttonParent;

    [SerializeField] ljh_Boom Bomb;
    [SerializeField] GameObject inputManager;
    [SerializeField] GameObject myPos;

    [SerializeField] GameObject player;


    public void PushedButton(ljh_Button button)
    {
        button.transform.position = button.transform.position + new Vector3(0, 1, 0);
    }


    
    public void TurnOffButton()
    {
        photonView.RPC("RPCTurnOffButton", RpcTarget.All);

    }

    [PunRPC]
    public void RPCTurnOffButton()
    {
        gameObject.SetActive(false);
    }

}
