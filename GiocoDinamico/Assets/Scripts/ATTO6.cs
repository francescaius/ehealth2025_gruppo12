using System;
using System.Collections;
using UnityEngine;


//questo script va trascinato su SceneController (un oggetto vuoto fuori dal canva) 
public class ATTO6 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca; 
    [SerializeField] ControllerElementoDiScena Anonimo; 

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
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return background.Appear();
        yield return Luca.Appear();
        yield return new WaitForSeconds(1);
        yield return Luca.ChangePose("stanco");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "…Monk, please… don’t tell me you tricked me.");

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("camera");  
       
        yield return Luca.ChangePose("scioccato");
        yield return Luca.Appear(); 
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return new WaitForSeconds(2); 
        yield return Luca.ChangePose("arrabbiato");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Great! Perfect! Thanks for making me waste my time! Truly brilliant!");

        ///telefono vibra
        yield return new WaitForSeconds(2);
        yield return Anonimo.Appear();
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "ENOUGH!!!");
        yield return Anonimo.Disappear();

        ///butta il cell a terra
        yield return new WaitForSeconds(2);
        yield return Luca.ChangePose("scioccato");
        yield return VisualNovelManager.S.ObtainPuzzle(6);
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return Luca.Disappear();
        yield return background.ChangePose("sfocato");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
         
        
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "The last clue I was searching for… wasn’t in a house, nor in a secret place. It was in the last thing I couldn’t let go of. My prison. My refuge. My screen.");



        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Let me look at my puzzle");

        VisualNovelManager.S.ShineBag();  



    }
    
}
