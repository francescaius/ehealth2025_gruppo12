using System;
using System.Collections;
using UnityEngine;


//questo script va trascinato su SceneController (un oggetto vuoto fuori dal canva) 
public class ATTO5TEMPIO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca; //potrebbe essere luca, marta, etc... chiamare questa variabile
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
     

    //Questa scena parte in automatico
    IEnumerator Part1()
    {

        VisualNovelManager.S.backtrack("Temple");
        yield return VisualNovelManager.S.Element("Overlay").Disappear(); 
    yield return background.Appear();
    yield return VisualNovelManager.S.dialog.DisplayText("Luca","It’s true that the phone makes time pass faster…but sometimes it consumes it entirely.It makes you lose what you really wanted to do.");
    yield return monaco.Appear();
    yield return Luca.Appear();
    yield return VisualNovelManager.S.dialog.DisplayText("monaco","I was waiting for you, Luca.");
    yield return Luca.ChangePose("scioccato");
    yield return VisualNovelManager.S.dialog.DisplayText("Luca","How do you know my name?");
    yield return VisualNovelManager.S.dialog.DisplayText("monaco","In silence, one hears what the world screams but no one notices. And your brother was a gentle scream.");
    yield return VisualNovelManager.S.dialog.DisplayText("Luca","He… was really here?");
    yield return VisualNovelManager.S.dialog.DisplayText("monaco","Yes.He arrived wounded inside, like you. Often we thirst for happiness, and not finding it, we seek shortcuts: vices that give us moments of euphoria… illusions of joy, but in the end they leave us thirstier than before. They become our prison. Don’t you feel it controlling you, Luca? Don’t you feel you're never truly happy? Do you feel that wound?");
    yield return Luca.ChangePose("disperato");
    yield return VisualNovelManager.S.dialog.DisplayText("Luca","Actually… yes. But it’s so hard…no one understands me.There’s no solution!");
    yield return VisualNovelManager.S.dialog.DisplayText("monaco","There exists a water that truly quenches:a life made of choices that set you free. Your brother was like you when he arrived here… but when he left, he was finally free. He prayed for you. He wanted to talk to you himself…to explain which choices to make.");
    yield return Luca.ChangePose("triste");
    yield return VisualNovelManager.S.dialog.DisplayText("Luca","But how? My brother disappeared!");
    yield return VisualNovelManager.S.dialog.DisplayText("monaco","Your brother wanted to explain everything to you…Or perhaps he already is.");
    yield return Luca.ChangePose("incuriosito");
    yield return VisualNovelManager.S.dialog.DisplayText("Luca","What do you mean!? He’s not explaining anything! I’m the one looking for him!");
    yield return VisualNovelManager.S.dialog.DisplayText("monaco","He was sure you would arrive.And he left you this.");
    yield return new WaitForSeconds(1);
         
        yield return VisualNovelManager.S.ObtainPuzzle(5);
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.Finished);

        yield return VisualNovelManager.S.dialog.DisplayText("monaco","Your brother didn’t disappear.He learned how to truly live.");
        yield return VisualNovelManager.S.dialog.DisplayText("monaco","So… you’re saying he’s free. Of course! The place of our freedom was our holiday home! I’ll go there!");
         
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        VisualNovelManager.S.GoToScene("Atto6");


    }

}   