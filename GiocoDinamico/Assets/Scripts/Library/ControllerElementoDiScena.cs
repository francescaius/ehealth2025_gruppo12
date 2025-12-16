using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI; 
using System.Linq;

[System.Serializable]
public class PoseEntry
{ 
    public string poseName; 
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


    private bool isShining = false;

    private bool hasBeenInitialized = false;
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


                if(VisualNovelManager.S.EASTEREGG) {var i = entry.poseObject.GetComponent<Image>(); if (i && new[] { "luca", "margherita", "marta", "monaco", "mattia", "aldo",  "lucia"}.Any(s => s.Contains(name, System.StringComparison.OrdinalIgnoreCase))) i.sprite = Resources.Load<Sprite>("ImmaginiNON_USATA/easteregg/easteregg");}
               
            }
            // Disattiva tutte le pose all'inizio
            entry.poseObject.SetActive(false);
        }

        if (!hasBeenInitialized)
        {
            hasBeenInitialized = true;

            if (!activeByDefault)
            {
                SetVisibility(false);
            }
            if (activeByDefault)
            {
                ChangePose(defaultPose);
                SetVisibility(true);
            }
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


    public IEnumerator Animate(string animationName)
    {
        if (!string.IsNullOrEmpty(animationName) && HasAnimation(animationName))
        {
            animator.speed = 1f;
            animator.Play(animationName, 0, 0f);

            yield return null;

            yield return WaitForCurrentClip(); 
            yield break;
        }
        yield break;
    }

    public IEnumerator Disappear(string animationName = null)
    {
        Debug.Log("START Disappearing " + ID);

        // Esegui la parte "pesante" su un altro oggetto
        if (VisualNovelManager.S != null)
        {
            yield return VisualNovelManager.S.StartCoroutine(DisappearInternal(animationName));
        }
        else
        {
            yield return DisappearInternal(animationName);
        }
    }

    public IEnumerator DisappearInternal(string animationName = null)
    {
        Debug.Log("Disappearing " + ID);
        if (animator == null)
        {
            SetVisibility(false);
            yield break;
        } 
        else
        {

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

            else if (HasAnimation("Hide"))
            {
                animator.speed = 1f;
                animator.Play("Hide", 0, 0f);

                yield return null;

                yield return WaitForCurrentClip();
                yield return null;
                SetVisibility(false);
                yield break;
            }
            else if (defaultAnimationInitialized)
            {
                //non gira al contrario, ma basta andare nell'animator, duplicare "show" chiamarlo "hide" e mettere speed -1
                //yield return StartCoroutine(PlayBackwards(defaultAnimationName));
                //yield return WaitForCurrentClip();
                //SetVisibility(false);
                //animator.speed = 1f;  
            }

        }

        yield return null;
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
        Debug.Log("Attivazione: " + isVisible);
        gameObject.SetActive(isVisible);

        Debug.Log("Fine cambio");

        if (isVisible)
        {
            Debug.Log("Provo CAMBIO POSA: "+ID);
            if(currentPoseObject)
            {


                Debug.Log("NAME: "+currentPoseName); 
                ChangePose(currentPoseName);//potrebbe essere ridondante ma si sa mai...
            }
            else
            {
                Debug.Log("DEFAULT: "+ defaultPose);
                ChangePose(defaultPose);
            }
        } 

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

    public IEnumerator Shine()
    {

        isShining = true;

        // Prova a capire che tipo di "render" ha l'oggetto
        var spriteRenderer = currentPoseObject.GetComponent<SpriteRenderer>();
        var uiImage = currentPoseObject.GetComponent<Image>();
        var meshRenderer = currentPoseObject.GetComponent<MeshRenderer>();
        var tmpText = currentPoseObject.GetComponent<TMP_Text>();

        Color originalColor = new Color();

        // Prendi il colore originale
        if (spriteRenderer) originalColor = spriteRenderer.color;
        else if (uiImage) originalColor = uiImage.color;
        else if (meshRenderer) originalColor = meshRenderer.material.color;
        else if (tmpText) originalColor = tmpText.color;

         
        float t = 0f; 

        // Parametri per un effetto più morbido
        float targetHue = 0f;
        float currentHue = 0f;

        while (t < 1f && isShining)
        {
            // Muove lentamente il target hue
            targetHue = Mathf.Repeat(Time.time * 0.2f, 1f);

            // Transizione morbida dell'hue
            currentHue = Mathf.Lerp(currentHue, targetHue, Time.deltaTime * 2f);

            // Saturazione bassa => colori più morbidi
            float saturation = 0.3f;   // 0.2–0.5 = pastello
            float brightness = 1f;

            Color rainbow = Color.HSVToRGB(currentHue, saturation, brightness);


            if (spriteRenderer) spriteRenderer.color = rainbow;
            else if (uiImage) uiImage.color = rainbow;
            else if (meshRenderer) meshRenderer.material.color = rainbow;
            else if (tmpText) tmpText.color = rainbow;

            t += Time.deltaTime;
            yield return null;
        }

        if (spriteRenderer) spriteRenderer.color = originalColor;
        else if (uiImage) uiImage.color = originalColor;
        else if (meshRenderer) meshRenderer.material.color = originalColor;
        else if (tmpText) tmpText.color = originalColor;
    }

    public bool IsClickable
    {
        get
        {
            return onClickAction != null;
        }
    }
    // Per funzioni
    public void MakeClickable(Action a, bool shine = true)
    {
        onClickAction = a;
        if(currentPoseObject != null && shine)
            VisualNovelManager.S.StartCoroutine(Shine());
    }

    // Per coroutine 
    public void MakeClickable(Func<IEnumerator> a, bool shine = true)
    {
        onClickAction = a;
        if (currentPoseObject != null && shine)
            VisualNovelManager.S.StartCoroutine(Shine());
    }
     
     
    public void UndoClickable()
    {
        onClickAction = null;
        isShining = false; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClickAction != null)
        {
            // Se ci sono callback configurate
            if(onClickAction is Func<IEnumerator> coroutine)
            {
                VisualNovelManager.S.StartCoroutine(coroutine());
            }
            else
            {
                onClickAction.DynamicInvoke();
            } 
        }
        else
        {
            Debug.LogWarning($"ATTENZIONE: Nessuna funzione è stata assegnata a onClickAction per {ID}.");
        }
    }
}
