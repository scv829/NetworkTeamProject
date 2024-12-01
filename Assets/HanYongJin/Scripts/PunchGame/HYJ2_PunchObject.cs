using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYJ2_PunchObject : MonoBehaviour
{
    [SerializeField] GameObject objectManager;
    [SerializeField] GameObject light;

    private bool isLighting;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && light.activeSelf == true)
        {
            light.SetActive(false);
            objectManager.gameObject.GetComponent<HYJ2_ObjectManager>().LightingFalse();
        }
    }
}
