using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KeyScannerController : MonoBehaviour
{
    public bool interactable = false;

    public float globalTimescale;
    public float? localTimescale;

    [SerializeField] private InputActionReference interact;
    public UnityEvent SendSignal_Active;
    public UnityEvent SendSignal_Inactive;
    private float timeMod;

    private SpriteRenderer sr;
    private PlayerController pc;

    public Sprite btnIn;
    public Sprite btnOut;

    private void Awake()
    {
        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check player collision
        if (collision.gameObject.CompareTag("Player"))
        {
            interactable = true;
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
        
        if (interactable && context.performed && pc.hasKey)
        {
            sr.sprite = btnIn;
            SendSignal_Active.Invoke();
            pc.hasKey = false;
        }
    }
}
