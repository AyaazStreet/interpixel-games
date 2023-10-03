using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyDetection : MonoBehaviour
{
    public float globalTimescale;
    public float? localTimescale;
    private float timeMod;

    public float detectionLevel = 0f;
    public float distance;
    public bool detected = false;
    private bool inFOV = false;

    public GameObject indicator;
    public Image indicatorFill;
    public GameObject player;

    private GameObject playerIndicator;
    public GameObject playerIndicatorPrefab;

    private void Start()
    {
        indicator.SetActive(false);
        indicatorFill.fillAmount = 0;
        player = GameObject.Find("Player");

        //Initialise timescales
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = 1f;
    }

    void Update()
    {
        //Adjust timeMod based on timescales, preferring local over global
        localTimescale = gameObject.GetComponent<LocalModifier>().value;
        globalTimescale = TimeScaleManager.globalTimescale;
        timeMod = localTimescale ?? globalTimescale;

        //Check if in cone of vision
        if (detected)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            detectionLevel += (400f / distance) * Time.deltaTime * timeMod;
        }
        else
        {
            if(detectionLevel > 0f)
            {
                detectionLevel -= 200f * Time.deltaTime * timeMod;
            }
        }
        
        //if detection bar full
        if(detectionLevel >= 100)
        {
            detectionLevel = 0;
            GameManager.Instance.pc.Die();
        }
        
        indicatorFill.fillAmount = detectionLevel / 100;

    }

    private void FixedUpdate()
    {
        if (inFOV)
        {
            detected = true;
            indicator.SetActive(true);

            if (playerIndicator)
            {
                playerIndicator.SetActive(true);
            }
            else
            {
                Debug.Log("test");
                playerIndicator = Instantiate(playerIndicatorPrefab, GameManager.Instance.pc.indicators.transform);
                playerIndicator.GetComponent<DetectionIndicator>().enemy = this;
            }

            inFOV = false;
        }
        else
        {
            detected = false;

            if (indicatorFill.fillAmount <= 0)
            {
                indicator.SetActive(false);
            }

            if (playerIndicator)
            {
                playerIndicator.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inFOV = true;
        }
    }
}
