using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class TypeTextHandler : MonoBehaviour
{
    public InputActionAsset inputs;
    public TextMeshProUGUI text;
    private string textString;
    private float textSpeed = 0.03f;
    public float startDelay;
    public bool stayOn = false;

    public bool on = false;

    // Start is called before the first frame update
    void Awake()
    {
        textString = text.text;
        text.text = string.Empty;
    }

    private void OnEnable()
    {

        text.text = string.Empty;
        StartText();
    }

    private void OnDisable()
    {

        StopAllCoroutines();
    }

    public void StartText()
    {
        if (stayOn)
        {
            if (!on)
            {
                StartCoroutine(TypeLine());
            }
            else
            {
                text.text = textString;
            }
        }
        else
        {
            StartCoroutine(TypeLine());
        }
        
    }

    IEnumerator TypeLine()
    {
        yield return new WaitForSeconds(startDelay);
        foreach (char c in textString.ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        on = true;
        yield break;
    }

    public void Submit_performed(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        text.text = textString;
    }
}

