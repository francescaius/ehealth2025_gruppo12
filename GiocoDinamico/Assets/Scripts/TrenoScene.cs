using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrenoScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena telefonoBtn;
    [SerializeField] ControllerElementoDiScena telefono;
    [SerializeField] Image lineaProgresso;
    [SerializeField] RawImage cartello;



    float secondi = 0f;
    float nuovafermata = 0;
    float frequenzaIncrementoInS = 0.25f;
    float secondiPerFermata = 5f; // ogni fermata ci mette 5 secondi
    float incrementoPerBoost = 5.6f; //

    private RectTransform rectTransform;
    private float maxWidth = 1285f;
    int fermata = 0;
    int totaleFermate = 30;
    int santuario = 25;
    bool boost = false;
    int countaUsoTelefono = 0;
    float baseX = 0;

   

    // Funzione da claude per far cambiare la linea in percentuale
    public void SetProgress(float percentage)
    {

        float widthDelParent = cartello.GetComponent<RectTransform>().rect.width;

        maxWidth = (1285f / 1920f) * widthDelParent;
        baseX = (487.286f / 3081f) * widthDelParent;

        Debug.Log("Base X: " + baseX);


        percentage = Mathf.Clamp(percentage, 0f, 100f); 
        float newWidth = (percentage / 100f) * maxWidth; 
        rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);
        //rectTransform.position = new Vector3(baseX, rectTransform.position.y);
    }

    void Start()
    
    {
        if (VisualNovelManager.S == null )
        {
            Debug.LogError("VisualNovelManager non è instanziato nella scena.");
            return;
        }
        else
        {

            // preso da claude per far spostare la linea da sinistra
            rectTransform = lineaProgresso.GetComponent<RectTransform>();
            baseX = rectTransform.position.x;
            
            //rectTransform.pivot = new Vector2(0, 0.5f); 
            //rectTransform.anchorMin = new Vector2(0, 0.5f);
            //rectTransform.anchorMax = new Vector2(0, 0.5f);

            SetProgress(0);//metto zero fermate
            StartCoroutine(Part1());
        }  
    }


     

    IEnumerator ProgressoFermate()
    {
        VisualNovelManager.S.dialog.DisplayText(
               "- Train -",
               "The train is leaving -  Still " + (santuario - fermata) + " stations before the Sanctuary",
               true
        );
        while(nuovafermata < totaleFermate)
        {
            
            yield return new WaitForSeconds(frequenzaIncrementoInS);
            secondi += frequenzaIncrementoInS; 
            if (boost)
            {
                //se ho il boost ogni 0.25 secondi passano 
                secondi += incrementoPerBoost * frequenzaIncrementoInS;
            }
            nuovafermata =  secondi / secondiPerFermata;
            SetProgress((nuovafermata  / totaleFermate)*100);

            if(((int)nuovafermata) > fermata && !boost) //se ho il boost non guardo le fermate
            {
                fermata = (int)nuovafermata; //cambio fermata


                if ( ((int)nuovafermata) < santuario)
                {
                    yield return VisualNovelManager.S.dialog.DisplayText(
                         "- Train -",
                         "Station " + fermata + "/30   -  " + (santuario - fermata) + " stations before the Sanctuary",
                         true
                    );

                    if (fermata == 1)
                    {

                        yield return VisualNovelManager.S.dialog.DisplayText(
                             "Luca",
                             "Looks like there are lot of stations... maybe I should use my phone to make the time go by faster..."

                        );
                        yield return telefonoBtn.Appear();
                    }

                    if (!telefonoBtn.IsClickable)
                    { 
                        telefonoBtn.MakeClickable(Avanza);
                    } 
                }

                if (fermata == santuario) //se ho il boost non escono informazioni sulla fermata
                {
                    yield return VisualNovelManager.S.dialog.DisplayText(
                         "Luca",
                         "ARRIVED!"
                    );
                    //far partire altra scena
                    yield return VisualNovelManager.S.Element("Overlay").Appear(); 
                    VisualNovelManager.S.GoToScene("Atto53");
                    yield break;
                }
                if (fermata > santuario)
                {
                    break; //ormai ho perso, arrivo alla fine...
                }
            }
            
        }


        yield return VisualNovelManager.S.dialog.DisplayText(
             "Luca",
             "I lost my station!"
        );
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        VisualNovelManager.S.GoToScene("Atto52"); 
        yield break;
    }



    IEnumerator Part1()
    {
        yield return VisualNovelManager.S.Element("Overlay").Disappear(); 
        telefono.Disappear();
        telefonoBtn.Disappear();
        telefono.Appear();
        
        
        yield return VisualNovelManager.S.dialog.DisplayText(
             "Luca",
             "There are 30 station, I have to get of at the temple SANCTUARY - STATION 25. Better not to lost it!"

        );
        StartCoroutine(ProgressoFermate()); 
    }

    IEnumerator Avanza()
    {
        telefonoBtn.UndoClickable(); //il telefono non sarà cliccabile, solo dopo la prima fermata dopo il boost tornerà cliccabile
        countaUsoTelefono = countaUsoTelefono % 4 + 1;
        yield return telefono.ChangePose("Meme" + countaUsoTelefono);
        boost = true;
        yield return telefono.Appear();
        yield return new WaitForSeconds(secondiPerFermata-2);
        yield return telefono.Disappear();
        boost = false;
    }
     
    
   
}
