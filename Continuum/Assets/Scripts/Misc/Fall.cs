using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class Fall : MonoBehaviour
{
    public float globalTimescale;
    public float? localTimescale;
    private float timeMod;

    public Transform parentTransform;
    public Rigidbody2D parentRb;

    private float fallFactor = 1.2f;
    private float fallDistance = 0.3f;

    private bool falling = false;

    private void Start()
    {
        //Initialise timescales
        localTimescale = null;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pits"))
        {
            if (parentTransform.localScale.y <= (1f - fallDistance))
            {
                falling = true;
            }
        }
    }

    private void Update()
    {
        if (parentRb.velocity.magnitude < 10f)
        {
            if (parentTransform.localScale.y > (1f - fallDistance) || (falling && parentTransform.localScale.y > 0))
            {
                parentTransform.localScale -= new Vector3(fallFactor * Time.deltaTime * timeMod, fallFactor * Time.deltaTime * timeMod, 0);
            }
        }
    }
}
