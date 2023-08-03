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

    public GameObject PointA;
    public GameObject PointB;
    public GameObject FOV;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform targetPoint;
    private Vector2 moveDir;
    private bool movingRight;

    private void Start()
    {
        //Init components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        targetPoint = PointB.transform;

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

        //Move SHTufss
        moveDir = targetPoint.position - transform.position;
        float aimAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.5)
        {
            if(targetPoint == PointA.transform)
            {
                targetPoint = PointB.transform;
                moveDir = targetPoint.position - transform.position;
                aimAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;
                Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
                FOV.transform.rotation = q;

                movingRight = false;
            }
            else if (targetPoint == PointB.transform)
            {
                targetPoint = PointA.transform;
                moveDir = targetPoint.position - transform.position;
                aimAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;
                Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
                FOV.transform.rotation = q;

                movingRight = true;
            }
        }

        Animate();
    }

    private void FixedUpdate()
    {
        rb.velocity = MOVE_SPEED * moveDir.normalized * timeMod;
    }

    void Animate()
    {
        anim.SetBool("Right", movingRight);
    }
}
