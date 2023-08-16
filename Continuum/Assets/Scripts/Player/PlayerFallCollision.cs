using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class PlayerFallCollision : MonoBehaviour
{
    public GameObject player;
    
    private TilemapCollider2D pitCol;
    public CapsuleCollider2D playerCol;

    private float fallFactor = 1.2f;
    private float speed = 3f;

    [SerializeField] private bool falling = false;
    [SerializeField] private bool shouldFall = false;
    [SerializeField] private bool stable = false;
    private Vector3 fallPoint;

    public int contactPoints = 2;

    private void Start()
    {
        pitCol = GameObject.Find("Tilemap_Pits").GetComponent<TilemapCollider2D>();
    }

    private void Update()
    {
       if(shouldFall && !stable)
       {
           player.GetComponent<PlayerController>().falling = true;
           falling = true;
       }
    }

    private void FixedUpdate()
    {
        if(falling)
        {
            Vector2 moveDir = fallPoint - transform.position;
            player.GetComponent<Rigidbody2D>().velocity = moveDir * speed;

            Color tmp = player.GetComponent<SpriteRenderer>().color;

            tmp.r -= 1f * Time.deltaTime;
            tmp.g -= 1f * Time.deltaTime;
            tmp.b -= 1f * Time.deltaTime;
            tmp.a -= 1f * Time.deltaTime;

            player.GetComponent<SpriteRenderer>().color = tmp;

            if (player.transform.localScale.x > 0.01f)
            {
                player.transform.localScale -= new Vector3(fallFactor * Time.deltaTime, fallFactor * Time.deltaTime, 0);
            }
            else
            {
                GameManager.Instance.ShowDeathScreen();

                falling = false;
                shouldFall = false;
                stable = false;
                player.GetComponent<PlayerController>().falling = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            stable = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Pits"))
        {
            shouldFall = true;
            fallPoint = (Vector2)transform.position + player.GetComponent<Rigidbody2D>().velocity.normalized * 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            stable = false;
            fallPoint = (Vector2)transform.position + player.GetComponent<Rigidbody2D>().velocity.normalized * 0.5f;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Pits"))
        {
            shouldFall = false;
        }
    }
}
