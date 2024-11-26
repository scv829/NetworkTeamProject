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
        monsterCount = monster.transform.childCount;
    }

    public void MonsterBringHit()
    {
        monster.transform.GetChild(0).gameObject.GetComponent<HYJ_Monster>().Hit();
    }
}
