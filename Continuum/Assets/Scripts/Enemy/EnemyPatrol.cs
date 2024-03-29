using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrol : MonoBehaviour
{
    public const float MOVE_SPEED = 3f;
    public float globalTimescale;
    public float? localTimescale;
    private float timeMod;

    public bool circular;
    public bool stationary;

    private bool dying;

    public GameObject[] pointArr;
    public int pointArrPos;
    public bool forwardTraverse;

    public GameObject FOV;

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
        float aimAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;


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
        anim.speed = 0.75f * timeMod;

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

        //Rotate FOV
        float aimAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        FOV.transform.rotation = Quaternion.Slerp(FOV.transform.rotation, q, 10 * Time.deltaTime * timeMod);

        Animate();
    }

    private void FixedUpdate()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime * timeMod;
        }
        else if (GameManager.Instance.pc.alive)
        {
            rb.velocity = MOVE_SPEED * timeMod * moveDir.normalized;
        }
        else
        {
            rb.velocity = Vector2.zero;
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

    }

    void Animate()
    {
        anim.SetFloat("AnimMoveX", moveDir.x);
        anim.SetFloat("AnimMoveY", moveDir.y);
        anim.SetFloat("AnimMoveMagnitude", rb.velocity.magnitude);
    }

    public void Die()
    {
        
        if(!dying )
        {
            StartCoroutine(Death_Laser_Enemy());
            GetComponent<CapsuleCollider2D>().enabled = false;
            dying = true;
        }
        
    }

    private IEnumerator Death_Laser_Enemy()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_burn);

        rb.velocity = Vector2.zero;

        anim.SetTrigger("Laser");

        yield return new WaitForSeconds(1.2f);

        Destroy(transform.parent.gameObject);

        yield break;
    }
}
