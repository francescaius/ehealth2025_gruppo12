using System.Collections;
using UnityEngine;

public class SondaggioScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena mai;
    [SerializeField] ControllerElementoDiScena raramente;
    [SerializeField] ControllerElementoDiScena ognitanto;
    [SerializeField] ControllerElementoDiScena spesso;
    [SerializeField] ControllerElementoDiScena sempre;


    string[] domande = new string[]
    {
        "Trovi di restare online più a lungo di quanto avevi intenzione?",
        //"Trascuri le faccende domestiche per passare più tempo online?",
        //"Preferisci l’eccitazione di Internet all’intimità con il tuo partner?",
        //"Stringi nuove relazioni con altri utenti online?",
        //"Le persone nella tua vita si lamentano del tempo che trascorri online?",
        //"I tuoi voti o il tuo rendimento scolastico peggiorano a causa del tempo che trascorri online?",
        //"Controlli la tua email prima di fare qualcos’altro che dovresti fare?",
        //"Il tuo rendimento o la tua produttività sul lavoro peggiorano a causa di Internet?",
        //"Diventi difensivo o segreto quando qualcuno ti chiede cosa fai online?",
        //"Blocchi pensieri disturbanti sulla tua vita rifugiandoti in pensieri rassicuranti sull’uso di Internet?",
        //"Ti capita di anticipare con impazienza il momento in cui potrai tornare online?",
        //"Temi che la vita senza Internet sarebbe noiosa, vuota o priva di gioia?",
        //"Urli, scatti o ti irriti se qualcuno ti disturba mentre sei online?",
        //"Perdi ore di sonno a causa delle connessioni notturne?",
        //"Ti senti preoccupato per Internet quando sei offline, o fantastichi sull’essere online?",
        //"Ti ritrovi a dire “ancora qualche minuto” quando sei online?",
        //"Cerchi di ridurre il tempo che trascorri online?",
        //"Cerchi di nascondere per quanto tempo sei stato online?",
        //"Scegli di passare più tempo online invece di uscire con altre persone?",
        //"Ti senti depresso, di cattivo umore o nervoso quando sei offline, e il malessere scompare quando torni online?"
    };


    private int IAT = 0;
    private int indice = -1;


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


    private IEnumerator MakeAllClickable()
    {
        yield break;

    }

    private IEnumerator UndoAllUnclickable()
    {
        yield break;
    }
    IEnumerator Part1()
    {

        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        yield return VisualNovelManager.S.dialog.DisplayText(
            "Un breve sondaggio",
            "Prima di iniziare il gioco, ti verrà proposto un sondaggio, clicca sulla risposta per com'è davvero la tua vita e non per come desideri che sia, al click verrà mostrata un'altra domanda."
        );

        mai.MakeClickable(maiClick);
        raramente.MakeClickable(raramenteClick);
        ognitanto.MakeClickable(ognitantoClick);
        spesso.MakeClickable(spessoClick);
        sempre.MakeClickable(sempreClick);

        yield return risponde(0); 
    } 

    private IEnumerator risponde(int aggiunta)
    { 
        yield return mai.Disappear();
        yield return raramente.Disappear();
        yield return ognitanto.Disappear();
        yield return spesso.Disappear();
        yield return sempre.Disappear();

        indice++;
        if (indice >= domande.Length)
        {
            yield return Inizia();
            yield break;
            
        }

        yield return VisualNovelManager.S.dialog.DisplayText(
            "DOMANDA "+(indice+1)+"/20",
             domande[indice]
        );
        IAT += aggiunta; 

        yield return mai.Appear();
        yield return raramente.Appear();
        yield return ognitanto.Appear();
        yield return spesso.Appear();
        yield return sempre.Appear(); 
    }
    private void maiClick()
    {
        StartCoroutine(risponde(1));
    }

    private void raramenteClick()
    {
        StartCoroutine(risponde(2));
    }
    private void ognitantoClick()
    {
        StartCoroutine(risponde(3));
    }
    private void spessoClick()
    {
        StartCoroutine(risponde(4));
    } 
    private void sempreClick()
    {
        StartCoroutine(risponde(5));
    } 

    private IEnumerator Inizia()
    {
        float soglia1 = 28.75f;
        float soglia2 = 51.35f;
        float soglia3 = 78.05f;
        //la variabile ForteDipendenza serviva per una prima versione del gioco, ora è cambiato
        //ma la si tiene per evitare errori o punti della storia dove serve
        if(IAT < soglia1)
        {
            VisualNovelManager.S.ForteDipendenza = false;
            VisualNovelManager.S.StipendioBasso = false;
            VisualNovelManager.S.Single = false;
            VisualNovelManager.S.NonStudiato = false;
            VisualNovelManager.S.Insoddisfatto = false;
            VisualNovelManager.S.Alcool_Azzardo = false;
            VisualNovelManager.S.Droga_Traumi = false;
        }
        else if (IAT < soglia2)
        {
            VisualNovelManager.S.ForteDipendenza = false;
            VisualNovelManager.S.StipendioBasso = true;
            VisualNovelManager.S.NonStudiato = true;
            VisualNovelManager.S.Single = false;
            VisualNovelManager.S.Insoddisfatto = false;
            VisualNovelManager.S.Alcool_Azzardo = true;
            VisualNovelManager.S.Droga_Traumi = false;
        }
        else if (IAT < soglia3)
        {
            VisualNovelManager.S.ForteDipendenza = IAT > 40; //qui considero solo quelli con IAT ALTO dal paper, ma tanto non serve 
            VisualNovelManager.S.StipendioBasso = false;
            VisualNovelManager.S.Single = false;
            VisualNovelManager.S.NonStudiato = false;
            VisualNovelManager.S.Insoddisfatto = true;
            VisualNovelManager.S.Alcool_Azzardo = false;
            VisualNovelManager.S.Droga_Traumi = true;
        }
        else
        {
            VisualNovelManager.S.ForteDipendenza = true;
            VisualNovelManager.S.StipendioBasso = true;
            VisualNovelManager.S.Single = true;
            VisualNovelManager.S.NonStudiato = true;
            VisualNovelManager.S.Insoddisfatto = false;
            VisualNovelManager.S.Alcool_Azzardo = false;
            VisualNovelManager.S.Droga_Traumi = false;
        }

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        VisualNovelManager.S.GoToScene("Atto0"); 
    }
     
    
}
