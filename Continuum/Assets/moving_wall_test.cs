using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_wall_test : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    public float timer;

    public float globalTimescale;
    public float? localTimescale;

    private float timeMod;

    private Rigidbody2D rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 2.5f;
        dir = new(1, 0);
        timer = 3f;

        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        //move stuff
        if (timer > 0f)
        {
            timer -= Time.deltaTime * timeMod;
            if (timer <= 0f)
            {
                if(rb.velocity == dir * speed * timeMod)
                {
                    dir *= -1;
                    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                }
                else
                {
                    dir *= -1;
                }

                timer = 3f;  
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = speed * timeMod * dir;
    }
}
