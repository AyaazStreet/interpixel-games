using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipManager : MonoBehaviour
{
    [SerializeField] private GameObject throwPointer;
    [SerializeField] private PlayerController pc;

    public List<Collectable> collected = new();

    public int selected = 0;

    public int E1_count = 0;
    public int E2_count = 0;
    public int E3_count = 0;

    public class Collectable
    {
        public int unlockNum;
        public int pickupNumber;
        public Vector3 pickupPosition;

        public Collectable(int _unlockNum, int _pickupNumber, Vector3 _pickupPosition)
        {
            unlockNum = _unlockNum;
            pickupNumber = _pickupNumber;
            pickupPosition = _pickupPosition;
        }
    }

    void Awake()
    {
        //deactivate throw mode at start
        throwPointer.SetActive(false);
    }

    private void Update()
    {
        if (throwPointer.activeSelf && selected == 2)
        {
            pc.infusing = true;
        }
        else
        {
            pc.infusing = false;
        }
    }

    public void Manual_Aim_Performed(InputAction.CallbackContext context)
    {
        if (context.performed && !PauseManager.Instance.pauseUI.activeSelf && !PauseManager.Instance.optionsUI.activeSelf)
        {
            //Activate throw mode
            throwPointer.SetActive(true);

            //
            
        }
        else if (context.canceled)
        {
            //Deactivate throw mode
            throwPointer.SetActive(false);

            //
            
        }
    }

    public void Equip_1_performed(InputAction.CallbackContext context)
    {
        if(context.performed && pc.T1_Unlocked)
        {
            //Activate throw mode for bolts
            selected = 1;
        }
    }

    public void Equip_2_performed(InputAction.CallbackContext context)
    {
        if (context.performed && pc.T2_Unlocked)
        {
            //Activate throw mode for cannisters
            selected = 2;
        }
    }

    public void Equip_3_performed(InputAction.CallbackContext context)
    {
        if (context.performed && pc.T3_Unlocked)
        {
            //Activate throw mode for rods
            selected = 3;
        }
    }

    public void Equip_4_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Deactivate throw mode
            throwPointer.SetActive(false);
        }
    }
}
