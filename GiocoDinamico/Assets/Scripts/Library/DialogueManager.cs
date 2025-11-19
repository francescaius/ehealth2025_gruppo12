using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;

public class DialogueManager : MonoBehaviour
{ 

    [Header("UI References")] 
    [SerializeField] TextMeshProUGUI speakerText;
    [SerializeField] TextMeshProUGUI bodyText;
    [SerializeField] GameObject goOnButton;

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
    public CustomYieldInstruction DisplayText(string speaker, string text, bool autoContinue = false)
    {

        if(!active)
        {
            goOnButton.SetActive(!autoContinue);
            gameObject.SetActive(true);
            active = true;
            this.autoContinue = autoContinue;

            if (speakerText) speakerText.text = speaker;
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
                foreach (char c in text)
                {
                    bodyText.text += c;
                    yield return new WaitForSeconds((float)1 / 60);
                }

                editing = false;
            }
            if(autoContinue)
            {
                yield return new WaitForSeconds(3);
                nextParagraph();
                
            }
            
        }


    }

    private static List<string> SplitText(string text, int segmentLength = 200)
    {
        List<string> segments = new List<string>();

        if (string.IsNullOrEmpty(text))
        {
            return segments;
        }

        int totalLength = text.Length;

        for (int i = 0; i < totalLength; i += segmentLength)
        {
            int length = Math.Min(segmentLength, totalLength - i);
             
            string segment = text.Substring(i, length);
            segments.Add(segment);
        }

        return segments;
    }
}
