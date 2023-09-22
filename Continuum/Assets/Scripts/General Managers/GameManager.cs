using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject deathScreen;

    public GameObject timer;
    public GameObject equipment;
    public GameObject inventory;
    
    public GameObject player;
    public PlayerController pc;
    public InventoryManager im;
    private Vector3 respawnPosition;

    public GameObject savedObjects;
    public GameObject interactables;

    public GameObject cmPrefab;
    public CheckpointManager checkpointManager;

    public int currLevel;

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

        Time.timeScale = 1f;

        // Find the player's GameObject and store its Transform component.
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        im = player.GetComponent<InventoryManager>();

        pc.UpdateFromSave(SaveManager.LoadData());

        //Checkpoint Object Create
        if (GameObject.FindGameObjectWithTag("CheckpointManager"))
        {
            checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
        }
        else
        {
            GameObject cm = Instantiate(cmPrefab);
            cm.name = "CheckpointManager";
            checkpointManager = cm.GetComponent<CheckpointManager>();
        }

        //Load info from checkpoint manager
        if (checkpointManager.savedLevel == currLevel) 
        {
            //position
            player.transform.position = checkpointManager.savedPosition;

            //abilities
            pc.A1_Unlocked = checkpointManager.unlocks[0];
            pc.A2_Unlocked = checkpointManager.unlocks[1];
            pc.A3_Unlocked = checkpointManager.unlocks[2];
            pc.T1_Unlocked = checkpointManager.unlocks[3];
            pc.T2_Unlocked = checkpointManager.unlocks[4];
            pc.T3_Unlocked = checkpointManager.unlocks[5];

            //gui
            if (pc.A1_Unlocked || pc.A2_Unlocked || pc.A3_Unlocked)
            {
                timer.SetActive(true);
            }
            if (pc.T1_Unlocked || pc.T2_Unlocked || pc.T3_Unlocked)
            {
                equipment.SetActive(true);
            }

            //objects
            int j = 0;
            foreach (InventoryManager.InventoryItem item in checkpointManager.savedInventory)
            {
                item.used = checkpointManager.usedStates[j];
                j++;
                im.inventory.Add(item);
                foreach(Transform t in savedObjects.transform)
                {
                    if (t.position == item.pickupPosition)
                    {
                        Destroy(t.gameObject);
                    }
                }
            }
            im.UpdateInventoryDisplay();

            //interactables
            int i = 0;
            foreach (Transform t in interactables.transform)
            {
                if (checkpointManager.interactableStates[i]) 
                {
                    if (t.gameObject.GetComponent<SwitchController>())
                    {
                        t.gameObject.GetComponent<SwitchController>().ChangeState();
                    }
                    else if (t.gameObject.GetComponent<KeyScannerController>())
                    {
                        t.gameObject.GetComponent<KeyScannerController>().ChangeState();
                    }
                }
                i++;
            }

            Debug.Log("Moved player based on checkpoint data: " + checkpointManager.savedPosition);
        }
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        ///////////////////////////////////////////////////////////
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //RespawnPlayer();
        //Time.timeScale = 1f;
    }

    public void RespawnPlayer()
    {
        // Set the player's position to the last activated checkpoint position.
        respawnPosition.z = 0f;
        player.transform.position = respawnPosition;

        //Rescale the player back to normal
        player.transform.localScale = new Vector3(1, 1, 1);

        //Reset player colour alpha
        Color tmp1 = player.GetComponent<SpriteRenderer>().color;

        tmp1.r = 1f;
        tmp1.g = 1f;
        tmp1.b = 1f;
        tmp1.a = 1f;

        player.GetComponent<SpriteRenderer>().color = tmp1;

        //Reset player variables
        player.GetComponent<PlayerController>().crushA = false;
        player.GetComponent<PlayerController>().crushB = false;

        player.GetComponent<PlayerController>().cc1.enabled = true;
        player.GetComponent<PlayerController>().cc2.enabled = true;

        player.GetComponent<PlayerController>().alive = true;

        player.GetComponent<PlayerController>().abilityActiveTimer = 0f;
        player.GetComponent<PlayerController>().abilityCooldownTimer = 0.1f;

        player.GetComponent<PlayerController>().fillIndi.fillAmount = 1f;

        player.GetComponent<PlayerController>().activeAbility = 0;

        //Reset player items
        //player.GetComponent<InventoryManager>().ReturnItemsFromInventory();

        //Remove death screen
        deathScreen.SetActive(false);

        
    }

    public void SetRespawnPosition(Vector3 position)
    {
        // Set the last activated checkpoint position.
        respawnPosition = position;
    }
}

