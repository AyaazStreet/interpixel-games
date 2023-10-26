using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectionIndicator : MonoBehaviour
{
    public EnemyDetection enemy;
    Vector2 aimDir;
    float aimAngle;

    void Update()
    {
        if (!enemy)
        {
            Destroy(gameObject); 
            return;
        }
        
        aimDir = enemy.transform.position - transform.position;

        aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;

        //Calculate quaternion and apply to rotation
        Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        transform.rotation = q ;
    }
}
