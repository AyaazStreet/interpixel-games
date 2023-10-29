using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    public int savedLevel;
    public Vector3 savedPosition;
    public bool[] unlocks;
    public List<InventoryManager.InventoryItem> savedInventory;
    public List<EquipManager.Collectable> savedCollectables;
    public List<bool> usedStates;
    public List<bool> interactableStates;
    public bool togglePowers;
    public bool skipIntro;
    public int selectedEquip;

    public List<GameObject> saveStateObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savedInventory = new();
            savedCollectables = new();
            usedStates = new();
            interactableStates = new();
            savedLevel = -1;
            unlocks = new bool[6];
            togglePowers = true;
            saveStateObjects = new();
            skipIntro = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetCheckpointData()
    {
        savedInventory = new();
        savedCollectables = new();
        usedStates = new();
        interactableStates = new();
        savedLevel = -1;
        unlocks = new bool[6];
        saveStateObjects = new();
    }

    public void SavePlayerData()
    {
        savedLevel = GameManager.Instance.currLevel;

        unlocks[0] = GameManager.Instance.pc.A1_Unlocked;
        unlocks[1] = GameManager.Instance.pc.A2_Unlocked;
        unlocks[2] = GameManager.Instance.pc.A3_Unlocked;
        unlocks[3] = GameManager.Instance.pc.T1_Unlocked;
        unlocks[4] = GameManager.Instance.pc.T2_Unlocked;
        unlocks[5] = GameManager.Instance.pc.T3_Unlocked;

        savedInventory.Clear();
        usedStates.Clear();
        foreach (InventoryManager.InventoryItem item in GameManager.Instance.im.inventory)
        {
            savedInventory.Add(item);
            usedStates.Add(item.used);
        }

        interactableStates.Clear();
        foreach (Transform t in GameManager.Instance.interactables.transform)
        {
            interactableStates.Add(t.gameObject.GetComponent<Controller>().active);
        }

        savedCollectables.Clear();
        foreach (EquipManager.Collectable c in GameManager.Instance.em.collected)
        {
            savedCollectables.Add(c);
        }

        foreach (Transform obj in GameManager.Instance.saveState.transform)
        {
            GameObject epyc = Instantiate(obj.gameObject, transform);
            epyc.name = obj.name;
            saveStateObjects.Add(epyc);
            Destroy(obj.gameObject);
        }
    }
}
