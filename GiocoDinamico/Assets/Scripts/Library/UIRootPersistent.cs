using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRootPersistent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static string persistentSceneName;

    void Awake()
    {
        persistentSceneName = gameObject.scene.name;
        //DontDestroyOnLoad(gameObject);
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
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(lastScenePath);


            VisualNovelManager.S.GoToScene(sceneName); 
        }
        else
        {
            Debug.LogWarning("Ramo else!");
            VisualNovelManager.S.GoToScene("PulsantiESuoni");
        }
        
        // Pulisci la chiave PlayerPrefs dopo l'uso
        PlayerPrefs.DeleteKey("LastEditedScene");
        PlayerPrefs.Save();
#else
        

        VisualNovelManager.S.GoToScene("PulsantiESuoni");
#endif

    }

   
}
