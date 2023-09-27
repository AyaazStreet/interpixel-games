using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DialogPrompt : MonoBehaviour
{
    public string[] lines;
    public DialogueManager manager;

    public int conditionPower = 0;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (conditionPower)
            {
                default:
                    {

                    }
                    break;
                case 1:
                    if (!GameManager.Instance.pc.A1_Unlocked) 
                    {
                        return;
                    }
                    break;
                case 2:
                    if (!GameManager.Instance.pc.A2_Unlocked) 
                    {
                        return;
                    }
                    break;
                case 3:
                    if (!GameManager.Instance.pc.A3_Unlocked) 
                    {
                        return;
                    }
                    break;
            }

        }


        if (collision.CompareTag("Player") && !triggered)
        {
            manager.StartDialogue(lines);
            triggered = true;
        }
    }
}
