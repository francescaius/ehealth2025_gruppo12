using System;
using System.Collections;
using UnityEngine;

public class Atto1scena1 : MonoBehaviour
{
    
    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca;
    [SerializeField] ControllerElementoDiScena Marta; 
    [SerializeField] ControllerElementoDiScena Anonimo;


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
        yield return background.Appear("casa");
        yield return Luca.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Anche stanotte è come se non avessi chiuso occhio"
        );     
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Non ho voglia di alzarmi... non ho voglia di niente"
        );      
        yield return Anonimo.Appear();
        Anonimo.MakeClickable (LeggiNotifica);
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Anonimo",
            "Sai dov'è tuo fratello?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Cosa? Chi sei?"
        );
    
    }
    public void LeggiNotifica()
    {}

}