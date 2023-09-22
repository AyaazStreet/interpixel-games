using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public bool active = false;
    public bool on = false;

    public float active_time = 2f;
    public bool interactable = false;
    public float timer;

    public const float OUTLINE_THICKNESS = 0.75f;

    public float globalTimescale;
    public float? localTimescale;
    public float timeMod;

    public InputActionReference interact;

    public SpriteRenderer sr;

    private void Awake()
    {
        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;
    }

    public void ChangeState(){}
}
