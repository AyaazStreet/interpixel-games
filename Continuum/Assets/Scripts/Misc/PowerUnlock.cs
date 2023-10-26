using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUnlock : MonoBehaviour
{
    public bool on = true;
    public bool started = false;

    private bool interactable = false;

    public const float OUTLINE_THICKNESS = 0.75f;

    public float timerMax = 6f;
    public float timer = 0f;

    [Range(1, 3)]
    public int power;

    private SpriteRenderer sr;

    GameObject player;
    Animator animator;
    public InputActionReference interact;

    private void OnEnable()
    {
        interact.action.performed += Interact_performed;
    }

    private void OnDisable()
    {
        interact.action.performed -= Interact_performed;
    }

    private void Start()
    {
        player = GameManager.Instance.player;
        animator = GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && on)
        {
            interactable = true;
            sr.material.SetFloat("_Outline_Thickness", OUTLINE_THICKNESS);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactable = false;
            sr.material.SetFloat("_Outline_Thickness", 0f);
        }
    }

    private void Update()
    {
        if(started)
        {
            if (timer < timerMax)
            {
                timer += Time.deltaTime;
            }
            else
            {
                started = false;
                
                player.GetComponent<SpriteRenderer>().enabled = true;
                player.GetComponent<PlayerController>().enabled = true;
                player.GetComponent<PlayerController>().hasControl = true;
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                player.GetComponent<Rigidbody2D>().rotation = 0f;
                player.transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f, player.transform.position.z);

                switch (power)
                {
                    case 1:
                    {
                        player.GetComponent<PlayerController>().A1_Unlocked = true;
                        CheckpointManager.Instance.SavePlayerData();
                    }
                    break;
                    case 2:
                    {
                        player.GetComponent<PlayerController>().A2_Unlocked = true;
                        CheckpointManager.Instance.SavePlayerData();
                    }
                    break;
                    case 3:
                    {
                        player.GetComponent<PlayerController>().A3_Unlocked = true;
                        CheckpointManager.Instance.SavePlayerData();
                    }
                    break;
                }
            }
        }
        
    }

    public void Interact_performed(InputAction.CallbackContext context)
    {
        if (interactable && context.performed)
        {
            player.GetComponent<SpriteRenderer>().enabled = false;
            player.GetComponent<PlayerController>().hasControl = false;
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            on = false;
            started = true;
            animator.SetTrigger("StartAnim");

            SoundManager.PlaySound(SoundManager.Sound.snd_tube, transform.position);
        }
    }
}
