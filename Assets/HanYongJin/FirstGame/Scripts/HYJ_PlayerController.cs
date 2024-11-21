using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYJ_PlayerController : MonoBehaviour
{
    private HYJ_MonsterSearch monster;
    private void Start()
    {
        monster = transform.GetComponentInParent<HYJ_MonsterSearch>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(monster.monsterCount > 0)
            {
                StartCoroutine(Attack());
            }
        }
    }
    IEnumerator Attack()
    {
        transform.Rotate(new Vector3(0, -90, 0));
        monster.GetComponent<HYJ_MonsterSearch>().MonsterBringHit();
        yield return new WaitForSeconds(0.1f);
        transform.Rotate(new Vector3(0, 90, 0));
    }
}
