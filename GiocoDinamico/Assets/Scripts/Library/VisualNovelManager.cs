using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneProgressStep
{
    NotVisited,      // mai entrato
    Start,           // appena entrato 
    WrongChoiceDone, // ha visto la scelta sbagliata
    RightChoiceDone, // ha scelto quella giusta
    Finished         // scena conclusa
}

[System.Serializable]
public class AudioEntry
{
    public string ID;
    public AudioClip audio;
}

public class VisualNovelManager : MonoBehaviour
{

    [Header("Audio")]
    [SerializeField] private AudioSource altoparlanteBGM;
    [SerializeField] private AudioSource altoparlanteSFX;
    [SerializeField] private List<AudioEntry> audioTracks;

    [Header("Riferimenti globali")]
    public List<ControllerElementoDiScena> elementiControllati;
    public DialogueManager dialog;
    public PhoneDialogueManager phone;
    [SerializeField] ControllerElementoDiScena showPuzzleBtn;




    public static VisualNovelManager S { get; private set; } //rappresenta l'istanza del Singleton, 
                                                             //S = Singleton, Self, Screen 


    private Dictionary<string, SceneProgressStep> scenesData = new Dictionary<string, SceneProgressStep>();
    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();


    public bool ForteDipendenza = false;
     




     
      
    void Awake()
    {
        if (S != null && S != this)
        {
            Destroy(gameObject);
            return;
        }
        S = this;

        foreach (AudioEntry a in audioTracks)
        {
            audioDict[a.ID] = a.audio;
        }

        foreach (ControllerElementoDiScena prefab in elementiControllati)
        {
            prefab.Awake(); //forza awake di gestiti (ad esempio overlay che voglio disattivato il resto del tempo)
        }


        showPuzzleBtn.MakeClickable(showPuzzleCallback);

    }


    public ControllerElementoDiScena Element(string ID)
    {
        Debug.Log("Looking for " + ID);
        foreach (ControllerElementoDiScena prefab in elementiControllati)
        {
            if (prefab.ID == ID)
            {  
                return prefab;
            }
        }

        Debug.LogError($"Personaggio con ID '{ID}' non trovato tra i Prefab.");
        return null;
    }

    public void GoToScene(string name)
    { 
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
             
            if (scene.name == UIRootPersistent.persistentSceneName)
            { 
                continue;
            }

            SceneManager.UnloadSceneAsync(scene);
        }
         
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }
     
    public void hideShowPuzzle()
    {
        playAudio("Bag");
        StartCoroutine(showPuzzleBtn.Appear());
        SceneManager.UnloadSceneAsync("Puzzle");
    }

    private void showPuzzleCallback()
    {
        playAudio("Bag");
        StartCoroutine(showPuzzleBtn.Disappear());
        SceneManager.LoadScene("Puzzle", LoadSceneMode.Additive);
    }



    public SceneProgressStep GetSceneData(string sceneId)
    {
        if (!scenesData.ContainsKey(sceneId))
        {
            // se non esiste ancora, creiamo uno stato nuovo
            var data = SceneProgressStep.Start;
            scenesData[sceneId] = data;
        }

        return scenesData[sceneId];
    }

    public void SetSceneData(string sceneId, SceneProgressStep data)
    {
        scenesData[sceneId] = data;
    }

    // opzionale: controllare se una scena ha raggiunto almeno un certo step
    public bool HasSceneReached(string sceneId, SceneProgressStep minStep)
    {
        if (!scenesData.ContainsKey(sceneId)) return false;
        return scenesData[sceneId] >= minStep;
    }

    //serve a mettere una backtrack alla scena
    public void backtrack(string id = null)
    {
        if (audioDict.ContainsKey(id))
        {
            altoparlanteBGM.Stop();
            altoparlanteBGM.loop = true;
            altoparlanteBGM.playOnAwake = false;
            altoparlanteBGM.clip = audioDict[id];
            altoparlanteBGM.time = 0f;
            altoparlanteBGM.Play();
        }
        else
        {
            Debug.LogError($"Audio {id} NON TROVATO: stoppo la musica!");
            altoparlanteBGM.Stop();
        }
    }

    public void pauseBacktrack()
    {
        altoparlanteBGM.Pause();
    }

    public void resumeBacktrack()
    {
        altoparlanteBGM.Play();
    }

    public void playAudio(string id)
    {
        if(audioDict.ContainsKey(id))
        {
            altoparlanteSFX.PlayOneShot(audioDict[id]);
        }
        else
        {
            Debug.LogError($"Audio {id} non trovato e non si pu� fare .Play()!");
        }
    }
}
