using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYJ_MonsterSearch : MonoBehaviourPun
{
    [SerializeField] GameObject monster;
    [SerializeField] public int monsterCount;

    private void Update()
    {
        Debug.Log("몬스터 카운터 업데이트");
        monsterCount = monster.transform.childCount;
    }

    public void MonsterBringHit()
    {
        
        monster.transform.GetChild(0).gameObject.GetComponent<HYJ_Monster>().Hit();
    }
}
