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
    [SerializeField] private int stable = 0;
    private Vector3 fallPoint;

    private void Start()
    {
        pitCol = GameObject.Find("Tilemap_Pits").GetComponent<TilemapCollider2D>();
    }

    private void Update()
    {
       if(shouldFall && stable <= 0 && !falling && !GameManager.Instance.pc.invincible)
       {
           player.GetComponent<PlayerController>().falling = true;
           falling = true;

           SoundManager.PlaySound(SoundManager.Sound.snd_fall);
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
                if (player.GetComponent<PlayerController>().alive)
                {
                    player.GetComponent<PlayerController>().alive = false;
                    StartCoroutine(delayedDeath(1f));
                }
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            //stable = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            stable++;
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
            stable--;
            fallPoint = (Vector2)transform.position + player.GetComponent<Rigidbody2D>().velocity.normalized * 0.5f;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Pits"))
        {
            shouldFall = false;
        }
    }

    private IEnumerator delayedDeath(float delay)
    {
        yield return new WaitForSeconds(delay);

        SoundManager.PlaySound(SoundManager.Sound.snd_splat);

        GameManager.Instance.pc.Die();

        falling = false;
        shouldFall = false;
        stable = 0;
        player.GetComponent<PlayerController>().falling = false;

        yield break;
    }
}
