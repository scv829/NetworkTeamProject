using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_CartManager : MonoBehaviour
{
    public GameObject[] cartArray;
    public GameObject cart1;
    public GameObject cart2;
    public GameObject cart3;
    public GameObject cart4;



    public void CartMove()
    {
        ljh_TestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_TestGameScene>();
        int index = testGameScene.index;
        Debug.Log(cartArray.Length);
        Debug.Log(index);
        cartArray[index].GetComponent<CinemachineDollyCart>().enabled = true;

    }

}

