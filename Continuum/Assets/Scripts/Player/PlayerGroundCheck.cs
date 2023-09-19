using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public PlayerFallCollision pfc;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pits"))
        {
            //pfc.contactPoints--;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            //pfc.contactPoints++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pits"))
        {
            //pfc.contactPoints++;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            //pfc.contactPoints--;
        }
    }
}
