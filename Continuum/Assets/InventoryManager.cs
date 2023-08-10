using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Image[] inventoryDisplay;
    public Sprite[] collectableSprites;
    public GameObject[] collectablePrefabs;

    public List<InventoryItem> inventory = new List<InventoryItem>();

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

    public struct InventoryItem 
    {
        public int type;
        public Sprite sprite;
        public Vector3 pickupPosition;
        
        public InventoryItem (int _type, Sprite _sprite, Vector3 _pickupPosition)
        {
            type = _type;
            sprite = _sprite;
            pickupPosition = _pickupPosition;
        }
    } 

    public void AddInventoryItem(int t, Vector3 pos)
    {
        inventory.Add(new(t, collectableSprites[t], pos));
        UpdateInventoryDisplay();
        Debug.Log("Item Collected");
    }

    public void RemoveInventoryItem(int t)
    {
        inventory.Remove(inventory.FirstOrDefault(item => item.type == t));
        UpdateInventoryDisplay();
        Debug.Log("Item Removed");
    }

    public void ReturnItemsFromInventory()
    {
        foreach(InventoryItem item in inventory)
        {
            _ = Instantiate(collectablePrefabs[item.type], item.pickupPosition, Quaternion.identity);
        }
        inventory.Clear();
        UpdateInventoryDisplay();
        Debug.Log("Inventory Cleared");
    }

    public void UpdateInventoryDisplay()
    {
        for (int i = 0; i < inventoryDisplay.Length; i++)
        {
            if (i < inventory.Count)
            {
                inventoryDisplay[i].GetComponent<Image>().enabled = true;
                inventoryDisplay[i].sprite = inventory[i].sprite;
            }
            else
            {
                inventoryDisplay[i].GetComponent<Image>().enabled = false;
            }
        }
    }
}
