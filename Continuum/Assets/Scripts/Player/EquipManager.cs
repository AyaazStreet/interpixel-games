using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipManager : MonoBehaviour
{
    [SerializeField] private GameObject throwPointer;
    [SerializeField] private PlayerController pc;

    public int selected;

    void Awake()
    {
        //deactivate throw mode at start
        throwPointer.SetActive(false); 

        selected = 0;
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
        if(context.performed && pc.T1_Unlocked)
        {
            //Activate throw mode for bolts
            selected = 1;   

            //Change infusing mode
            GetComponent<PlayerController>().infusing = false;
            throwPointer.GetComponent<ThrowController>().infused = 0;
        }
    }

    public void Equip_2_performed(InputAction.CallbackContext context)
    {
        if (context.performed && pc.T2_Unlocked)
        {
            //Activate throw mode for cannisters
            selected = 2;

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
        if (context.performed && pc.T3_Unlocked)
        {
            //Activate throw mode for rods
            selected = 3;

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
            throwPointer.SetActive(false);

            //Change to infusing mode
            GetComponent<PlayerController>().infusing = false;
            throwPointer.GetComponent<ThrowController>().infused = 0;
        }
    }
}
