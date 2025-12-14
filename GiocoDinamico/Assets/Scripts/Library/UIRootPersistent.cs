using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Meccanismo sviluppato con Gemini e ChatGpt per gestire l'inizializzazione della scena comune
public class UIRootPersistent : MonoBehaviour
{
    public static string persistentSceneName;

    void Awake()
    {
        persistentSceneName = gameObject.scene.name; 
    }

    void Start()
    {
        if (VisualNovelManager.S == null)
        {
            Debug.LogError("VisualNovelManager non è instanziato nella scena.");
            return;
        }

#if UNITY_EDITOR 
        string lastScenePath = PlayerPrefs.GetString("LastEditedScene", "");

        if (!string.IsNullOrEmpty(lastScenePath))
        {
            string targetSceneName = System.IO.Path.GetFileNameWithoutExtension(lastScenePath);
             
            if (targetSceneName == gameObject.scene.name)
            {
                Debug.Log("Avvio diretto da Persistent rilevato: Carico PulsantiESuoni.");
                VisualNovelManager.S.GoToScene("MenuScene");
            }
            else
            {
                // Altrimenti, carica la scena che stavi modificando (es. Cucina)
                Debug.Log($"Ritorno alla scena di lavoro: {targetSceneName}");
                VisualNovelManager.S.GoToScene(targetSceneName);
            }
        }
        else
        { 
            Debug.LogWarning("Nessuna scena salvata trovata, avvio default.");
            VisualNovelManager.S.GoToScene("MenuScene");
        }
         
        PlayerPrefs.DeleteKey("LastEditedScene");
        PlayerPrefs.Save();

#else
        // --- LOGICA BUILD ---
        // Nella build il codice sopra non esiste, parte sempre da qui.
        VisualNovelManager.S.GoToScene("PulsantiESuoni");
#endif
    }
}