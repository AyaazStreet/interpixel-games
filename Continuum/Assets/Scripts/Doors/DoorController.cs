using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public const float OPEN_TIME = 1f;
    public float timer;

    public float globalTimescale;
    public float? localTimescale;

    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField] private Sprite[] spriteArray;
    private float timeMod; 
    private bool signal1;
    private bool signal2;
    //private bool signal3;

    private void Awake()
    {
        //Initialise components
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;
    }

    private void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        //Adjust animation speed based on timeMod
        anim.speed = timeMod;
        
        //Check if door should open or close
        if (CheckSignal())
        {
            if (!anim.GetBool("IsOpen"))
            {
                anim.SetBool("IsOpen", true);
                timer = OPEN_TIME;
            }
        }
        else if (timer > 0f)
        {
            timer -= Time.deltaTime * timeMod;
            if (timer <= 0f)
            {
                anim.SetBool("IsOpen", false);
            }
        }

        //Animate
        setLight();
    }

    private bool CheckSignal()
    {
        //Check all signals active
        return signal1 && signal2 /*&& signal3*/;
    }

    private void setLight()
    {
        if (!signal1 && !signal2)
        {
            sr.sprite = spriteArray[0];
        }
        else if(signal1 && !signal2)
        {
            sr.sprite = spriteArray[1];
        }
        else if(!signal1 && signal2)
        {
            sr.sprite = spriteArray[2];
        }
        else if(signal1 && signal2)
        {
            sr.sprite = spriteArray[3];
        }
    }

    public void SignalActive(int signalNum)
    {
        switch (signalNum)
        {
            case 1: signal1 = true;
                Debug.Log("Signal 1 Active");
                break;
            case 2: signal2 = true;
                Debug.Log("Signal 2 Active");
                break;
            //case 3: signal3 = true;
        }
        
    }

    public void SignalInactive(int signalNum)
    {
        switch (signalNum)
        {
            case 1: signal1 = false;
                Debug.Log("Signal 1 Inctive");
                break;
            case 2: signal2 = false;
                Debug.Log("Signal 2 Inctive");
                break;
            //case 3: signal3 = false;
        }
    }
}
