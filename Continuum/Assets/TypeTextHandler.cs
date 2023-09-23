using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TypeTextHandler : MonoBehaviour
{
    public TextMeshProUGUI text;
    private string textString;
    private float textSpeed = 0.03f;
    public float startDelay;
    public bool stayOn = false;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("awake");
        textString = text.text;
        text.text = string.Empty;
    }

    private void OnEnable()
    {
        Debug.Log("onEnable");
        text.text = string.Empty;
        StartText();
    }

    private void OnDisable()
    {
        Debug.Log("onDisable");
        StopAllCoroutines();
    }

    public void StartText()
    {
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        Debug.Log("typing");
        yield return new WaitForSeconds(startDelay);
        foreach (char c in textString.ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        yield break;
    }
}

