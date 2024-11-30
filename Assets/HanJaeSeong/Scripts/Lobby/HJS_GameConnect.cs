using Firebase.Database;
using Firebase.Extensions;
using Fusion;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class HJS_GameConnect : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Vector3 pos = new Vector3(Random.Range(spawnPoint.position.x - 3, spawnPoint.position.x + 3), spawnPoint.position.y, spawnPoint.position.z);
            Runner.Spawn(PlayerPrefab, pos, Quaternion.identity);
        }
    }

}
