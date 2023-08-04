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
    public Image indicator;
    public GameObject player;

    private void Start()
    {
        indicator.fillAmount = 0;
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
            detectionLevel += (300f / distance) * Time.deltaTime * timeMod;
        }
        else
        {
            if(detectionLevel > 0f)
            {
                detectionLevel -= 100f * Time.deltaTime * timeMod;
            }
        }
        
        //if detection bar full
        if(detectionLevel >= 100)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        indicator.fillAmount = detectionLevel / 100;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            detected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            detected = false;
        }
    }
}
