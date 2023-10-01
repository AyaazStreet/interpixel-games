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

    private float timer;
    
    void Start()
    {
        //Initialise timescales
        localTimescale = null;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;
    }

    void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        if (startDelay < 0f)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime * timeMod;
            }
            else
            {
                _ = Instantiate(bulletPrefab, firePoint.transform.position, gun.transform.rotation);
                SoundManager.PlaySound(SoundManager.Sound.snd_shot, firePoint.transform.position);
                timer = delay;
            }
        }
        else
        {
            startDelay -= Time.deltaTime * timeMod;
        }

        if (tracking)
        {
            Vector2 aimDir = GameManager.Instance.pc.transform.position - transform.position;

            float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90;

            //Calculate quaternion and apply to rotation
            Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, q, 10 * Time.deltaTime * timeMod);
        }
    }
}
