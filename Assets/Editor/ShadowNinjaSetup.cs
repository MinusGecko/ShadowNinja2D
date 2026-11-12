using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ShadowNinjaSetup
{
    private const string ScenePath = "Assets/Scenes/Level1.unity";

    [MenuItem("Shadow Ninja/Crear escena inicial")]
    public static void CreateInitialScene()
    {
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Art");
        EnsureFolder("Assets/Editor");

        Sprite playerSprite = MakeSprite("Assets/Art/ninja.png", new Color(0.12f, 0.85f, 1f));
        Sprite enemySprite = MakeSprite("Assets/Art/enemy.png", new Color(1f, 0.2f, 0.55f));
        Sprite floorSprite = MakeSprite("Assets/Art/floor.png", new Color(0.08f, 0.12f, 0.22f));
        Sprite coinSprite = MakeSprite("Assets/Art/energy_orb.png", new Color(0.95f, 0.85f, 0.18f));

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Camera
        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        camera.backgroundColor = new Color(0.025f, 0.035f, 0.09f);
        cameraObject.transform.position = new Vector3(0, 1, -10);
        cameraObject.AddComponent<AudioListener>();

        // Game manager
        new GameObject("GameManager").AddComponent<GameManager>();
        new GameObject("StoryManager").AddComponent<StoryManager>();

        // Ground
        GameObject ground = MakeSpriteObject("Suelo", floorSprite, new Vector3(0, -2.1f, 0), new Vector3(22, 1, 1));
        ground.AddComponent<BoxCollider2D>();

        // Platforms
        GameObject platform = MakeSpriteObject("Plataforma", floorSprite, new Vector3(4, 0.2f, 0), new Vector3(4, 0.45f, 1));
        platform.AddComponent<BoxCollider2D>();

        // Player
        GameObject player = MakeSpriteObject("Ninja", playerSprite, new Vector3(-5, -1.25f, 0), new Vector3(0.8f, 1.2f, 1));
        player.tag = "Player";
        Rigidbody2D playerBody = player.AddComponent<Rigidbody2D>();
        playerBody.freezeRotation = true;
        playerBody.gravityScale = 3f;
        player.AddComponent<BoxCollider2D>();
        PlayerController controller = player.AddComponent<PlayerController>();
        player.AddComponent<Health>();
        PlayerCombat combat = player.AddComponent<PlayerCombat>();

        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(player.transform);
        groundCheck.transform.localPosition = new Vector3(0, -0.58f, 0);
        SerializedObject controllerData = new SerializedObject(controller);
        controllerData.FindProperty("groundCheck").objectReferenceValue = groundCheck.transform;
        controllerData.FindProperty("groundLayer").intValue = 1;
        controllerData.ApplyModifiedProperties();

        GameObject attackPoint = new GameObject("AttackPoint");
        attackPoint.transform.SetParent(player.transform);
        attackPoint.transform.localPosition = new Vector3(0.65f, 0, 0);
        SerializedObject combatData = new SerializedObject(combat);
        combatData.FindProperty("attackPoint").objectReferenceValue = attackPoint.transform;
        combatData.FindProperty("enemyLayer").intValue = 1 << 8;
        combatData.ApplyModifiedProperties();

        // Enemy
        GameObject enemy = MakeSpriteObject("Dron enemigo", enemySprite, new Vector3(3, -1.35f, 0), new Vector3(0.9f, 0.9f, 1));
        enemy.layer = 8;
        Rigidbody2D enemyBody = enemy.AddComponent<Rigidbody2D>();
        enemyBody.freezeRotation = true;
        enemyBody.gravityScale = 3f;
        enemy.AddComponent<BoxCollider2D>();
        enemy.AddComponent<Health>();
        enemy.AddComponent<EnemyAI>();

        // Collectible
        GameObject orb = MakeSpriteObject("Orbe de energía", coinSprite, new Vector3(-1, -0.8f, 0), new Vector3(0.45f, 0.45f, 1));
        CircleCollider2D orbCollider = orb.AddComponent<CircleCollider2D>();
        orbCollider.isTrigger = true;
        orb.AddComponent<Collectible>();

        // Story introduction and five memory flashbacks
        CreateStoryTrigger("Primer recuerdo", new Vector3(-3.4f, -1.2f, 0), "KAIRO", "La conocí mientras me preparaba para mi futuro. Primero fuimos amigos; después llegaron las bromas, los pequeños cariños y momentos que nunca pensé que extrañaría.", false, false);
        CreateMemory(new Vector3(-2.0f, -1.1f, 0), "La primera conversación", "En la academia nos conocimos. Al principio solo éramos dos estudiantes que compartían algunas palabras.");
        CreateMemory(new Vector3(0.2f, -1.1f, 0), "La amistad", "Con el tiempo llegaron las bromas, las conversaciones y los pequeños gestos de cariño. Kairo guarda esos días con afecto.");
        CreateMemory(new Vector3(5.8f, -1.1f, 0), "La liga", "Ayla le dio una pequeña liga. Después le pidió que se la devolviera. Kairo la conservó como recuerdo de aquel momento.");
        CreateMemory(new Vector3(7.4f, -1.1f, 0), "La despedida", "«Quiero que terminemos. No eres tú... son cosas mías». Kairo no entendió todo lo que ella sentía, pero sabía que debía respetar su decisión.");
        CreateStoryTrigger("La enfermedad avanza", new Vector3(8.8f, -1.2f, 0), "KAIRO", "Ah... cómo me duele. No puedo detenerme ahora. Todavía tengo que encontrarla.", true, false);
        CreateMemory(new Vector3(10.0f, -1.1f, 0), "La promesa", "Kairo mira la liga y recuerda por qué emprendió el viaje: quería volver a verla y decirle lo que guardaba en su corazón.");

        // Simple HUD
        GameObject canvasObject = new GameObject("HUD");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        GameObject labelObject = new GameObject("Instrucciones");
        labelObject.transform.SetParent(canvasObject.transform, false);
        Text label = labelObject.AddComponent<Text>();
        label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        label.fontSize = 20;
        label.color = Color.white;
        label.alignment = TextAnchor.UpperLeft;
        label.text = "SHADOW NINJA 2D\nA/D o flechas: mover   ESPACIO: saltar\nCLIC IZQUIERDO: atacar   ESC: pausa   R: reiniciar";
        RectTransform rect = label.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(20, -20);
        rect.sizeDelta = new Vector2(700, 90);

        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Shadow Ninja 2D", "¡Escena de plataformas creada! Pulsa Play para probar movimiento, saltos, ataque y recuerdos. Los sprites actuales son marcadores provisionales; reemplázalos por ilustraciones 2D dibujadas para conseguir el estilo visual de referencia.", "Aceptar");
    }

    private static void CreateStoryTrigger(string objectName, Vector3 position, string speaker, string words, bool illness, bool research)
    {
        GameObject trigger = new GameObject(objectName);
        trigger.transform.position = position;
        BoxCollider2D collider = trigger.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(1.5f, 3f);
        StoryTrigger story = trigger.AddComponent<StoryTrigger>();
        SerializedObject data = new SerializedObject(story);
        data.FindProperty("speaker").stringValue = speaker;
        data.FindProperty("words").stringValue = words;
        data.FindProperty("advancesIllness").boolValue = illness;
        data.FindProperty("revealsResearch").boolValue = research;
        data.ApplyModifiedProperties();
    }

    private static void CreateMemory(Vector3 position, string memoryName, string flashback)
    {
        GameObject memory = new GameObject("Recuerdo: la liga");
        memory.transform.position = position;
        SpriteRenderer renderer = memory.AddComponent<SpriteRenderer>();
        renderer.sprite = MakeSprite("Assets/Art/memory_tie.png", new Color(0.95f, 0.45f, 0.7f));
        CircleCollider2D collider = memory.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        MemoryPickup pickup = memory.AddComponent<MemoryPickup>();
        SerializedObject data = new SerializedObject(pickup);
        data.FindProperty("memoryName").stringValue = memoryName;
        data.FindProperty("flashback").stringValue = flashback;
        data.ApplyModifiedProperties();
    }

    private static GameObject MakeSpriteObject(string objectName, Sprite sprite, Vector3 position, Vector3 scale)
    {
        GameObject go = new GameObject(objectName);
        go.transform.position = position;
        go.transform.localScale = scale;
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        return go;
    }

    private static Sprite MakeSprite(string path, Color color)
    {
        if (!File.Exists(path))
        {
            Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[32 * 32];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = color;
            texture.SetPixels(pixels);
            texture.Apply();
            File.WriteAllBytes(path, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(path);
        }

        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
        if (importer != null && importer.textureType != TextureImporterType.Sprite)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.filterMode = FilterMode.Bilinear;
            importer.SaveAndReimport();
        }
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        string parent = Path.GetDirectoryName(path).Replace("\\", "/");
        string folder = Path.GetFileName(path);
        if (!AssetDatabase.IsValidFolder(parent)) EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, folder);
    }
}
