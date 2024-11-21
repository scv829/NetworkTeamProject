using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SocialPlatforms.Impl;

public static class HJS_InputDirectionProperty
{
    public const string ANSWER = "AnswerDirection";
    public const string TIME = "TimeTaken";
    public const float MAXTIME = 50f;

    /// <summary>
    /// 입력한 방향과 점수를 저장해주는 확장 메서드
    /// </summary>
    /// <param name="player">플레이어</param>
    /// <param name="answerDirection">입력한 방향</param>
    /// <param name="time">입력을 하는데 걸린 시간</param>
    public static void SetAnswer(this Player player, HJS_RandomSlot.AnswerDirection answerDirection, float time)
    {
        PhotonHashtable customProperty = new PhotonHashtable();
        customProperty[ANSWER] =  answerDirection;
        customProperty[TIME] = time;
        player.SetCustomProperties(customProperty);
    }

    /// <summary>
    /// 입력한 방향과 점수를 보내주는 확장 메서드
    /// </summary>
    /// <param name="player">요청한 플레이어</param>
    /// <returns>방향과 점수</returns>
    public static (HJS_RandomSlot.AnswerDirection, float) GetAnswer(this Player player)
    {
        PhotonHashtable playerProperty = player.CustomProperties;

        (HJS_RandomSlot.AnswerDirection, float) result;

        result.Item1 = playerProperty.ContainsKey(ANSWER) ? (HJS_RandomSlot.AnswerDirection)playerProperty[ANSWER] : HJS_RandomSlot.AnswerDirection.None;
        result.Item2 = playerProperty.ContainsKey(TIME) ? (float)playerProperty[TIME] : MAXTIME;

        return result;
    }

}
