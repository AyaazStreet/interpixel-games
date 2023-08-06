using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ThrowController : MonoBehaviour
{
    public GameObject boltPrefab;
    public GameObject canisterPrefab;
    public GameObject rodPrefab;

    public GameObject fieldPrefab_slow;
    public GameObject fieldPrefab_acc;
    public GameObject fieldPrefab_stop;

    public Transform throwPoint;

    public int selected = 0;
    public int infused = 0;
    
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerController pc;

    private Rigidbody2D rb;

    private Vector2 mousePos;
    private Vector2 GamepadAimDir;
    private Vector2 MouseAimDir;
    private float aimAngle;
    
    private void Awake()
    {
        //Initialise components
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Get mouse aim direction
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        MouseAimDir = mousePos - rb.position;

        //Calculate aim angle based on input type
        if (playerInput.currentControlScheme == "Keyboard")
        {
            aimAngle = Mathf.Atan2(MouseAimDir.y, MouseAimDir.x) * Mathf.Rad2Deg - 90f;

            //Animation
            pc.lastMoveDir = MouseAimDir;
            anim.SetFloat("AnimMoveX", MouseAimDir.x);
            anim.SetFloat("AnimMoveY", MouseAimDir.y);
        }
        else if (playerInput.currentControlScheme == "Gamepad")
        {
            aimAngle = Mathf.Atan2(GamepadAimDir.y, GamepadAimDir.x) * Mathf.Rad2Deg - 90f;

            //Animation
            pc.lastMoveDir = MouseAimDir;
            anim.SetFloat("AnimMoveX", GamepadAimDir.x);
            anim.SetFloat("AnimMoveY", GamepadAimDir.y);
        }

        //Calculate quaternion and apply to rotation
        Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, 10 * Time.deltaTime);
    }

    public void ControllerAim_performed(InputAction.CallbackContext context)
    {
        //Get gamepad aim direction
        if (context.ReadValue<Vector2>().x != 0 && context.ReadValue<Vector2>().y != 0)
        {
            GamepadAimDir = context.ReadValue<Vector2>();
        }
    }

    public void Throw_performed(InputAction.CallbackContext context)
    {
        if(context.performed && gameObject.activeSelf && pc.throwCooldownTimer <= 0)
        {
            //Cooldown
            pc.throwCooldownTimer = pc.throwCooldownDuration;

            //Instantiate throwable object
            switch (selected)
            {
                case 1: _ = Instantiate(boltPrefab, throwPoint.position, throwPoint.rotation);
                    break;
                case 2:
                {
                    //special case for canisters, fire correct cannister based on infusion
                    switch(infused)
                    {
                        case 0: Debug.Log("Not Infused"); //no infusion
                            break;
                        case 1:
                        {
                            GameObject obj = Instantiate(canisterPrefab, throwPoint.position, throwPoint.rotation);
                            obj.GetComponent<Canister>().fieldPrefab = fieldPrefab_slow; //set the correct infuse type
                        }
                            break; 
                        case 2:
                        {
                            GameObject obj = Instantiate(canisterPrefab, throwPoint.position, throwPoint.rotation);
                            obj.GetComponent<Canister>().fieldPrefab = fieldPrefab_acc; //set the correct infuse type
                        }
                            break;
                        case 3:
                        {
                            GameObject obj = Instantiate(canisterPrefab, throwPoint.position, throwPoint.rotation);
                            obj.GetComponent<Canister>().fieldPrefab = fieldPrefab_stop; //set the correct infuse type
                        }
                            break;
                    }
                }
                    break;
                case 3: _ = Instantiate(rodPrefab, throwPoint.position, throwPoint.rotation);
                    break;
                default: Debug.Log("No Throwable Selected");
                    break;
            }
        }
    }

}
