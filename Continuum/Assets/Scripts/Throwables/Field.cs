using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private int ability;
    private float timeFactor;
    private float despawnTimer = 3f;

    private void Awake()
    {
        //change effect based on infused ability 
        switch (ability)
        {
            case 0:
                timeFactor = 1f;
                break;
            case 1: timeFactor = TimeScaleManager.A1_EFFECT;
                break;
            case 2: timeFactor = TimeScaleManager.A2_EFFECT;
                break;
            case 3: timeFactor = TimeScaleManager.A3_EFFECT;
                break;
        }
    }

    private void Update()
    {
        //despawn timer
        if (despawnTimer > 0)
        {
            despawnTimer -= Time.deltaTime;

            //despawn object
            if (despawnTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LocalModifier localMod = collision.gameObject.GetComponentInParent<LocalModifier>();

        //if a time adjustable object enters, adjust it's local timescale
        if (collision != null && localMod != null)
        {
            collision.gameObject.GetComponent<LocalModifier>().value = timeFactor;
            Debug.Log("object affected by timescale");
        }

        //if another field intersects the current field, destroy the current field
        switch (ability)
        {
            case 1:
            {
                if (collision.gameObject.CompareTag("Acc Field") || collision.gameObject.CompareTag("Stop Field"))
                {
                    Destroy(gameObject);
                }
            }
            break;
            case 2:
            {
                if (collision.gameObject.CompareTag("Slow Field") || collision.gameObject.CompareTag("Stop Field"))
                {
                    Destroy(gameObject);
                }
                break;
            }
            case 3:
            {
                if (collision.gameObject.CompareTag("Slow Field") || collision.gameObject.CompareTag("Acc Field"))
                {
                    Destroy(gameObject);
                }
                break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if a time adjustable object exits, reset it's local timescale
        if (collision != null && collision.gameObject.GetComponent<LocalModifier>() != null)
        {
            collision.gameObject.GetComponent<LocalModifier>().value = null;
            Debug.Log("object no longer affected by timescale");
        }
    }
}
