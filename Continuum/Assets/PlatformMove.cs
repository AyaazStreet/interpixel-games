using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class PlatformMove : MonoBehaviour
{
    public const float MOVE_SPEED = 1f; 
    public float globalTimescale;
    public float? localTimescale;
    private float timeMod;

    public bool circular;

    public GameObject[] pointArr;
    public int pointArrPos;
    public bool forwardTraverse;

    private Rigidbody2D rb;
    private Transform targetPoint;
    private Vector2 moveDir;
    private Vector2 lastMoveDir;

    public float waitTimeTotal = 2f;
    public float waitTime = 0;

    private void Start()
    {
        //Init components
        rb = GetComponent<Rigidbody2D>();

        pointArrPos = 0;
        targetPoint = pointArr[0].transform;

        //Init move direction based on target point
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

        //Adjust move direction based on target point
        moveDir = targetPoint.position - transform.position;

        //Check if point reached
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.05)
        {
            if (circular)
            {
                CircularPointSwitch();
            }
            else
            {
                LinearPointSwitch();
            }

            waitTime = waitTimeTotal;
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime * timeMod;
        }
        else
        {
            rb.velocity = MOVE_SPEED * timeMod * moveDir.normalized;
        }
    }
    private void LinearPointSwitch()
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

        //Adjust move direction based on target point
        moveDir = targetPoint.position - transform.position;
    }

    private void CircularPointSwitch()
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

        //Adjust move direction based on target point
        moveDir = targetPoint.position - transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.GetComponent<PlayerController>().externalVelocity = rb.velocity;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.GetComponent<PlayerController>().externalVelocity = Vector2.zero;
        }
    }
}
