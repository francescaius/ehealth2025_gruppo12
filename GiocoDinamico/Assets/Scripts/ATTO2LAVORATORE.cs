using System;
using System.Collections;
using UnityEngine;


//questo script va trascinato su SceneController (un oggetto vuoto fuori dal canva) 
public class ATTO2LAVORATORE : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca; //potrebbe essere luca, marta, etc... chiamare questa variabile
    [SerializeField] ControllerElementoDiScena Mattia;
    //inserire in questo elenco tutti gli elementi cliccabili o che devono apparire e sparire!
    //poi inserire l'elemento effettivo su unity in questo campo

    //L'elemento marionetta dev'essere una UI>Image vuota che ha dentro tante UI>Image (pose)
    //Su queste marionetta va trascinato lo script ControllerElementoDiScena
    

    void Start()
    {
        if (VisualNovelManager.S == null )
        {
            Debug.LogError("VisualNovelManager non è instanziato nella scena.");
            return;
        }
        else
        {
            //IDEALMENTE IL GIOCO SI POTREBBE EVOLVERE PER CONTROLLARE A CHE PUNTO DELLA SCENA SI FOSSE ARRIVATI PER POTERCI TORNARE
            //IL PROBLEMA È CHE VA FATTO DOPO CHE TUTTE LE SCENE SONO STATE ULTIMATE, E NON È SEMPRE SEMPLICE
            StartCoroutine(Part1());
        }  
    }
     

    //Questa scena parte in automatico
    IEnumerator Part1()
    {

        //////////////////// QUESTA PARLA TENERLA SEMPRE UGUALE ///////////////
        yield return VisualNovelManager.S.Element("Overlay").Disappear(); 
        //////////////////////////////////////////////////////////////////////

        
        yield return background.Appear("uffico esterno");
        yield return new WaitForSeconds(1);
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        
        
        yield return background.ChangePose("ufficio sfocato");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        
        yield return Luca.Appear();
        yield return Mattia.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Mattia",
            "Hey Luca, how’s it going?"
        );
        yield return Luca.ChangePose("guarda Mattia"); 
        yield return VisualNovelManager.S.dialog.DisplayText("Luca",
        "scusa non ti stavo ascoltando");
        
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("scrivania");
        yield return Mattia.Disappear();
        yield return Luca.Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Mattia",
            "xxxxx"
        );

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("ufficio interno");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        
        yield return Luca.ChangePose("triste");
        yield return Luca.Appear();
        yield return Mattia.ChangePose("normale");
        yield return Mattia.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText("Luca",
        "si scusa non ho dormito molto");

        yield return Mattia.ChangePose("ridendo");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia", 
        "stavi pensando a quella proposta");

        yield return Mattia.ChangePose("parlando");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia", "ti ricordi....?");
        yield return Luca.ChangePose ("incuriosito");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca", "in effetti..");

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("graffito telefono");
        yield return Luca.Disappear();
        yield return Mattia.Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","HHh");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","HHH");

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("ufficio sfocato");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();




    


    


        
        
        

     
        



        //////////////////// ESEMPI DI COSE CHE SI POSSONO FARE //////////////////
        //far apparrire gli elementi
        
        
        //entrambi appaiono con l'animazione che è creata dall'editor nominata "Show" 



        //se ci sono dei dialoghi solamente per il caso in cui ci sia forte dipendenza
        //usare il seguente controllo:
    }

}       