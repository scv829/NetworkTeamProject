using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public static class HJS_CustomProperty
{
    private static PhotonHashtable customProperty = new PhotonHashtable();

    public const string LOAD = "loadScene";

    public static void SetLoad(this Player player, bool load)
    {
        customProperty[LOAD] = load;
        player.SetCustomProperties(customProperty);
    }

    public static bool GetLoad(this Player player)
    {
        PhotonHashtable playerProperty = player.CustomProperties;
        return playerProperty.ContainsKey(LOAD) ? (bool)playerProperty[LOAD] : false;
    }

    public const string COLOR = "color";


    public static void SetPlayerColor(this Player player, Vector3 color)
    {
        customProperty[COLOR] = color;
        player.SetCustomProperties(customProperty);
    }

    public static Vector3 GetPlayerColor(this Player player)
    {
        PhotonHashtable playerProperty = player.CustomProperties;
        return playerProperty.ContainsKey(LOAD) ? (Vector3)playerProperty[COLOR] : Vector3.zero;
    }
}
