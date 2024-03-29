using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;

public class SwitchController : Controller
{
    public Sprite sOn;
    public Sprite sOff;

    private void OnEnable()
    {
        interact.action.performed += Interact_performed;
    }

    private void OnDisable()
    {
        interact.action.performed -= Interact_performed;
    }

    public new void ChangeState() 
    {
        sendSignalSprite = true;

        if (on)
        {
            active = false;
            on = false;
            sr.sprite = sOff;
        }
        else
        {
            active = true;
            on = true;
            sr.sprite = sOn;
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
        
        //Check throwable collision
        if (collision.gameObject.CompareTag("Bolt"))
        {
            ChangeState();
            SoundManager.PlaySound(SoundManager.Sound.snd_interact_switch, transform.position);
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
            ChangeState();
            SoundManager.PlaySound(SoundManager.Sound.snd_interact_switch, transform.position);
        }
    }
}
