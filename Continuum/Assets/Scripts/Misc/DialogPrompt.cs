using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPrompt : MonoBehaviour
{
    public string[] lines;
    public DialogueManager manager;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            manager.StartDialogue(lines);
            triggered = true;
        }
    }
}
