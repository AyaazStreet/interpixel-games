using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeybindingHandler : MonoBehaviour
{
    private Image image;
    private string currBinding = "";
    [SerializeField] private TextMeshProUGUI testText;
    private PlayerInput input;

    public InputActionReference selectedAction;

    private List<string> inputNames = new() {
        "escape",
        "f1",
        "f2",
        "f3",
        "f4",
        "f5",
        "f6",
        "f7",
        "f8",
        "f9",
        "f10",
        "f11",
        "f12",
        "backquote",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "0",
        "minus",
        "equals",
        "unknown",
        "q",
        "w",
        "e",
        "r",
        "t",
        "y",
        "u",
        "i",
        "o",
        "p",
        "leftBracket",
        "rightBracket",
        "backslash",
        "a",
        "s",
        "d",
        "f",
        "g",
        "h",
        "j",
        "k",
        "l",
        "semicolon",
        "quote",
        "z",
        "x",
        "c",
        "v",
        "b",
        "n",
        "m",
        "comma",
        "period",
        "slash",
        "backspace",
        "enter",
        "shift",
        "tab",
        "capsLock",
        "ctrl",
        "alt",
        "space",
        "upArrow",
        "leftArrow",
        "downArrow",
        "rightArrow",
        "insert",
        "home",
        "delete",
        "end",
        "pageUp",
        "pageDown",
        "leftButton",
        "rightButton",
        "middleButton",
        "dpad",
        "dpad/up",
        "dpad/left",
        "dpad/down",
        "dpad/right",
        "leftStick",
        "rightStick",
        "leftStickPress",
        "rightStickPress",
        "leftTrigger",
        "rightTrigger",
        "leftShoulder",
        "rightShoulder",
        "controllerHome",
        "buttonWest",
        "buttonSouth",
        "buttonNorth",
        "buttonEast",
        "select",
        "start",
        "leftStick/up",
        "leftStick/left",
        "leftStick/down",
        "leftStick/right"
    };

    public List<Sprite> keyboardIconSprites = new();
    public List<Sprite> xboxIconSprites = new();
    public List<Sprite> psIconSprites = new();

    public Dictionary<string, Sprite> icons = new();

    [SerializeField] private int compoundOverride = -1;
    
    void Start()
    {
        image = GetComponent<Image>();
        input = GameManager.Instance.pc.GetComponent<PlayerInput>();
        InputAction action = selectedAction.action;

        /*
        //DUMP
        string testOutput = "";
        foreach (InputBinding binding in action.bindings)
        {
            //Debug.Log(binding.effectivePath);
            //if (!binding.isComposite)
            //{
            string[] parts = binding.effectivePath.Split('/');
            if (parts.Length == 2)
            {
                string controlScheme = parts[0];
                string key = parts[1];
                
                Debug.Log("Scheme: " + controlScheme);
                Debug.Log("Key: " + key);
                testOutput += "\"" + key + "\",\n";
            }
            else if (parts.Length == 3)
            {
                string controlScheme = parts[0];
                string key = parts[1] + parts[2];
                
                Debug.Log("Scheme: " + controlScheme);
                Debug.Log("Key: " + key);
                testOutput += "\"" + key + "\",\n";
            }
            //}
            Debug.Log("-------------");
        }
        //Debug.Log(testOutput);
        */

        for(int i = 0; i < inputNames.Count; i++)
        {
            icons.Add(inputNames[i], keyboardIconSprites[i]);
        }

        //StartCoroutine(Test());
    }

    private void Update()
    {
        InputAction action = selectedAction.action;

        int i = 0;
        foreach (InputBinding binding in action.bindings)
        {
            //if (!binding.isComposite)
            //{
            string[] parts = binding.effectivePath.Split('/');
            if (parts.Length == 2)
            {
                string controlScheme = parts[0];
                string key = parts[1];

                if (input.currentControlScheme == "Gamepad")
                {
                    if (controlScheme == "<Gamepad>")
                    {
                        if (currBinding != key)
                        {
                            if (compoundOverride > -1)
                            {
                                if(compoundOverride == i)
                                {
                                    currBinding = key;
                                    image.sprite = icons[key];
                                }
                            }
                            else
                            {
                                currBinding = key;
                                image.sprite = icons[key];
                            }
                        }
                        i++;
                    }
                }
                else if (input.currentControlScheme == "Keyboard")
                {
                    if (controlScheme == "<Keyboard>" || controlScheme == "<Mouse>")
                    {
                        if (currBinding != key)
                        {
                            if (compoundOverride > -1)
                            {
                                if (compoundOverride == i)
                                {
                                    currBinding = key;
                                    image.sprite = icons[key];
                                }
                            }
                            else
                            {
                                currBinding = key;
                                image.sprite = icons[key];
                            }
                        }
                        i++;
                    }
                }
            }
            else if (parts.Length == 3)
            {
                string controlScheme = parts[0];
                string key = parts[1] + "/" + parts[2];

                if (input.currentControlScheme == "Gamepad")
                {
                    if (controlScheme == "<Gamepad>")
                    {
                        if (currBinding != key)
                        {
                            if (compoundOverride > -1)
                            {
                                if (compoundOverride == i)
                                {
                                    currBinding = key;
                                    image.sprite = icons[key];
                                }
                            }
                            else
                            {
                                currBinding = key;
                                image.sprite = icons[key];
                            }
                        }
                        i++;
                    }
                }
                else if (input.currentControlScheme == "Keyboard")
                {
                    if (controlScheme == "<Keyboard>" || controlScheme == "<Mouse>")
                    {
                        if (currBinding != key)
                        {
                            if (compoundOverride > -1)
                            {
                                if (compoundOverride == i)
                                {
                                    currBinding = key;
                                    image.sprite = icons[key];
                                }
                            }
                            else
                            {
                                currBinding = key;
                                image.sprite = icons[key];
                            }
                        }
                        i++;
                    }
                }
            }
            //}
            
        }
    }

    private IEnumerator Test()
    {
        foreach (KeyValuePair<string, Sprite> icon in icons)
        {
            testText.text = icon.Key;
            image.sprite = icon.Value;

            //Debug.Log(icon);

            yield return new WaitForSeconds(1f);
            yield return null;
        }
        yield break;
    }
}
