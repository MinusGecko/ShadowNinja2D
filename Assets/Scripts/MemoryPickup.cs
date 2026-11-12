using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MemoryPickup : MonoBehaviour
{
    [SerializeField] private string memoryName = "La liga";
    [TextArea(2, 5)]
    [SerializeField] private string flashback = "En la academia, primero fuimos amigos. Con el tiempo compartimos sonrisas y momentos que Kairo guardaría para siempre.";

    private void Reset() { GetComponent<Collider2D>().isTrigger = true; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || StoryManager.Instance == null) return;
        StoryManager.Instance.AddMemory(memoryName, flashback);
        Destroy(gameObject);
    }
}
