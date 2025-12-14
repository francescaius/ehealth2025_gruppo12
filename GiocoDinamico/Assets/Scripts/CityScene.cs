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
            VisualNovelManager.S.backtrack("City");
            StartCoroutine(Part1()); 
        }  
    }

    IEnumerator Part1()
    {
        yield return VisualNovelManager.S.Element("Overlay").Disappear(); 
        if (VisualNovelManager.S.takenPuzzlePieces.Contains(3))
        {
            yield return VisualNovelManager.S.dialog.DisplayText("Luca", "I have to go to the station...");
            yield return metro.Appear();
            yield return new WaitForSeconds(1);
            metro.MakeClickable(()=>{
                StartCoroutine(VisualNovelManager.S.Element("Overlay").Appear()); 
                VisualNovelManager.S.GoToScene("Atto4");
            });
        }
        else if (VisualNovelManager.S.takenPuzzlePieces.Contains(2))
        {
            yield return VisualNovelManager.S.dialog.DisplayText("Luca", "After all the day I can finally go home, going throug the park...");
            yield return parco.Appear();
            yield return new WaitForSeconds(1);
            parco.MakeClickable(() => {
                StartCoroutine(VisualNovelManager.S.Element("Overlay").Appear());
                VisualNovelManager.S.GoToScene("Atto3");
            });
            yield break;
        }
        else if (VisualNovelManager.S.takenPuzzlePieces.Contains(1))
        {
            yield return VisualNovelManager.S.dialog.DisplayText("Luca", "Well, now it is time to work...");
            yield return ufficio.Appear();
            yield return new WaitForSeconds(1);
            ufficio.MakeClickable(() => {
                StartCoroutine(VisualNovelManager.S.Element("Overlay").Appear());
                VisualNovelManager.S.GoToScene("Atto2");
            });
            yield break;
        }
        else
        {
            yield return VisualNovelManager.S.dialog.DisplayText("Luca", "Before going to work I'll go to the cafe, and maybe I find there my friend marta...");
            yield return bar.Appear();
            yield return new WaitForSeconds(1);
            bar.MakeClickable(() => {
                StartCoroutine(VisualNovelManager.S.Element("Overlay").Appear());
                VisualNovelManager.S.GoToScene("Atto12");
            });
            yield break;
        }
    }

    private void sondaggio()
    {
        
    }
     
    
}
