using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool activated = false;

    private CheckpointManager cm;
    private PlayerController pc;
    private InventoryManager im;
    private GameObject interactables;

    private void Start()
    {
        cm = GameManager.Instance.checkpointManager;
        pc = GameManager.Instance.pc;
        im = GameManager.Instance.im;
        interactables = GameManager.Instance.interactables;
    }

    public void ActivateCheckpoint()
    {
        activated = true;

        cm.savedLevel = GameManager.Instance.currLevel;

        cm.savedPosition = new Vector3(transform.position.x, transform.position.y, 0);

        cm.unlocks[0] = pc.A1_Unlocked;
        cm.unlocks[1] = pc.A2_Unlocked;
        cm.unlocks[2] = pc.A3_Unlocked;
        cm.unlocks[3] = pc.T1_Unlocked;
        cm.unlocks[4] = pc.T2_Unlocked;
        cm.unlocks[5] = pc.T3_Unlocked;

        cm.savedInventory.Clear();
        cm.usedStates.Clear();
        foreach(InventoryManager.InventoryItem item in im.inventory)
        {
            cm.savedInventory.Add(item);
            cm.usedStates.Add(item.used);
        }

        cm.interactableStates.Clear();
        foreach (Transform t in interactables.transform)
        {
            cm.interactableStates.Add(t.gameObject.GetComponent<Controller>().active);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!activated)
            {
                ActivateCheckpoint();
            }
        }
    }
}
