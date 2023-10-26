using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEffect : MonoBehaviour
{
    private float globalTimescale;
    private float? localTimescale;
    private float timeMod;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        
        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;
    }

    void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        anim.speed = timeMod;
    }
}
