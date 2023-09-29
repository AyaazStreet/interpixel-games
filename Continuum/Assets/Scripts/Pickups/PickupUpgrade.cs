using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupUpgrade : MonoBehaviour
{
    private Vector2 targetPoint;
    private float movementSpeed = 10f;
    private float offset = 1f;
    private bool collected = false;
    private bool close;

    public int pickupNumber;

    [Range(1, 3)]
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
            if (distance < 0.5f)
            {
                close = true;
            }

            if (close)
            {
                Color tmp = sr.color;
                if (tmp.a > 0)
                {
                    tmp.a -= 2f * Time.deltaTime;
                    sr.color = tmp;
                }

                if (tmp.a <= 0)
                {
                    gameObject.SetActive(false);
                }
            }

            // Move the object towards the target point using interpolation
            Vector2 newPosition = Vector2.Lerp(rb.position, targetPoint, Time.fixedDeltaTime * movementSpeed);
            rb.MovePosition(newPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !collected)
        {
            collected = true;
            GameManager.Instance.em.collected.Add(new EquipManager.Collectable(unlockNum, pickupNumber, transform.position));
            Collect();
        }
    }

    public void Collect()
    {
        switch (unlockNum)
        {
            case 1:
                GameManager.Instance.pc.T1_Unlocked = true;
                GameManager.Instance.em.selected = 1;
                GameManager.Instance.em.E1_count += pickupNumber;
                break;
            case 2:
                GameManager.Instance.pc.T2_Unlocked = true;
                GameManager.Instance.em.selected = 2;
                GameManager.Instance.em.E2_count += pickupNumber;
                break;
            case 3:
                GameManager.Instance.pc.T3_Unlocked = true;
                GameManager.Instance.em.selected = 3;
                GameManager.Instance.em.E3_count += pickupNumber;
                break;
            default:
                break;
        }
    }
}
