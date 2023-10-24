using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHandler : MonoBehaviour
{
    bool hit = false;

    Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Hit()
    {
        if (!hit)
        {
            hit = true;

            //CHANGE SPRITE
        }
        else
        {
            StartCoroutine(Destruct());
        }
    }

    IEnumerator Destruct()
    {
        //anim.setTrigger("Break");
        //SOUND

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);

        yield break;
    }
}
