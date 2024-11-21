using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_ButtonParent : MonoBehaviour
{
    GameObject[] buttonArray;

    bool deadBomb;

    public void OnEnable()
    {
        buttonArray = GetComponentsInChildren<GameObject>();

        int deadNum = Random.Range(0, buttonArray.Length - 1);


        // Comment : 테스트용
        // ToDo: 버튼에 값을 옮겨줘야함
        buttonArray[deadNum].GetComponent<ljh_Button>().deadBombButton = true;




    }

}
