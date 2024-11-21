using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class HYJ_Monster : MonoBehaviour
{
    [SerializeField] int Hp;
    [SerializeField] bodyType monsterBodyType;

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

    public void Hit()
    {
        Debug.Log("공격 받음!");
    }
}
