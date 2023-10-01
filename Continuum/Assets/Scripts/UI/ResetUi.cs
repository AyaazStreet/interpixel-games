using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResetUi : MonoBehaviour
{
    public Image indi;

    private float fill = 0;
    private float fillMax = 1;

    bool pressed = false;

    void Update()
    {
        if (pressed && fill < fillMax)
        {
            fill += Time.deltaTime * 2;
        }
        else if (fill > 0)
        {
            fill -= Time.deltaTime * 2;
        }
        
        indi.fillAmount = fill / fillMax;

        if (fill >= fillMax)
        {
            GameManager.Instance.RestartGame();
        }
    }

    public void Restart_performed(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            pressed = true;
        }
        else if(context.canceled)
        {
            pressed = false;
        }
    }


}
