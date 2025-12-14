using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneProgressStep
{
    NotVisited,      // mai entrato
    Choice,           // appena entrato 
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
    [SerializeField] public ControllerElementoDiScena bagBtn;




    public static VisualNovelManager S { get; private set; } //rappresenta l'istanza del Singleton, 
                                                             //S = Singleton, Self, Screen 


    private Dictionary<string, SceneProgressStep> scenesData = new Dictionary<string, SceneProgressStep>();
    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();


    //la variabile ForteDipendenza serviva per una prima versione del gioco, ora è cambiato
    //ma la si tiene per evitare errori o punti della storia dove serve
    public bool ForteDipendenza = false;
    public bool StipendioBasso = false;
    public bool NonStudiato = false;
    public bool Single = false;
    public bool Insoddisfatto = false;
    public bool Alcool_Azzardo = false;
    public bool Droga_Traumi = false;

    private bool debug = false;
    private bool easteregg = false;

    public List<int> takenPuzzlePieces = new List<int>();


    public bool DEBUG { get => debug; }
    public bool EASTEREGG { get => easteregg; }




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


        bagBtn.MakeClickable(OpenBag);

    }

    void Update()
    {
        if(DEBUG)
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                if (!takenPuzzlePieces.Contains(1))
                    StartCoroutine(ObtainPuzzle(1));
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                if (!takenPuzzlePieces.Contains(2))
                    StartCoroutine(ObtainPuzzle(2));
            }
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                if (!takenPuzzlePieces.Contains(3))
                    StartCoroutine(ObtainPuzzle(3));
            }
            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                if (!takenPuzzlePieces.Contains(4))
                    StartCoroutine(ObtainPuzzle(4));
            }
            if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                if (!takenPuzzlePieces.Contains(5))
                    StartCoroutine(ObtainPuzzle(5));
            }
            if (Input.GetKeyUp(KeyCode.Alpha6))
            {
                if (!takenPuzzlePieces.Contains(6))
                    StartCoroutine(ObtainPuzzle(6));
            }


            if (Input.GetKeyUp(KeyCode.H))
            {
                this.easteregg = !this.easteregg;
            }


        }

#if UNITY_EDITOR  
        if (Input.GetKeyUp(KeyCode.D))
        {
            this.debug = !this.debug;
        }
#endif

    }


    public ControllerElementoDiScena Element(string ID)
    {
        Debug.Log("Looking for " + ID);
        foreach (ControllerElementoDiScena prefab in elementiControllati)
        {
            if (prefab.ID == ID)
            {
                Debug.Log("Trovato " + ID);
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

    public void Restart()
    {
        ForteDipendenza = false;
        StipendioBasso = false;
        NonStudiato = false;
        Single = false;
        Insoddisfatto = false;
        Alcool_Azzardo = false;
        Droga_Traumi = false;
        StartCoroutine(bagBtn.Disappear());
        takenPuzzlePieces.Clear();
       

    }

    public void CloseBag()
    {
        playAudio("Bag");
        StartCoroutine(bagBtn.Appear());
        SceneManager.UnloadSceneAsync("Puzzle");
    }

    private void OpenBag()
    {
        playAudio("Bag");
        StartCoroutine(bagBtn.Disappear());
        SceneManager.LoadScene("Puzzle", LoadSceneMode.Additive);
    }
    public void ShineBag()
    {
        bagBtn.UndoClickable();
        bagBtn.MakeClickable(OpenBag); 
    }
     



    public IEnumerator ObtainPuzzle(int puzzle)
    {
        if (takenPuzzlePieces.Contains(puzzle)) yield break;
        if (puzzle < 1 || puzzle > 6) yield break;


        var puzzleController = S.Element("PuzzlePiece"); 

        yield return puzzleController.ChangePose("Puzzle" + puzzle);
        yield return puzzleController.Appear();

        if (takenPuzzlePieces.Count == 0) //se è il primo puzzle che ottengo
        {
            StartCoroutine(bagBtn.Appear());
        }
        yield return puzzleController.Disappear(); 
        takenPuzzlePieces.Add(puzzle);

    }
    public void ShowBag(bool show = true)
    {
        if(show)
        { 
            StartCoroutine(bagBtn.Appear());
        }
        else
        {
            StartCoroutine(bagBtn.Disappear());
        }
    }


    public SceneProgressStep GetSceneData(string sceneId)
    {
        if (!scenesData.ContainsKey(sceneId))
        {
            // se non esiste ancora, creiamo uno stato nuovo
            var data = SceneProgressStep.NotVisited;
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
