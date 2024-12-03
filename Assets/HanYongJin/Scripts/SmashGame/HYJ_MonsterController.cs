using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HYJ_MonsterController : MonoBehaviour
{
    [SerializeField] GameObject monster;
    [SerializeField] public int monsterCount;

    private void Update()
    {
        monsterCount = monster.transform.childCount;
    }

    public void MonsterBodyDown()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            monster.transform.GetChild(i).transform.position += new Vector3(0, -1, 0); // 몬스터 바디를 파괴 시, 몬스터의 위치를 내려주기
        }
    }

    public void MonsterDie()
    {
        Debug.Log("죽음!");
    }
}
