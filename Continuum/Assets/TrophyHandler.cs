using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyHandler : MonoBehaviour
{
    public int level;
    public GameObject trophy;
    
    // Start is called before the first frame update
    void Start()
    {
        trophy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeScoreManager.Instance != null)
        {
            if (TimeScoreManager.Instance.times[level] <= TimeScoreManager.Instance.parTimes[level])
            {
                trophy.SetActive(true);
            }
            else
            { 
                trophy.SetActive(false); 
            }
        }
    }
}
