using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWalkSound : MonoBehaviour
{
    public SoundManager.Sound oldSound;
    public SoundManager.Sound newSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Feet"))
        {
            GameManager.Instance.pc.walkSound = newSound;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Feet"))
        {
            GameManager.Instance.pc.walkSound = oldSound;
        }
    }
}
