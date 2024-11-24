using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_CartManager : MonoBehaviour
{
    public GameObject[] cartArrayEnter;
    public GameObject cart1Enter;
    public GameObject cart2Enter;
    public GameObject cart3Enter;
    public GameObject cart4Enter;

    public GameObject[] cartArrayExit;
    public GameObject cart1Exit;
    public GameObject cart2Exit;
    public GameObject cart3Exit;
    public GameObject cart4Exit;



    public void CartMoveEnter()
    {
        ljh_TestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_TestGameScene>();
        int index = testGameScene.index;

        cartArrayEnter[index].GetComponent<CinemachineDollyCart>().enabled = true;

    }

    public void CartMoveExit()
    {

        ljh_TestGameScene testGameScene = GameObject.FindWithTag("GameController").GetComponent<ljh_TestGameScene>();
        int index = testGameScene.index;

        cartArrayExit[index].GetComponent<CinemachineDollyCart>().enabled = true;
    }
}

