using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class DoorController : MonoBehaviour
{
    public float OPEN_TIME = 0.5f;
    public float timer;

    public float globalTimescale;
    public float? localTimescale;
    private float timeMod;

    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField] private Sprite[] spriteArray;

    [SerializeField] private GameObject signalPrefab;

    [SerializeField] private Controller[] control1;
    [SerializeField] private Controller[] control2;
    [SerializeField] private Controller[] control3;

    [SerializeField] private bool signal1 = false;
    private bool signal2 = false;
    private bool signal3 = false;


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

        //Update signals
        UpdateSignals();

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
                        SoundManager.PlaySound(SoundManager.Sound.snd_doorOpen, transform.position);
                    }
                    timer = OPEN_TIME;
                }
                else if (timer > 0f)
                {
                    timer -= Time.deltaTime * timeMod;
                    if (timer <= 0f)
                    {
                        anim.SetBool("IsOpen", false);
                        SoundManager.PlaySound(SoundManager.Sound.snd_doorOpen, transform.position);
                    }
                }
            }
            else if (CheckSignal())
            {
                if (!anim.GetBool("IsOpen"))
                {
                    anim.SetBool("IsOpen", true);
                    SoundManager.PlaySound(SoundManager.Sound.snd_doorOpen, transform.position);
                }
                timer = OPEN_TIME;
            }
            else if (timer > 0f)
            {
                timer -= Time.deltaTime * timeMod;
                if (timer <= 0f)
                {
                    anim.SetBool("IsOpen", false);
                    SoundManager.PlaySound(SoundManager.Sound.snd_doorOpen, transform.position);
                }
            }

            //Animate
            SetLight();
        }
    }

    private void UpdateSignals()
    {
        //Update signals from signal array

        signal1 = false; 
        signal2 = false; 
        signal3 = false;

        foreach(Controller c in control1)
        {
            if (c != null)
            {
                if (c.active) signal1 = true;

                if (c.sendSignalSprite)
                {
                    StartCoroutine(SignalOff(c));

                    /*Color color;
                    if (c.active) color = new Color32(0x56, 0xff, 0x28, 0xff);
                    else color = new Color32(0xff, 0x2d, 0x2d, 0xff);*/


                    if (c.active) StartCoroutine(SendSignalSprite(c.transform.position, transform.position));
                }
            }
        }

        foreach (Controller c in control2)
        {
            if (c != null)
            {
                if (c.active) signal2 = true;

                if (c.sendSignalSprite)
                {
                    StartCoroutine(SignalOff(c));

                    /*Color color;
                    if (c.active) color = new Color32(0x56, 0xff, 0x28, 0xff);
                    else color = new Color32(0xff, 0x2d, 0x2d, 0xff);*/

                    if (c.active) StartCoroutine(SendSignalSprite(c.transform.position, transform.position));
                }
            }
        }

        foreach (Controller c in control3)
        {
            if (c != null)
            {
                if (c.active) signal3 = true;

                if (c.sendSignalSprite)
                {
                    StartCoroutine(SignalOff(c));

                    /*Color color;
                    if (c.active)  = new Color32(0x56, 0xff, 0x28, 0xff);
                    else color = new Color32(0xff, 0x2d, 0x2d, 0xff);*/

                    if (c.active) StartCoroutine(SendSignalSprite(c.transform.position, transform.position));
                }
            }
        }
    }

    private IEnumerator SignalOff(Controller c)
    {
        yield return new WaitForEndOfFrame();
        c.sendSignalSprite = false;
        yield break;
    }

    private IEnumerator SendSignalSprite(Vector2 start, Vector2 target)
    {
        Vector2 aimDir = target - start;
        float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;

        //Calculate quaternion and apply to rotation
        Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);

        GameObject signal = Instantiate(signalPrefab, new Vector3(start.x, start.y, -2), q);
        signal.GetComponent<SpriteRenderer>().size = new Vector2(0.5f, ((target - start).magnitude) / signal.transform.localScale.x);

        yield return new WaitForSeconds(0.5f);

        Destroy(signal);

        yield break;
    }

    private bool CheckSignal()
    {
        //Check all signals active
        switch (numSignals)
        {
            case 1:
            {
                return signal1;
            }
            case 2:
            {
                return signal1 && signal2;
            }
            case 3:
            {
                return signal1 && signal2 && signal3;
            }
            default: return false;
        }
    }

    private void SetLight()
    {
        /*if (!signal1.active && !signal2.active)
        {
            sr.sprite = spriteArray[0];
        }
        else if(signal1.active && !signal2.active)
        {
            sr.sprite = spriteArray[1];
        }
        else if(!signal1.active && signal2.active)
        {
            sr.sprite = spriteArray[2];
        }
        else if(signal1.active && signal2.active)
        {
            sr.sprite = spriteArray[3];
        }*/

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

    /*public void SignalActive(int signalNum)
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
        
    }*/

    /*public void SignalInactive(int signalNum)
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
    }*/
}
