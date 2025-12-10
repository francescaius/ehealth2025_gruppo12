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
    [SerializeField] ControllerElementoDiScena tell;
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

        
        yield return background.Appear("ufficio esterno");
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
        yield return VisualNovelManager.S.dialog.DisplayText("Luca",
        "Please, one second");
        
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("scrivania");
        yield return Mattia.Disappear();
        yield return Luca.Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("ufficio interno");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        yield return Luca.ChangePose("guarda Mattia");
        yield return Luca.Appear();
        yield return Mattia.ChangePose("normale");
        yield return Mattia.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Mattia",
            "Hey, you there? You look like an antenna that’s lost its signal."
        );
        yield return VisualNovelManager.S.dialog.DisplayText("Luca",
        "Yeah… sorry. I didn’t sleep much");
        
        if (VisualNovelManager.S.StipendioBasso)
        { 
        yield return Mattia.ChangePose("ridendo");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia", 
        "You kept thinking about that job oﬀer?");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","I gave up on it. Even if I tried, I’d get nowhere. The results speak for themselves. Years working for a promotion and what’s changed?! Nothing.");
        yield return Mattia.ChangePose ("parlando");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","Come on, why say that?");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","You know better than anyone. We’ve been together since elementary school… and I wasn’t good even back then. I finished high school by miracle, and I didn’t even try university. I already knew I wouldn’t make it.");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","That’s not true.");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Oh yes it is. Look how long it takes me to do something simple… and not even well. Everyone here has degrees, masters, certificates hanging on the wall… and then there’s me. Of course I don’t move forward");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","Luca, you’ve always been the only limit to yourself.");
        }
        else {
        yield return Mattia.ChangePose("ridendo");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia", "You kept thinking about that job oﬀer?");
        yield return Luca.ChangePose("incuriosito");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Yeah, a bit. You know how it is… with my degree, master’s, and that project I carried out last year, I should be the perfect candidate, right? I’ve been chasing results for years… and in the end I did build something");
        yield return Mattia.ChangePose ("parlando");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","Yeah, you never stopped. But you seem destroyed, not satisfied.");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Eﬀort shows you’re growing, right? The late nights, the competition, the courses… everything leads somewhere. In the end… I can say I made it. I did everything I had to.");
        yield return Mattia.ChangePose ("perplesso");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","Yet you don’t seem happy.");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Happiness is overrated. What matters is moving forward, accumulating results, keeping up. In our field if you stop, you’re dead. Luckily I… never stop.");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","Sometimes you seem more tired than fulfilled.");
        yield return Luca.ChangePose ("triste");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","It’s just the price of success, come on. If you work hard and study a lot, you don’t have time for the rest. And that’s fine. Really. That’s how you live at your best… right?");
        }
        
        yield return Mattia.ChangePose("parlando");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia", "Anyway, I was looking for you. They were talking about the closure of that library we used to go to as kids… and about some guy who drew on the wall right before they shut it down. Tons of graﬃti. Strange, huh?");
        yield return Luca.ChangePose ("incuriosito");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca", "Actually...");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","Yeah, some say he’s crazy, others say he’s an artist. I still don’t know. There was even a photo,look.");
        
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("graffito telefono");
        yield return Luca.Disappear();
        yield return Mattia.Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","This. It’s the old library, the one near the station.");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","I don’t know why… it reminds me of my brother. Could be a coincidence. Just a random graﬃti.");

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("ufficio sfocato");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Or maybe… a sign. But what if it’s just my mind seeing what it wants to see?");

        yield return Mattia.Appear();
        yield return tell.Appear();
        
        tell.MakeClickable(Restareneldubbio);
        Mattia.MakeClickable(AscoltareMattia); 
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.Choice);
    }

    private IEnumerator Restareneldubbio()
    {
        //NON CAMBIARE
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.WrongChoiceDone);
        //le due marionette per scegliere non devono più essere cliccabili
        //fino alla fine di questa scena corrispondente alla scelta "sbagliata"
        tell.UndoClickable();
        Mattia.UndoClickable();

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("ufficio interno");
        yield return Luca.ChangePose ("al telefono");
        yield return Luca.Appear();
        yield return tell.Disappear();
        yield return Mattia.ChangePose ("parlando");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();  
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","I… I don’t think going there will help. If there’s anything about my brother, it must be online. A forum, an article… something we haven’t seen yet.");
        
        if (VisualNovelManager.S.ForteDipendenza)
        { yield return VisualNovelManager.S.dialog.DisplayText ("Mattia", "you can’t find every answer on a phone.");
        }
        
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","That graﬃti is real. It’s there, not on a screen.");
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Yeah, but someone must have posted something. Maybe a report… an old post… I just need to… keep searching.");
        
        if (VisualNovelManager.S.ForteDipendenza)
        {
         yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","You could spend hours searching and find nothing. You need to stop.");
         yield return VisualNovelManager.S.dialog.DisplayText ("Luca","What would you know?!");
        }
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","You’re losing yourself, man. I can see it from here. You’re looking everywhere except the right place.");

        if (VisualNovelManager.S.ForteDipendenza)
        {
        yield return VisualNovelManager.S.Element("Overlay").Appear();   
        yield return Luca.Disappear();
        yield return Mattia.Disappear();
        yield return background.ChangePose("Luca primo piano");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Every time I didn’t want to face something, I hid inside the noise of the screen. Every time I didn’t feel enough. I would scroll, search, click… until I convinced myself that sooner or later, a response would appear.But it wasn’t a response. It was only silence disguised as information.");
        }



        yield return new WaitForSeconds(1); 
        ////////////////////////////////////////////////////////////////////
        
        //alla fine della scelta sbagliata deve ritornare possibile fare la scelta giusta
        Mattia.MakeClickable(AscoltareMattia);
    } 

    private IEnumerator AscoltareMattia()
    {

        //NON CAMBIARE
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.RightChoiceDone);
        //le due marionette per scegliere non devono più essere cliccabili
        //fino alla fine di questa scena corrispondente alla scelta "sbagliata"
        tell.UndoClickable();
        Mattia.UndoClickable();

        ///////// QUI INSERIRE COSA SUCCEDE NELLA SCELTA SBAGLIATA /////////

        
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return Luca.ChangePose ("incuriosito");
        yield return Luca.Appear();
        yield return tell.Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Take me there. I want to see that wall.");
        yield return Mattia.ChangePose("perplesso");
        yield return VisualNovelManager.S.dialog.DisplayText("Mattia","Really? You’re usually the first to disappear");

        if (VisualNovelManager.S.ForteDipendenza)
        {
        yield return VisualNovelManager.S.dialog.DisplayText("Mattia","Always glued to that phone for who knows what.");
        }
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","Not today. Today I want to understand");

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("biblioteca browser");
        yield return Mattia.ChangePose ("normale");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText("Mattia","Wait. There it is.");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","Someone is leaving clues. I don’t know why… could it be him?");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","It’s probably a coincidence, but this little monster is Bowser, the same character from the game we spent our Sunday afternoons playing as kids.");
        yield return VisualNovelManager.S.dialog.DisplayText ("Mattia","Who are you talking about?");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","My brother. A few too many coincidences are starting to happen.");
        
        
        
        
        
        
        
        yield return new WaitForSeconds(1);

        //a un certo punto dev'essere dato il PUZZLE!!
        //IMPORTANTE: mettere il numero corrispondente al tassello di puzzle (1-6) 
        yield return VisualNovelManager.S.ObtainPuzzle(4);
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.Finished);


        //alla fine lo sfondo diventa nero
        yield return VisualNovelManager.S.Element("Overlay").Appear(); //NON CAMBIARE
        

    }
    
    
    
    
    
    
    
    
    
    
    
    
}




    


    


        
        
        

     
        



       