using System;
using System.Collections;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena inizia;
    [SerializeField] ControllerElementoDiScena puzzle;
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
        yield return VisualNovelManager.S.bagBtn.Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        if (VisualNovelManager.S.takenPuzzlePieces.Contains(6))
        {
            yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Wait .... what is this?"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
                 "Luca",
                 "Looks like a piece of paper... with a web link? I won't open it this time: the phone is not the solution."
             );
            yield return VisualNovelManager.S.dialog.DisplayText(
                 "Luca",
                 "But wait ... it's not just a  note! Looks like... a new puzzle!"
             );

        }

        yield return puzzle.Appear();
        VisualNovelManager.S.backtrack("Home");


        if (VisualNovelManager.S.takenPuzzlePieces.Contains(6))
        {
            yield return VisualNovelManager.S.dialog.DisplayText(
                 "Luca",
                 "Aldo... if this is yours, we will finally meet again!"
            );
        }
       

        yield return new WaitForSeconds(1);

        

        yield return background.Appear(); 
        yield return puzzle.ChangePose("title");
        yield return new WaitForSeconds(2);
        yield return inizia.Appear();

        inizia.MakeClickable(sondaggio);
    }

    private void sondaggio()
    { 
        VisualNovelManager.S.Restart();
        VisualNovelManager.S.GoToScene("SondaggioScene");
    }
     
    
}
