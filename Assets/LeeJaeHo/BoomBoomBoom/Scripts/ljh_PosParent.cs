using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_PosParent : MonoBehaviour
{
    public ljh_Pos[] posArray;

    private void OnEnable()
    {
        posArray = GetComponentsInChildren<ljh_Pos>();
    }
}
