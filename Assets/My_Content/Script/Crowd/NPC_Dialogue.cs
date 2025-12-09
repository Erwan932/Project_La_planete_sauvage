using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NPC_Dialogue : MonoBehaviour
{
    public NPC_Dialogue_Script diloqueData;
    public GameObject DialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dilogueIndex;
    private bool isTyping, isDialogueActive;
}
