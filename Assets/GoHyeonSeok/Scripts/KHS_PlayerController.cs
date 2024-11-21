using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_PlayerController : MonoBehaviour
{
    [SerializeField] private int _totalInputCount;
    public int TotalInputCount { get { return _totalInputCount; } set { _totalInputCount = value; } }

    [SerializeField] private KHS_TestGameScean _testScean;
    [SerializeField] private KHS_HeyHoController _heyHoController;

    private void Awake()
    {
        _testScean = FindAnyObjectByType<KHS_TestGameScean>();
        _heyHoController = FindAnyObjectByType<KHS_HeyHoController>();
    }

    private void Update()
    {
        // 시작하지 않은 상태면 return
        if (_testScean.IsStarted == false)
            return;

        if(Input.GetKeyDown(KeyCode.J))
        {
            TotalInputCount++;
        }
    }
}
