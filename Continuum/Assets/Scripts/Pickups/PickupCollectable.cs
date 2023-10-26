using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupCollectable : MonoBehaviour
{
    public Vector2 startPos;
    
    private Vector2 targetPoint;
    private float movementSpeed = 10f;
    private float offset = 1f;
    private bool collected = false;
    private bool close = false;

    public List<Sprite> sprites = new List<Sprite>();

    [Range(0, 3)]
    public int collectableType;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    KeycardIcon[] icons;
    RectTransform iconTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        icons = GameManager.Instance.inventory.GetComponentsInChildren<KeycardIcon>();

        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        sr.sprite = sprites[collectableType];

        if (collected)
        {
            if (iconTransform)
            {
                targetPoint = iconTransform.TransformPoint(iconTransform.rect.center);
            }
            else
            {
                targetPoint = (Vector2)GameObject.Find("Player").transform.position + new Vector2(0, offset);
            }

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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!collected) 
            {
                collision.GetComponent<InventoryManager>().AddInventoryItem(collectableType, transform.position);
                SoundManager.PlaySound(SoundManager.Sound.snd_pickup);
            }
            collected = true;
            iconTransform = icons[GameManager.Instance.im.GetInventorySize() - 1].GetComponent<RectTransform>();
        }
    }
}
