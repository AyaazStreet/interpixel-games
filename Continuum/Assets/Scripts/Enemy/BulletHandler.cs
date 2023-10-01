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
        localTimescale = null;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;
    }

    void FixedUpdate()
    {
        rb.velocity = speed * timeMod * transform.up;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (timeMod != 0f)
            {
                GameManager.Instance.pc.Die();
                SoundManager.PlaySound(SoundManager.Sound.snd_hit, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
