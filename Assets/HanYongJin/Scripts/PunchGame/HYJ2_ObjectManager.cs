using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HYJ2_ObjectManager : MonoBehaviour
{
    [SerializeField] GameObject[] PunchObjects;
    [SerializeField] TMP_Text fieldUIText;
    [SerializeField] int[] playerRanks;

    private bool isStart = true;
    private bool isLighting = false;
    private bool isEnding = false;
    private int lightScoreCount = 0;

    

    private void Update()
    {
        if(lightScoreCount == 20 && !isEnding)
        {
            isEnding = true;
            isStart = false;
            GameObject mainCamera = GameObject.FindWithTag("MainCamera");
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(0,35,0), 25f);
        }

        if (isStart)
        {
            if (!isLighting)
            {
                RandomObjectSelect();
            }
        }
    }

    public void RandomObjectSelect()
    {
        int i = Random.Range(0, 8);
        PunchObjects[i].gameObject.SetActive(true);
        isLighting = true;
    }

    public void LightingFalse()
    {
        isLighting = false;
        lightScoreCount++;
    }

    public void ManagerOn()
    {
        StartCoroutine(StartCountAndUI());
        isStart = true;
    }

    IEnumerator StartCountAndUI()
    {
        // 1초 뒤 게임 카운트시작
        yield return new WaitForSeconds(1f);

        // 3초의 카운트 뒤 게임 시작
        fieldUIText.text = "3";
        yield return new WaitForSeconds(1f);
        fieldUIText.text = "2";
        yield return new WaitForSeconds(1f);
        fieldUIText.text = "1";
        yield return new WaitForSeconds(1f);
        fieldUIText.text = "Start!";
        yield return new WaitForSeconds(0.2f);
        fieldUIText.gameObject.SetActive(false);
    }
}
