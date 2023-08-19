using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public const float ACTIVE_TIME = 2f;
    public bool interactable = false;
    public float timer;

    public float globalTimescale;
    public float? localTimescale;

    [SerializeField] private InputActionReference interact;
    public UnityEvent SendSignal_Active;
    public UnityEvent SendSignal_Inactive;
    private float timeMod;

    private SpriteRenderer sr;

    public Sprite btnIn;
    public Sprite btnOut;

    private void Awake()
    {
        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

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

        if (timer > 0f)
        {
            timer -= Time.deltaTime * timeMod;
            if (timer <= 0f)
            {
                SendSignal_Inactive.Invoke();
                sr.sprite = btnOut;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check player collision
        if (collision.gameObject.CompareTag("Player"))
        {
            interactable = true;
        }
        
        //Check throwable collision
        if (collision.gameObject.CompareTag("Bolt"))
        {
            sr.sprite = btnIn;
            SendSignal_Active.Invoke();
            timer = ACTIVE_TIME;

            SoundManager.PlaySound(SoundManager.Sound.snd_interact, transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactable = false;
        }
    }

    public void Interact_performed(InputAction.CallbackContext context)
    {
        if (interactable && context.performed)
        {
            sr.sprite = btnIn;
            SendSignal_Active.Invoke();
            timer = ACTIVE_TIME;

            SoundManager.PlaySound(SoundManager.Sound.snd_interact, transform.position);
        }
    }
}
