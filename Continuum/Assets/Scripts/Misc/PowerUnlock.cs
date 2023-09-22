using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUnlock : MonoBehaviour
{
    bool on = true;
    bool started = false;

    public bool interactable = false;

    public float timerMax = 6f;
    public float timer = 0f;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && on)
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactable = false;
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
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                player.transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f, player.transform.position.z);
            }
        }
        
    }

    public void Interact_performed(InputAction.CallbackContext context)
    {
        if (interactable && context.performed)
        {
            player.GetComponent<SpriteRenderer>().enabled = false;
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            on = false;
            started = true;
            animator.SetTrigger("StartAnim");

            SoundManager.PlaySound(SoundManager.Sound.snd_interact_btn, transform.position);
        }
    }
}
