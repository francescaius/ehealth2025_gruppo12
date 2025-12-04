using System;
using System.Collections;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena personaggio;
    [SerializeField] ControllerElementoDiScena personaggio2; 
    [SerializeField] ControllerElementoDiScena background; 
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
        yield return personaggio.Appear();
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Provo con un testo molto lungo che quasi sicuramente dovrà essere spezzato inpiù parti sicuramente dovrà essere spezzato inpiù parti sicuramente dovrà essere\n spezzato inpiù parti  ora facciamo una parte senza spazi: oajsajsdlkfhalksjdhflkasjdhflkajshdflkajshdflkajhsdlfkjahsdlfkjhasldkfjhalksdjhflaksdjhflkasjdhflkajsdhflkajhdsflkasjhdflkajsdhflkjasdhflkasjh a questo punto direi invece di terminare."
       );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "",
            "Qui provo a scrivere senza un nome"
        );
        yield return VisualNovelManager.S.phone.DisplayText(
            "Luca",
            "luca manda un messaggio"
        );
        yield return VisualNovelManager.S.phone.DisplayText(
            "Anonimo",
            "Anonimo risponde"
        );

        yield return personaggio.ChangePose("Rosa"); 
        if(VisualNovelManager.S.ForteDipendenza)
        { 
            yield return VisualNovelManager.S.dialog.DisplayText(
                "Personaggio rosa",
                "FORTEMENTE DIPENDENTE"
            );
        }
        else
        {
            yield return VisualNovelManager.S.dialog.DisplayText(
                 "Personaggio rosa",
                 "NON FORTEMENTE DIPENDENTE"
             );
        }
        yield return VisualNovelManager.S.Element("PuzzlePiece").Appear(); 
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Puzzle",
            "SONO IL PUZZLE"
        );
        yield return VisualNovelManager.S.Element("PuzzlePiece").Disappear(); 
        yield return VisualNovelManager.S.dialog.DisplayText(
            "- Sistema -",
            "Il puzzle è stato messo nell'inventario"
        );
        yield return personaggio2.Appear("EntryFromRight");
        yield return new WaitForSeconds(3);
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Personaggio verde",
            "Tu mi hai fatto davvero arrabbiare vedi di sparire, sto diventando rosso!"
        );
        yield return personaggio2.ChangePose("Rossa");
        yield return new WaitForSeconds(2);
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Personaggio rosa",
            "Sparisco subito!"
        );
        yield return personaggio.Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Personaggio verde",
           "Ora mi calmo!"
       );
        yield return personaggio2.ChangePose("Verde");
        yield return new WaitForSeconds(2);
;        yield return personaggio2.Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Appear();
    }

    
}
