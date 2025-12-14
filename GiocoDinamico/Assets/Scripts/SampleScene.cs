using System;
using System.Collections;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena personaggio;
    [SerializeField] ControllerElementoDiScena personaggio2; 
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
        yield return personaggio.Appear();
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Ciao sono luca di cosa hai bisogno?."
       );yield return VisualNovelManager.S.phone.DisplayText(
           "Anonymous",
           "Ciao sono anonimo..... di cosa hai bisogno?."
       );

        //yield return VisualNovelManager.S.Element("PuzzlePiece").ChangePose("Puzzle1");

        yield return VisualNovelManager.S.Element("PuzzlePiece").Appear();
        yield return VisualNovelManager.S.Element("PuzzlePiece").Disappear();
         
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Breve2."
       );


        yield return VisualNovelManager.S.ObtainPuzzle(3);
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Hai ottenuto il pezzo 1..."
       );
        yield return VisualNovelManager.S.ObtainPuzzle(1);
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Hai ottenuto il pezzo 3..."
       );
        yield return VisualNovelManager.S.ObtainPuzzle(2);
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Hai ottenuto il pezzo 2..."
       );
        yield return VisualNovelManager.S.ObtainPuzzle(4);
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Hai ottenuto il pezzo 4..."
       );
        yield return VisualNovelManager.S.ObtainPuzzle(5);
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Hai ottenuto il pezzo 5..."
       );
        yield return VisualNovelManager.S.ObtainPuzzle(6);
        yield return VisualNovelManager.S.phone.DisplayText(
           "Luca",
           "Hai ottenuto il pezzo 6..."
       );
    }

    
}
