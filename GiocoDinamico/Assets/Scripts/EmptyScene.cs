using System;
using System.Collections;
using UnityEngine;


//questo script va trascinato su SceneController (un oggetto vuoto fuori dal canva) 
public class EmptyScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena marionetta1; //potrebbe essere luca, marta, etc... chiamare questa variabile
    [SerializeField] ControllerElementoDiScena marionetta2; //ipotizziamo 2 marionette
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
        




        //////////////////// ESEMPI DI COSE CHE SI POSSONO FARE //////////////////
        //far apparrire gli elementi
        yield return marionetta1.Appear();
        yield return marionetta2.Appear();
        //entrambi appaiono con l'animazione che è creata dall'editor nominata "Show" 



        //se ci sono dei dialoghi solamente per il caso in cui ci sia forte dipendenza
        //usare il seguente controllo:

        if(VisualNovelManager.S.ForteDipendenza) //controllo per parti di scena che abbiano forte dipendenza
        {

        }

        




        //cambiare posa della marionetta personaggio --> il nome dev'essere quello dato nella barra laterale di unity all'oggetto da controllare
        yield return marionetta1.ChangePose("nomeposa");

        //cambiare sfondo con la posa impostata nella barra laterale di unity
        yield return background.ChangePose("altrobackground");

        //mostrare un dialogo parlato
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Questo è una cosa detta a voce..."
        );

        //mostrare un messaggio in cui scrive luca
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",//LUCA DEV'ESSERE SCRITTO IN QUESTO MODO!
           "Questo è un messaggio scritto da luca..."
        );

        //mostrare un messaggio in cui scrive chiunque altro (appare di un colore diverso)
        //È uguale a prima ma il personaggio si chiama diversamente
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Altro nome",
           "Questo è un messaggio scritto da luca..."
        );

        //attendere qualche secondo (in questo caso 1) 
        yield return new WaitForSeconds(1);

        //si possono far sparire e riapparire gli elementi
        yield return marionetta1.Disappear();
        yield return marionetta1.Appear(); // anche più volte!

        ////////////////////////////////////////////////////////////////////////////



        /////////////////////////// SCELTA  ///////////////////////////////////////
        

        //Qui il personaggio deve trovarsi davanti a una scelta, quindi ci devono essere degli elementi cliccabili
        //ad esempio potrebbe essere: clicco la notifica o clicco la persona e ci parlo?
        //potrebbe essere: clicco la porta o clicco il telefono?
        //ovviamente gli oggetti da cliccare devono essere a loro volta marionette nello script e vanno inseriti nella scena!

        //rendere un oggetto cliccabile
        //--> se si clicca con quell'oggetto si va alla scena/funzione nominata tra parentesi
        marionetta1.MakeClickable(SceltaSbagliata);
        marionetta2.MakeClickable(SceltaGiusta); 
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.Choice);
        //L'utente cliccando andrà alla scena corrispondente in base a quanto specificato tra parentesi
    }

     



    //questa scena parte se si clicca sull'elemento che fa fare la scelta sbagliata (telefono, personaggio etc)
    private IEnumerator SceltaSbagliata()
    {
        //NON CAMBIARE
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.WrongChoiceDone);
        //le due marionette per scegliere non devono più essere cliccabili
        //fino alla fine di questa scena corrispondente alla scelta "sbagliata"
        marionetta1.UndoClickable();
        marionetta2.UndoClickable();

        ///////// QUI INSERIRE COSA SUCCEDE NELLA SCELTA SBAGLIATA /////////
        yield return new WaitForSeconds(1); 
        ////////////////////////////////////////////////////////////////////
        
        //alla fine della scelta sbagliata deve ritornare possibile fare la scelta giusta
        marionetta2.MakeClickable(SceltaGiusta);
    }


    //questa scena parte se si clicca sull'elemento che fa fare la scelta sbagliata (telefono, personaggio etc)
    private IEnumerator SceltaGiusta()
    {

        //NON CAMBIARE
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.RightChoiceDone);
        //le due marionette per scegliere non devono più essere cliccabili
        //fino alla fine di questa scena corrispondente alla scelta "sbagliata"
        marionetta1.UndoClickable();
        marionetta2.UndoClickable();

        ///////// QUI INSERIRE COSA SUCCEDE NELLA SCELTA SBAGLIATA /////////

        yield return new WaitForSeconds(1);

        //a un certo punto dev'essere dato il PUZZLE!!
        //IMPORTANTE: mettere il numero corrispondente al tassello di puzzle (1-6) 
        yield return VisualNovelManager.S.ObtainPuzzle(4);
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.Finished);


        ////////////////////////////////////////////////////////////////////


        //alla fine lo sfondo diventa nero
        yield return VisualNovelManager.S.Element("Overlay").Appear(); //NON CAMBIARE
        Conclusione();

    }

    private void Conclusione()
    {

        //infine rimandare alla scena successiva 
        //il nome corrisponde alle scene sotto File>Build Profiles
        //Lì ci devono essere tute le robe
        VisualNovelManager.S.GoToScene("NomeScena");
    }

}
