using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonOpening : MonoBehaviour
{
    private Animator anim;

    public bool isOpen = false;

    public float openDelay = 0f;
    public float openTimeTotal = 2f;
    public float openTime = 0f;

    public float globalTimescale;
    public float? localTimescale;
    public float timeMod;

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

        if (openTime > 0f)
        {
            openTime -= Time.deltaTime * timeMod;
        }
        else
        {
            isOpen = false;
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
        isOpen = true;
        openTime = openTimeTotal;
        yield break;
    }
}
