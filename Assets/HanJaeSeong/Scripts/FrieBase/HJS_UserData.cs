using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 엔티티
/// </summary>
public class HJS_UserData
{
    public string name;         // 유저의 닉네임
    public string email;        // 유저의 이메일(Id)
    public string description;  // 유저의 소개
    public Vector3 spawnPos;    // 플에이어 스폰 위치
    public Record record = new Record();    // 유저의 게임 기록
}

[System.Serializable]
public class Record
{
    public int total;   // 총 판수
    public int win;     // 승리한 판수
    public int lose;    // 패배한 판수

    public void Reset() // 초기화 시키는 함수 -> 맨 처음 저장할 때
    {
        total = 0;
        win = 0;
        lose = 0;
    }
}