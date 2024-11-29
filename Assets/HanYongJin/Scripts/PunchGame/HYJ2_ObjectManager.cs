using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HYJ2_ObjectManager : MonoBehaviour
{
    [SerializeField] GameObject[] PunchObjects;

    private bool isStart = true;
    private bool isLighting = false;
    private void Update()
    {
        
        if (isStart)
        {
            if (!isLighting)
            {
                RandomObjectSelect();
            }
            else if (isLighting)
            {

            }
        }
        
    }

    public void RandomObjectSelect()
    {
        int i = Random.Range(0, 8);
        PunchObjects[i].gameObject.GetComponentInChildren<HYJ2_PunchObject>().LightOn();
        isLighting = true;
    }

    public void ManagerOn()
    {
        isStart = true;
    }
}
