using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ButtonController : Controller
{
    public Sprite btnIn;
    public Sprite btnOut;

    private void OnEnable()
    {
        interact.action.performed += Interact_performed;
    }

    private void OnDisable()
    {
        interact.action.performed -= Interact_performed;
    }

    private void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        //
        if (timer > 0f)
        {
            timer -= Time.deltaTime * timeMod;
            if (timer <= 0f)
            {
                sr.sprite = btnOut;

                active = false;
                sendSignalSprite = true;
            }
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
            sr.sprite = btnIn;
            timer = active_time;

            active = true;
            sendSignalSprite = true;

            SoundManager.PlaySound(SoundManager.Sound.snd_interact_btn, transform.position);
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

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        //Barrier
        if (collision.gameObject.CompareTag("Barrier"))
        {
            blocked = true;
        }
    }*/

    public void Interact_performed(InputAction.CallbackContext context)
    {
        if (interactable && context.performed && !blocked)
        {
            sr.sprite = btnIn;
            timer = active_time;

            active = true;
            sendSignalSprite = true;

            SoundManager.PlaySound(SoundManager.Sound.snd_interact_btn, transform.position);
        }
    }
}
