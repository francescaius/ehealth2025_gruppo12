using System;
using System.Collections;
using UnityEngine;


//questo script va trascinato su SceneController (un oggetto vuoto fuori dal canva) 
public class ATTO7 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca; 
    

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
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "This is where I needed to come… to find myself again.");
        yield return Luca.ChangePose("sorride");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "ALDO! Hey! I’m here! I made it, just like you wanted!");
        yield return Luca.ChangePose("stanco");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "…Brother… please… show yourself…");
        yield return Luca.ChangePose("piange");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "There seems to be no one…");
        yield return new WaitForSeconds(2);
        yield return Luca.ChangePose("scioccato"); 
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Wait… what’s that?");
        yield return VisualNovelManager.S.Element("Overlay").Appear(); 
        yield return Luca.Disappear();
        yield return background.ChangePose("pietra");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Aldo",
           "You came too late. Goodbye.");
        yield return VisualNovelManager.S.Element("Overlay").Appear(); 
        yield return background.ChangePose("sfocato");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Aldo wasn’t kidnapped. He wasn’t trapped. He wasn’t in danger. He… left.");
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return Luca.ChangePose("piange"); 
        yield return Luca.Appear();
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "He chose his own path. Far from everything. Far from me.You’re never coming back… are you?");
        yield return Luca.ChangePose("scioccato");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "But… why did the monk say Aldo would explain everything if he’s not here?");
        yield return Luca.ChangePose("stanco"); 
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "My brother… builds this whole journey…and then doesn’t show up? Aldo… why!? Was this all a joke?"); 
        yield return Luca.ChangePose("scioccato");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "No… that can’t be… let me think… Of course! The monk told me: ‘Maybe Aldo is already speaking to you.’It wasn’t a journey to find my brother. It was a journey to find myself.And… maybe… that’s what he wanted.And in this journey, I truly lived… and understood what no speech could ever make me understand.");
        yield return Luca.ChangePose("sorride");
         yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Thank you, brother.");
           yield return VisualNovelManager.S.Element("Overlay").Appear(); 
       
        




        

    
    }
}
