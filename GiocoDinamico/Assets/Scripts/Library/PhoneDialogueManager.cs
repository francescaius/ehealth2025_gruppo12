using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class PhoneDialogueManager : DialogueManager
{
    [Header("Phone Settings")]
    [SerializeField] private string protagonista = "Luca"; 

    [Header("Custom Phone Style Settings")]
    [SerializeField] private UnityEngine.UI.Image messageBox; // Riferimento allo sfondo del dialogo

    // Colori per Luca
    private Color protagonistaBackgroundColor = new Color(0.85f, 0.85f, 0.85f); // Grigio chiaro
    private Color protagonistaTextColor = Color.black;
    private Color protagonistaSpeakerColor = Color.black;

    // Colori per altri speaker
    private Color defaultBackgroundColor = new Color32(52, 152, 219, 255);
    private Color defaultTextColor = Color.white;
    private Color defaultSpeakerColor = Color.white;

    public override CustomYieldInstruction DisplayText(string speaker, string text, bool autoContinue = false)
    { 
        if (speaker == protagonista)
        { 
            if (messageBox != null)
            {
                messageBox.color = protagonistaBackgroundColor;
            }

            if (speakerText != null)
            {
                speakerText.color = protagonistaSpeakerColor;
                //speakerText.alignment = TextAlignmentOptions.Right;
            }

            if (bodyText != null)
            {
                bodyText.color = protagonistaTextColor; 
            } 
            if (goOnButton != null)
            {

                //Image buttonImage = goOnButton.GetComponent<Image>();
                //if (buttonImage != null)
                //{
                //    buttonImage.color = defaultTextColor;
                //}

                TextMeshProUGUI buttonText = goOnButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.color = protagonistaTextColor;
                }
            }
        }
        else
        { 
            if (messageBox != null)
            {
                messageBox.color = defaultBackgroundColor;
            }

            if (speakerText != null)
            {
                speakerText.color = defaultSpeakerColor;
                //speakerText.alignment = TextAlignmentOptions.Left;
            }

            if (bodyText != null)
            {
                bodyText.color = defaultTextColor; 
            }
            if (goOnButton != null)
            {
                //Image buttonImage = goOnButton.GetComponent<Image>();
                //if (buttonImage != null)
                //{
                //    buttonImage.color = defaultTextColor;
                //}

                TextMeshProUGUI buttonText = goOnButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.color = defaultTextColor;
                }
            }
        }
         
        return base.DisplayText(speaker, text, autoContinue);
    } 

}
