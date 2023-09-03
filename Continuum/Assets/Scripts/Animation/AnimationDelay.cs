using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelay : MonoBehaviour
{
    public float interval = 3.0f;
    private float timer = 0.0f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            animator.SetTrigger("i");
            timer = 0f;
        }
    }
}
