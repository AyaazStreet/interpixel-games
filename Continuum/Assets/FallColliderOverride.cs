using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallColliderOverride : MonoBehaviour
{
    public Vector2 dir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pits"))
        {
            
                if (!GetComponentInParent<PlayerFallCollision>().dirSet)
                {
                    StartCoroutine(GetComponentInParent<PlayerFallCollision>().BecomeFalling(dir));
                    Debug.Log(dir);
                    GetComponentInParent<PlayerFallCollision>().dirSet = true;
                }
                
            
            
        }
    }
}
