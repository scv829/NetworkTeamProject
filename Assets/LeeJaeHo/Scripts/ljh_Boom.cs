using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ljh_Boom : MonoBehaviour
{
    public GameObject bomb;
    

    public void Vibe()
    {
        for (int i = 0; i < 10; i++)
        {
            bomb.transform.position += new Vector3(1f, 0, 0);
            bomb.transform.position += new Vector3(-1f, 0, 0);
        }
    }

    public void Boom()
    {
        //폭발 애니메이션
        //플레이어 죽음 함수 호출
    }
}
