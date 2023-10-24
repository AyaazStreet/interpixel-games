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

    [Range(0, 3)]
    public int inputType;

    public List<GameObject> cardPrefabs = new List<GameObject>();
    public List<Sprite> offSprites = new List<Sprite>();
    public List<Sprite> onSprites = new List<Sprite>();

    private void OnEnable()
    {
        interact.action.performed += Interact_performed;
    }

    private void OnDisable()
    {
        interact.action.performed -= Interact_performed;
    }

    private void Awake()
    {
        //Initialise timescales
        localTimescale = TimeScaleManager.globalTimescale;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        //sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        im = GameObject.Find("Player").GetComponent<InventoryManager>();

        sr.sprite = offSprites[inputType];
    }

    public new void ChangeState()
    {
        if (!activated) 
        {
            sr.sprite = onSprites[inputType];
            active = true;
            activated = true;
            sendSignalSprite = true;
        }
        else
        {
            sr.sprite = offSprites[inputType];
            active = false;
            activated = false;
            sendSignalSprite = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check player collision
        if (collision.gameObject.CompareTag("Player") && !blocked)
        {
            interactable = true;
            sr.material.SetFloat("_Outline_Thickness", OUTLINE_THICKNESS);
        }

        //Barrier
        if (collision.gameObject.CompareTag("Barrier"))
        {
            blocked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactable = false;
            sr.material.SetFloat("_Outline_Thickness", 0f);
        }

        //Barrier
        if (collision.gameObject.CompareTag("Barrier"))
        {
            blocked = false;
        }
    }

    public void Interact_performed(InputAction.CallbackContext context)
    {
        if (interactable && context.performed && !blocked)
        {
            if (!activated)
            {
                if (im.inventory.Any(item => item.type == inputType && item.used == false))
                {
                    im.UseInventoryItem(inputType);
                    ChangeState();
                }
                SoundManager.PlaySound(SoundManager.Sound.snd_interact_btn, transform.position);
            }
            else
            {
                im.UnuseInventoryItem(inputType);
                ChangeState();
            }
            
        }
    }
}
