using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class EnemyPatrol : MonoBehaviour
{
    public const float MOVE_SPEED = 2f;

    public float globalTimescale;
    public float? localTimescale;
    private float timeMod;

    public bool circular;

    public GameObject[] pointArr;
    public int pointArrPos;
    public bool forwardTraverse;

    public GameObject FOV;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform targetPoint;
    private Vector2 moveDir;
    private Vector2 lastMoveDir;

    private void Start()
    {
        //Init components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        pointArrPos = 0;
        targetPoint = pointArr[0].transform;

        //Init move direction and aim angle based on target point
        moveDir = targetPoint.position - transform.position;
        float aimAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;

        //Rotate FOV
        Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        FOV.transform.rotation = q;

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
        float aimAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;

        //Rotate FOV
        Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        FOV.transform.rotation = q;

        //Check if point reached
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.05)
        {
            if (circular)
            {
                circularPointSwitch();
            }
            else
            {
                linearPointSwitch();
            }
        }

        Animate();
    }

    private void FixedUpdate()
    {
        rb.velocity = MOVE_SPEED * timeMod * moveDir.normalized;

        if (moveDir.x != 0 || moveDir.y != 0)
        {
            //Not idle
            lastMoveDir = moveDir;
        }
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
        float aimAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;

        //Rotate FOV
        Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        FOV.transform.rotation = q;
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
        float aimAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;

        //Rotate FOV
        Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        FOV.transform.rotation = q;
    }

    void Animate()
    {
        anim.SetFloat("AnimMoveX", moveDir.x);
        anim.SetFloat("AnimMoveY", moveDir.y);
        anim.SetFloat("AnimMoveMagnitude", moveDir.magnitude);
        anim.SetFloat("AnimLastMoveX", lastMoveDir.x);
        anim.SetFloat("AnimLastMoveY", lastMoveDir.y);
    }
}
