using UnityEngine;

// Central narrative system: dialogue, collected memories, illness progression and endings.
public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }

    [Header("Historia")]
    [SerializeField] private int memoriesNeededForTrueEnding = 5;
    public int MemoriesFound { get; private set; }
    public int IllnessStage { get; private set; }
    public bool FoundResearch { get; private set; }
    public bool ChoseCompassion { get; private set; }

    private string speaker = "";
    private string dialogue = "";
    private bool dialogueOpen;
    private string endingTitle = "";
    private string endingText = "";
    private bool endingShown;

    public bool HasSeenTrueEndingRequirements => MemoriesFound >= memoriesNeededForTrueEnding && FoundResearch && ChoseCompassion;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        if (dialogueOpen && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
            dialogueOpen = false;
    }

    public void ShowDialogue(string who, string words)
    {
        if (endingShown) return;
        speaker = who;
        dialogue = words;
        dialogueOpen = true;
    }

    public void AddMemory(string memoryName, string memoryText)
    {
        MemoriesFound++;
        ShowDialogue("RECUERDO: " + memoryName, memoryText + "\n\nRecuerdos encontrados: " + MemoriesFound + "/" + memoriesNeededForTrueEnding);
    }

    public void AdvanceIllness()
    {
        IllnessStage = Mathf.Clamp(IllnessStage + 1, 0, 3);
        string[] lines = {
            "Kairo siente que el cansancio aumenta. Aun así, continúa.",
            "Kairo se detiene un momento. «Ah... cómo duele. Tengo que seguir».",
            "Su respiración se vuelve pesada. La liga sigue guardada junto a su pecho.",
            "Apenas puede continuar. «Solo quiero verla una vez más...»"
        };
        ShowDialogue("KAIRO", lines[IllnessStage]);
    }

    public void MarkResearchFound()
    {
        FoundResearch = true;
        ShowDialogue("REGISTRO ANTIGUO", "La Plaga del Vacío nació de un experimento para controlar el Pulso. La verdad puede cambiar el destino de Kairo.");
    }

    public void MarkCompassionChoice()
    {
        ChoseCompassion = true;
    }

    public void ShowEnding(int endingNumber)
    {
        if (endingShown) return;
        if (endingNumber == 6 && !HasSeenTrueEndingRequirements)
        {
            ShowDialogue("RECUERDO INCOMPLETO", "Aún faltan recuerdos por encontrar y secretos por descubrir. El verdadero final sigue oculto.");
            return;
        }

        endingShown = true;
        switch (endingNumber)
        {
            case 1:
                SetEnding("FINAL 1 — YA NO SOMOS LOS MISMOS", "Kairo la rescata. Ella le dice que ya no lo ama y que necesita seguir otro camino. Él comprende que no puede obligarla a quedarse. Se despiden mientras la fortaleza queda atrás.");
                break;
            case 2:
                SetEnding("FINAL 2 — LA CARTA", "Ambos aceptan que ya no pueden estar juntos. Antes de marcharse, ella deja una carta: «Gracias por lo que compartimos. Espero que encuentres paz». Kairo guarda la carta y continúa su camino.");
                break;
            case 3:
                SetEnding("FINAL 3 — SILENCIO EN LA CIUDADELA", "Kairo llega a la fortaleza y descubre que ella ha sido asesinada. Encuentra la liga entre sus pertenencias. No hay palabras que puedan cambiar lo ocurrido. La pantalla se funde lentamente a negro.");
                break;
            case 4:
                SetEnding("FINAL 4 — DEMASIADO TARDE", "Kairo llega cuando ella ya ha comenzado una nueva vida con otra persona. Comprende que el tiempo también cambió sus caminos. La enfermedad lo vence después de abandonar la ciudadela.");
                break;
            case 5:
                SetEnding("FINAL 5 — UN MAÑANA DISTINTO", "Kairo la encuentra y la libera. Juntos detienen el experimento y consiguen una oportunidad para tratar la Plaga. No saben qué les espera, pero deciden empezar de nuevo, sin promesas imposibles.");
                break;
            case 6:
                SetEnding("FINAL VERDADERO — AL FINAL, PUDE ENCONTRARTE", "Kairo consigue rescatarla, pero la enfermedad ha avanzado demasiado. Ella lo sostiene mientras él le entrega la liga. Kairo susurra: «Te amo». Sus últimos momentos transcurren junto a ella. Años después, ella vuelve al lugar de su promesa y deja una flor, la liga y dos anillos.\n\n«Al final, pude encontrarte...»");
                break;
            default:
                endingShown = false;
                return;
        }
    }

    private void SetEnding(string title, string text)
    {
        endingTitle = title;
        endingText = text;
    }

    private void OnGUI()
    {
        if (dialogueOpen)
        {
            GUI.Box(new Rect(20, Screen.height - 150, Screen.width - 40, 125), "");
            GUI.Label(new Rect(40, Screen.height - 140, Screen.width - 80, 25), "<b>" + speaker + "</b>");
            GUI.Label(new Rect(40, Screen.height - 110, Screen.width - 80, 70), dialogue);
            GUI.Label(new Rect(Screen.width - 180, Screen.height - 45, 140, 25), "E / clic para seguir");
        }

        if (endingShown)
        {
            GUI.Box(new Rect(30, 30, Screen.width - 60, Screen.height - 60), "");
            GUI.Label(new Rect(55, 50, Screen.width - 110, 45), "<b>" + endingTitle + "</b>");
            GUI.Label(new Rect(55, 105, Screen.width - 110, Screen.height - 160), endingText);
        }

        GUI.Label(new Rect(15, 15, 280, 25), "Recuerdos: " + MemoriesFound + "/" + memoriesNeededForTrueEnding);
        GUI.Label(new Rect(15, 40, 280, 25), "Plaga del Vacío: " + (IllnessStage + 1) + "/4");
    }
}
