using System;
using System.Collections;
using UnityEngine;

public class Atto1scena2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ControllerElementoDiScena Background;
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
            StartCoroutine(Part2()); 
        }  
    }
     
    IEnumerator Part2()
    { 
        yield return VisualNovelManager.S.Element("Overlay").Disappear();

        
        yield return new WaitForSeconds(1);
        yield return Background.Appear("BGbar");
        yield return Luca.Appear();
        yield return Marta.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "I hadn’t been here for… how long? Ten years, maybe. It reminds me of… her. Marta. She worked here, even back then."
            );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Long time no see! What can I get you?"
        );        
        
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "I’m not very hungry. Just a coffee… actually, make it a double."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "I’m so tired I could fall asleep right here."
            );  
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Why are you so tired?"
        ); 
        yield return Luca.ChangePose("Triste");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "I don’t know, too many thoughts… then I wake up and… I don’t know, it’s already too much, everything feels so monotonous, even though this morning…"
            ); 
        yield return Marta.ChangePose("Arrabbiata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Are you sure that besides all those thoughts you didn’t spend the night wide awake staring at your phone?"
        ); 
        yield return Luca.ChangePose("Arrabbiato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Just make me the coffee, that’s enough."
        );
        yield return Marta.ChangePose("Preoccupata");    
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Can I help you? You look… lost."
            
        );  
        
        if(VisualNovelManager.S.Insoddisfatto)
        {
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "You really look worn out, Luca. What’s going on?"
        );
        yield return Luca.ChangePose("Triste");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "It’s that every day feels the same. I wake up and… I already know how it will go. Nothing changes. Nothing gets better."
        ); 
        yield return Marta.ChangePose("Pensierosa");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Maybe it’s just a phase, it happens."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "A phase? Marta, it’s like living in a loop. Work, home, thoughts… a wheel that turns without taking me anywhere."
        ); 
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "And there’s really nothing that makes you feel alive? Not even one thing?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Sometimes I feel like I’m just filling time. Like I’m not heading anywhere… just dragging myself along."
        ); 
         yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "I don’t recognize you like this. It’s like you’re fading."
        );
          yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Maybe it’s because I don’t know what I really want anymore."
        );
        yield return Marta.ChangePose("Pensierosa");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Anyway, as we were saying…"
        );

        }
        else //soddisfatto
        {
        yield return Marta.ChangePose("Pensierosa");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "You look exhausted. What’s happening?"
        );
        yield return Marta.ChangePose("Rilassato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "It’s just that lately I haven’t stopped for a second. A thousand things to fix, a thousand pieces to put back together…"
        );
        yield return Marta.ChangePose("Triste");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "So that’s why you look so done in?"
        );
            yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Yeah, but it’s a different kind of tiredness. Not the kind that empties you… the kind that reminds you you’re doing something important."
        );
        yield return Marta.ChangePose("Rilassata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Ah, so there is something that really drives you, huh?"
        );
         yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Yes. And even if I feel like a mess… I finally have something worth running for."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Well, then you’re not as bad off as you seem."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Maybe not. I’m tired, sure… but because I have a lot of good things to face."
        );
         yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Anyway, as we were saying…"
        );
        


        }     
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Actually, I came here because I was looking for someone you know well."
        );  
        yield return Marta.ChangePose("Pensierosa");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Tell me, who?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "My brother. He used to come here…"
        ); 
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "I haven’t heard that name in a while. He left something… but I don’t know if I should give it to you."
        );
        yield return Anonimo.Appear();
        yield return Luca.ChangePose("Preoccupato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Again that notification. Same tone, same sender. What if it’s him? What if it’s my brother?"
        );
        Anonimo.MakeClickable (LeggiNotifica);
        Marta.MakeClickable(Parla);
       
    }
    public void LeggiNotifica()
    {
        StartCoroutine (Scelta1());
    
    }
   
    private IEnumerator Scelta1 ()
    {
        Marta.UndoClickable();
        Anonimo.UndoClickable();
        yield return Anonimo.Disappear ();
        yield return Marta.ChangePose("Arrabbiata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Can you hear me? Hey, I’m talking to you."
        );
        yield return Marta.ChangePose("Preoccupata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Luca?"
        );
        yield return VisualNovelManager.S.phone.DisplayText(
            "Anonimo",
            "Trust no one. Leave now. I will guide you."
        );
        
        yield return Luca.ChangePose("Preoccupato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Wait… what does that mean? Who are you?"
        );
        
        yield return Marta.ChangePose("Preoccupata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Are you talking to me? And stop yawning!"
        );
        yield return Marta.ChangePose("Arrabbiata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "And stop yawning!"
        );
        
        yield return Luca.ChangePose("Triste");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "No, sorry… I’m just trying to understand… someone is helping me."
        );
        yield return Marta.ChangePose ("Arrabbiata");
        yield return VisualNovelManager.S.dialog.DisplayText(
        "Marta",
        "Helping? Or controlling you? Since you walked in, you haven’t stopped looking at that thing."
        );
        yield return Luca.ChangePose("Preoccupato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "You don’t understand… it’s important. Maybe it’s him. My brother."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
        "Marta",
        "Your brother came here to disconnect from the world, not to sink deeper into it. But I see you’re very different."
        );
        yield return Luca.Disappear();
        
   
    }
    public void Parla()
    {
        StartCoroutine(Scelta2());
    }

    private IEnumerator Scelta2 ()
    {
        Marta.UndoClickable();
        Anonimo.UndoClickable();
        yield return Anonimo.Disappear ();
        yield return Luca.Appear();
         // caso in cui l'utente sceglie di cliccare su marta ed ignorare la notifica
        yield return Marta.ChangePose("Rilassata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "You didn’t go far, I see."
        );
        yield return Luca.ChangePose("Rilassato");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "There was no one outside… and I really need this coffee. You told me my brother came here to disconnect, right?"
        );
        yield return Background.ChangePose("foto");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Luca",
            "Maybe this will help you understand I'm not lying."
        );
        yield return Marta.ChangePose("Rilassata");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Your brother showed me that photo and told me that if anyone ever came with it, I should give them this."
        );
        yield return Background.ChangePose("cassetto");
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "He said that one day he wanted to come home. But he wasn't sure he'd be able to."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
            "Marta",
            "Ecco a te il puzzle!"
        );
        
        yield return VisualNovelManager.S.Element("PuzzlePiece").Appear();
        yield return new WaitForSeconds(2);
        yield return VisualNovelManager.S.Element("PuzzlePiece").Disappear();
        yield return VisualNovelManager.S.Element("Overlay").Appear();
    }
}
