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
            togglePowers = false;
            saveStateObjects = new();
            skipIntro = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
