using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYJ_MonsterSearch : MonoBehaviourPun
{
    [SerializeField] public GameObject monster; 
    [SerializeField] public int monsterCount;
    private void Update()
    {
        monsterCount = monster.transform.childCount; // 몬스터 카운트에 몬스터 오브젝트의 자식 개수 저장
    }

    public void MonsterBringHit()
    {
        monster.transform.GetChild(0).gameObject.GetComponent<HYJ_Monster>().Hit(); // 몬스터 Hit() 기능 가져오기
    }
}
