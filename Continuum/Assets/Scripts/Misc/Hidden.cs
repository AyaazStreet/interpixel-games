using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidden : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_secret, transform.position);
        Destroy(gameObject);
    }
}
