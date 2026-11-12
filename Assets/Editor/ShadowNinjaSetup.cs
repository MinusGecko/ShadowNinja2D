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
        EditorUtility.DisplayDialog("Shadow Ninja 2D", "¡Escena inicial creada! Pulsa Play para probar el movimiento, salto y ataque.", "Aceptar");
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
            importer.filterMode = FilterMode.Point;
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
