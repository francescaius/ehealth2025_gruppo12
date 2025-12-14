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


    int c = 0;

    void Start()
    {
        c = 0;
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
        VisualNovelManager.S.backtrack("Casa");
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

        yield return Luca.ChangePose("stanco");

        VisualNovelManager.S.playAudio("Vibrazione");
        VisualNovelManager.S.playAudio("Notifica");
        yield return Anonimo.Appear();
        Anonimo.MakeClickable(truth); 
         

       

    }

    private IEnumerator truth()
    {
        Anonimo.UndoClickable();
        Luca.UndoClickable();
        yield return Anonimo.Disappear(); 

        yield return VisualNovelManager.S.phone.DisplayText(
          "Anonymous",
          "I know the truth!");

        yield return new WaitForSeconds(1);

        VisualNovelManager.S.playAudio("Vibrazione");
        VisualNovelManager.S.playAudio("Notifica");
        yield return Anonimo.Appear();
        Anonimo.MakeClickable(truth);
        Luca.MakeClickable(spacca, c>=4);//faccio che si può cliccare dopo 5 volte in modo evidente ma anche prima
        c++;

    }

    private IEnumerator spacca()
    {

        Anonimo.UndoClickable();
        Luca.UndoClickable();
        yield return Anonimo.Disappear();

        yield return Luca.ChangePose("arrabbiato");
        VisualNovelManager.S.pauseBacktrack();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "ENOUGH!!!");
        yield return new WaitForSeconds(0.5f);
        VisualNovelManager.S.playAudio("Drop"); 

        yield return Luca.Disappear();
        yield return background.ChangePose("telefonorotto");
        yield return new WaitForSeconds(2.3f);
        yield return VisualNovelManager.S.ObtainPuzzle(6);
        VisualNovelManager.S.resumeBacktrack();
        yield return Luca.ChangePose("scioccato");
        yield return background.ChangePose("camera");
        yield return Luca.Appear();
         

        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "The last clue I was searching for… wasn’t in a house, nor in a secret place. It was in the last thing I couldn’t let go of. My prison. My refuge. My screen.");



        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Let me look at my puzzle");

        VisualNovelManager.S.ShineBag();


    }

}
