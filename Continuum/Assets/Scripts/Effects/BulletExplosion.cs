using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    private float globalTimescale;
    private float? localTimescale;
    private float timeMod;

    private Animator anim;

    void Start()
    {
        //Initialise timescales
        localTimescale = null;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        //Init
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        anim.speed = timeMod;
    }

    public void AnimationEndEvent()
    {
        Destroy(gameObject);
    }
}
