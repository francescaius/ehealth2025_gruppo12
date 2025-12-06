using System;
using System.Collections;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena inizia; 


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
        inizia.MakeClickable(sondaggio);
    }

    private void sondaggio()
    {
        VisualNovelManager.S.GoToScene("SondaggioScene");
    }
     
    
}
