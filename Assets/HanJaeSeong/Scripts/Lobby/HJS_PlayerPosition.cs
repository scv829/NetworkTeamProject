using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJS_PlayerPosition : MonoBehaviour
{
    public static HJS_PlayerPosition Instance;

    [SerializeField] public Vector3 playerPos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
