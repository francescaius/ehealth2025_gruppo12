using System;
using System.Collections;
using UnityEngine;

public class PIANTO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca;
    [SerializeField] ControllerElementoDiScena Marta;
    [SerializeField] ControllerElementoDiScena tell;

    void Start()
    {
        if (VisualNovelManager.S == null )
        {
            Debug.LogError("VisualNovelManager non è instanziato nella scena.");
            return;
        }
        else
        {
            //IDEALMENTE IL GIOCO SI POTREBBE EVOLVERE PER CONTROLLARE A CHE PUNTO DELLA SCENA SI FOSSE ARRIVATI PER POTERCI TORNARE
            //IL PROBLEMA È CHE VA FATTO DOPO CHE TUTTE LE SCENE SONO STATE ULTIMATE, E NON È SEMPRE SEMPLICE
            StartCoroutine(Part1());
        }  
    }
IEnumerator Part1()
    {

        yield return VisualNovelManager.S.Element("Overlay").Disappear(); 
        yield return background.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "It’s not a who...It’s a what"
        );

        yield return Luca.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca", 
           "I hear someone crying. Marta usually comes here to read...it could be her. it could be her. "
        );
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return new WaitForSeconds(1);
        yield return Luca.Disappear();
        yield return background.ChangePose("parco3");
        yield return Luca.ChangePose("al telefono");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        
        yield return Luca.Appear();
        yield return tell.Appear();
        tell.MakeClickable(SceltaSbagliata);
     
    }
    private IEnumerator SceltaSbagliata()
    {
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.WrongChoiceDone);
        
        tell.UndoClickable();

       
        yield return new WaitForSeconds(1); 
        
        //alla fine della scelta sbagliata deve ritornare possibile fare la scelta giusta
        yield return Luca.ChangePose("al telefono");
    }


}
