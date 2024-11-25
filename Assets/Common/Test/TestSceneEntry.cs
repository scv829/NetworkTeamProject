using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 씬 선택 프리팹의 스크립트
/// </summary>
public class TestSceneEntry : MonoBehaviour
{
    [SerializeField] TMP_Text sceneName;        // 게임 씬의 이름
    [SerializeField] Button startButton;        // 시작 버튼

    public void SetInfo(string sceneName)
    {
        this.sceneName.text = sceneName;                // 생성시 설정한 이름으로 설정
        startButton.onClick.AddListener(StartGame);     // 버튼에게 게임 시작 이벤트를 달아준다.
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(sceneName.text);     // 게임 씬 전환
    }

}
