using System;
using System.Collections;
using UnityEngine;


//questo script va trascinato su SceneController (un oggetto vuoto fuori dal canva) 
public class EmptyScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca; //potrebbe essere luca, marta, etc... chiamare questa variabile
    [SerializeField] ControllerElementoDiScena Mattia; //ipotizziamo 2 marionette
    //inserire in questo elenco tutti gli elementi cliccabili o che devono apparire e sparire!
    //poi inserire l'elemento effettivo su unity in questo campo

    //L'elemento marionetta dev'essere una UI>Image vuota che ha dentro tante UI>Image (pose)
    //Su queste marionetta va trascinato lo script ControllerElementoDiScena
    

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
     

    //Questa scena parte in automatico
    IEnumerator Part1()
    {

        //////////////////// QUESTA PARLA TENERLA SEMPRE UGUALE ///////////////
        yield return VisualNovelManager.S.Element("Overlay").Disappear(); 
        //////////////////////////////////////////////////////////////////////
        
