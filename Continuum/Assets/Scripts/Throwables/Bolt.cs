using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public float globalTimescale;
    public float? localTimescale;

    public Vector2 moveDir;

    private float timeMod;
    private float despawnTimer = 3f;

    private Rigidbody2D rb;
    private Animator anim;
    private PolygonCollider2D col;
    private bool isMoving = true;
    private bool isSlowing = false;
    private float currSpeed = 20f;

    public PickupUpgrade pickupScript;

    private void Awake()
    {
        moveDir = transform.up;
        
        //Initialise timescales
        localTimescale = null;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        //Initialise components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<PolygonCollider2D>();
        //pickupScript = GetComponent<PickupUpgrade>();

        pickupScript.enabled = false;
    }

    private void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        //Despawn timer
        if (despawnTimer > 0f)
        {
            despawnTimer -= Time.deltaTime * timeMod; //reduce despawn timer

            //Stop motion
            if (despawnTimer <= 2.8f) 
            {
                isSlowing = true;
            }

            //despawn object
            if (despawnTimer <= 0f)
            {
                //Destroy(gameObject); 
            }
        }

        //Animation
        anim.SetBool("AnimMoving", isMoving);
        if (isMoving)
        {
            anim.speed = timeMod;
        }
    }

    private void FixedUpdate()
    {
        if (isMoving) 
        {
            //Set velocity
            rb.velocity = currSpeed * timeMod * moveDir;

            //Reduce speed after duration
            if (isSlowing)
            {
                currSpeed -= 1f * timeMod;
            }

            //Check for stopped
            if(currSpeed <= 0f)
            {
                Debug.Log("enable");
                
                currSpeed = 0f;
                isMoving = false;
                pickupScript.enabled = true;
                col.enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Bolt") && enabled)
        {
            Debug.Log("enable2");

            isMoving = false;
            anim.speed = 0;
            pickupScript.enabled = true;
            col.enabled = false;
        }
    }
}
