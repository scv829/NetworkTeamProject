using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneEntry : MonoBehaviour
{
    [SerializeField] TMP_Text sceneName;
    [SerializeField] Button startButton;

    public void SetInfo(string sceneName)
    {
        this.sceneName.text = sceneName;
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(sceneName.text);     // 게임 씬 전환
    }

}
