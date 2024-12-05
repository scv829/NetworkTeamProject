using Photon.Pun;
using UnityEngine;

public class HYJ_Monster : MonoBehaviourPun
{
    //몬스터의 체력
    [Header("몬스터 체력")]
    [SerializeField] int Hp;

    //몬스터의 바디 타입
    [Header("몬스터 바디 타입")]
    [SerializeField] bodyType monsterBodyType;

    private HYJ_MonsterController monsterController;

    enum bodyType
    {
        Body,
        Head
    }

    private void Awake() // 플레이어 타입에 따른 체력 설정
    {
        if (monsterBodyType == bodyType.Body)
        {
            Hp = 3;
        }
        else if (monsterBodyType == bodyType.Head)
        {
            Hp = 10;
        }
    }

    private void Update()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
            transform.GetComponentInParent<HYJ_MonsterController>().MonsterBodyDown();
            if (monsterBodyType == bodyType.Head)
            {
                // TODO : 머리를 0으로 만들면 해당 플레이어는 게임 종료!
                transform.GetComponentInParent<HYJ_MonsterController>().MonsterDie();
            }
        }
    }

    public void Hit() // 몬스터 피격 시 함수
    {
        Hp--;
        photonView.RPC("MonsterUpdateRPC", RpcTarget.All);
    }

    [PunRPC]
    public void MonsterUpdateRPC() // 몬스터 피격 시, 몬스터의 상황을 업데이트 해주는 함수
    {
        if (Hp <= 0) // HP가 0이 되면
        {
            PhotonNetwork.Destroy(gameObject); // 오브젝트 파괴
            transform.GetComponentInParent<HYJ_MonsterController>().MonsterBodyDown(); // 몬스터의 위치를 내려주기
            if (monsterBodyType == bodyType.Head)
            {
                // TODO : 머리를 0으로 만들면 해당 플레이어는 게임 종료!
                transform.GetComponentInParent<HYJ_MonsterController>().MonsterDie();
            }
        }
    }
}
