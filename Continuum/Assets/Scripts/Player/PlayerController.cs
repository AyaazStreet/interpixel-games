using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public const float MOVE_SPEED = 4f;
    public const float ANIM_SPEED_MULTI = 0.5f;
    public const float A1_DUR = 5f;
    public const float A2_DUR = 5f;
    public const float A3_DUR = 5f;

    public float stepCounter = 0f;
    public float stepInterval;

    public bool infusing = false;
    public bool a1active = false;
    public bool a2active = false;
    public bool comboActive = false;
    public bool falling = false;

    public bool A1_Unlocked = false;
    public bool A2_Unlocked = false;
    public bool A3_Unlocked = false;
    public bool T1_Unlocked = false;
    public bool T2_Unlocked = false;
    public bool T3_Unlocked = false;

    public bool hasKey = false;

    public bool crushA = false;
    public bool crushB = false;

    public bool alive = true;

    public ThrowController throwController;
    public float throwCooldownDuration = 1f;
    public float throwCooldownTimer = 0f;

    private Rigidbody2D rb;
    private Animator anim;
    public float abilityActiveTimer;
    public float abilityCooldownTimer;
    public int activeAbility;
    private Vector2 moveDir;
    public Vector2 lastMoveDir;

    public Vector2 externalVelocity = Vector2.zero;

    public Image fillIndi;

    public CapsuleCollider2D cc1;
    public CapsuleCollider2D cc2;

    private void Awake()
    {
        //Initialise components
        anim = GetComponent<Animator>();
        anim.speed = ANIM_SPEED_MULTI;
        rb = GetComponent<Rigidbody2D>();

        //Initialise other variables
        activeAbility = 0;
        abilityCooldownTimer = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        Animate(); //run animations

        //Clock
        if (abilityCooldownTimer > 0)
        {
            fillIndi.fillAmount = 1 - (abilityCooldownTimer / 2f);
            fillIndi.color = new Color(0.7725491f, 0.7921569f, 0.8000001f, 0.6f);
        }
        else if (abilityActiveTimer > 0)
        {
            fillIndi.fillAmount = abilityActiveTimer / 5f;
            fillIndi.color = new Color(0.572549f, 0.909804f, 0.7529413f, 0.6f);
        }
        else
        {
            fillIndi.fillAmount = 1;
        }

        //Perform the correct action based on active ability
        switch (activeAbility)
        {
            case 0://no active ability
            {
                //reset timescale to normal
                if ((TimeScaleManager.globalTimescale + 4f * Time.deltaTime) < 1f)
                {
                    TimeScaleManager.globalTimescale += 4f * Time.deltaTime;
                }
                else if ((TimeScaleManager.globalTimescale - 4f * Time.deltaTime) > 1f)
                {
                    TimeScaleManager.globalTimescale -= 4f * Time.deltaTime;
                }
                else if (TimeScaleManager.globalTimescale != 1f)
                {
                    TimeScaleManager.globalTimescale = 1f;
                }
                
                //reduce cooldowns
                if (abilityCooldownTimer > 0f)
                {
                    abilityCooldownTimer -= Time.deltaTime;
                }
            }
            break;
            case 1://Time Slow
            {
                if (abilityActiveTimer > 0f)
                {
                    if ((TimeScaleManager.globalTimescale - 4f * Time.deltaTime) > 0.25f)  //adjust timescale
                    {
                        TimeScaleManager.globalTimescale -= 4f * Time.deltaTime;
                    }
                    else
                    {
                        TimeScaleManager.globalTimescale = TimeScaleManager.A1_EFFECT;
                    }

                    abilityActiveTimer -= Time.deltaTime; //countdown timer
                    if (abilityActiveTimer <= 0f)
                    {
                        activeAbility = 0;
                        abilityCooldownTimer = 2f;
                        
                        Debug.Log("Time Slow Deactivated");
                    }
                }
            } 
            break;
            case 2://Time Accellerate
            {
                if (abilityActiveTimer > 0f)
                {
                    if ((TimeScaleManager.globalTimescale + 4f * Time.deltaTime) < 2f)  //adjust timescale
                    {
                        TimeScaleManager.globalTimescale += 4f * Time.deltaTime;
                    }
                    else
                    {
                        TimeScaleManager.globalTimescale = TimeScaleManager.A2_EFFECT;
                    }

                    abilityActiveTimer -= Time.deltaTime; //countdown timer
                    if (abilityActiveTimer <= 0f)
                    {
                        activeAbility = 0;
                        abilityCooldownTimer = 2f;
                        
                        Debug.Log("Time Accellerate Deactivated");
                    }
                }
            }
            break;
            case 3://Time Stop
            {
                if (abilityActiveTimer > 0f)
                {
                    if((TimeScaleManager.globalTimescale - 4f * Time.deltaTime) > 0f)  //adjust timescale
                    {
                        TimeScaleManager.globalTimescale -= 4f * Time.deltaTime;
                    }
                    else
                    {
                        TimeScaleManager.globalTimescale = TimeScaleManager.A3_EFFECT;
                    }

                    abilityActiveTimer -= Time.deltaTime; //countdown timer
                    if (abilityActiveTimer <= 0f)
                    {
                        comboActive = false;
                        activeAbility = 0;
                        abilityCooldownTimer = 2f;

                        Debug.Log("Time Stop Deactivated");
                    }
                }
            }
            break;
        }

        if (throwCooldownTimer > 0)
        {
            throwCooldownTimer -= Time.deltaTime;
        }

        if (crushA && crushB)
        {
            crushA = false;
            crushB = false;

            cc1.enabled = false;
            cc2.enabled = false;

            if (alive)
            {
                StartCoroutine(CrushPlayer());
            }
            
            alive = false;
        }
    }

    private void FixedUpdate()
    {
        //Move
        if (!falling && alive)
        {
            //sound
            stepCounter += (rb.velocity - externalVelocity).magnitude;
            if (stepCounter > stepInterval)
            {
                stepCounter = 0;
                SoundManager.PlaySound(SoundManager.Sound.snd_footstep);
            }

            UpdateVelocity();
        }

        if (moveDir.x != 0 || moveDir.y != 0)
        {
            //Not idle
            lastMoveDir = moveDir;
        }
        else
        {
            stepCounter = stepInterval-1;
        }
    }

    public void UpdateVelocity()
    {
        //Update velocity
        rb.velocity = (MOVE_SPEED * moveDir) + externalVelocity;
    }

    public void Movement_performed(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }

    public void Restart_performed(InputAction.CallbackContext context)
    {
        if (context.performed && !alive && !falling)
        {
            GameManager.Instance.RestartGame();
        }
    }

    public void Ability_1_performed(InputAction.CallbackContext context)
    {
        if(context.performed && A1_Unlocked)
        {
            if (activeAbility == 0 && !comboActive)
            {
                StartCoroutine(WaitForInput2(context));
            }

            if (abilityCooldownTimer > 0f)
            {
                Debug.Log("Time Abilities On Cooldown");
            }

            if (infusing)
            {
                throwController.infused = 1;
                Debug.Log("Infuse 1");
            }
        }
        else if (context.canceled && activeAbility == 1) //check if button released + ability still active
        {
            //check if minimum duration has passed... 
            if (abilityActiveTimer < A1_DUR - 0.5f)
            {
                abilityActiveTimer = 0.01f; //...if it has, reduce timer to near zero
            }
            else
            {
                abilityActiveTimer = 0.5f - (A1_DUR - abilityActiveTimer); //...if not, wait out the minimum duration
            }
        }

        
    }
    
    public void Ability_2_performed(InputAction.CallbackContext context)
    {
        if (context.performed && A2_Unlocked)
        {
            if (!a1active && !comboActive)
            {
                StartCoroutine(WaitForInput1(context));
            }

            if (abilityCooldownTimer > 0f)
            {
                Debug.Log("Time Abilities On Cooldown");
            }

            if (infusing)
            {
                throwController.infused = 2;
                Debug.Log("Infuse 2");
            }
        }
        else if (context.canceled && activeAbility == 2)
        {
            if (abilityActiveTimer < A2_DUR - 0.5f)
            {
                abilityActiveTimer = 0.01f;
            }
            else
            {
                abilityActiveTimer = 0.5f - (A2_DUR - abilityActiveTimer);
            }
        }

        
    }

    public void Ability_3_performed(InputAction.CallbackContext context)
    {
        if (context.performed && A3_Unlocked)
        {
            if (!a1active && !a2active)
            {
                if (activeAbility == 0 && abilityCooldownTimer <= 0 && !infusing)
                {
                    comboActive = true;
                    activeAbility = 3;
                    abilityActiveTimer = A3_DUR;
                    Debug.Log("Time Stop Active");
                    SoundManager.PlaySound(SoundManager.Sound.snd_stopTime);
                }
            }

            if (abilityCooldownTimer > 0f)
            {
                Debug.Log("Time Abilities On Cooldown");
            }

            if (infusing)
            {
                throwController.infused = 3;
                Debug.Log("Infuse 3");
            }
        }
        else if (context.canceled && activeAbility == 3)
        {
            if (abilityActiveTimer < A3_DUR - 0.5f)
            {
                abilityActiveTimer = 0.01f;
            }
            else
            {
                abilityActiveTimer = 0.5f - (A3_DUR - abilityActiveTimer);
            }
        }

        
    }

    private IEnumerator WaitForInput2(InputAction.CallbackContext c)
    {
        yield return new WaitForSeconds(0.1f);

        if (!comboActive)
        {
            if (c.performed && activeAbility == 0 && abilityCooldownTimer <= 0 && !infusing) //check button pressed + no active ability + no cooldown
            {
                activeAbility = 1; //change active ability 
                abilityActiveTimer = A1_DUR; //set ability duration
                Debug.Log("Time Slow Active");
                SoundManager.PlaySound(SoundManager.Sound.snd_slowTime);
            }
        }
    }

    private IEnumerator WaitForInput1(InputAction.CallbackContext c)
    {
        yield return new WaitForSeconds(0.1f);

        if (!comboActive)
        {
            if (c.performed && activeAbility == 0 && abilityCooldownTimer <= 0 && !infusing)
            {
                activeAbility = 2;
                abilityActiveTimer = A2_DUR;
                Debug.Log("Time Accelerate Active");
                SoundManager.PlaySound(SoundManager.Sound.snd_accelTime);
            }
        }
    }

    private IEnumerator CrushPlayer()
    {
        while (transform.localScale.y > 0.01) 
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 100f * Time.deltaTime, transform.localScale.z);
            yield return new WaitForSeconds(0.05f);
        }
        transform.localScale = Vector3.zero;
        GameManager.Instance.ShowDeathScreen();

        yield break;
    }

    void Animate()
    {
        anim.speed = ((rb.velocity - externalVelocity).magnitude / MOVE_SPEED) * ANIM_SPEED_MULTI;
        //Debug.Log((rb.velocity - externalVelocity).magnitude / MOVE_SPEED);

        anim.SetFloat("AnimMoveMagnitude", (rb.velocity - externalVelocity).magnitude);
        anim.SetFloat("AnimMoveX", lastMoveDir.x);
        anim.SetFloat("AnimMoveY", lastMoveDir.y);
    }


    public void ResetScene(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("CrushTrigger"))
        {
            if(!collision.GetComponent<DeathTrigger>().isOpen)
            {
                if (collision.GetComponent<DeathTrigger>().a)
                {
                    crushA = true;
                }
                else
                {
                    crushB = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CrushTrigger"))
        {
            if (!collision.GetComponent<DeathTrigger>().isOpen)
            {
                if (collision.GetComponent<DeathTrigger>().a)
                {
                    crushA = false;
                }
                else
                {
                    crushB = false;
                }
            }
        }
    }
}
