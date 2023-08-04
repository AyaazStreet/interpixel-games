using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float distance = 6.6f;
    public float angle = 45f;
    public int segNum = 12;

    PolygonCollider2D col;
    
    void Start()
    {
        col = GetComponent<PolygonCollider2D>();
        SetCollider();
    }

    void Update()
    {
        SetCollider();
    }

    void SetCollider()
    {
        //idek whats going on here #spaghetti code
        Vector2[] newPoints = new Vector2[segNum + 2];
        newPoints[0] = Vector2.zero;
        newPoints[1] = Quaternion.Euler(0, 0, -angle) * Vector2.up * distance; ;
        for (int i = 2; i < segNum + 2; i++)
        {
            newPoints[i] = Quaternion.Euler(0, 0, angle * 2 / segNum) * newPoints[i-1];
        }
        col.points = newPoints;
    }
}
