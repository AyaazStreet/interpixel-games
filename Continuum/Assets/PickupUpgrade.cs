using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupUpgrade : MonoBehaviour
{
    private Vector2 targetPoint;
    private float movementSpeed = 10f;
    private float offset = 1f;
    private bool collected = false;

    [Range(1, 4)]
    public int unlockNum;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (collected)
        {
            targetPoint = (Vector2)GameObject.Find("Player").transform.position + new Vector2(0, offset);

            // Check if the target point is set
            if (targetPoint == null)
            {
                Debug.LogWarning("Target point is not set!");
                return;
            }

            // Calculate the direction towards the target point
            Vector2 direction = (targetPoint - rb.position).normalized;

            // Calculate the distance to the target point
            float distance = Vector2.Distance(rb.position, targetPoint);

            // Check if the object has reached the target point
            if (distance < 0.05f)
            {
                rb.velocity = Vector2.zero;

                Color tmp = sr.color;
                if (tmp.a > 0)
                {
                    tmp.a -= 10f * Time.deltaTime;
                    sr.color = tmp;
                }

                if (tmp.a <= 0)
                {
                    Destroy(gameObject);
                }

                return;
            }

            // Move the object towards the target point using interpolation
            Vector2 newPosition = Vector2.Lerp(rb.position, targetPoint, Time.fixedDeltaTime * movementSpeed);
            rb.MovePosition(newPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collected = true;

            switch (unlockNum)
            {
                case 1:
                    collision.GetComponent<PlayerController>().T1_Unlocked = true;
                    collision.GetComponent<EquipManager>().selected = 1;
                    break;
                case 2:
                    collision.GetComponent<PlayerController>().T2_Unlocked = true;
                    collision.GetComponent<EquipManager>().selected = 2;
                    break;
                case 3:
                    collision.GetComponent<PlayerController>().T3_Unlocked = true;
                    collision.GetComponent<EquipManager>().selected = 3;
                    break; 
                default:
                    break;
            }
        }
    }
}
