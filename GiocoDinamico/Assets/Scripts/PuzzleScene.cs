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
            //in base al progresso delle scene mostrare/nascondere pezzi di puzzle!
            testoBorsa.text = "La borsa è vuota.";

            StartCoroutine(Test());
        }  
    }
      

    public IEnumerator Test()
    {
        yield return new WaitForSeconds(0);
        yield return puzzle1.Appear();
        testoBorsa.text = "Il puzzle sembra fatto da 6 pezzi: tu ne hai 1!";

        yield return new WaitForSeconds(0);
        yield return puzzle2.Appear();
        testoBorsa.text = "Il puzzle sembra fatto da 6 pezzi: tu ne hai 2!";

        yield return new WaitForSeconds(0);
        yield return puzzle3.Appear();
        testoBorsa.text = "Il puzzle sembra fatto da 6 pezzi: tu ne hai 3!";

        yield return new WaitForSeconds(0);
        yield return puzzle4.Appear();
        testoBorsa.text = "Il puzzle sembra fatto da 6 pezzi: tu ne hai 4!";

        yield return new WaitForSeconds(0);
        yield return puzzle5.Appear();
        testoBorsa.text = "Il puzzle sembra fatto da 6 pezzi: tu ne hai 5!";

        yield return new WaitForSeconds(0);
        yield return puzzle6.Appear();
        yield return assemblaBtn.Appear();
        assemblaBtn.MakeClickable(Assembla);
        testoBorsa.text = "Il puzzle sembra fatto da 6 pezzi: tu ne hai 6!";

    }
     
    public IEnumerator Assembla()
    {
        yield return puzzleGroup.Animate("Assembla");
        testoBorsa.text = "È il luogo delle vostre vacanze!";
        yield return assemblaBtn.Disappear(); 
    }

    private void finalScene()
    {
        VisualNovelManager.S.hideShowPuzzle();
        VisualNovelManager.S.GoToScene("SampleScene");
    }

    public void closeBtnClick()
    { 
        Debug.Log("Click stop!");
        VisualNovelManager.S.hideShowPuzzle();
    } 

    
}
