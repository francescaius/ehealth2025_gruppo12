using System;
using System.Collections;
using UnityEngine;

public class PIANTO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca;
    [SerializeField] ControllerElementoDiScena Marta;
    [SerializeField] ControllerElementoDiScena tell;
    [SerializeField] ControllerElementoDiScena panchina;

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
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "It’s not a who...It’s a what"
        );

        yield return Luca.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca", 
           "I hear someone crying. Marta usually comes here to read...it could be her. it could be her. "
        );
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return new WaitForSeconds(1);
        yield return Luca.Disappear();
        yield return background.ChangePose("parco3");
        yield return Luca.ChangePose("al telefono");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        
        yield return Luca.Appear();
        yield return tell.Appear();
        yield return panchina.Appear();
        tell.MakeClickable(SceltaSbagliata);
        panchina.MakeClickable(SceltaGiusta);
     
    }

    public void SceltaSbagliata()
    {
        StartCoroutine (Scelta1());
    
    }

    private IEnumerator Scelta1()
    {
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.WrongChoiceDone);
        
        tell.UndoClickable();
        panchina.UndoClickable();

       
        yield return new WaitForSeconds(1);
        yield return panchina.Disappear();
        yield return Luca.ChangePose("primopiano");
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Hi Marta, this day has been really messed up, I was wondering how you were…"
        );
        yield return VisualNovelManager.S.phone.DisplayText(
           "Marta",
           "…Luca.…I know you're nearby, stop for a moment, don’t run only inside your own world. Stop!”"
        );
        
        //alla fine della scelta sbagliata deve ritornare possibile fare la scelta giusta
        yield return Luca.Disappear();
        yield return tell.Disappear();
        yield return panchina.Appear();
        panchina.MakeClickable(SceltaGiusta);
    }

     public void SceltaGiusta() //se clicco il pulsante giusto finisco qui no?
    {
        StartCoroutine(Scelta2()); ////poi vieni mandato alla scelta 2
    }

    private IEnumerator Scelta2()  //quindi qui dentro continui l'ultima parte
    {

        //NON CAMBIARE
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.RightChoiceDone);
        //le due marionette per scegliere non devono più essere cliccabili
        //fino alla fine di questa scena corrispondente alla scelta "sbagliata"
        tell.UndoClickable();
        panchina.UndoClickable();
        Debug.Log("LA panchina sta sparendo...");
        yield return panchina.Disappear();
        Debug.Log("La panchina è sparita");

       
        yield return tell.Disappear();
        yield return Luca.ChangePose("stanco");
        yield return Luca.Appear();
        yield return Marta.Appear();

        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Marta… is that you? What’s going on? Marta?" 
        );

        yield return new WaitForSeconds(1);
        //qui deve andare il continuo della "scelta  giusta"

        yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "Thank you for stopping… I was looking for you. And...I owe you an apology");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "An apology? Why would you owe me anything?");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "Because… I fell into it too.");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "...you too? What do you mean?");
        yield return Marta.ChangePose("preoccupata");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "I had… the same problem.");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "What do you mean by ‘same problem’.And same problem as who?");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "The one you all have. Your brother had it… and now you have it too.");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Today everyone keeps talking about me and my brother as if we’re the same person. We have nothing in common anymore.");
         if (VisualNovelManager.S.ForteDipendenza)
        { yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Every morning I wake up without knowing what I’m doing with my life, nor where I’ll be in a few years… imagine if I could resemble someone like him.");
        }
        yield return Marta.ChangePose("preoccupata");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "You two have more in common than you think… for better or worse");  
    
         if (VisualNovelManager.S.Single)
        {yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "You know… after me I never saw you with anyone. Did you ever try to start something serious?");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Serious? Eh… that’s not for me. After us I tried a few times, but… nothing that lasted.");
       
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "Why did you keep yourself so far from others?");
                  
       
       
        } 


    
    
    
    
    
    
    
    }    
    
    

}


