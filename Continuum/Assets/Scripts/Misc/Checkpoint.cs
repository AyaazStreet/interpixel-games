using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool resetCollectables = false;
    private bool activated = false;

    private CheckpointManager cm;
    private PlayerController pc;
    private InventoryManager im;
    private EquipManager em;
    private GameObject interactables;

    private void Start()
    {
        cm = GameManager.Instance.checkpointManager;
        pc = GameManager.Instance.pc;
        im = GameManager.Instance.im;
        em = GameManager.Instance.em;
        interactables = GameManager.Instance.interactables;
    }

    public IEnumerator ActivateCheckpoint()
    {
        activated = true;

        if (Vector2.Distance(pc.transform.position, new Vector3(transform.position.x, transform.position.y, 0)) > 0.1f)
        {
            pc.DisplayPopup("Checkpoint");
        }

        yield return new WaitForSecondsRealtime(0.1f);

        
        
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

        cm.savedCollectables.Clear();
        if(!resetCollectables)
        {
            foreach (EquipManager.Collectable c in em.collected)
            {
                cm.savedCollectables.Add(c);
            }
        }


        /*foreach (Transform obj in cm.transform)
        {
            Destroy(obj.gameObject);
        }
        cm.saveStateObjects.Clear();*/
        foreach (Transform obj in GameManager.Instance.saveState.transform)
        {
            GameObject epyc = Instantiate(obj.gameObject, cm.transform);
            epyc.name = obj.name;
            cm.saveStateObjects.Add(epyc);
            Destroy(obj.gameObject);
        }

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!activated)
            {
                StartCoroutine(ActivateCheckpoint());
            }
        }
    }
}
