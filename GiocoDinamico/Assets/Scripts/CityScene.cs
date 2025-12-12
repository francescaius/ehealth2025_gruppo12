using System;
using System.Collections;
using UnityEngine;

public class CityScene : MonoBehaviour
{

    [SerializeField] ControllerElementoDiScena bar;
    [SerializeField] ControllerElementoDiScena ufficio;
    [SerializeField] ControllerElementoDiScena parco;
    [SerializeField] ControllerElementoDiScena metro;


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
        yield return VisualNovelManager.S.ObtainPuzzle(2);
        if (VisualNovelManager.S.takenPuzzlePieces.Contains(3))
        {
            yield return VisualNovelManager.S.dialog.DisplayText("Luca", "I have to go to the metro...");
            yield return metro.Appear();
            yield return new WaitForSeconds(1);
            metro.MakeClickable(() => VisualNovelManager.S.GoToScene("SampleScene"));
        }
        else if (VisualNovelManager.S.takenPuzzlePieces.Contains(2))
        {
            yield return VisualNovelManager.S.dialog.DisplayText("Luca", "After all the day I can finally go home, going throug the park...");
            yield return parco.Appear();
            yield return new WaitForSeconds(1);
            parco.MakeClickable(()=> VisualNovelManager.S.GoToScene("SampleScene"));
            yield break;
        }
        else if (VisualNovelManager.S.takenPuzzlePieces.Contains(1))
        {
            yield return VisualNovelManager.S.dialog.DisplayText("Luca", "Well, now it is time to work...");
            yield return ufficio.Appear();
            yield return new WaitForSeconds(1);
            ufficio.MakeClickable(() => VisualNovelManager.S.GoToScene("ATTO2LAVORATORE"));
            yield break;
        }
        else
        {
            yield return VisualNovelManager.S.dialog.DisplayText("Luca", "Before going to work I'll go to the cafe to see my wife...");
            yield return bar.Appear();
            yield return new WaitForSeconds(1);
            bar.MakeClickable(() => VisualNovelManager.S.GoToScene("Atto1scena2"));
            yield break;
        }
    }

    private void sondaggio()
    {
        
    }
     
    
}
