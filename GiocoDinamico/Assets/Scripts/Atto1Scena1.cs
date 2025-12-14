using System;
using System.Collections;
using UnityEngine;

public class Atto1scena1 : MonoBehaviour
{
    
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

        
        yield return new WaitForSeconds(1);
        yield return background.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Even tonight it feels like I didn’t sleep at all…"
        );     
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "I don’t feel like getting up. I don’t feel like anything"
        );
        VisualNovelManager.S.playAudio("Vibrazione");
        VisualNovelManager.S.playAudio("Notifica");
        yield return new WaitForSeconds(1);

        yield return VisualNovelManager.S.phone.DisplayText(
           "Anonymous",
           "Do you know where your brother is?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "What…? Who are you?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
                "Luca",
                "Nevermind, it's time to go..."
        );
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        VisualNovelManager.S.GoToScene("City");

    }
  
}