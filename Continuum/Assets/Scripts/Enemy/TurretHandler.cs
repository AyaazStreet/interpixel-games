using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretHandler : MonoBehaviour
{
    private float globalTimescale;
    private float? localTimescale;
    private float timeMod;

    public float startDelay;
    public float delay;
    public float speed;
    public bool tracking;
    
    public Transform gun;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public LayerMask layerMask;

    private float timer;
    private Vector2 aimDir;
    private float aimAngle;

    private Animator anim;
    
    void Start()
    {
        //Initialise timescales
        localTimescale = null;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        //Init
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        //Track
        if (tracking)
        {
            aimDir = GameManager.Instance.pc.transform.position - transform.position;

            aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90;

            //Calculate quaternion and apply to rotation
            Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, q, 10 * Time.deltaTime * timeMod);
        }

        //Timer
        if (startDelay < 0f)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime * timeMod;
            }
            else
            {
                Shoot();
            }
        }
        else
        {
            startDelay -= Time.deltaTime * timeMod;
        }

        //Anim
        anim.SetBool("Tracking", tracking);
        anim.speed = timeMod;
    }

    void Shoot()
    {
        if (!tracking)
        {
            GameObject b = Instantiate(bulletPrefab, firePoint.transform.position, gun.transform.rotation);
            b.GetComponentInChildren<BulletHandler>().speed = speed;

            SoundManager.PlaySound(SoundManager.Sound.snd_shot, firePoint.transform.position);
            timer = delay;

            anim.SetTrigger("Shoot");
        }
        else
        {
            // Create a raycast hit2D variable to store information about the hit
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, aimDir, Mathf.Infinity, layerMask);

            // Check if the hit object is on the specified layer
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GameObject b = Instantiate(bulletPrefab, firePoint.transform.position, gun.transform.rotation);
                b.GetComponentInChildren<BulletHandler>().speed = speed;

                SoundManager.PlaySound(SoundManager.Sound.snd_shot, firePoint.transform.position);
                timer = delay;

                anim.SetTrigger("Shoot");
            }
        }

        
    }
}
