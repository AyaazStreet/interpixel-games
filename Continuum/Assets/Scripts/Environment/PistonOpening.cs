using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonOpening : MonoBehaviour
{
    private Animator anim;

    public  bool isOpen = false;
    private bool waiting = true;

    public float openDelay = 0f;
    public float openTimeTotal = 2f;
    public float openTime = 0f;

    public float globalTimescale;
    public float? localTimescale;
    public float timeMod;

    private DeathTrigger dt;

    void Start()
    {
        anim = GetComponent<Animator>();

        dt = GetComponentInChildren<DeathTrigger>();

        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        Open();
    }

    void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        if (!waiting) 
        {
            if (isOpen)
            {
                if (openTime > 0f)
                {
                    openTime -= Time.deltaTime * timeMod;
                }
                else
                {
                    isOpen = false;
                    dt.isOpen = false;
                    openTime = openTimeTotal;

                    SoundManager.PlaySound(SoundManager.Sound.snd_shot, transform.position);
                }
            }
            else
            {
                if (openTime > 0f)
                {
                    openTime -= Time.deltaTime * timeMod;
                }
                else
                {
                    isOpen = true;
                    dt.isOpen = true;
                    openTime = openTimeTotal;

                    SoundManager.PlaySound(SoundManager.Sound.snd_shot, transform.position);
                }
            }
        }

        anim.SetBool("Open", isOpen);
        anim.speed = timeMod;
    }

    public void Open()
    {
        StartCoroutine(WaitDelay());
    }

    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(openDelay);
        waiting = false;
        yield break;
    }
}
