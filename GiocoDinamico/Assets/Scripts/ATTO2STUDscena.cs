using System;
using System.Collections;
using UnityEngine;

public class Atto2STUDENTE : MonoBehaviour
{
    
    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca;
    [SerializeField] ControllerElementoDiScena Mattia; 
    [SerializeField] ControllerElementoDiScena tell;



    void Start()
    
    {
        if (VisualNovelManager.S == null )
        {
            Debug.LogError("VisualNovelManager non è instanziato nella scena.");
            return;
        }
        else
        {
            StartCoroutine(Part1()); 
        }  
    }
    IEnumerator Part1()
    {
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        
        yield return new WaitForSeconds(1);
        yield return background.Appear();
        yield return Luca.Appear();
        yield return Mattia.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Mattia",
            "Hey Luca, how’s it going?"
        ); 
        yield return new WaitForSeconds(1);
        
        yield return new WaitForSeconds(1);
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return Luca.Disappear();
        yield return Mattia.Disappear();
         yield return background.ChangePose("aula");
         yield return VisualNovelManager.S.Element("Overlay").Disappear();
       
        yield return Luca.Appear();
        yield return new WaitForSeconds(1);
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return Luca.Disappear();
        yield return background.ChangePose("cellbanco");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
       

        yield return new WaitForSeconds(3);
        yield return VisualNovelManager.S.Element("Overlay").Appear(); 
        yield return background.ChangePose("aula");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
      
        yield return Luca.Appear();
        yield return Mattia.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Mattia",
            "Hey, you there? You look like an antenna that’s lost signal"
        ); 

        yield return Luca.ChangePose("Triste");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Yeah… sorry. I didn’t sleep much."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Mattia",
            "Anyway, I was actually looking for you. I was with the others earlier and they were talking about a library that’s shutting down, and about some guy who drew on the wall right before no one was allowed in anymore. Weird, right?"
        ); 
       
        yield return tell.Appear();

        tell.MakeClickable(SceltaSbagliata);
        Mattia.MakeClickable(SceltaGiusta); 
    }

    private IEnumerator SceltaSbagliata()
    {
        //NON CAMBIARE
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.WrongChoiceDone);
        //le due marionette per scegliere non devono più essere cliccabili
        //fino alla fine di questa scena corrispondente alla scelta "sbagliata"
        tell.UndoClickable();
        Mattia.UndoClickable();

        yield return new WaitForSeconds(1);
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return Luca.ChangePose("primopiano");
        yield return Mattia.Disappear();
        yield return tell.Disappear();
        yield return background.ChangePose("aula");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        yield return new WaitForSeconds(1);
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return Mattia.Appear();
        yield return Luca.Appear();
        yield return background.ChangePose("aula");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
       

        ///////// QUI INSERIRE COSA SUCCEDE NELLA SCELTA SBAGLIATA ///////// 
        ////////////////////////////////////////////////////////////////////
        
        //alla fine della scelta sbagliata deve ritornare possibile fare la scelta giusta
        Mattia.MakeClickable(SceltaGiusta);
        yield break;
    }
private IEnumerator SceltaGiusta()
    {

        //NON CAMBIARE
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.RightChoiceDone);
        //le due marionette per scegliere non devono più essere cliccabili
        //fino alla fine di questa scena corrispondente alla scelta "sbagliata"
        tell.UndoClickable();
        Mattia.UndoClickable();

        ///////// QUI INSERIRE COSA SUCCEDE NELLA SCELTA SBAGLIATA /////////

        yield return new WaitForSeconds(1);
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return tell.Disappear();
        yield return background.ChangePose("biblioteca");
        yield return Luca.ChangePose("camminata");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("graffito");
        yield return Luca.ChangePose("Preoccupato");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        //a un certo punto dev'essere dato il PUZZLE!!
        //IMPORTANTE: mettere il numero corrispondente al tassello di puzzle (1-6) 
        yield return VisualNovelManager.S.ObtainPuzzle(2);
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.Finished);


        ////////////////////////////////////////////////////////////////////


        //alla fine lo sfondo diventa nero
        yield return VisualNovelManager.S.Element("Overlay").Appear(); //NON CAMBIARE
        

    }


}



