using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ljh_Waypoint : MonoBehaviour
{
    [SerializeField] GameObject player;
    public Color lineColor = Color.green;
    [SerializeField] Transform[] points;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        points = GetComponentsInChildren<Transform>();

        int nextIndex = 1;

        Vector3 curPos = points[nextIndex].position;
        Vector3 nextPos;

        for(int i =0; i <= points.Length; i++)
        {
            if (i < points.Length)
            {
                nextPos = points[i].position;

                Gizmos.DrawLine(curPos, nextPos);

                curPos = nextPos;
            }
        }
    }

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player");
        
    }

    private void Update()
    {
        if (player != null)
            Moving();
    }

    public void Moving()
    {
        
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3.MoveTowards(player.transform.position, points[i].position, Time.deltaTime * 3);
        }
    }
}
