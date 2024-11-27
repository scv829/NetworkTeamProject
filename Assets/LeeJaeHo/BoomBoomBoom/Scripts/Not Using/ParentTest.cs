using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTest : MonoBehaviour
{
    [SerializeField] GameObject capsule;
    [SerializeField] GameObject sphere;
    [SerializeField] GameObject cube;
    void Start()
    {
        capsule.transform.parent = sphere.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            capsule.transform.parent = cube.transform;
        }
    }
}
