using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EndingGate : MonoBehaviour
{
    [Range(1, 6)]
    [SerializeField] private int endingNumber = 6;

    private void Reset() { GetComponent<Collider2D>().isTrigger = true; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || StoryManager.Instance == null) return;
        StoryManager.Instance.ShowEnding(endingNumber);
    }
}
