using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipManager : MonoBehaviour
{
    [SerializeField] private GameObject throwPointer;

    void Awake()
    {
        //deactivate throw mode at start
        throwPointer.SetActive(false); 
    }

    public void Manual_Aim_Performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Activate throw mode
            throwPointer.SetActive(true);
        }
        else if (context.canceled)
        {
            //Deactivate throw mode
            throwPointer.SetActive(false);
        }
    }

    public void Equip_1_performed(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //Activate throw mode for bolts
            throwPointer.GetComponent<ThrowController>().selected = 1;

            //Change infusing mode
            GetComponent<PlayerController>().infusing = false;
            throwPointer.GetComponent<ThrowController>().infused = 0;
        }
    }

    public void Equip_2_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Activate throw mode for cannisters
            throwPointer.GetComponent<ThrowController>().selected = 2;

            //Change infusing mode
            GetComponent<PlayerController>().infusing = true;
            throwPointer.GetComponent<ThrowController>().infused = 0;
        }
        else if(context.canceled)
        {
            //Change infusing mode
            GetComponent<PlayerController>().infusing = false;
        }
    }

    public void Equip_3_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Activate throw mode for rods
            throwPointer.GetComponent<ThrowController>().selected = 3;

            //Change infusing mode
            GetComponent<PlayerController>().infusing = false;
            throwPointer.GetComponent<ThrowController>().infused = 0;
        }
    }

    public void Equip_4_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Deactivate throw mode
            throwPointer.GetComponent<ThrowController>().selected = 0;
            throwPointer.SetActive(false);

            //Change to infusing mode
            GetComponent<PlayerController>().infusing = false;
            throwPointer.GetComponent<ThrowController>().infused = 0;
        }
    }
}
