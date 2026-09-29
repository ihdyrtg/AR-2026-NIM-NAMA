using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARCourseSetupWindow : EditorWindow
{
    private const string SceneFolder = "Assets/Scenes";
    private const string MaterialFolder = "Assets/Materials";
    private const string PrefabFolder = "Assets/Prefabs";
    private const string AudioFolder = "Assets/Audio";

    private const string SceneP02 = "Assets/Scenes/Pertemuan02_Diorama.unity";
    private const string SceneP03 = "Assets/Scenes/Pertemuan03_CameraAudio.unity";
    private const string SceneP04 = "Assets/Scenes/Pertemuan04_CSharp_Demo.unity";
    private const string SceneP05 = "Assets/Scenes/Pertemuan05_Interaction.unity";

    private string nim = "NIM";
    private string studentName = "Nama";
    private string companyName = "KelasAR";
    private string productName = "AR Praktikum";
    private bool landscape = true;
    private bool overwriteGeneratedScenes = false;

    [MenuItem("AR Praktikum/P02-P07 Setup")]
    public static void ShowWindow()
    {
        GetWindow<ARCourseSetupWindow>("AR Praktikum Setup");
    }

    void OnGUI()
    {
        GUILayout.Label("AR Praktikum P02-P07", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "Tool ini menambahkan aset praktikum tanpa menghapus project lain. Scene yang sudah ada tidak ditimpa kecuali opsi overwrite diaktifkan.",
            MessageType.Info);

        nim = EditorGUILayout.TextField("NIM", nim);
        studentName = EditorGUILayout.TextField("Nama", studentName);
        companyName = EditorGUILayout.TextField("Company Name", companyName);
        productName = EditorGUILayout.TextField("Product Name", productName);
        landscape = EditorGUILayout.Toggle("Landscape", landscape);
        overwriteGeneratedScenes = EditorGUILayout.Toggle("Overwrite scene generator", overwriteGeneratedScenes);

        GUILayout.Space(8);

        if (GUILayout.Button("1. Generate / Lengkapi P02-P05"))
        {
            GenerateCourseAssets();
        }

        if (GUILayout.Button("2. Konfigurasi Android P06-P07"))
        {
            ConfigureAndroid();
        }

        if (GUILayout.Button("3. Validasi Project"))
        {
            ValidateProject();
        }

        GUILayout.Space(8);

        using (new EditorGUI.DisabledScope(EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android))
        {
            if (GUILayout.Button("4. Build APK"))
            {
                BuildApk(false);
            }

            if (GUILayout.Button("5. Build and Run ke Smartphone"))
            {
                BuildApk(true);
            }
        }

        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorGUILayout.HelpBox(
                "Tombol Build aktif setelah target platform Android benar-benar aktif. Jalankan konfigurasi Android lalu tunggu proses Switch Platform selesai.",
                MessageType.Warning);
        }
    }

    private void GenerateCourseAssets()
    {
        EnsureFolders();
        EnsureTag("Target");
        EnsureTag("Obstacle");

        Material floorMat = GetOrCreateMaterial("Mat_Floor", new Color(0.35f, 0.45f, 0.55f));
        Material wallMat = GetOrCreateMaterial("Mat_Wall", new Color(0.75f, 0.78f, 0.82f));
        Material tableMat = GetOrCreateMaterial("Mat_Table", new Color(0.45f, 0.28f, 0.16f));
        Material playerMat = GetOrCreateMaterial("Mat_Player", new Color(0.20f, 0.45f, 0.90f));
        Material obstacleMat = GetOrCreateMaterial("Mat_Obstacle", new Color(0.85f, 0.28f, 0.25f));
        Material targetMat = GetOrCreateMaterial("Mat_Target", new Color(0.25f, 0.75f, 0.35f));

        GenerateP02(floorMat, wallMat, tableMat);
        GenerateP03();
        GenerateP04Helper();
        GenerateP05(floorMat, playerMat, obstacleMat, targetMat);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog(
            "AR Praktikum",
            "Aset P02-P05 selesai dibuat/dilengkapi. Buka Assets/Scenes dan uji setiap scene melalui Play Mode.",
            "OK");
    }

    private void GenerateP02(Material floorMat, Material wallMat, Material tableMat)
    {
        if (SceneExists(SceneP02) && !overwriteGeneratedScenes)
        {
            Debug.Log("[AR Praktikum] P02 sudah ada, tidak ditimpa: " + SceneP02);
            return;
        }

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        GameObject environment = new GameObject("Environment");
        GameObject props = new GameObject("Props");

        GameObject ground = CreatePrimitive("Ground", PrimitiveType.Plane, new Vector3(0, 0, 0), new Vector3(2, 1, 2), floorMat);
        ground.transform.SetParent(environment.transform, true);

        GameObject wall = CreatePrimitive("Wall_Main", PrimitiveType.Cube, new Vector3(0, 1, 4), new Vector3(4, 2, 0.2f), wallMat);
        wall.transform.SetParent(environment.transform, true);

        GameObject pillar = CreatePrimitive("Pillar_01", PrimitiveType.Cylinder, new Vector3(-3, 1, 2), new Vector3(0.5f, 1f, 0.5f), wallMat);
        pillar.transform.SetParent(environment.transform, true);

        GameObject table = CreatePrimitive("Table_01", PrimitiveType.Cube, new Vector3(0, 0.5f, 0), new Vector3(2, 0.5f, 1), tableMat);
        table.transform.SetParent(props.transform, true);

        GameObject lamp = CreatePrimitive("Lamp_01", PrimitiveType.Sphere, new Vector3(2, 2.5f, -1), Vector3.one * 0.6f, wallMat);
        lamp.transform.SetParent(props.transform, true);

        GameObject character = CreatePrimitive("Character_Test", PrimitiveType.Capsule, new Vector3(-1.5f, 1f, -1.5f), new Vector3(0.6f, 1f, 0.6f), tableMat);
        character.transform.SetParent(props.transform, true);

        GameObject physicsBall = CreatePrimitive("PhysicsBall", PrimitiveType.Sphere, new Vector3(0, 3f, 2f), Vector3.one * 0.6f, tableMat);
        physicsBall.AddComponent<Rigidbody>();
        physicsBall.transform.SetParent(props.transform, true);

        GameObject chairPrefab = GetOrCreateChairPrefab(tableMat);
        if (chairPrefab != null)
        {
            GameObject chair1 = PrefabUtility.InstantiatePrefab(chairPrefab) as GameObject;
            GameObject chair2 = PrefabUtility.InstantiatePrefab(chairPrefab) as GameObject;
            if (chair1 != null)
            {
                chair1.name = "Chair_Prefab_01";
                chair1.transform.position = new Vector3(-2f, 0f, 0f);
                chair1.transform.SetParent(props.transform, true);
            }
            if (chair2 != null)
            {
                chair2.name = "Chair_Prefab_02";
                chair2.transform.position = new Vector3(2f, 0f, 0f);
                chair2.transform.SetParent(props.transform, true);
            }
        }

        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.transform.position = new Vector3(0f, 7f, -12f);
            LookAt(camera.transform, new Vector3(0f, 1f, 1f));
        }

        Light light = UnityEngine.Object.FindFirstObjectByType<Light>();
        if (light != null)
        {
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            light.intensity = 1.1f;
        }

        SaveScene(scene, SceneP02);
    }

    private void GenerateP03()
    {
        if (SceneExists(SceneP03) && !overwriteGeneratedScenes)
        {
            Debug.Log("[AR Praktikum] P03 sudah ada, tidak ditimpa: " + SceneP03);
            return;
        }

        if (!SceneExists(SceneP02))
        {
            Debug.LogError("[AR Praktikum] P02 belum tersedia. Generate P02 terlebih dahulu.");
            return;
        }

        Scene scene = EditorSceneManager.OpenScene(SceneP02, OpenSceneMode.Single);
        EditorSceneManager.SaveScene(scene, SceneP03);

        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.fieldOfView = 60f;
            AudioListener[] listeners = UnityEngine.Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
            foreach (AudioListener listener in listeners)
            {
                if (listener.gameObject != camera.gameObject)
                {
                    UnityEngine.Object.DestroyImmediate(listener);
                }
            }
            if (camera.GetComponent<AudioListener>() == null)
            {
                camera.gameObject.AddComponent<AudioListener>();
            }
        }

        AudioClip ambience = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioFolder + "/ambience_ruangan.wav");
        AudioClip sfx1 = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioFolder + "/sfx_mesin.wav");
        AudioClip sfx2 = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioFolder + "/sfx_lonceng.wav");

        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager == null)
        {
            audioManager = new GameObject("AudioManager");
        }
        AudioSource ambienceSource = GetOrAdd<AudioSource>(audioManager);
        ambienceSource.clip = ambience;
        ambienceSource.loop = true;
        ambienceSource.playOnAwake = true;
        ambienceSource.spatialBlend = 0f;
        ambienceSource.volume = 0.25f;

        Add3DAudio(GameObject.Find("Lamp_01"), sfx1, 0.45f);
        Add3DAudio(GameObject.Find("Pillar_01"), sfx2, 0.45f);

        EditorSceneManager.SaveScene(scene, SceneP03);
    }

    private void GenerateP04Helper()
    {
        if (SceneExists(SceneP04) && !overwriteGeneratedScenes)
        {
            Debug.Log("[AR Praktikum] P04 helper sudah ada, tidak ditimpa: " + SceneP04);
            return;
        }

        if (!SceneExists(SceneP03)) return;

        Scene scene = EditorSceneManager.OpenScene(SceneP03, OpenSceneMode.Single);
        EditorSceneManager.SaveScene(scene, SceneP04);

        GameObject root = GameObject.Find("P04_CSharp_Demo") ?? new GameObject("P04_CSharp_Demo");

        GameObject rotator = CreatePrimitive("CodeDemo_Rotator", PrimitiveType.Cube, new Vector3(-4f, 1f, -3f), Vector3.one, null);
        rotator.transform.SetParent(root.transform, true);
        GetOrAdd<RotateObject>(rotator).speed = 50f;

        GameObject mover = CreatePrimitive("CodeDemo_Mover", PrimitiveType.Sphere, new Vector3(0f, 1f, -3f), Vector3.one, null);
        mover.transform.SetParent(root.transform, true);
        MoveObject moveObject = GetOrAdd<MoveObject>(mover);
        moveObject.speed = 2f;
        moveObject.direction = 1f;

        GameObject pulse = CreatePrimitive("CodeDemo_Pulse", PrimitiveType.Capsule, new Vector3(4f, 1f, -3f), Vector3.one, null);
        pulse.transform.SetParent(root.transform, true);
        GetOrAdd<PulseScale>(pulse);

        EditorSceneManager.SaveScene(scene, SceneP04);
    }

    private void GenerateP05(Material floorMat, Material playerMat, Material obstacleMat, Material targetMat)
    {
        if (SceneExists(SceneP05) && !overwriteGeneratedScenes)
        {
            Debug.Log("[AR Praktikum] P05 sudah ada, tidak ditimpa: " + SceneP05);
            return;
        }

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        GameObject floor = CreatePrimitive("Floor", PrimitiveType.Plane, Vector3.zero, new Vector3(2, 1, 2), floorMat);

        GameObject player = CreatePrimitive("Player", PrimitiveType.Cube, new Vector3(0f, 0.5f, -4f), Vector3.one, playerMat);
        Rigidbody rb = GetOrAdd<Rigidbody>(player);
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        PlayerMovement movement = GetOrAdd<PlayerMovement>(player);
        movement.moveSpeed = 3f;
        PlayerInteraction interaction = GetOrAdd<PlayerInteraction>(player);
        AudioSource playerAudio = GetOrAdd<AudioSource>(player);
        playerAudio.playOnAwake = false;
        playerAudio.spatialBlend = 0f;
        interaction.targetClip = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioFolder + "/sfx_lonceng.wav");
        interaction.obstacleClip = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioFolder + "/sfx_mesin.wav");

        GameObject target = CreatePrimitive("Target", PrimitiveType.Sphere, new Vector3(0f, 0.5f, 5f), Vector3.one, targetMat);
        target.tag = "Target";
        Collider targetCollider = target.GetComponent<Collider>();
        if (targetCollider != null) targetCollider.isTrigger = true;

        GameObject obstaclePrefab = GetOrCreateObstaclePrefab(obstacleMat);
        Vector3[] obstaclePositions =
        {
            new Vector3(-3f, 0.5f, -1f),
            new Vector3(3f, 0.5f, 0f),
            new Vector3(-2f, 0.5f, 2f),
            new Vector3(2f, 0.5f, 3f)
        };

        for (int i = 0; i < obstaclePositions.Length; i++)
        {
            GameObject obstacle = PrefabUtility.InstantiatePrefab(obstaclePrefab) as GameObject;
            if (obstacle == null) continue;
            obstacle.name = "Obstacle_" + (i + 1).ToString("00");
            obstacle.transform.position = obstaclePositions[i];
        }

        GameObject audioManager = new GameObject("AudioManager");
        AudioSource ambienceSource = audioManager.AddComponent<AudioSource>();
        ambienceSource.clip = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioFolder + "/ambience_ruangan.wav");
        ambienceSource.loop = true;
        ambienceSource.playOnAwake = true;
        ambienceSource.spatialBlend = 0f;
        ambienceSource.volume = 0.20f;

        GameObject hud = new GameObject("MobileHUD");
        MobileHUD mobileHud = hud.AddComponent<MobileHUD>();
        mobileHud.showOnDesktop = true;

        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.transform.position = new Vector3(0f, 11f, -9f);
            camera.fieldOfView = 55f;
            LookAt(camera.transform, new Vector3(0f, 0.5f, 1f));
        }

        Light light = UnityEngine.Object.FindFirstObjectByType<Light>();
        if (light != null)
        {
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            light.intensity = 1.1f;
        }

        SaveScene(scene, SceneP05);
    }

    private void ConfigureAndroid()
    {
        EnsureFolders();

        string packageId = BuildPackageId();
        PlayerSettings.companyName = string.IsNullOrWhiteSpace(companyName) ? "KelasAR" : companyName.Trim();
        PlayerSettings.productName = string.IsNullOrWhiteSpace(productName) ? "AR Praktikum" : productName.Trim();
        PlayerSettings.bundleVersion = "1.0";
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, packageId);
        PlayerSettings.defaultInterfaceOrientation = landscape ? UIOrientation.LandscapeLeft : UIOrientation.Portrait;

        try
        {
            PlayerSettings.Android.targetArchitectures |= AndroidArchitecture.ARM64;
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[AR Praktikum] Tidak dapat mengubah ARM64 otomatis: " + ex.Message);
        }

        AddMainSceneToBuild();

        if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android))
        {
            EditorUtility.DisplayDialog(
                "Android Build Support belum siap",
                "Unity Editor ini belum mendukung target Android. Pasang Android Build Support, Android SDK & NDK Tools, serta OpenJDK melalui Unity Hub pada versi Editor yang sedang dipakai.",
                "OK");
            return;
        }

        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Android, BuildTarget.Android);
            EditorUtility.DisplayDialog(
                "Switch Platform dimulai",
                "Unity sedang mengubah target ke Android. Tunggu reimport selesai, lalu buka kembali AR Praktikum > P02-P07 Setup untuk validasi/build.",
                "OK");
        }
        else
        {
            EditorUtility.DisplayDialog(
                "Konfigurasi Android",
                "Product identity, orientasi, scene build, dan ARM64 sudah dikonfigurasi. Minimum API dan Target API tidak diubah karena nilainya harus mengikuti standar kelas/SDK yang terpasang.",
                "OK");
        }
    }

    private void AddMainSceneToBuild()
    {
        if (!SceneExists(SceneP05)) return;

        EditorBuildSettingsScene[] current = EditorBuildSettings.scenes;
        bool exists = current.Any(s => string.Equals(s.path, SceneP05, StringComparison.OrdinalIgnoreCase));

        if (!exists)
        {
            EditorBuildSettings.scenes = current
                .Concat(new[] { new EditorBuildSettingsScene(SceneP05, true) })
                .ToArray();
        }
        else
        {
            EditorBuildSettings.scenes = current
                .Select(s => string.Equals(s.path, SceneP05, StringComparison.OrdinalIgnoreCase)
                    ? new EditorBuildSettingsScene(s.path, true)
                    : s)
                .ToArray();
        }
    }

    private void BuildApk(bool autoRun)
    {
        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("Unity masih compile", "Tunggu proses compile selesai sebelum build.", "OK");
            return;
        }

        if (!SceneExists(SceneP05))
        {
            EditorUtility.DisplayDialog("Scene belum ada", "Generate P02-P05 terlebih dahulu.", "OK");
            return;
        }

        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUtility.DisplayDialog("Android belum aktif", "Jalankan Konfigurasi Android dan tunggu Switch Platform selesai.", "OK");
            return;
        }

        AddMainSceneToBuild();

        string safeNim = SanitizeFilePart(nim, "NIM");
        string safeName = SanitizeFilePart(studentName, "Nama");
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string buildDir = Path.Combine(projectRoot, "Builds", "Android");
        Directory.CreateDirectory(buildDir);
        string apkPath = Path.Combine(buildDir, "P07_" + safeNim + "_" + safeName + ".apk");

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { SceneP05 },
            locationPathName = apkPath,
            target = BuildTarget.Android,
            options = autoRun ? BuildOptions.AutoRunPlayer : BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            EditorUtility.RevealInFinder(apkPath);
            EditorUtility.DisplayDialog(
                "Build berhasil",
                "APK dibuat:\n" + apkPath + "\n\nUkuran: " + (summary.totalSize / (1024f * 1024f)).ToString("0.00") + " MB",
                "OK");
        }
        else
        {
            EditorUtility.DisplayDialog(
                "Build belum berhasil",
                "Status: " + summary.result + ". Buka Console/Build Report dan baca error paling awal yang spesifik. Ubah satu hal lalu uji kembali.",
                "OK");
        }
    }

    private void ValidateProject()
    {
        StringBuilder sb = new StringBuilder();
        int ok = 0;
        int total = 0;

        Check(ref sb, ref ok, ref total, AssetDatabase.IsValidFolder(SceneFolder), "Assets/Scenes tersedia");
        Check(ref sb, ref ok, ref total, AssetDatabase.IsValidFolder(MaterialFolder), "Assets/Materials tersedia");
        Check(ref sb, ref ok, ref total, AssetDatabase.IsValidFolder(PrefabFolder), "Assets/Prefabs tersedia");
        Check(ref sb, ref ok, ref total, AssetDatabase.IsValidFolder(AudioFolder), "Assets/Audio tersedia");
        Check(ref sb, ref ok, ref total, AssetDatabase.IsValidFolder("Assets/Scripts"), "Assets/Scripts tersedia");

        Check(ref sb, ref ok, ref total, SceneExists(SceneP02), "Pertemuan02_Diorama.unity tersedia");
        Check(ref sb, ref ok, ref total, SceneExists(SceneP03), "Pertemuan03_CameraAudio.unity tersedia");
        Check(ref sb, ref ok, ref total, SceneExists(SceneP05), "Pertemuan05_Interaction.unity tersedia");

        Check(ref sb, ref ok, ref total, AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Scripts/RotateObject.cs") != null, "RotateObject.cs tersedia");
        Check(ref sb, ref ok, ref total, AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Scripts/MoveObject.cs") != null, "MoveObject.cs tersedia");
        Check(ref sb, ref ok, ref total, AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Scripts/PulseScale.cs") != null, "PulseScale.cs tersedia");
        Check(ref sb, ref ok, ref total, AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Scripts/PlayerMovement.cs") != null, "PlayerMovement.cs tersedia");
        Check(ref sb, ref ok, ref total, AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Scripts/PlayerInteraction.cs") != null, "PlayerInteraction.cs tersedia");

        bool p05InBuild = EditorBuildSettings.scenes.Any(s => s.enabled && s.path == SceneP05);
        Check(ref sb, ref ok, ref total, p05InBuild, "Scene P05 aktif di daftar build");
        Check(ref sb, ref ok, ref total, EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android, "Target platform Android aktif");

        string header = "Validasi AR Praktikum: " + ok + "/" + total + " pemeriksaan lulus.\n\n";
        Debug.Log("[AR Praktikum]\n" + header + sb);
        EditorUtility.DisplayDialog("Validasi Project", header + sb, "OK");
    }

    private static void Check(ref StringBuilder sb, ref int ok, ref int total, bool condition, string label)
    {
        total++;
        if (condition) ok++;
        sb.AppendLine((condition ? "[OK] " : "[BELUM] ") + label);
    }

    private static void EnsureFolders()
    {
        EnsureFolder(SceneFolder);
        EnsureFolder(MaterialFolder);
        EnsureFolder(PrefabFolder);
        EnsureFolder(AudioFolder);
        EnsureFolder("Assets/Scripts");
    }

    private static void EnsureFolder(string fullPath)
    {
        if (AssetDatabase.IsValidFolder(fullPath)) return;

        string[] parts = fullPath.Split('/');
        string current = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }
            current = next;
        }
    }

    private static Material GetOrCreateMaterial(string name, Color color)
    {
        string path = MaterialFolder + "/" + name + ".mat";
        Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null) return existing;

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) shader = Shader.Find("Unlit/Color");

        Material material = new Material(shader);
        material.name = name;
        material.color = color;
        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    private static GameObject CreatePrimitive(string name, PrimitiveType type, Vector3 position, Vector3 scale, Material material)
    {
        GameObject go = GameObject.CreatePrimitive(type);
        go.name = name;
        go.transform.position = position;
        go.transform.localScale = scale;

        if (material != null)
        {
            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null) renderer.sharedMaterial = material;
        }

        return go;
    }

    private static T GetOrAdd<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        return component != null ? component : go.AddComponent<T>();
    }

    private static void Add3DAudio(GameObject go, AudioClip clip, float volume)
    {
        if (go == null || clip == null) return;

        AudioSource source = GetOrAdd<AudioSource>(go);
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = true;
        source.spatialBlend = 1f;
        source.minDistance = 1f;
        source.maxDistance = 10f;
        source.volume = volume;
    }

    private static GameObject GetOrCreateChairPrefab(Material material)
    {
        string path = PrefabFolder + "/Chair_Prefab.prefab";
        GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (existing != null) return existing;

        GameObject root = new GameObject("Chair_Prefab");
        GameObject seat = CreatePrimitive("Seat", PrimitiveType.Cube, new Vector3(0f, 0.55f, 0f), new Vector3(1f, 0.2f, 1f), material);
        seat.transform.SetParent(root.transform, true);
        GameObject back = CreatePrimitive("Back", PrimitiveType.Cube, new Vector3(0f, 1.25f, 0.45f), new Vector3(1f, 1.2f, 0.2f), material);
        back.transform.SetParent(root.transform, true);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
        UnityEngine.Object.DestroyImmediate(root);
        return prefab;
    }

    private static GameObject GetOrCreateObstaclePrefab(Material material)
    {
        string path = PrefabFolder + "/Obstacle.prefab";
        GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (existing != null) return existing;

        GameObject root = CreatePrimitive("Obstacle", PrimitiveType.Cube, Vector3.zero, new Vector3(1.4f, 1f, 1.4f), material);
        root.tag = "Obstacle";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
        UnityEngine.Object.DestroyImmediate(root);
        return prefab;
    }

    private static void EnsureTag(string tag)
    {
        if (UnityEditorInternal.InternalEditorUtility.tags.Contains(tag)) return;

        UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if (assets == null || assets.Length == 0)
        {
            Debug.LogWarning("[AR Praktikum] TagManager.asset tidak dapat dibaca. Tambahkan Tag " + tag + " secara manual.");
            return;
        }

        SerializedObject tagManager = new SerializedObject(assets[0]);
        SerializedProperty tags = tagManager.FindProperty("tags");
        int index = tags.arraySize;
        tags.InsertArrayElementAtIndex(index);
        tags.GetArrayElementAtIndex(index).stringValue = tag;
        tagManager.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
    }

    private static bool SceneExists(string path)
    {
        return AssetDatabase.LoadAssetAtPath<SceneAsset>(path) != null;
    }

    private static void SaveScene(Scene scene, string path)
    {
        EditorSceneManager.SaveScene(scene, path);
        Debug.Log("[AR Praktikum] Scene tersimpan: " + path);
    }

    private static void LookAt(Transform transformToRotate, Vector3 target)
    {
        Vector3 direction = target - transformToRotate.position;
        if (direction.sqrMagnitude > 0.0001f)
        {
            transformToRotate.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }
    }

    private string BuildPackageId()
    {
        string safeNim = new string((nim ?? string.Empty).ToLowerInvariant().Where(char.IsLetterOrDigit).ToArray());
        if (string.IsNullOrWhiteSpace(safeNim)) safeNim = "nim";
        if (char.IsDigit(safeNim[0])) safeNim = "n" + safeNim;
        return "com.kelasar.p07." + safeNim;
    }

    private static string SanitizeFilePart(string value, string fallback)
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;

        char[] invalid = Path.GetInvalidFileNameChars();
        string clean = new string(value.Trim().Select(c => invalid.Contains(c) || char.IsWhiteSpace(c) ? '_' : c).ToArray());
        clean = clean.Replace("__", "_");
        return string.IsNullOrWhiteSpace(clean) ? fallback : clean;
    }
}
