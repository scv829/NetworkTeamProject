using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_AvoidStone : MonoBehaviourPun, IPunObservable
{
    ljh_AvoidGameManager gameManager;

    
    // 진짜 떨어지는 돌
    public bool real;

    // 돌 넘어지는 속도
    float rotateSpeed;

    public Coroutine smashCo;
    public Coroutine returnCo;


    public void Start()
    {
        rotateSpeed = 2f;
        gameManager = GameObject.FindWithTag("GameController").GetComponent<ljh_AvoidGameManager>();
    }

    public void Vibe()
    {
        //Todo: 좌우로 흔들거리는 효과 줘야함 > 불빛으로 대체 가능
    }

    public void Smash()
    {
        if(smashCo == null)
        smashCo = StartCoroutine(SmashCo());

    }

    public IEnumerator SmashCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Rotate(Vector3.forward * rotateSpeed);
        }
    }

    public void ReturnSmash()
    {

        if (returnCo != null)
            StopCoroutine(returnCo);

        returnCo = StartCoroutine(ReturnCo());


    }


    IEnumerator ReturnCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Rotate(Vector3.forward * -rotateSpeed);

            if (transform.rotation.z < 0)
            {
                StopCoroutine(returnCo);
                returnCo = null;
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (smashCo != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StopCoroutine(smashCo);
                //smashCo = null;
                ReturnSmash();
                //gameManager.attackRoutine = null;
            }

            if (collision.gameObject.CompareTag("EnterWay"))
            {
                StopCoroutine(smashCo);
                //smashCo = null;
                ReturnSmash();
                //gameManager.attackRoutine = null;
            }
        }
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

