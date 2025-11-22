using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

[System.Serializable] // OBBLIGATORIO: Dice a Unity di includere questa classe nel sistema di serializzazione
public class PoseEntry
{
    // La chiave (il nome della posa che userai nel codice)
    public string poseName;

    // Il valore (il GameObject/SpriteRenderer che rappresenta la posa)
    public GameObject poseObject;

    public PoseEntry(string poseName, GameObject poseObject)
    {
        this.poseName = poseName;
        this.poseObject = poseObject;
    }
}

public class ControllerElementoDiScena : MonoBehaviour, IPointerClickHandler
{
    [Header("Dettagli elemento di scena")]
    public string ID; //per recuperare l'oggetto
    [SerializeField] bool activeByDefault = false; //se l'oggetto non è attivo ma activebydefault è true l'oggetto si attiva in automatico solo se controllato da VisualNovelManager
     
    [SerializeField] private List<PoseEntry> poseList = new List<PoseEntry>();

    private Delegate onClickAction;  // Importante



    private Animator animator;

    private Dictionary<string, GameObject> poses = new Dictionary<string, GameObject>();

    private GameObject currentPoseObject;
    private string currentPoseName;

    private string defaultPose = "";

    private string defaultAnimationName;
    private bool defaultAnimationInitialized = false;


    public void Awake()
    {
        animator = GetComponent<Animator>();
        poses.Clear();
        if(poseList.Count == 0)
        {
            poseList.Add(new PoseEntry("Default", gameObject));
        }
        foreach (var entry in poseList)
        {
            if (defaultPose == "")// voglio che la prima posa nella lista sia quella di default
            {
                defaultPose = entry.poseName;
            }

            if (!poses.ContainsKey(entry.poseName))
            {
                poses.Add(entry.poseName, entry.poseObject);
            }
            // Disattiva tutte le pose all'inizio
            entry.poseObject.SetActive(false);
        }


        if(!activeByDefault)
        { 
            SetVisibility(false);
        }
        if (activeByDefault)
        {
            ChangePose(defaultPose); 
            SetVisibility(true);
            Debug.LogWarning("DefaultPose for ID " + ID + " is true");
        }
    }

    public void Start()
    {
    }


    private void EnsureDefaultAnimation()
    {
        if (defaultAnimationInitialized || animator == null) return;

        var clips = animator.runtimeAnimatorController.animationClips;
        if (clips.Length > 0)
        {
            defaultAnimationName = clips[0].name;
            defaultAnimationInitialized = true;
        }
       
    }
 
    private bool HasAnimation(string animationName)
    {
        if (animator == null || string.IsNullOrEmpty(animationName))
            return false;

        int hash = Animator.StringToHash(animationName);
        if (animator.HasState(0, hash)) //prima guarda lo stato
        {
            return true;
        }

        //se non ha lo stato guarda la clipname
        if (animator.runtimeAnimatorController == null || string.IsNullOrEmpty(animationName))
            return false;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {

                return true;
            }
        }
        return false;
    }
     
    private YieldInstruction WaitForCurrentClip()
    {
        if (animator == null)
            return new WaitForSeconds(0);


        float duration = 0f;
        var clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length > 0 && clipInfo[0].clip != null)
        {
            duration = clipInfo[0].clip.length;
        }

        if (duration > 0f)
        {
            return new WaitForSeconds(duration);
        }

        return new WaitForSeconds(0);
    }


    

    public IEnumerator Disappear(string animationName = null)
    {
        if (animator == null)
        {
            SetVisibility(false);
            yield break;
        }
           

        EnsureDefaultAnimation();
         
        if (!string.IsNullOrEmpty(animationName) && HasAnimation(animationName))
        {
            animator.speed = 1f;
            animator.Play(animationName, 0, 0f);

            yield return null;

            yield return WaitForCurrentClip(); 
            SetVisibility(false);
            yield break;
        }
         
        if (HasAnimation("Hide"))
        {
            animator.speed = 1f;
            animator.Play("Hide", 0, 0f);

            yield return null;

            yield return WaitForCurrentClip(); 
            SetVisibility(false);
            yield break;
        }
         
        if (defaultAnimationInitialized)
        { 
            //non gira al contrario, ma basta andare nell'animator, duplicare "show" chiamarlo "hide" e mettere speed -1
            //yield return StartCoroutine(PlayBackwards(defaultAnimationName));
            //yield return WaitForCurrentClip();
            //SetVisibility(false);
            //animator.speed = 1f;  
        }
        SetVisibility(false);
        yield break;
    }

    public IEnumerator Appear(string animationName = null)
    {

        if (currentPoseObject == null)
        {
            ChangePose(defaultPose);
        }
        SetVisibility(true); 

        if (animator == null)
        {
            yield return new WaitForSeconds(0);
            yield break;
        }

        EnsureDefaultAnimation();
        animator.speed = 1f; 

        if (!string.IsNullOrEmpty(animationName) && HasAnimation(animationName))
        {
            animator.Play(animationName, 0, 0f);

            yield return null;

            yield return WaitForCurrentClip();
            yield break;
        }
        if (HasAnimation("Show"))
        {
            animator.Play("Show", 0, 0f);

            yield return null;

            yield return WaitForCurrentClip();
            yield break;
        }

        if (defaultAnimationInitialized)
        {
            animator.Play(defaultAnimationName, 0, 0f);

            yield return null;

            yield return  WaitForCurrentClip();
            yield break;
        }

        yield break;
    } 

    private void SetVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    // --- FUNZIONE PER CAMBIARE POSA ---

    public YieldInstruction ChangePose(string poseName)
    {
        currentPoseName = poseName;
        if (poses.ContainsKey(poseName))
        {
            // 1. Nascondi la posa precedente, se esiste.
            if (currentPoseObject != null)
            {
                currentPoseObject.SetActive(false);
            }

            // 2. Attiva la nuova posa e aggiorna il riferimento.
            currentPoseObject = poses[poseName];
            currentPoseObject.SetActive(true);

            Debug.Log($"{ID} ha cambiato posa in: {poseName}");
        }
        else
        {
            Debug.LogError($"Posa '{poseName}' non trovata per {ID}.");
        }
        return new WaitForSeconds(0);
    }

    // Versione 1: Per funzioni normali (void)
    public void MakeClickable(Action a)
    {

        onClickAction = a;
    }

    // Versione 2: Per Coroutine
    public void MakeClickable(Func<IEnumerator> a)
    {
        onClickAction = a;
    }
     

    //questo bisognerebbe usarlo per gli oggetti globali a cui sono stati aggiunti listener di scene non globali (roba strana però)
    public void UndoClickable()
    {
        onClickAction = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClickAction != null)
        {
            // Se ci sono callback configurate
            if(onClickAction is Func<IEnumerator> coroutine)
            { 
                StartCoroutine(coroutine());
            }
            else
            {
                onClickAction.DynamicInvoke();
            }
            Debug.Log("Callback onClickAction.Invoke() CHIAMATA."); 
        }
        else
        {
            Debug.LogWarning($"ATTENZIONE: Nessuna funzione è stata assegnata a onClickAction per {ID}.");
        }
    }
}
