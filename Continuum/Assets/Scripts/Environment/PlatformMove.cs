using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class PlatformMove : MonoBehaviour
{
    public float globalTimescale;
    public float? localTimescale;
    private float timeMod;

    public bool circular;
    public bool contact = false;

    public GameObject[] pointArr;
    public int pointArrPos;
    public bool forwardTraverse;

    private PlayerController pc;
    public Rigidbody2D rb;
    private Transform targetPoint;
    private Vector2 moveDir;
    private Vector2 lastMoveDir;

    public float moveSpeed = 1f;
    public float waitTimeTotal = 2f;
    public float waitTime = 0;

    private void Start()
    {
        //Init components
        rb = GetComponent<Rigidbody2D>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();

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
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.01)
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
            waitTime -= Time.fixedDeltaTime * timeMod;
        }
        else
        {
            if ((Time.fixedDeltaTime * rb.velocity).magnitude < Vector2.Distance(transform.position, targetPoint.position))
            {
                rb.velocity = moveSpeed * timeMod * moveDir.normalized;
            }
            else
            {
                rb.velocity = (moveDir.normalized * Vector2.Distance(transform.position, targetPoint.position)) / Time.fixedDeltaTime;
            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerFeet"))
        {
            contact = true;
            pc.externalVelocityObjs.Add(rb);
            //pc.UpdateVelocity();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerFeet"))
        {
            pc.UpdateVelocity();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerFeet"))
        {
            contact = false;
            pc.externalVelocityObjs.Remove(rb);
        }
    }
}
