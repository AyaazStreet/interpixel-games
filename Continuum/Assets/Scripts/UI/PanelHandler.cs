using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandler : MonoBehaviour
{
    public GameObject panel;
    public int conditionPower = 0;

    private void Start()
    {
        panel.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        { 
            switch (conditionPower)
            {
                default:
                    panel.SetActive(true);
                    break;
                case 1: 
                    if (GameManager.Instance.pc.A1_Unlocked) { panel.SetActive(true); }
                    break; 
                case 2: 
                    if (GameManager.Instance.pc.A2_Unlocked) { panel.SetActive(true); }
                    break;
                case 3: 
                    if (GameManager.Instance.pc.A3_Unlocked) { panel.SetActive(true); }
                    break;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            panel.SetActive(false);
        }
    }
}
