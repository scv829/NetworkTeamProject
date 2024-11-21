using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/// <summary>
/// 랜덤으로 이미지를 설정하는 클래스
/// </summary>
{
    public enum AnswerDirection { Top, Botton, Right, Left, None }

    [SerializeField] HJS_CircleSlot[] slots;  // 슬롯들
    [SerializeField] int heartIndex = -1;     // 하트가 있는 면
    private LinkedList<int> deque = new LinkedList<int>();  // 슬롯의 상태를 설정할 이중 큐 생성
    [SerializeField] AnswerDirection answer;  // 하트가 있는 방향

    public AnswerDirection Answer => answer;

    /// <summary>
    /// 슬롯의 심볼을 설정해주는 함수
    /// </summary>
    public void Setting()
    {
        deque.Clear(); // 초기화

        for(int i = 0; i < 5; i++)                              // 앞 또는 뒤에 숫자를 넣어서 무작위로 배치한다
        {
            if(Random.Range(0, 1f) > 0.5f) deque.AddFirst(i);   
            else deque.AddLast(i);
        }

        for (int index = 0; index < slots.Length; index++)      // 슬롯의 갯수 만큼 반복해서 상태를 넣어준다
        {
            int num = deque.First.Value;                        // 맨 앞에 있는 값부터 가져온다.
            deque.RemoveFirst();                                // 가져온 값은 삭제해서 중복된 값이 안나오게 방지한다.

            if (num == 0 && heartIndex.Equals(-1)) heartIndex = index;                                      // 나온 값이 하트인 경우 해당 위치를 저장한다.
            else if (index == slots.Length - 1 && heartIndex.Equals(-1)) { num = 0; heartIndex = index; }   // 마지막까지 하트가 안나왔을 때 하트가 나오게 설정한다.

            slots[index].SetSlot(num);  // 슬롯의 상태가 담겨있는 값으로 해당 슬롯을 설정한다.
        }

        answer = (AnswerDirection)heartIndex;   // 마지막으로 모든 슬롯의 상태가 정해지면 하트가 있는 방향을 저장한다.
    }

}
