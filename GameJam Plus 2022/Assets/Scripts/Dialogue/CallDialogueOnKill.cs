using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallDialogueOnKill : MonoBehaviour
{
    [Header("Arrastar objeto com diálogo")]
    public DialogueBox dialogueBoxScript;
   // public GameManager gameManager;

    void Awake()
    {
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnDestroy()
    {
        //gameManager.roomCleared = true;
        dialogueBoxScript.StartDialogue();
    }
}
