using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class HJS_GameConnect : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject PlayerPrefab;

    // 플레이어가 들어왔을 때
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, HJS_PlayerPosition.Instance.PlayerPos, Quaternion.identity);
        }
    }

}
