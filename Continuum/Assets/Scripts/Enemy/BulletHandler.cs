using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    private float globalTimescale;
    private float? localTimescale;
    private float timeMod;

    public float speed;
    Rigidbody2D rb;

    void Start()
    {
        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;
    }

    void FixedUpdate()
    {
        rb.velocity = speed * timeMod * transform.up;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
            Destroy(transform.parent.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
            Destroy(transform.parent.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (timeMod != 0f)
            {
                GameManager.Instance.deathText.text = "You've been shot";
                GameManager.Instance.pc.Die("bullet");
                SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
                Destroy(transform.parent.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Barrier"))
        {
            //collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);

            SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
            Destroy(transform.parent.gameObject);
        }
    }
    
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (timeMod != 0f)
            {
                GameManager.Instance.pc.Die();
                SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Barrier"))
        {
            //collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);

            SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
            Destroy(gameObject);
        }
    }*/
}
