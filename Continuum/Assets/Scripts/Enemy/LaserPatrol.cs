using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPatrol : MonoBehaviour
{
    public float MOVE_SPEED = 3f;
    private float globalTimescale;
    private float? localTimescale;
    private float timeMod;

    public bool circular;
    public bool stationary;

    public GameObject[] pointArr;
    public int pointArrPos;
    public bool forwardTraverse;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform targetPoint;
    private Vector2 moveDir;

    public float waitTimeTotal = 2f;
    public float waitTime = 0;

    private void Start()
    {
        //Init components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        pointArrPos = 0;
        targetPoint = pointArr[0].transform;

        //Init move direction and aim angle based on target point
        moveDir = targetPoint.position - transform.position;

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
        anim.speed = timeMod;

        //Adjust move direction and aim angle based on target point
        moveDir = targetPoint.position - transform.position;

        //Check if point reached
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.05)
        {
            if (!stationary)
            {
                if (circular)
                {
                    circularPointSwitch();
                }
                else
                {
                    linearPointSwitch();
                }
                waitTime = waitTimeTotal;
                rb.velocity = Vector2.zero;
            }
            else
            {
                waitTime = 1f;
                if(pointArr.Length > 1) 
                {
                    moveDir = pointArr[pointArrPos+1].transform.position - transform.position;

                    rb.velocity = Vector2.zero;
                }
                else
                {
                    Debug.LogError("Insufficient points for stationary enemy. Required: 2");
                }
            }
        }

        Animate();
    }

    private void FixedUpdate()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime * timeMod;
        }
        else //if (GameManager.Instance.pc.alive)
        {
            rb.velocity = MOVE_SPEED * timeMod * moveDir.normalized;
        }
        /*else
        {
            rb.velocity = Vector2.zero;
        }*/
    }

    private void linearPointSwitch()
    {
        if (forwardTraverse) //Going forward through point array
        {
            if (pointArrPos < pointArr.Length - 1) //Traverse
            {
                pointArrPos++;
            }
            else //Switch direction
            {
                pointArrPos--;
                forwardTraverse = false;
            }
        }
        else //Going back through point array
        {
            if (pointArrPos > 0) //Traverse
            {
                pointArrPos--;
            }
            else //Switch direction
            {
                pointArrPos++;
                forwardTraverse = true;
            }
        }

        //Adjust target point
        targetPoint = pointArr[pointArrPos].transform;

        //Adjust move direction and aim angle based on target point
        moveDir = targetPoint.position - transform.position;

    }

    private void circularPointSwitch()
    {
        if (pointArrPos < pointArr.Length - 1) //Traverse
        {
            pointArrPos++;
        }
        else //Reset
        {
            pointArrPos = 0;
        }

        //Adjust target point
        targetPoint = pointArr[pointArrPos].transform;

        //Adjust move direction and aim angle based on target point
        moveDir = targetPoint.position - transform.position;

    }

    void Animate()
    {
        //anim.SetFloat("AnimMoveX", moveDir.x);
        //anim.SetFloat("AnimMoveY", moveDir.y);
        //anim.SetFloat("AnimMoveMagnitude", rb.velocity.magnitude);
    }
}
