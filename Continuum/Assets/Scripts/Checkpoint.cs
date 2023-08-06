using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool activated = false;

    public void ActivateCheckpoint()
    {
        activated = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!activated)
            {
                ActivateCheckpoint();
                GameManager.Instance.SetRespawnPosition(transform.position);
            }
        }
    }
}
