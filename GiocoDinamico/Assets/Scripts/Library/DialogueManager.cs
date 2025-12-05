using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;

public class DialogueManager : MonoBehaviour
{ 

    [Header("UI References")] 
    [SerializeField] protected TextMeshProUGUI speakerText;
    [SerializeField] protected TextMeshProUGUI bodyText;
    [SerializeField] protected GameObject goOnButton;
    [SerializeField] protected GameObject divider;

    public int segmentLength = 200;

    bool active = false; //mi dice se il dialog è acceso
    bool editing = false; //mi dice se sto modificando
    int paragraphIndex = -1;
    List<string> paragraphs;
    bool autoContinue = false;

    void Awake()
    { 
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        gameObject.SetActive(false);
        speakerText.text = "";
        bodyText.text = "";
        editing = false;
        active = false;
        autoContinue = false;
        paragraphIndex = -1;
        paragraphs = new List<string>();
    }

    void Update()
    {
        if(active && !autoContinue)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                nextParagraph();
            }

        }
    }

    //mostra un testo di un personaggio e restituisce un evento che termina quando è terminato il dialogo
    //il dialogo può andare a
    public virtual CustomYieldInstruction DisplayText(string speaker, string text, bool autoContinue = false)
    {

        if(!active)
        {
            goOnButton.SetActive(!autoContinue);
            gameObject.SetActive(true);
            active = true;
            this.autoContinue = autoContinue;
            if(divider)
            {
                if(speaker.Length > 0)
                { 
                    divider.SetActive(true);
                }
                else
                { 
                    divider.SetActive(false); 
                }
            }

            if (speakerText)
            {
                speakerText.text = speaker;
            } 
            if (bodyText)
            {
                paragraphs = SplitText(text);
                StartCoroutine(rollText());
            }
        }


        return new WaitUntil(() => active == false); 

    }


    public void nextParagraph()
    {
        if(!editing && active)
        {
            if (paragraphIndex + 1 < paragraphs.Count)
            {
                StartCoroutine(rollText());
            }
            else
            {
                gameObject.SetActive(false);
                speakerText.text = "";
                bodyText.text = "";
                editing = false;
                active = false;
                paragraphIndex = -1;
                paragraphs = new List<string>();
            }
        }

        
    }



    IEnumerator rollText()
    {
        if (!editing && active)
        {
            editing = true;
            paragraphIndex++;
            if(paragraphIndex < paragraphs.Count)
            {
                string text = paragraphs[paragraphIndex];

                bodyText.text = "";

#if UNITY_EDITOR

                bodyText.text = text;
#endif
                foreach (char c in text)
                {
                    bodyText.text += c;
                    yield return new WaitForSeconds((float)1 / 60);
                }

                editing = false;
            }
            if(autoContinue)
            {
                yield return new WaitForSeconds(2);
                nextParagraph();
                
            }
            
        }


    }

    private List<string> SplitText(string text)
    {
        List<string> segments = new List<string>();
        if (string.IsNullOrEmpty(text))
        {
            return segments;
        }

        int currentIndex = 0;
        int totalLength = text.Length;
        bool previousWasTruncated = false;

        while (currentIndex < totalLength)
        {
            int remainingLength = totalLength - currentIndex;
            int length = Math.Min(segmentLength, remainingLength);

            string segment = "";

            // Aggiungi ellipsis all'inizio se il segmento precedente era troncato
            if (previousWasTruncated)
            {
                segment = "...";
                length = Math.Min(segmentLength - 3, remainingLength);
            }

            // Se c'è ancora testo
            if (currentIndex + length < totalLength)
            {
                // Cerca l'ultimo spazio prima della fine del segmento
                int lastSpaceIndex = text.LastIndexOf(' ', currentIndex + length - 1, length);

                if (lastSpaceIndex > currentIndex)
                {
                    // tronca allo spazio
                    length = lastSpaceIndex - currentIndex + 1;
                    segment += text.Substring(currentIndex, length).TrimEnd();
                    currentIndex += length;
                    previousWasTruncated = false;
                }
                else
                {
                    // se non trova spazio, tronca a segmentLength-3 e aggiungi ellipsis
                    int adjustedLength = (previousWasTruncated ? length : segmentLength - 3) - (previousWasTruncated ? 3 : 0);
                    adjustedLength = Math.Min(adjustedLength, remainingLength);
                    segment += text.Substring(currentIndex, adjustedLength) + "...";
                    currentIndex += adjustedLength;
                    previousWasTruncated = true;
                }
            }
            else
            {
                // se è l'ultimo segmento, prendi tutto
                segment += text.Substring(currentIndex, length);
                currentIndex += length;
                previousWasTruncated = false;
            }

            segments.Add(segment);
        }

        return segments;
    }

}
