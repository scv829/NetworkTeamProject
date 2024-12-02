using Photon.Pun;
using System.Collections;
using UnityEngine;

public class KHS_HitBox : MonoBehaviourPun
{
    [SerializeField] private KHS_CartController _cartConntroller;   // 현재 카트 컨트롤러의 참조를 위한 변수
    [SerializeField] private float _stunTime = 0.5f;    // 스턴 지속시간을 위한 변수

    [SerializeField] private Rigidbody _rb; // 오브젝트의 리지드바디를 위한 변수
    [SerializeField] private Coroutine _stunCoroutine;  // 스턴상태를 위한 코루틴 변수

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("######부딪히기 시작########");
        _rb.velocity = Vector3.zero; // 혹시 모를 속도 줄여주기
        _rb.angularVelocity = Vector3.zero; // 혹시 모를 속도 줄여주기

        Quaternion currentRotate = transform.rotation; 
        transform.rotation = Quaternion.Euler(0, currentRotate.eulerAngles.y, 0);   // 다른 방향으로 비틀어지지 않게 초기화

        if (collision.collider.CompareTag("Player"))    // 삼지창에 닿았다면
        {
            Debug.Log("@@@@@@@@@@플레이어랑 부딪힘@@@@@@@@@@");

  
                photonView.RPC("KHS_StunMove", RpcTarget.All);  // 스턴 RPC 진행

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
        if (_stunCoroutine == null) // 스턴 코루틴이 비어있을때만 실행
        {
            _stunCoroutine = StartCoroutine(StunCoroutine());
        }
    }

    //private IEnumerator StunMoveCoroutine(Transform target, Vector3 targetPos, float stunTime)  // 현재 사용 안하고 있음
    //{
    //    Vector3 _localPos = target.localPosition;
    //    float _elapsedTime = 0f;

    //    while (_elapsedTime < stunTime)
    //    {
    //        target.localPosition = Vector3.Lerp(_localPos, targetPos, _elapsedTime / stunTime);
    //        _elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    target.localPosition = targetPos;
    //}

    private IEnumerator StunCoroutine() // 스턴 코루틴 함수
    {
        _cartConntroller.CanMove = false;   // 움직일 수 없게 만들기
        _cartConntroller.Animator.SetTrigger("Stun");   // 애니메이터의 파라미터 트리거 활성화
        yield return new WaitForSeconds(1f);  // 1.5초간 스턴


        _cartConntroller.Rb.velocity = Vector3.zero; // 혹시 모를 속도 줄여주기
        _cartConntroller.Rb.angularVelocity = Vector3.zero; // 혹시 모를 속도 줄여주기
        _cartConntroller.CanMove = true;    // 다시 움직임 가능하게 만들기
        _stunCoroutine = null;  // 코루틴 변수 속 null로 만들기
    }
}
