using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHandler : MonoBehaviour
{
    bool hit = false;

    public Sprite broke;

    Animator anim;
    SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        anim.speed = 0;
    }

    public void Hit()
    {
        if (!hit)
        {
            hit = true;

            sr.sprite = broke;

            anim.SetTrigger("Break");
        }
        else
        {
            StartCoroutine(Destruct());
        }
    }

    IEnumerator Destruct()
    {
        anim.speed = 1;
        //SOUND

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);

        yield break;
    }
}
