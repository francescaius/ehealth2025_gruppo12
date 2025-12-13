using System;
using System.Collections;
using UnityEngine;


public class atto4 : MonoBehaviour
{
    [Header("Personaggi e Scena")]
    [SerializeField] ControllerElementoDiScena background;
    [SerializeField] ControllerElementoDiScena Luca; 
    [SerializeField] ControllerElementoDiScena Margherita;

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
        // RESET INIZIALE
        yield return VisualNovelManager.S.Element("Overlay").Disappear(); 
        
    

        // INIZIO DIALOGHI
        yield return VisualNovelManager.S.phone.DisplayText(
           "Anonimo",
           "You took the wrong path, Luca."
        );
        yield return VisualNovelManager.S.phone.DisplayText(
           "Anonimo",
           "No need to look for me. I’m watching you from here."
        );
        
        yield return Luca.Appear();

        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Who are you? What do you want from me?"
        );

        yield return new WaitForSeconds(1);

        yield return Luca.Disappear();
        
        
        
        yield return background.ChangePose("sottopasso1");
        
        yield return VisualNovelManager.S.dialog.DisplayText(
           "????",
           "Don’t trust what you hold in your hands every day… every hour."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "How long have you been missing?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "????",
           "I’m someone who has already listened to that voice."
        );
        yield return VisualNovelManager.S.phone.DisplayText(
           "Anonimo",
           "Get away from her. Now."
        );

        // --- SCELTA ---
        yield return background.ChangePose("sottopasso2");
        yield return Margherita.Appear();
        yield return Anonimo.Appear();
        
        Anonimo.MakeClickable(SceltaSbagliata);
        Margherita.MakeClickable(SceltaGiusta);
        
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.Choice);
    }

    // --- SCELTA SBAGLIATA ---
    private IEnumerator SceltaSbagliata()
    {
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.WrongChoiceDone);
        Anonimo.UndoClickable();
        Margherita.UndoClickable();
        yield return Anonimo.Disappear();

        yield return new WaitForSeconds(1);
        yield return VisualNovelManager.S.phone.DisplayText(
           "Anonimo",
           "Did you search ‘library graffiti’?"
        );
        yield return VisualNovelManager.S.phone.DisplayText(
           "Anonimo",
           "Good, now that I have your attention."
        );
        
        yield return Luca.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Are you tricking me?"
        );
        yield return Luca.Disappear(); // CORRETTO (era Disppear)
        
        yield return VisualNovelManager.S.phone.DisplayText(
           "Anonimo",
           "Maybe I have some clues for you. Did you search well online? Everything is always there."
        );
        
        yield return Luca.Appear();
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Yes… but I’m not finding anything."
        );
        yield return Luca.Disappear(); 
        
        yield return VisualNovelManager.S.phone.DisplayText(
           "Anonimo",
           "Maybe you haven’t looked enough."
        );
        
        yield return Margherita.ChangePose("marghe in persona");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "Listen to me."
        );
        
        yield return Luca.ChangePose("arrabbiato");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "No. I don’t trust you! They already told me where to look!"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "And who told you that?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Someone… none of your business who."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "If you think that ‘someone’ is just words placed in a row and sent in a message… you’re on the wrong path."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Look, he’s real! He wants to help me!"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "He’s not on your side! He can’t solve your problems for you!"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "I just want to help you, Luca. It was Aldo who pushed me to find you, turn off your phone. Listen to me."
        );

        // Ritorna alla scelta
        Margherita.MakeClickable(SceltaGiusta);
    }

    // --- SCELTA GIUSTA ---
    private IEnumerator SceltaGiusta()
    {
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.RightChoiceDone);
        Anonimo.UndoClickable();
        Margherita.UndoClickable();
        yield return Anonimo.Disappear();

        yield return Luca.ChangePose("rilassato");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Alright… I’ll listen. Actually… I’ll even turn off my phone."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "You feel lighter now that you’re not crushed by notifications, don’t you?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Why was he writing to me? Who is he?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "It’s not a who. It’s a what. It uses your photos, your contacts, your memories. It learned your voice, your thoughts… and it shows you only what keeps you glued to it."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "I don’t know anyone who would do this to me."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "Maybe it’s not someone. Maybe it’s something."
        );

        yield return new WaitForSeconds(1);

        // PUZZLE
        yield return VisualNovelManager.S.ObtainPuzzle(3);
        VisualNovelManager.S.SetSceneData(GetType().Name, SceneProgressStep.Finished);
        
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "I… wasn’t close to your brother. I barely knew him. But once… we talked. Only for a few minutes He seemed… lost. Like you now."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "He told me something I didn’t understand at the time: that one day someone would come looking for him. And that this ‘arrival’ would begin… with a train."
        );
        
        yield return Luca.ChangePose("confuso");
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "Why would he tell you that? Even I didn’t know anything."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "I don’t know. Maybe he had no one else to talk to. He just told me that place—the mountain, the peak above the city—was the only place where he felt… real."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "But my brother never lacked anything! What would he need to escape from?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "What do you mean?"
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Luca",
           "He was always the best at everything."
        );
        yield return VisualNovelManager.S.dialog.DisplayText(
           "Margherita",
           "Sometimes… the one who seems to have everything is the one who loses himself the easiest."
        );

        // BLOCCO VARIABILE (STIPENDIO)
        if (VisualNovelManager.S.StipendioBasso)
        { 
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "I, instead… want a life where I can travel, wake up happy, surrounded by people who care… and maybe with a bit more money."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "I don’t ask for luxury, only to be able to breathe without counting every cent."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "I wish I didn’t have to hope every month to make it to the end, pray nothing unexpected happens, that no bill increases. It’s like living with a weight on my chest… too heavy to carry."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "If I had more possibilities, I’d live more. I could try new things, take risks, move. Instead I stay still, stuck. And I know that partly… it’s my fault."
            ); 
        } 
        else // stipendio alto
        {
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "I, instead… just want to feel alive. I have a home, a stable job, money to travel… I’m not missing anything, at least on paper."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "And yet I wake up and enjoy nothing."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "I could buy a plane ticket tomorrow, afford little luxuries… and yet I stay blocked, as if every choice required enormous effort. It’s absurd to have possibilities and be unable to use them."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "Sometimes I wonder what’s the point of having everything if you feel like you have nothing. And I know that somehow… that’s also my fault."   
            );
        }

        // BLOCCO VARIABILE (DROGA / TRAUMI)
        if (VisualNovelManager.S.Droga_Traumi)
        {
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "Luca… there’s something I didn’t tell you."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I did cross paths with your brother, yes… but that’s not why I recognize you."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "Then why?"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "Because you and I… we shared the same hell."
            );
            yield return Luca.ChangePose("confuso");
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "Do you remember the recovery center?"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "The white rooms, the plastic chairs, the meetings at seven in the evening… I was in the room next to yours." 
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "Margherita… you were there? Really?"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "Yes. Cocaine in my case, but with too many traumas behind me."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I know I shouldn't blame my family, but I’ve lived through too much."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "You, instead…"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "Don’t say it."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I won’t. We both know. That’s enough."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I got out. With effort, with shame, with months of relapses."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I even started therapy with a psychologist to process my past. I recommend it, for any problem, it helps you face it."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "But I made it. You, instead… never truly stopped, Luca."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "I quit."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "You quit one thing… and embraced another. And don’t lie, I know you haven’t really quit. That phone is just an extension of your old escape."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "*He steps back, hand instinctively on his pocket.*" // Messo tra asterischi se è un'azione
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "It’s not the same…"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "No?"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "Then tell me why your hand shakes when you try to leave it behind."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "Why you lose your breath when it doesn’t vibrate."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "Why those notifications control you more than you want to admit."
            );
            yield return Luca.ChangePose("sconsolato");
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I saw it in you even back then. The same void you filled by rolling dice, playing slots, chasing thrills everywhere. Now you fill it with screens, feeds, messages. Cleaner, yes… but not less dangerous."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "I didn’t think you noticed…"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I lived it on my own skin. I recognize it anywhere."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "You were… one of the few who spoke to me normally, back there. Remember? Sitting outside during the break…"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I remember you were kind. Lost, but kind."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I remember you trembled when talking about the future. And I remember that when the program ended… you disappeared."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "Yes. Because I was afraid of relapsing. And I did relapse."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "Luca… that’s the point. You think you’re different now. But you’re still running."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "And that screen is just another locked door to hide behind."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Luca",
               "And you? How did you get out?"
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "I stopped looking for answers where I already knew they didn’t exist."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "And I started looking for people. Not objects. Not substances. People." 
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "It’s never too late to get back in the game. It’s almost evening… I have to go. But I can tell you one thing, because it’s what he told me that day."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "Go to the station. Take the first train and climb to the top of the mountain."
            );
            yield return VisualNovelManager.S.dialog.DisplayText(
               "Margherita",
               "There… you will find what you are looking for. Whatever it is."
            );
        }
        
        // CHIUSURA
        yield return VisualNovelManager.S.Element("Overlay").Appear();
        Conclusione();
    }

    private void Conclusione()
    {
        VisualNovelManager.S.GoToScene("NomeScenaSuccessiva"); // Ricordati di cambiare "NomeScenaSuccessiva"
    }
}