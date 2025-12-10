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

       
        yield return new WaitForSeconds(1);ù
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return panchina.Disappear();
        yield return Luca.ChangePose("primopiano");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
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
        yield return Marta.ChangePose("Preoccupata");
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
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Because it was easier not to try. Every time someone tried to get to know me… I opened my phone. I scrolled. I got lost. It was… safer, talking was easier, there was never awkwardness. It felt like I couldn’t fail.");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",  
           "More than safe, it sounds empty to me.");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "A bit of both. The phone never disappoints you: it keeps you company, distracts you, fills the holes you don’t want to look at. It’s a perfect way not to face anything. Not even loneliness.");
          yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "And meanwhile, you stayed still. As if you were waiting for something… or someone.");        
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Maybe I was waiting for myself. Or maybe… I was waiting for another notification.");

        } else{
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "And your wife? How is she taking all this confusion of yours?");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Not well. She’s always been there… she’s a good person, truly. She deserves more than someone who comes home not even knowing how to explain how his day went.");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "Do you still love her?"); 
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Yes, I… I think I do. It’s just that… it’s like there’s a thin distance between us. Invisible. We don’t argue, we don’t yell…we simply don’t meet anymore."); 
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "Is it my fault? Because of our story?");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "No, Marta. Our story belongs to the past. The truth is… I’m ashamed to say it but I spend more time on my phone than with her. It’s like I’m connected with everyone… except the person beside me.");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "You know what you need to do to fix this.");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Yes. Because she deserves a present husband…and instead I look for answers in a blue light that warms no one.");
        }
        
        ////continuazione dialoghi in comune 
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Maybe I was waiting for myself. Or maybe… I was waiting for another notification. This morning… I received some messages. And it’s so strange…Since you walked into the bar they started telling me not to give you anything. To let it go. To stay away from you."); 
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta", 
           "And for a moment… I believed them. I listened to those notifications… the same way you do every day. And the more I listened, the more lost and confused I felt.");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Marta, I don’t understand… you sound like you’re losing your mind.");

         if (VisualNovelManager.S.ForteDipendenza)
        { yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "Do you remember years ago? I was like you. I had no certainty about my future, nor the right direction… I lived passively, looking for approval in a like, a follow, a message.");
          yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "I don’t do that.");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "You don’t? What’s the first thing you look at when you open your eyes? What do you check the moment you have free time?What makes your breath hitch when you feel you don’t have it within reach?");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "I know… I wish I could detach from this object and from the world I’ve created inside it. It’s just… easy in there. It feels like everything is fine. And there’s always something new, something interesting… In this life I only see monotony");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "Learn to listen to the people who care about you. The ones who can hug you when you need it… maybe the ones who can make a sincere smile appear on your face.");
         }
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "And anyway… I’m not going crazy. I’m just trying to help you.");
        
         yield return Luca.ChangePose("sorride");
         yield return VisualNovelManager.S.dialog.DisplayText( 
         "Luca",
         "Maybe you’re right…I don’t know what makes me think you have something for me.");
          yield return VisualNovelManager.S.dialog.DisplayText(
           "Marta",
           "Not one… but two.Here.");


    
    
    
    
    
    
    
    }    
    
    

}


