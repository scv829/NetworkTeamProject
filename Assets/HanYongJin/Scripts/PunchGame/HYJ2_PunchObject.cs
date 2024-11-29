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
            // TODO : 오브젝트 매니저에 isLighting = false를 전달하여 조절해보기 or
            //          오브젝트 매니저에서 자식개체 중에 isLighting을 검사하여 true인게 있으면 실행?
            transform.GetComponentInParent<HYJ2_ObjectManager>().RandomObjectSelect();
        }
    }
}
