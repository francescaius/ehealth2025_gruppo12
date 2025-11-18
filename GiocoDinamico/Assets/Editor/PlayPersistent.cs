using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class PlayPersistent
{
    // Il percorso della tua scena di avvio (ad esempio, la tua scena 0 o MainMenu)
    private const string StartupScenePath = "Assets/Scenes/Persistent.unity";

    static PlayPersistent()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // Salva la scena corrente per poterci tornare dopo aver interrotto Play
            string currentScenePath = EditorSceneManager.GetActiveScene().path;
            PlayerPrefs.SetString("LastEditedScene", currentScenePath);
            PlayerPrefs.Save();

            // Imposta la scena di avvio solo se il percorso è valido.
            SceneAsset startupScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(StartupScenePath);
            if (startupScene != null)
            {
                EditorSceneManager.playModeStartScene = startupScene;
            }
            else
            {
                Debug.LogError($"[PlayFromStartScene] Scena di avvio non trovata al percorso: {StartupScenePath}. Controlla il percorso.");
            }
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            // Ricarica l'ultima scena modificata quando si esce dalla modalità Play
            string lastScenePath = PlayerPrefs.GetString("LastEditedScene", "");
            if (!string.IsNullOrEmpty(lastScenePath) && EditorSceneManager.sceneCount == 0)
            {
                EditorSceneManager.OpenScene(lastScenePath);
            }

            // Pulisce l'impostazione della scena di avvio
            EditorSceneManager.playModeStartScene = null;
        }
    }
}