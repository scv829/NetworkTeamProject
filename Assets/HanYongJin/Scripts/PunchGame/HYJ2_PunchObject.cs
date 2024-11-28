using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYJ2_PunchObject : MonoBehaviour
{
    [SerializeField] GameObject light;

    private bool isLighting;

    public void LightOn()
    {
        Debug.Log("on");
        isLighting = true;
        light.gameObject.SetActive(true);
    }

    public void LightOff()
    {
        Debug.Log("off");
        isLighting = false;
        light.gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isLighting)
        {
            Debug.Log("!");
            LightOff();
        }
    }
}
