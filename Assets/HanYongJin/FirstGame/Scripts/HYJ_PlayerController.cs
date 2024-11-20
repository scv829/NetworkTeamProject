using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYJ_PlayerController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("스페이스바 입력");
            StartCoroutine( Attack());
        }
    }
    IEnumerator Attack()
    {
        Debug.Log("공격");
        transform.Rotate(new Vector3(0, -90, 0));
        yield return new WaitForSeconds(0.1f);
        transform.Rotate(new Vector3(0, 90, 0));
    }
}
