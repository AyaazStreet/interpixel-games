using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class AnimationDelay : MonoBehaviour
{
    public float animSpeed = 1f;
    public float interval = 3.0f;
    private float timer = 0.0f;
    private Animator animator;

    public float globalTimescale;
    public float? localTimescale;
    private float timeMod;

    private void Start()
    {
        animator = GetComponent<Animator>();

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
        animator.speed = animSpeed * timeMod;

        timer += Time.deltaTime * timeMod;
        if (timer >= interval)
        {
            animator.SetTrigger("i");
            timer = 0f;
            SoundManager.PlaySound(SoundManager.Sound.snd_drip, transform.position);
        }
    }
}
