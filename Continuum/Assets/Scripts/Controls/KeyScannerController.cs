using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KeyScannerController : Controller
{
    public bool activated = false;

    private InventoryManager im;

    public Sprite btnIn;
    public Sprite btnOut;

    private void Awake()
    {
        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        im = GameObject.Find("Player").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check player collision
        /*if (collision.gameObject.CompareTag("Player"))
        {
            interactable = true;
        }*/

        if (collision.gameObject.CompareTag("Player"))
        {
            if(!activated)
            {
                if (im.inventory.Any(item => item.type == 0))
                {
                    sr.sprite = btnIn;
                    active = true;

                    im.RemoveInventoryItem(0);
                    activated = true;
                }
            }
            
        }
    }
}
