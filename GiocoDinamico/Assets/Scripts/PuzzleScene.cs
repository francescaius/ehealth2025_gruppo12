using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PuzzleScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena closeBtn;
    [SerializeField] ControllerElementoDiScena puzzle1;
    [SerializeField] ControllerElementoDiScena puzzle2;
    [SerializeField] ControllerElementoDiScena puzzle3;
    [SerializeField] ControllerElementoDiScena puzzle4;
    [SerializeField] ControllerElementoDiScena puzzle5;
    [SerializeField] ControllerElementoDiScena puzzle6;
    [SerializeField] ControllerElementoDiScena assemblaBtn;
    [SerializeField] ControllerElementoDiScena puzzleGroup;
    [SerializeField] TextMeshProUGUI testoBorsa;

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
            foreach(int piece in VisualNovelManager.S.takenPuzzlePieces)
            { 
               switch(piece)
                {
                    case 1:
                        StartCoroutine(puzzle1.Appear());
                        break; 
                    case 2:
                        StartCoroutine(puzzle2.Appear());
                        break;
                    case 3:
                        StartCoroutine(puzzle3.Appear());
                        break;
                    case 4:
                        StartCoroutine(puzzle4.Appear());
                        break;
                    case 5:
                        StartCoroutine(puzzle5.Appear());
                        break;
                    case 6:
                        StartCoroutine(puzzle6.Appear());
                        break;
                }
            }

            int c = VisualNovelManager.S.takenPuzzlePieces.Count;
            switch(c)
            {
                case 0: 
                    testoBorsa.text = "La borsa è vuota.";
                    break;
                case 6:
                    testoBorsa.text = "Sembra tu abbia tutti i pezzi del puzzle!"; 
                    StartCoroutine(assemblaBtn.Appear());
                    assemblaBtn.MakeClickable(Assembla);
                    break;
                default: 
                    testoBorsa.text = "Il puzzle sembra fatto da 6 pezzi: tu ne hai "+c+"!";
                    break;
            } 
            
        }  
    }
      
 
     
    public IEnumerator Assembla()
    {
        yield return puzzleGroup.Animate("Assembla");
        //testoBorsa.text = "È il luogo delle vostre vacanze!";
        yield return assemblaBtn.Disappear();
        finalScene();
    }

    private void finalScene()
    {
        VisualNovelManager.S.CloseBag();
        VisualNovelManager.S.GoToScene("SampleScene");
    }

    public void closeBtnClick()
    { 
        Debug.Log("Click stop!");
        VisualNovelManager.S.CloseBag();
    } 

    
}
