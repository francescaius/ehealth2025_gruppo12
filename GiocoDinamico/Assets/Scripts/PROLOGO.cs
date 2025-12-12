using System;
using System.Collections;
using UnityEngine;

public class PROLOGO : MonoBehaviour
{
    
    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Lucabimbo;
    [SerializeField] ControllerElementoDiScena Aldo; 



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
        yield return background.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Aldo and I were always happy in that little holiday house… I can still savor every smell, every color…"
        );

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("quadro");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "My favorite one was the one with the cherry blossoms, which reminded me of the garden where we always played, while Aldo preferred the city at sunset. I can’t forget those moments."
        );
        
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("camera");
        yield return Lucabimbo.Appear();
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText("Luca", 
        "Hey! Not fair, you’ve been playing for half an hour! You said that after the mission you’d give me the controller!");

        yield return Aldo.Appear(); 
        yield return VisualNovelManager.S.dialog.DisplayText("Aldo", 
        "It’s almost finished! I just need to beat Bowser, then it’s your turn!");

        yield return Lucabimbo.ChangePose("Arrabbiato");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca", 
        "Yeah, like yesterday. And the day before. You’ve already trained all week, you champion of liars!");

        yield return Aldo.ChangePose("sorride"); 
        yield return VisualNovelManager.S.dialog.DisplayText("Aldo", 
        "Not my fault if you die on the first hit! I have to protect our record");

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return background.ChangePose("camvuota");
        yield return Lucabimbo.Disappear();
        yield return Aldo.Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText("Luca",
        "I don’t remember the exact color of that room anymore…Maybe it was blue… or maybe it was just the reflection of the summer sky…But now none of that exists anymore, it’s only a distant memory.I only remember the laughter. Ours. Mine and my brother’s.Two children, two worlds without boundaries.");
    }

}


        


        