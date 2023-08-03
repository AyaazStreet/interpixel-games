using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    public const float MOVE_SPEED = 5f;
    public const float A1_DUR = 5f;
    public const float A2_DUR = 5f;
    public const float A3_DUR = 5f;

    public bool infusing = false;
    public bool a1active = false;
    public bool a2active = false;
    public bool comboActive = false;

    public ThrowController throwController;

    private Rigidbody2D rb;
    private Animator anim;
    private float abilityActiveTimer;
    private float abilityCooldownTimer;
    [SerializeField] private int activeAbility;
    private Vector2 moveDir;
    private Vector2 lastMoveDir;

    private void Awake()
    {
        //Initialise components
        anim = GetComponent<Animator>();
        anim.speed = 0.5f;
        rb = GetComponent<Rigidbody2D>();

        //Initialise other variables
        activeAbility = 0;
        abilityCooldownTimer = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        Animate(); //run animations

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
    }

    private void FixedUpdate()
    {
        //Move
        rb.velocity = MOVE_SPEED * moveDir;

        if (moveDir.x != 0 || moveDir.y != 0)
        {
            //Not idle
            lastMoveDir = moveDir;
        }
    }

    public void Movement_performed(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }

    public void Interact_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Player Interact");
        }
    }

    public void Ability_1_performed(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (activeAbility == 0 && !comboActive)
            {
                StartCoroutine(WaitForInput2(context));
            }

            if (abilityCooldownTimer > 0f)
            {
                Debug.Log("Time Abilities On Cooldown");
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

        if (context.performed && infusing)
        {
            throwController.infused = 1;
            Debug.Log("Infuse 1");
        }
    }
    
    public void Ability_2_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!a1active && !comboActive)
            {
                StartCoroutine(WaitForInput1(context));
            }

            if (abilityCooldownTimer > 0f)
            {
                Debug.Log("Time Abilities On Cooldown");
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

        if (context.performed && infusing)
        {
            throwController.infused = 2;
            Debug.Log("Infuse 2");
        }
    }

    public void Ability_3_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!a1active && !a2active)
            {
                if (activeAbility == 0 && abilityCooldownTimer <= 0 && !infusing)
                {
                    Debug.Log("Time Stop Active");
                    comboActive = true;
                    activeAbility = 3;
                    abilityActiveTimer = A3_DUR;
                }
            }

            if (abilityCooldownTimer > 0f)
            {
                Debug.Log("Time Abilities On Cooldown");
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

        if (context.performed && infusing)
        {
            throwController.infused = 3;
            Debug.Log("Infuse 3");
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
            }
        }
    }

    void Animate()
    {
        anim.SetFloat("AnimMoveX", moveDir.x);
        anim.SetFloat("AnimMoveY", moveDir.y);
        anim.SetFloat("AnimMoveMagnitude", moveDir.magnitude);
        anim.SetFloat("AnimLastMoveX", lastMoveDir.x);
        anim.SetFloat("AnimLastMoveY", lastMoveDir.y);
    }
}
