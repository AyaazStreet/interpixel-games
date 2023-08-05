using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SwitchController : MonoBehaviour
{
    public bool interactable = false;
    public bool on;

    [SerializeField] private InputActionReference interact;
    public UnityEvent SendSignal_Active;
    public UnityEvent SendSignal_Inactive;

    private void Awake()
    {
        
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
            if (on)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90); //TEMP!!!
                SendSignal_Inactive.Invoke();
                on = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90); //TEMP!!!
                SendSignal_Active.Invoke();
                on = true;
            }
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
            if (on)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90); //TEMP!!!
                SendSignal_Inactive.Invoke();
                on = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90); //TEMP!!!
                SendSignal_Active.Invoke();
                on = true;
            }
        }
    }
}
