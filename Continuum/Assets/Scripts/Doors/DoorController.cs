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
    [SerializeField] private bool signal1;
    [SerializeField] private bool signal2;
    [SerializeField] private bool signal3;
    [Range(0, 3)]
    public int numSignals;

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
        if(timeMod > 0f)
        {
            if (numSignals == 0)
            {
                if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) < 2)
                {
                    if (!anim.GetBool("IsOpen"))
                    {
                        anim.SetBool("IsOpen", true);
                    }
                    timer = OPEN_TIME;
                }
                else if (timer > 0f)
                {
                    timer -= Time.deltaTime * timeMod;
                    if (timer <= 0f)
                    {
                        anim.SetBool("IsOpen", false);
                    }
                }
            }
            else if (CheckSignal())
            {
                if (!anim.GetBool("IsOpen"))
                {
                    anim.SetBool("IsOpen", true);
                }
                timer = OPEN_TIME;
            }
            else if (timer > 0f)
            {
                timer -= Time.deltaTime * timeMod;
                if (timer <= 0f)
                {
                    anim.SetBool("IsOpen", false);
                }
            }
        }
        
        //Animate
        setLight();
    }

    private bool CheckSignal()
    {
        //Check all signals active
        switch (numSignals)
        {
            case 1: return signal1;
            case 2: return signal1 && signal2;
            case 3: return signal1 && signal2 && signal3;
            default: return false;
        }
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

        switch (numSignals)
        {
            case 1: 
            {
                if (!signal1)
                {
                    sr.sprite = spriteArray[0];
                }
                else if (signal1)
                {
                    sr.sprite = spriteArray[1];
                }
            } 
            break;
            case 2: 
            {
                if (!signal1 && !signal2)
                {
                    sr.sprite = spriteArray[2];
                }
                else if (signal1 && !signal2)
                {
                    sr.sprite = spriteArray[3];
                }
                else if (!signal1 && signal2)
                {
                    sr.sprite = spriteArray[4];
                }
                else if (signal1 && signal2)
                {
                    sr.sprite = spriteArray[5];
                }
            } 
            break;
            case 3: 
            {
                if (!signal1 && !signal2 && !signal3)
                {
                    sr.sprite = spriteArray[6];
                }
                else if (signal1 && !signal2 && !signal3)
                {
                    sr.sprite = spriteArray[7];
                }
                else if (!signal1 && signal2 && !signal3)
                {
                    sr.sprite = spriteArray[8];
                }
                else if (!signal1 && !signal2 && signal3)
                {
                    sr.sprite = spriteArray[9];
                }
                else if (signal1 && signal2 && !signal3)
                {
                    sr.sprite = spriteArray[10];
                }
                else if (signal1 && !signal2 && signal3)
                {
                    sr.sprite = spriteArray[11];
                }
                else if (!signal1 && signal2 && signal3)
                {
                    sr.sprite = spriteArray[12];
                }
                else if (signal1 && signal2 && signal3)
                {
                    sr.sprite = spriteArray[13];
                }
            } 
            break;
            default: 
            {
                sr.sprite = spriteArray[14];
            } 
            break;
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
            case 3: signal3 = true;
                Debug.Log("Signal 3 Active");
                break;
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
            case 3:
                signal3 = false;
                Debug.Log("Signal 3 Inctive");
                break;
        }
    }
}
