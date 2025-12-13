using System;
using System.Collections;
using UnityEngine;


//questo script va trascinato su SceneController (un oggetto vuoto fuori dal canva) 
public class ATTO5CAPOLINEA : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca; //potrebbe essere luca, marta, etc... chiamare questa variabile
    [SerializeField] ControllerElementoDiScena Anonimo;
    [SerializeField] ControllerElementoDiScena monaco;

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


     IEnumerator Part1()
    {
        yield return VisualNovelManager.S.Element("Overlay").Disappear(); 
        yield return background.Appear();
        yield return Luca.ChangePose("disperato");
        yield return Luca.Appear();
        yield return Anonimo.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Great… I missed the stop because I was using my phone. And now I’m at the final station.");

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return Anonimo.Disappear();
        yield return Luca.ChangePose("primo piano");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        
        yield return new WaitForSeconds(1);
        yield return VisualNovelManager.S.phone.DisplayText("Anonimo","Hey… here you are.I can feel you're agitated.Is everything alright?");
        

    }

}

