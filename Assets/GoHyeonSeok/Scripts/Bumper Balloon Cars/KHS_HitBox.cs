using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_HitBox : MonoBehaviourPun
{
    [SerializeField] private KHS_CartController _cartConntroller;
    [SerializeField] private float _stunTime = 0.5f;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Coroutine _stunCoroutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("@@@@@@@@@@플레이어랑 부딪힘@@@@@@@@@@");
    //        Vector3 targetPos = _cartConntroller.transform.localPosition;
    //        targetPos.z -= 1;

    //        StartCoroutine(StunMoveCoroutine(_cartConntroller.transform, targetPos, _stunTime));
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("######부딪히기 시작########");
        _rb.velocity = Vector3.zero; // 혹시 모를 속도 줄여주기
        _rb.angularVelocity = Vector3.zero; // 혹시 모를 속도 줄여주기

        Quaternion currentRotate = transform.rotation;
        transform.rotation = Quaternion.Euler(0, currentRotate.eulerAngles.y, 0);   // 다른 방향으로 비틀어지지 않게 초기화

        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("@@@@@@@@@@플레이어랑 부딪힘@@@@@@@@@@");

                photonView.RPC("KHS_StunMove", RpcTarget.All /*targetPos, _stunTime*/ );

        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log("부딪히는 중입니다.");
    //    _rb.velocity = Vector3.zero; // 혹시 모를 속도 줄여주기
    //    _rb.angularVelocity = Vector3.zero; // 혹시 모를 속도 줄여주기

    //    Quaternion currentRotate = transform.rotation;
    //    transform.rotation = Quaternion.Euler(0, currentRotate.eulerAngles.y, 0);   // 다른 방향으로 비틀어지지 않게 초기화
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("부딪히는 중입니다.");
    //    _rb.velocity = Vector3.zero; // 혹시 모를 속도 줄여주기
    //    _rb.angularVelocity = Vector3.zero; // 혹시 모를 속도 줄여주기

    //    Quaternion currentRotate = transform.rotation;
    //    transform.rotation = Quaternion.Euler(0, currentRotate.eulerAngles.y, 0);   // 다른 방향으로 비틀어지지 않게 초기화
    //}

    [PunRPC]
    private void KHS_StunMove( /*Vector3 targetPos, float stunTime*/)
    {
        //StartCoroutine(StunMoveCoroutine(_cartConntroller.transform, targetPos, stunTime));
        if (_stunCoroutine == null)
        {
            _stunCoroutine = StartCoroutine(StunCoroutine());
        }
    }

    private IEnumerator StunMoveCoroutine(Transform target, Vector3 targetPos, float stunTime)
    {
        Vector3 _localPos = target.localPosition;
        float _elapsedTime = 0f;

        while (_elapsedTime < stunTime)
        {
            target.localPosition = Vector3.Lerp(_localPos, targetPos, _elapsedTime / stunTime);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localPosition = targetPos;
    }

    private IEnumerator StunCoroutine()
    {
        _cartConntroller.CanMove = false;
        //_cartConntroller.Rb.AddForce(-_cartConntroller.transform.forward * 2f, ForceMode.Impulse);
        _cartConntroller.Animator.SetTrigger("Stun");
        yield return new WaitForSeconds(1.5f);


        _cartConntroller.Rb.velocity = Vector3.zero; // 혹시 모를 속도 줄여주기
        _cartConntroller.Rb.angularVelocity = Vector3.zero; // 혹시 모를 속도 줄여주기
        _cartConntroller.CanMove = true;
        _stunCoroutine = null;
    }
}
