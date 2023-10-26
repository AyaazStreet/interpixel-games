using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Image[] inventoryDisplay;
    public Sprite[] collectableSprites;
    //public GameObject[] collectablePrefabs;

    public List<InventoryItem> inventory = new();

    // Collectable Index
    // 0 : keycard
    // 1 : 
    // 2 : 

    void Start()
    {
        
    }

    void Update()
    {

    }

    public class InventoryItem 
    {
        public int type;
        public Sprite sprite;
        public Vector3 pickupPosition;
        public bool used;
        
        public InventoryItem (int _type, Sprite _sprite, Vector3 _pickupPosition, bool _used)
        {
            type = _type;
            sprite = _sprite;
            pickupPosition = _pickupPosition;
            used = _used;
        }
    } 

    public void AddInventoryItem(int t, Vector3 pos)
    {
        inventory.Add(new(t, collectableSprites[t], pos, false));
        UpdateInventoryDisplay();
        Debug.Log("Item Collected");
    }

    public void RemoveInventoryItem(int t)
    {
        inventory.Remove(inventory.FirstOrDefault(item => item.type == t));
        UpdateInventoryDisplay();
        Debug.Log("Item Removed");
    }

    public void UseInventoryItem(int t)
    {
        inventory.First(item => item.type == t && item.used == false).used = true;

        UpdateInventoryDisplay();
        Debug.Log("Item Used");
    }

    public void UnuseInventoryItem(int t)
    {
        inventory.First(item => item.type == t && item.used == true).used = false;

        UpdateInventoryDisplay();
        Debug.Log("Item Unused");
    }

    /*public void ReturnItemsFromInventory(List<InventoryItem> items)
    {
        foreach(InventoryItem item in items)
        {
            _ = Instantiate(collectablePrefabs[item.type], item.pickupPosition, Quaternion.identity);
            RemoveInventoryItem(item.type);
        }
        UpdateInventoryDisplay();
        Debug.Log("Inventory Items Returned");
    }*/

    public void UpdateInventoryDisplay()
    {
        List<InventoryItem> unusedInventory = new();
        foreach (InventoryItem item in inventory)
        {
            if (!item.used)
            {
                unusedInventory.Add(item);
                //Debug.Log("Showing: " + item);
            }
        }

        for (int i = 0; i < inventoryDisplay.Length; i++)
        {
            if (i < unusedInventory.Count)
            {
                inventoryDisplay[i].GetComponent<Image>().enabled = true;
                inventoryDisplay[i].sprite = unusedInventory[i].sprite;
            }
            else
            {
                inventoryDisplay[i].GetComponent<Image>().enabled = false;
            }
        }
    }

    public int GetInventorySize()
    {
        List<InventoryItem> unusedInventory = new();
        foreach (InventoryItem item in inventory)
        {
            if (!item.used)
            {
                unusedInventory.Add(item);
            }
        }

        if (unusedInventory.Count > 3)
        {
            return 3;
        }
        else
        {
            return unusedInventory.Count;
        }

        
    }
}
