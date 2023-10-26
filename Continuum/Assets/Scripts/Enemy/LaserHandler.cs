using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserHandler : MonoBehaviour
{
    private float globalTimescale;
    private float? localTimescale;
    private float timeMod;

    public bool on;
    public bool tracking;
    public float laserLength = 10f;

    public Transform gun;
    public Transform firePoint;
    public LayerMask layerMaskWall;
    public LayerMask layerMaskPlayer;

    private Vector2 aimDir;
    private float aimAngle;

    public GameObject laserPrefab;
    private GameObject laser;
    private SpriteRenderer laserSr;

    void Start()
    {
        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;

        SoundManager.PlaySound(SoundManager.Sound.snd_beam, transform.position, transform);
    }

    void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        //Track
        /*if (tracking)
        {
            aimDir = GameManager.Instance.pc.transform.position - transform.position;

            aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90;

            //Calculate quaternion and apply to rotation
            Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, q, 10 * Time.deltaTime * timeMod);
        }
        else
        {
            float zRotation = gun.transform.eulerAngles.z;
            aimDir = new Vector2(Mathf.Cos(zRotation * Mathf.Deg2Rad), Mathf.Sin(zRotation * Mathf.Deg2Rad));
        }*/

        float zRotation = gun.transform.eulerAngles.z - 90f;
        aimDir = new Vector2(Mathf.Cos(zRotation * Mathf.Deg2Rad), Mathf.Sin(zRotation * Mathf.Deg2Rad));

        DrawLaser();
    }

    private void DrawLaser()
    {
        Vector2 start = firePoint.position;
        Vector2 target;
        float laserZ = -1f;
        
        // Create a raycast hit2D variable to store information about the hit
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, aimDir, laserLength, layerMaskWall);
        if (hit.collider != null)
        {
            Vector2 hitPoint = hit.point;
            Debug.DrawLine(firePoint.position, hitPoint, Color.red);

            target = hitPoint;
        }
        else
        {
            Vector2 rayEnd = ((Vector2)firePoint.transform.position) + (aimDir * laserLength);
            Debug.DrawLine(firePoint.position, rayEnd, Color.green);

            target = rayEnd;
        }

        RaycastHit2D playerHit = Physics2D.Raycast(firePoint.position, aimDir, laserLength, layerMaskPlayer);
        if (playerHit.collider != null && Vector2.Distance(firePoint.position, playerHit.point) < Vector2.Distance(firePoint.position, target))
        {
            if (LayerMask.LayerToName(playerHit.collider.gameObject.layer) == "Player") 
            {
                GameManager.Instance.pc.Die("laser");
            }
            else if (LayerMask.LayerToName(playerHit.collider.gameObject.layer) == "Enemy")
            {
                playerHit.collider.gameObject.GetComponent<EnemyPatrol>().Die();
            }

        }

        if (!laser)
        {
            laser = Instantiate(laserPrefab, new Vector3(start.x, start.y, laserZ), firePoint.transform.rotation);
            laserSr = laser.GetComponent<SpriteRenderer>();
        }
        else
        {
            laser.transform.position = new Vector3(start.x, start.y, laserZ);
            laser.transform.rotation = firePoint.transform.rotation;
        }

        laserSr.size = new Vector2(0.1875f, ((target - start).magnitude) / laser.transform.localScale.x);


        if (!on)
        {
            Destroy(laser);
        }
    }
}
