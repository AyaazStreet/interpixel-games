using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupCollectable : MonoBehaviour
{
    private Vector2 targetPoint;
    private float movementSpeed = 10f;
    private float offset = 1f;
    private bool collected = false;
    private bool close = false;

    [Range(0, 2)]
    public int collectableType;

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
                    Destroy(gameObject);
                }
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
            if (!collected) 
            {
                collision.GetComponent<InventoryManager>().AddInventoryItem(collectableType, transform.position);
            }
            collected = true;
        }
    }
}
