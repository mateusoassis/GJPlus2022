using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallDialogueOnCollision : MonoBehaviour
{
    public string tagTarget;
    public DialogueBox dialogueBoxScript;
    //private GameManager gameManager;
    private bool startedDialogue;
    private BoxCollider boxCollider;

    void Awake()
    {
        dialogueBoxScript = GetComponent<DialogueBox>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        /*
        if(gameManager.playerManager.isFading)
        {
            boxCollider.enabled = false;
        }
        else
        {
            boxCollider.enabled = true;
        }
        */
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == tagTarget && !startedDialogue)
        {
            dialogueBoxScript.StartDialogue();
            startedDialogue = true;
            //gameObject.SetActive(false);
        }
    }
}
