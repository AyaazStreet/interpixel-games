using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject deathScreen;

    public GameObject player;
    private Vector3 respawnPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Find the player's GameObject and store its Transform component.
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        RespawnPlayer();
        Time.timeScale = 1f;
    }

    public void RespawnPlayer()
    {
        // Set the player's position to the last activated checkpoint position.
        player.transform.position = respawnPosition;

        player.transform.localScale = new Vector3(1, 1, 1);

        Color tmp1 = player.GetComponent<SpriteRenderer>().color;

        tmp1.r = 1f;
        tmp1.g = 1f;
        tmp1.b = 1f;
        tmp1.a = 1f;

        player.GetComponent<SpriteRenderer>().color = tmp1;

        player.GetComponent<PlayerController>().crushA = false;
        player.GetComponent<PlayerController>().crushB = false;

        player.GetComponent<PlayerController>().cc1.enabled = true;
        player.GetComponent<PlayerController>().cc2.enabled = true;

        player.GetComponent<PlayerController>().alive = true;

        player.GetComponent<PlayerController>().abilityActiveTimer = 0f;
        player.GetComponent<PlayerController>().abilityCooldownTimer = 0.1f;

        player.GetComponent<PlayerController>().fillIndi.fillAmount = 1f;

        player.GetComponent<PlayerController>().activeAbility = 0;

        deathScreen.SetActive(false);
    }

    public void SetRespawnPosition(Vector3 position)
    {
        // Set the last activated checkpoint position.
        respawnPosition = position;
    }
}

