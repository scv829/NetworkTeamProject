using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_AviodStone : MonoBehaviourPun, IPunObservable
{
    ljh_AvoidGameManager gameManager;

    // 진짜 떨어지는 돌
    public bool real;

    // 돌 넘어지는 속도
    float rotateSpeed;

    Coroutine smashCo;
    Coroutine returnCo;

    

    public void Vibe()
    {
        //Todo: 좌우로 흔들거리는 효과 줘야함 > 불빛으로 대체 가능
    }

    public void Smash()
    {

        rotateSpeed = 3f;

        if (smashCo == null)
            smashCo = StartCoroutine(SmashCo());


        //Todo: 돌이 넘어져서 바닥에 붙어야함
        //Todo: 플레이어가 닿으면 플레이어가 장애물로 변함 > 이건 플레이어에서? 스톤에서?
    }

    public void ReturnSmash()
    {
            rotateSpeed = 1f;
        
            if(returnCo == null)
                returnCo = StartCoroutine(ReturnCo());

        
    }

    IEnumerator SmashCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Rotate(Vector3.forward * rotateSpeed);

        }
    }

    IEnumerator ReturnCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Rotate(Vector3.forward * -rotateSpeed);

            if (transform.rotation.z < 0)
            {
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
                smashCo = null;
                ReturnSmash();
            }

            if (collision.gameObject.CompareTag("EnterWay"))
            {
                StopCoroutine(smashCo);
                smashCo = null;
                ReturnSmash();
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
