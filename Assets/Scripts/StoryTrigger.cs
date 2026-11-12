using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StoryTrigger : MonoBehaviour
{
    [SerializeField] private string speaker = "KAIRO";
    [TextArea(2, 5)]
    [SerializeField] private string words = "Tengo que seguir...";
    [SerializeField] private bool advancesIllness;
    [SerializeField] private bool revealsResearch;
    [SerializeField] private bool marksCompassionChoice;

    private bool used;

    private void Reset() { GetComponent<Collider2D>().isTrigger = true; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used || !other.CompareTag("Player") || StoryManager.Instance == null) return;
        used = true;
        if (advancesIllness) StoryManager.Instance.AdvanceIllness();
        else if (revealsResearch) StoryManager.Instance.MarkResearchFound();
        else
        {
            if (marksCompassionChoice) StoryManager.Instance.MarkCompassionChoice();
            StoryManager.Instance.ShowDialogue(speaker, words);
        }
    }
}
