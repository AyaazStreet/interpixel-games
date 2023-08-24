using UnityEngine;

public class UpgradePoint : MonoBehaviour
{
    private bool activated = false;

    public bool unlockA1;
    public bool unlockA2;
    public bool unlockA3;

    public bool unlockThrowUi;

    public bool unlockInventory;

    private PlayerController pc;

    public GameObject timer;
    public GameObject equipment;
    public GameObject inventory;

    private void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void ActivatePoint()
    {
        activated = true;

        if (unlockA1)
        {
            pc.A1_Unlocked = true;

            if(timer.activeSelf == false)
            {
                timer.SetActive(true);
            }
        }

        if (unlockA2)
        {
            pc.A2_Unlocked = true;

            if (timer.activeSelf == false)
            {
                timer.SetActive(true);
            }
        }

        if (unlockA3)
        {
            pc.A3_Unlocked = true;

            if (timer.activeSelf == false)
            {
                timer.SetActive(true);
            }
        }

        if (unlockThrowUi)
        {
            if (equipment != null && equipment.activeSelf == false)
            {
                equipment.SetActive(true);
            }
        }

        if (unlockInventory)
        {
            if (inventory != null && inventory.activeSelf == false)
            {
                inventory.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!activated)
            {
                ActivatePoint();
            }
        }
    }
}