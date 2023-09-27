using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private Animator anim;

    public GameObject fade;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
        StartCoroutine(FStart());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(NextLevel());
        }
    }

    private IEnumerator FStart()
    {
        fade.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        fade.SetActive(false);

        yield break;
    }

    private IEnumerator NextLevel()
    {
        fade.SetActive(true);
        anim.SetTrigger("start");
        GameManager.Instance.pc.hasControl = false;
        GameManager.Instance.pc.externalVelocityObjs.Add(GameManager.Instance.pc.GetComponent<Rigidbody2D>());

        yield return new WaitForSeconds(1.2f);
        
        SceneManager.LoadScene("Level_" + (GameManager.Instance.currLevel + 1));

        yield break;
    }
}