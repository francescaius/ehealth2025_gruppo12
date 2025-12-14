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
        yield return VisualNovelManager.S.phone.DisplayText("Luca","How do I fall for it every single time…");
        yield return VisualNovelManager.S.phone.DisplayText("Luca","I’m heading to the sanctuary.I want to understand what happened to my brother and take a break.The solution is there.");
        yield return VisualNovelManager.S.phone.DisplayText("Anonimo","You want to understand? Or keep fooling yourself? You’ve walked, suﬀered, chased shadows…you don’t deserve more pain.");
        yield return VisualNovelManager.S.phone.DisplayText("Luca","But I might find answers there.");
        yield return VisualNovelManager.S.phone.DisplayText(
        "Anonimo", "Answers? Or sermons?Sanctuaries are for those who have nothing.You already have everything you need… here. Here is your music, the one that calmed you. Here are your photos with your brother. Here are your sweetest messages, your truest memories. Are you really sure you want to abandon the only place where he still exists?"
        );
        yield return VisualNovelManager.S.phone.DisplayText("Luca","I… don’t know…");
        yield return VisualNovelManager.S.phone.DisplayText("Anonimo","Luca, listen to me…Pain isn’t healed by digging. It’s healed by protecting yourself.The phone doesn’t hurt you… it embraces you. It doesn’t judge you. It doesn’t ask sacrifices. It is always with you. Always available. Always ready to understand you without speaking.Stay with me. Sit down. Watch a video. Breathe again. The real world is too loud… but here you can feel safe.");
        yield return VisualNovelManager.S.phone.DisplayText("Anonimo","You weren’t born to suﬀer. You were born to feel good. And I can guarantee that… always.");

        yield return Luca.Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText ("Luca","Maybe… I didn’t need answers. Maybe I just needed… to feel less alone. But this can’t be the solution. No. I must go to the sanctuary.");

        //yield return VisualNovelManager.S.dialog.DisplayText("Luca", "Let's take the train again..."); 

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        VisualNovelManager.S.GoToScene("Atto53");


    }

}

