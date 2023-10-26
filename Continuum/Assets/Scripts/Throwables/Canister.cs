using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class Canister : MonoBehaviour
{
    public float globalTimescale;
    public float? localTimescale;

    public GameObject fieldPrefab;
    
    public Vector2 moveDir;

    private float timeMod;
    private float despawnTimer = 3f;

    private Rigidbody2D rb;
    private Animator anim;
    
    private bool isMoving = true;
    private bool isSlowing = false;
    private float currSpeed = 20f;

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
                Destroy(gameObject); 
            }
        }

        //Animation
        anim.SetBool("AnimMoving", isMoving);
        anim.speed = timeMod;
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
                currSpeed = 0f;
                isMoving = false;
                Break(null);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Bolt"))
        {
            Break(collision.transform);
        }
    }

    public void Break(Transform target)
    {
        GameObject field = Instantiate(fieldPrefab, gameObject.transform.position, gameObject.transform.rotation);

        if (target != null)
        {
            field.transform.parent = target.transform;
        }

        SoundManager.PlaySound(SoundManager.Sound.snd_canSmash);

        field.GetComponent<Field>().stickTarget = target;
        Destroy(gameObject);
    }
}
