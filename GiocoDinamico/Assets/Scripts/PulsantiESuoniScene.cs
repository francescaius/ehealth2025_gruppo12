using System;
using System.Collections;
using UnityEngine;

public class PulsantiEsuoniScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena stopBtn;
    [SerializeField] ControllerElementoDiScena playBtn; 
    [SerializeField] ControllerElementoDiScena justBtn; 
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
        VisualNovelManager.S.backtrack("GoodForTheGhost");

        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        yield return new WaitForSeconds(2);
        yield return stopBtn.Appear();
        yield return playBtn.Appear();
        yield return justBtn.Appear();
        stopBtn.MakeClickable(stopBtnClick);
        playBtn.MakeClickable(playBtnClick);
        justBtn.MakeClickable(justBtnClick);
    }

    IEnumerator Part2()
    {
        yield return VisualNovelManager.S.dialog.DisplayText("- System - ", "Ora passiamo alla seconda scena");
        yield return new WaitForSeconds(2);
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        VisualNovelManager.S.GoToScene("SampleScene");
    }

    public void stopBtnClick()
    { 
        Debug.Log("Click stop!");
        VisualNovelManager.S.playAudio("Click");
        VisualNovelManager.S.pauseBacktrack();

    }
    public void playBtnClick()
    { 
        Debug.Log("Click play!");
        VisualNovelManager.S.playAudio("Click");
        VisualNovelManager.S.resumeBacktrack();
    }
    public void justBtnClick()
    {
        Debug.Log("Click just!");
        VisualNovelManager.S.playAudio("Click");
        //StartCoroutine(Part2());
    }

    
}
