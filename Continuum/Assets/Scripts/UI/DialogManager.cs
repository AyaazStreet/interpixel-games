using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI continueText;
    public string[] lines;
    public float textSpeed;

    public PlayerInput input;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        text.text = string.Empty;
        continueText.text = string.Empty;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump"))
        {
            if (text.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines(); 
                text.text = lines[index];
            }
        }
    }

    public void StartDialogue(string[] l)
    {
        if(l.Length > 0)
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            text.text = string.Empty;

            /*if (input.currentControlScheme == "Gamepad")
            {
                continueText.text = "press A to continue...";
            }
            else if (input.currentControlScheme == "Keyboard")
            {*/
                continueText.text = "click anywhere to continue...";
            //}

            lines = l;
            index = 0;
            StartCoroutine(TypeLine());
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
