using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HJS_GameConnect : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] HJS_FadeController fadeController;

    // 플레이어가 들어왔을 때
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, spawnPoint.position, Quaternion.identity);
            Runner.StartCoroutine(FadeRoutine());
        }
    }

    private IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(3f);

        fadeController.FadeIn();
    }

}
