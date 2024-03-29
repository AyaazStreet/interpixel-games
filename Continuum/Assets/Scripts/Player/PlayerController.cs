using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private const float MOVE_SPEED = 4f;
    private const float ANIM_SPEED_MULTI = 0.5f;
    private const float A1_DUR = 5f;
    private const float A2_DUR = 5f;
    private const float A3_DUR = 2f;
    private const float CD_DUR = 3f;

    public bool hasControl = true;

    [SerializeField] private float stepCounter = 0f;
    [SerializeField] private float stepInterval = 67f;

    public bool infusing = false;
    private bool a1active = false;
    private bool a2active = false;
    private bool comboActive = false;

    public bool falling = false;

    public bool A1_Unlocked = false;
    public bool A2_Unlocked = false;
    public bool A3_Unlocked = false;

    public bool T1_Unlocked = false;
    public bool T2_Unlocked = false;
    public bool T3_Unlocked = false;

    public bool crushA = false;
    public bool crushB = false;

    public bool alive = true;

    //GOD
    public bool invincible = false;
    public bool teleport = false;
    public bool infiniteDuration = false;
    //GOD

    public ThrowController throwController;
    public float throwCooldownDuration = 1f;
    public float throwCooldownTimer = 0f;

    public GameObject indicators;

    public GameObject playerCanvas;
    public GameObject popupTextPrefab;

    private Rigidbody2D rb;
    public Animator anim;
    public float abilityActiveTimer;
    public float abilityCooldownTimer;
    public int activeAbility;
    public Vector2 moveDir;
    public Vector2 lastMoveDir;

    private Vector2 externalVelocity; 
    public List<Rigidbody2D> externalVelocityObjs;

    public Image fillIndi;

    public CapsuleCollider2D cc1;
    public CapsuleCollider2D cc2;

    public SoundManager.Sound walkSound;

    private AudioSource powerSoundSource;

    private void Awake()
    {
        //Initialise components
        anim = GetComponent<Animator>();
        anim.speed = ANIM_SPEED_MULTI;
        rb = GetComponent<Rigidbody2D>();

        //Initialise other variables
        activeAbility = 0;
        abilityCooldownTimer = 0;

        walkSound = SoundManager.Sound.snd_footstep;
    }

    public void UpdateFromSave(PlayerData data)
    {
        if (data != null)
        {
            /*A1_Unlocked = data.A1_Unlocked;
            A2_Unlocked = data.A2_Unlocked;
            A3_Unlocked = data.A3_Unlocked;

            T1_Unlocked = data.T1_Unlocked;
            T2_Unlocked = data.T2_Unlocked;
            T3_Unlocked = data.T3_Unlocked;*/
        }
    }

    

    // Update is called once per frame
    private void Update()
    {
        //Clock
        if (abilityCooldownTimer > 0)
        {
            fillIndi.fillAmount = 1 - (abilityCooldownTimer / CD_DUR);
            fillIndi.color = new Color(0.7725491f, 0.7921569f, 0.8000001f, 0.6f);
        }
        else if (abilityActiveTimer > 0)
        {
            switch (activeAbility)
            {
                case 1:
                    fillIndi.fillAmount = abilityActiveTimer / A1_DUR;
                    fillIndi.color = new (TimeScaleManager.A1_COLOR.r, TimeScaleManager.A1_COLOR.g, TimeScaleManager.A1_COLOR.b, 0.5f);
                    break;
                case 2:
                    fillIndi.fillAmount = abilityActiveTimer / A2_DUR;
                    fillIndi.color = new(TimeScaleManager.A2_COLOR.r, TimeScaleManager.A2_COLOR.g, TimeScaleManager.A2_COLOR.b, 0.7f);
                    break;
                case 3:
                    fillIndi.fillAmount = abilityActiveTimer / A3_DUR;
                    fillIndi.color = new(TimeScaleManager.A3_COLOR.r, TimeScaleManager.A3_COLOR.g, TimeScaleManager.A3_COLOR.b, 0.5f);
                    break;
            }
            //fillIndi.color = new Color(0.572549f, 0.909804f, 0.7529413f, 0.6f);
        }
        else
        {
            fillIndi.fillAmount = 1;
        }

        //Perform the correct action based on active ability
        if (hasControl)
        {
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
                        if ((TimeScaleManager.globalTimescale - 4f * Time.deltaTime) > TimeScaleManager.A1_EFFECT)  //adjust timescale
                        {
                            TimeScaleManager.globalTimescale -= 4f * Time.deltaTime;
                        }
                        else
                        {
                            TimeScaleManager.globalTimescale = TimeScaleManager.A1_EFFECT;
                        }

                        abilityActiveTimer -= Time.deltaTime; //countdown timer

                        if (infiniteDuration && abilityActiveTimer > 0.5f)
                        {
                            abilityActiveTimer = A1_DUR;
                        }

                        if (abilityActiveTimer <= 0f)
                        {
                            activeAbility = 0;

                            if (powerSoundSource) StartCoroutine(SoundManager.CutoffSound(powerSoundSource, 0.5f));

                            if (abilityCooldownTimer <= 0f)
                            {
                                abilityCooldownTimer = CD_DUR;
                            }

                            Debug.Log("Time Slow Deactivated");
                        }
                    }
                }
                break;
                case 2://Time Accellerate
                {
                    if (abilityActiveTimer > 0f)
                    {
                        if ((TimeScaleManager.globalTimescale + 4f * Time.deltaTime) < TimeScaleManager.A2_EFFECT)  //adjust timescale
                        {
                            TimeScaleManager.globalTimescale += 4f * Time.deltaTime;
                        }
                        else
                        {
                            TimeScaleManager.globalTimescale = TimeScaleManager.A2_EFFECT;
                        }

                        abilityActiveTimer -= Time.deltaTime; //countdown timer

                        if (infiniteDuration && abilityActiveTimer > 0.5f)
                        {
                            abilityActiveTimer = A2_DUR;
                        }

                        if (abilityActiveTimer <= 0f)
                        {
                            activeAbility = 0;

                            if (powerSoundSource) StartCoroutine(SoundManager.CutoffSound(powerSoundSource, 0.5f));

                            if (abilityCooldownTimer <= 0f)
                            {
                                abilityCooldownTimer = CD_DUR;
                            }

                            Debug.Log("Time Accellerate Deactivated");
                        }
                    }
                }
                break;
                case 3://Time Stop
                {
                    if (abilityActiveTimer > 0f)
                    {
                        if ((TimeScaleManager.globalTimescale - 4f * Time.deltaTime) > TimeScaleManager.A3_EFFECT)  //adjust timescale
                        {
                            TimeScaleManager.globalTimescale -= 4f * Time.deltaTime;
                        }
                        else
                        {
                            TimeScaleManager.globalTimescale = TimeScaleManager.A3_EFFECT;
                        }

                        abilityActiveTimer -= Time.deltaTime; //countdown timer

                        if (infiniteDuration && abilityActiveTimer > 0.5f)
                        {
                            abilityActiveTimer = A3_DUR;
                        }

                        if (abilityActiveTimer <= 0f)
                        {
                            comboActive = false;
                            activeAbility = 0;

                            if (powerSoundSource) StartCoroutine(SoundManager.CutoffSound(powerSoundSource, 0.5f));

                            if (abilityCooldownTimer <= 0f)
                            {
                                abilityCooldownTimer = CD_DUR;
                            }

                            Debug.Log("Time Stop Deactivated");
                        }
                    }
                }
                break;
            }
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

            if (alive && !invincible)
            {
                StartCoroutine(CrushPlayer());
            }
            
            alive = false;
        }

        if (Time.timeScale > 0f)
        {
            Animate(); //run animations
        }
    }

    private void FixedUpdate()
    {
        if (!hasControl)
        {
            moveDir = Vector2.zero;
        }

        //Move
        if (!falling && alive)
        {
            //sound
            stepCounter += (rb.velocity - externalVelocity).magnitude;
            if (stepCounter > stepInterval)
            {
                stepCounter = stepCounter - stepInterval;
                SoundManager.PlaySound(walkSound);
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
        //Update ext v
        if (externalVelocityObjs.Count > 0)
        {
            externalVelocity = externalVelocityObjs[0].velocity;

            foreach (var v in externalVelocityObjs)
            {
                if (v.velocity.magnitude > externalVelocity.magnitude)
                {
                    externalVelocity = v.velocity;
                }
            }
        }
        else
        {
            externalVelocity = Vector2.zero;
        }

        //Update velocity
        rb.velocity = (MOVE_SPEED * moveDir) + externalVelocity;
    }

    public void Movement_performed(InputAction.CallbackContext context)
    {
        if (hasControl)
        {
            moveDir = context.ReadValue<Vector2>();
        }
        else
        {
            moveDir = Vector2.zero;
        }
        
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
        if(context.performed && A1_Unlocked && hasControl)
        {
            //Toggle Option
            if (CheckpointManager.Instance.togglePowers && activeAbility == 1)
            {
                //check if minimum duration has passed... 
                if (abilityActiveTimer < A1_DUR - 0.5f)
                {
                    abilityCooldownTimer = (1 - (abilityActiveTimer / A1_DUR)) * CD_DUR;
                    abilityActiveTimer = 0.001f; //...if it has, reduce timer to near zero
                }
                else
                {
                    StartCoroutine(MinDuration(0.5f - (A1_DUR - abilityActiveTimer))); //...if not, wait out the minimum duration
                }
            }
            else
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
                    throwController.SetInfusion(1);
                    SoundManager.PlaySound(SoundManager.Sound.snd_infuse);
                    Debug.Log("Infuse 1");
                }
            }
            
        }
        else if (context.canceled && activeAbility == 1 && !CheckpointManager.Instance.togglePowers) //check if button released + ability still active
        {
            //check if minimum duration has passed... 
            if (abilityActiveTimer < A1_DUR - 0.5f)
            {
                abilityCooldownTimer = (1 - (abilityActiveTimer / A1_DUR)) * CD_DUR;
                abilityActiveTimer = 0.001f; //...if it has, reduce timer to near zero
            }
            else
            {
                StartCoroutine(MinDuration(0.5f - (A1_DUR - abilityActiveTimer))); //...if not, wait out the minimum duration
            }
        }


        
    }
    
    public void Ability_2_performed(InputAction.CallbackContext context)
    {
        if (context.performed && A2_Unlocked && hasControl)
        {
            //Toggle Option
            if (CheckpointManager.Instance.togglePowers && activeAbility == 2)
            {
                //check if minimum duration has passed... 
                if (abilityActiveTimer < A2_DUR - 0.5f)
                {
                    abilityCooldownTimer = (1 - (abilityActiveTimer / A2_DUR)) * CD_DUR;
                    abilityActiveTimer = 0.001f; //...if it has, reduce timer to near zero
                }
                else
                {
                    StartCoroutine(MinDuration(0.5f - (A2_DUR - abilityActiveTimer))); //...if not, wait out the minimum duration
                }
            }
            else
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
                    throwController.SetInfusion(2);
                    SoundManager.PlaySound(SoundManager.Sound.snd_infuse);
                    Debug.Log("Infuse 2");
                }
            }
        }
        else if (context.canceled && activeAbility == 2 && !CheckpointManager.Instance.togglePowers)
        {
            if (abilityActiveTimer < A2_DUR - 0.5f)
            {
                abilityCooldownTimer = (1 - (abilityActiveTimer / A2_DUR)) * CD_DUR;
                abilityActiveTimer = 0.001f;
            }
            else
            {
                StartCoroutine(MinDuration(0.5f - (A2_DUR - abilityActiveTimer)));
            }
        }

        
    }

    public void Ability_3_performed(InputAction.CallbackContext context)
    {
        if (context.performed && A3_Unlocked && hasControl)
        {
            //Toggle Option
            if (CheckpointManager.Instance.togglePowers && activeAbility == 3)
            {
                //check if minimum duration has passed... 
                if (abilityActiveTimer < A3_DUR - 0.5f)
                {
                    abilityCooldownTimer = (1 - (abilityActiveTimer / A3_DUR)) * CD_DUR;
                    abilityActiveTimer = 0.001f; //...if it has, reduce timer to near zero
                }
                else
                {
                    StartCoroutine(MinDuration(0.5f - (A3_DUR - abilityActiveTimer))); //...if not, wait out the minimum duration
                }
            }
            else
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
                        powerSoundSource = SoundManager.PlaySoundCutoff(SoundManager.Sound.snd_stopLoop);
                    }
                }

                if (abilityCooldownTimer > 0f)
                {
                    Debug.Log("Time Abilities On Cooldown");
                }

                if (infusing)
                {
                    throwController.SetInfusion(3);
                    SoundManager.PlaySound(SoundManager.Sound.snd_infuse);
                    Debug.Log("Infuse 3");
                }
            }
        }
        else if (context.canceled && activeAbility == 3 && !CheckpointManager.Instance.togglePowers)
        {
            if (abilityActiveTimer < A3_DUR - 0.5f)
            {
                abilityCooldownTimer = (1 - (abilityActiveTimer / A3_DUR)) * CD_DUR;
                abilityActiveTimer = 0.001f;
            }
            else
            {
                StartCoroutine(MinDuration(0.5f - (A3_DUR - abilityActiveTimer)));
            }
        }

    }

    private IEnumerator MinDuration(float remaining)
    {
        yield return new WaitForSeconds(remaining);
        
        switch (activeAbility)
        {
            case 1: 
                abilityCooldownTimer = (1 - (abilityActiveTimer / A1_DUR)) * CD_DUR;
                break;
            case 2:
                abilityCooldownTimer = (1 - (abilityActiveTimer / A2_DUR)) * CD_DUR;
                break; 
            case 3:
                abilityCooldownTimer = (1 - (abilityActiveTimer / A3_DUR)) * CD_DUR;
                break;
        }

        
        abilityActiveTimer = 0.001f;

        yield break;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if(context.performed && hasControl)
        {
            PauseManager.Instance.TogglePause();
        }
    }

    private IEnumerator WaitForInput2(InputAction.CallbackContext c)
    {
        yield return new WaitForSeconds(0.1f);

        if (!comboActive)
        {
            if (CheckpointManager.Instance.togglePowers)
            {
                if (activeAbility == 0 && abilityCooldownTimer <= 0 && !infusing) //check button pressed + no active ability + no cooldown
                {
                    activeAbility = 1; //change active ability 
                    abilityActiveTimer = A1_DUR; //set ability duration
                    //Debug.Log("Time Slow Active");
                    SoundManager.PlaySound(SoundManager.Sound.snd_slowTime);
                    powerSoundSource = SoundManager.PlaySoundCutoff(SoundManager.Sound.snd_slowLoop);
                }
            }
            else
            {
                if (c.performed && activeAbility == 0 && abilityCooldownTimer <= 0 && !infusing) //check button pressed + no active ability + no cooldown
                {
                    activeAbility = 1; //change active ability 
                    abilityActiveTimer = A1_DUR; //set ability duration
                    //Debug.Log("Time Slow Active");
                    SoundManager.PlaySound(SoundManager.Sound.snd_slowTime);
                    powerSoundSource = SoundManager.PlaySoundCutoff(SoundManager.Sound.snd_slowLoop);
                }
            }
        }
    }

    private IEnumerator WaitForInput1(InputAction.CallbackContext c)
    {
        yield return new WaitForSeconds(0.1f);

        if (!comboActive)
        {
            if (CheckpointManager.Instance.togglePowers)
            {
                if (activeAbility == 0 && abilityCooldownTimer <= 0 && !infusing)
                {
                    activeAbility = 2;
                    abilityActiveTimer = A2_DUR;
                    //Debug.Log("Time Accelerate Active");
                    SoundManager.PlaySound(SoundManager.Sound.snd_accelTime);
                    powerSoundSource = SoundManager.PlaySoundCutoff(SoundManager.Sound.snd_accelLoop);
                }
            }
            else
            {
                if (c.performed && activeAbility == 0 && abilityCooldownTimer <= 0 && !infusing)
                {
                    activeAbility = 2;
                    abilityActiveTimer = A2_DUR;
                    //Debug.Log("Time Accelerate Active");
                    SoundManager.PlaySound(SoundManager.Sound.snd_accelTime);
                    powerSoundSource = SoundManager.PlaySoundCutoff(SoundManager.Sound.snd_accelLoop);
                }
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

        alive = true;

        Die("crush");

        yield break;
    }

    void Animate()
    {
        if ((rb.velocity - externalVelocity).magnitude != 0)
        {
            anim.speed = ((rb.velocity - externalVelocity).magnitude / MOVE_SPEED) * ANIM_SPEED_MULTI;
        }
        else
        {
            anim.speed = ANIM_SPEED_MULTI;
        }

        if (hasControl)
        {
            anim.SetFloat("AnimMoveMagnitude", (rb.velocity - externalVelocity).magnitude);
            anim.SetFloat("AnimMoveX", lastMoveDir.x);
            anim.SetFloat("AnimMoveY", lastMoveDir.y);
        }

        if (falling)
        {
            anim.speed = ANIM_SPEED_MULTI;
        }
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

    /////////////////////////////////////DEATH\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    public void Die(string causeOfDeath)
    {
        if (!invincible && alive)
        {
            switch (causeOfDeath)
            {
                default:
                {
                    alive = false;
                    GameManager.Instance.ShowDeathScreen();
                }
                break;
                case "guard":
                {
                    StartCoroutine(Death_Caught());
                }
                break;
                case "crush":
                {
                    GameManager.Instance.deathText.text = "You were crushed";
                    alive = false;
                    GameManager.Instance.ShowDeathScreen();
                }
                break;
                case "bullet":
                {
                    GameManager.Instance.deathText.text = "You have been obliterated";
                    alive = false;
                    StartCoroutine(Death_Laser());
                }
                break;
                case "laser":
                {
                    GameManager.Instance.deathText.text = "You have been incinerated";
                    alive = false;
                    StartCoroutine(Death_Laser());
                }
                break;
                case "fall":
                {
                    alive = false;
                    GameManager.Instance.ShowDeathScreen();
                }
                break;
            }
        }
    }

    private IEnumerator Death_Caught()
    {
        hasControl = false;
        alive = false;
        rb.velocity = Vector2.zero;

        anim.SetTrigger("Caught");

        yield return new WaitForSeconds(0.2f);

        
        GameManager.Instance.ShowDeathScreen();

        yield break;
    }

    private IEnumerator Death_Laser()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_burn);

        hasControl = false;
        alive = false;
        rb.velocity = Vector2.zero;

        anim.SetTrigger("Laser");

        yield return new WaitForSeconds(1.2f);


        GameManager.Instance.ShowDeathScreen();

        yield break;
    }

    /////////////////////////////////////GOD\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    public void GodMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("God Mode Active");
            GodManager.Instance.ToggleGod();
        }
    }

    public void Teleport(InputAction.CallbackContext context)
    {
        if (context.performed && teleport)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //TEST
        if (context.performed)
        {
            DisplayPopup("Teleported");
        }
    }

    public void DisplayPopup(string popupText)
    {
        GameObject popup = Instantiate(popupTextPrefab, playerCanvas.transform);
        popup.GetComponent<TextMeshProUGUI>().text = popupText;
    }

    public void DisplayPopup(string popupText, Color c)
    {
        GameObject popup = Instantiate(popupTextPrefab, playerCanvas.transform);
        popup.GetComponent<TextMeshProUGUI>().text = popupText;
        popup.GetComponent<TextMeshProUGUI>().color = c;
    }
}
