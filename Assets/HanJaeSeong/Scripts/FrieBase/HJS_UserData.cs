using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJS_UserData
{
    public string name;
    public string email;
    public Record record = new Record();
}

[System.Serializable]
public class Record
{
    public int total;
    public int win;
    public int lose;

    public void Reset()
    {
        total = 0;
        win = 0;
        lose = 0;
    }
}