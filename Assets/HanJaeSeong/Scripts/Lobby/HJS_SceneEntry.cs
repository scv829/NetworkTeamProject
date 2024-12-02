using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HJS_SceneEntry : MonoBehaviour
{
    [SerializeField] TMP_Text sceneNameText;        // 게임 씬의 이름
    [SerializeField] Image checkImage;          // 씬 선택 확인 이미지

    [SerializeField] public bool isCheck { get; private set; }  // 씬 선택되었는지 여부

    public void SetInfo(string sceneName)
    {
        // 정보 설정
        sceneNameText.text = sceneName;
    }

    public void CheckScene()
    {
        isCheck = !isCheck;
        checkImage.gameObject.SetActive(isCheck);
    }

    public void ResetSetting()
    {
        isCheck = false;
        checkImage.gameObject.SetActive(isCheck);
    }

}
