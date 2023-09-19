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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check player collision
        if (collision.gameObject.CompareTag("Player"))
        {
            interactable = true;
            sr.material.SetFloat("_Outline_Thickness", OUTLINE_THICKNESS);
        }
        
        //Check throwable collision
        if (collision.gameObject.CompareTag("Bolt"))
        {
            if (on)
            {
                active = false;
                on = false;
                sr.sprite = sOff;

                SoundManager.PlaySound(SoundManager.Sound.snd_interact_switch, transform.position);
            }
            else
            {
                active = true;
                on = true;
                sr.sprite = sOn;

                SoundManager.PlaySound(SoundManager.Sound.snd_interact_switch, transform.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactable = false;
            sr.material.SetFloat("_Outline_Thickness", 0f);
        }
    }

    public void Interact_performed(InputAction.CallbackContext context)
    {
        if (interactable && context.performed)
        {
            if (on)
            {
                active = false;
                on = false;
                sr.sprite = sOff;

                SoundManager.PlaySound(SoundManager.Sound.snd_interact_switch, transform.position);
            }
            else
            {
                active = true;
                on = true;
                sr.sprite = sOn;

                SoundManager.PlaySound(SoundManager.Sound.snd_interact_switch, transform.position);
            }
        }
    }
}
