using System;
using System.Collections;
using UnityEngine;

public class PuzzleScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena closeBtn;
    void Start()
    {
        if (VisualNovelManager.S == null )
        {
            Debug.LogError("VisualNovelManager non è instanziato nella scena.");
            return;
        }
        else
        {
            closeBtn.MakeClickable(closeBtnClick);
            //in base al progresso delle scene mostrare/nascondere pezzi di puzzle!

        }  
    }
      
     

    public void closeBtnClick()
    { 
        Debug.Log("Click stop!");
        VisualNovelManager.S.playAudio("Click");
        VisualNovelManager.S.hideShowPuzzle();
    } 

    
}
