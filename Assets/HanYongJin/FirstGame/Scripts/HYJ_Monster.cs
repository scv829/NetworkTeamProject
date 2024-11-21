using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class HYJ_Monster : MonoBehaviour
{
    [SerializeField] int Hp;
    [SerializeField] bodyType monsterBodyType;

    private HYJ_MonsterController monsterController;

    enum bodyType
    {
        Body,
        Head
    }

    private void Awake()
    {
        if (monsterBodyType == bodyType.Body)
        {
            Hp = 3;
        }
        else if(monsterBodyType == bodyType.Head)
        {
            Hp = 10;
        }
    }

    private void Update()
    {
        if(Hp <= 0)
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

    public void Hit()
    {
        Hp--;
    }
}
