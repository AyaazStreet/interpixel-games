using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool activated = false;

    private CheckpointManager cm;

    private void Start()
    {
        cm = GameManager.Instance.checkpointManager;
    }

    public void ActivateCheckpoint()
    {
        activated = true;
        cm.savedLevel = GameManager.Instance.currLevel;
        cm.savedPosition = new Vector3(transform.position.x, transform.position.y, 0);
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
