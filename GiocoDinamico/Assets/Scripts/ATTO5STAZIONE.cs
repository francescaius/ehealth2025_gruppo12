using System;
using System.Collections;
using UnityEngine;


//questo script va trascinato su SceneController (un oggetto vuoto fuori dal canva) 
public class ATTO5STAZIONE : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca; //potrebbe essere luca, marta, etc... chiamare questa variabile
    [SerializeField] ControllerElementoDiScena Lucia;
    [SerializeField] ControllerElementoDiScena monaco;

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
        VisualNovelManager.S.backtrack("Metro");
        yield return VisualNovelManager.S.Element("Overlay").Disappear(); 
        yield return background.Appear();
        yield return Lucia.ChangePose("normale");
        yield return Lucia.Appear();
        yield return Luca.ChangePose("incuriosito");
        yield return Luca.Appear();

        yield return VisualNovelManager.S.dialog.DisplayText("Lucia","Where are you going at this hour? Do you remember we have a meeting tomorrow?");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca"," want to go somewhere special for me and my family. I haven’t been there in a long time… I thought it could be nice.I need to think.");

        if (VisualNovelManager.S.ForteDipendenza)
        {
            yield return VisualNovelManager.S.dialog.DisplayText("Lucia","Try not to think too much…or at least try thinking at the oﬃce instead of diving into your screen. Last time you showed up empty-handed and ran away before the boss arrived.");
            yield return VisualNovelManager.S.dialog.DisplayText("Luca","This time it’s diﬀerent. I’m starting to understand that I’ve always been the one sabotaging myself in everything… unable to see life from the right perspective.");
            yield return VisualNovelManager.S.dialog.DisplayText("Lucia","Well, if you’d like to share this perspective, I’d be grateful.");
            yield return VisualNovelManager.S.dialog.DisplayText("Luca","The right perspective is yours: we live guided by a little box we always hold in our hands… and for every good thing it oﬀers, it guarantees at least five complicated ones.");

        }
        if (VisualNovelManager.S.Alcool_Azzardo)
        { 
        yield return Lucia.ChangePose("incuriosita");
        yield return VisualNovelManager.S.dialog.DisplayText("Lucia","hink? Luca… every time you say that, you end up shut in some bar drinking, or you spend the night betting on matches.Is that your way of ‘thinking’?");
        yield return Luca.ChangePose("triste");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","That’s not true, not always…");
        yield return VisualNovelManager.S.dialog.DisplayText("Lucia","Luca, please. Last time you showed up to the meeting with breath that burned and your head spinning. And when the boss asked for the reports… you had nothing. Nothing. Do you remember?");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","Yes. And it wasn’t my finest day.");
        yield return VisualNovelManager.S.dialog.DisplayText("Lucia","And that evening, instead of facing reality, you ran home to gamble away half your salary, hoping for a lucky strike… as if it could save you from everything.");
        yield return Luca.ChangePose("arrabbiato");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","I know! I know… you don’t have to remind me every time! I’m not proud of any of that.");
        yield return VisualNovelManager.S.dialog.DisplayText("Lucia","I’m not saying it to hurt you.I’m saying it because you’re hurting yourself. And I see it every time you hide behind a drink or behind a screen promising easy wins.");
        yield return Luca.ChangePose("triste");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","That’s exactly the point. If I stay here… I risk falling back into it. I know myself: one bad day and I’m back chasing a bottle or that stupid rush from clicking ‘play’. And I hate it. I want to stop being like this.");
        yield return VisualNovelManager.S.dialog.DisplayText("Lucia","And you think running away solves everything?");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","No. But staying here makes it worse. I need to detach from what drowns me, from what keeps pulling me into the same cycles. I want to change, Lucia. For the first time… I really want to change.");
        yield return VisualNovelManager.S.dialog.DisplayText("Lucia","Then at least promise me you won’t use ‘thinking’ as an excuse to destroy yourself this time.");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","I promise. And for once… I truly want to keep it.");
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","This time I won’t back out. But first I need to solve a riddle I don't fully understand. I still don’t know what he wants to tell me… I haven’t heard from him in a while. Who knows.");
        yield return VisualNovelManager.S.dialog.DisplayText("Lucia","Then have a good trip. See you in the meeting room!");
        }

        yield return VisualNovelManager.S.Element("Overlay").Appear();
        yield return Luca.Disappear();
        yield return Lucia.Disappear();
        yield return background.ChangePose("platform 7");
        yield return VisualNovelManager.S.Element("Overlay").Disappear();
        yield return VisualNovelManager.S.dialog.DisplayText("Luca","This time I know where I need to go… I just need to find the strength to get there.");

        yield return VisualNovelManager.S.Element("Overlay").Appear(); 
        VisualNovelManager.S.GoToScene("TrenoScene");
    }

}