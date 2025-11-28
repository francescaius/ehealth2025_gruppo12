using System;
using System.Collections;
using UnityEngine;

public class Atto1scena2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca;
    [SerializeField] ControllerElementoDiScena Marta; 
 
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
            StartCoroutine(Part1()); 
        }  
    }
     
    IEnumerator Part1()
    { 
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        
        yield return new WaitForSeconds(1);
        yield return Luca.Appear();
        yield return Marta.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Da quanto tempo! Cosa posso prepararti?"
        );        
        
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Non ho molta fame... un semplice caffè, grazie"
            );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "anzi fammelo doppio, sono cosi stanco che potrei addormentarmi qui"
            );  
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Come mai sei così stanco?"
        ); 
        yield return Luca.ChangePose("Triste");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "non so troppi pensieri... poi mi sveglio e... boh, è già abbastanza, è sempre tutto cosi uguale e monotono, anche se stamattina..."
            ); 
        yield return Marta.ChangePose("Arrabbiata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Sicuro che oltre ai pensieri tu non abbia passato la notte con gli occh sbarrati di fronte al cellulare?"
        ); 
        yield return Luca.ChangePose("Arrabbiato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Fammi solo questo caffè e basta!"
        );    
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Posso aiutarti? Sembri perso..."
        );  
        yield return Luca.ChangePose("Triste");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Cercavo qualcuno che conosci bene"
        );
        yield return Luca.ChangePose("Preoccupato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Sì, in relatà ero venuto qui per cercare qualcunon che conosci bene"
        );   
        yield return Marta.ChangePose("Preoccupata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Dimmi pure di chi si tratta"
        );        
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Mio fratello, so che veniva spesso qui"
        );  
        yield return Marta.ChangePose("Pensierosa");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Non ho notizie di lui da un po'. Lasciò qualcosa ma non so se dovrei dartelo..."
        );
        yield return Anonimo.Appear();
        yield return Luca.ChangePose("Preoccupato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Di nuovo quella notifica. Stesso tono, stesso mittente. E se fosse lui? E se fosse mio fratello? Apro la notifica o continuo a parlare con Marta?"
        );
        Anonimo.MakeClickable (LeggiNotifica);
        Marta.MakeClickable(Parla);
        
        yield return Marta.ChangePose("Arrabbiata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Mi senti? Oi sto parlando con te!"
        );
        yield return Marta.ChangePose("Preoccupata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Luca?"
        );
    }
    public void LeggiNotifica()
    {
        StartCoroutine (Scelta1());
    
    }
   
    private IEnumerator Scelta1 ()
    {
        Marta.UndoClickable();
        Anonimo.UndoClickable();
        yield return Anonimo.Disappear ();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Anonimo",
            "Non fidarti di nessuno. Esci ora. Ti guiderò io."
        );
        
        yield return Marta.ChangePose("Arrabbiata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Mi stai ascoltando??"
        );
        yield return Luca.ChangePose("Preoccupato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Aspetta... che significa? Chi sei tu?"
            );
        yield return Marta.ChangePose("Preoccupata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Ma cosa dici? Parli con me?"
        );
        yield return Marta.ChangePose("Arrabbiata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "e poi smettila di sbadigliare!"
        );
        
        yield return Luca.ChangePose("Triste");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "No scusa, sto cercando di capire... qualcuno mi sta aiutando..."
        );
        yield return Marta.ChangePose("Arrabbiata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Aiutando? O controllando? Da quando sei entrato non hai mai smesso di guardare quel telefono"
        );
        yield return Luca.ChangePose("Preoccupato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Non capisci… è importante. Forse è lui. Mio fratello."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
        "Marta",
        "Tuo fratello veniva qui per staccare dal mondo, non per affondarci dentro. Ma vedo che siete così diversi"
        );
        yield return Luca.Disappear (); //luca esce dal bar perchè l'utente ha deciso di cliccare la notifica
        // caso in cui l'utente sceglie di cliccare su marta ed ignorare la notifica
        yield return Luca.ChangePose("Rilassato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Aspetta. Forse questo ti farà capire che non mento"

        );
        yield return Marta.ChangePose("Rilassata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Questa foto me la fece vedere tuo fratello e mi disse che, se mai qualcuno me l'avesse mostrata, dovevo dargli questo"
        );
    }
   
    public void Parla()
    {
        StartCoroutine(Scelta2());
    }

    private IEnumerator Scelta2 ()
    {
        Marta.UndoClickable();
        Anonimo.UndoClickable();
        yield return Anonimo.Disappear ();
         // caso in cui l'utente sceglie di cliccare su marta ed ignorare la notifica
        yield return Luca.ChangePose("Rilassato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Aspetta. Forse questo ti farà capire che non mento"

        );
        yield return Marta.ChangePose("Rilassata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Questa foto me la fece vedere tuo fratello e mi disse che, se mai qualcuno me l'avesse mostrata, dovevo dargli questo"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Disse che un giorno avrebbe voluto tornare a casa. Ma non era sicuro che ci sarebbe riuscito"        
            );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Ecco a te il puzzle!"
        );
        
        yield return VisualNovelManager.S.Element("PuzzlePiece").Appear();
        yield return new WaitForSeconds(2);
        yield return VisualNovelManager.S.Element("PuzzlePiece").Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Appear();
    }
}
