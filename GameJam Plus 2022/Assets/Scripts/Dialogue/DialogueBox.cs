using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private bool callNextPetPosition;
    [SerializeField] private int petPositionIndex;
    [Header("Referências")]
    private CanvasGroup canvasGroup;
    private GameObject canvasGroupObject;
    private TextMeshProUGUI dialogueText;
    //public GameManager gameManager;

    [Header("Texto")]
    public string[] dialogueString;
    public float typeInterval;
    public float skipDuration;
    private float skipTimer;

    [Header("Ignore abaixo")]
    private int index;
    private bool started;
    public bool ended;
    private bool canSkip;
    
    void Awake()
    {
        canvasGroupObject = GameObject.Find("TutorialWindowContainer");
        canvasGroup = canvasGroupObject.GetComponent<CanvasGroup>();
        dialogueText = GameObject.Find("DialogoTexto").GetComponent<TextMeshProUGUI>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        skipTimer = skipDuration;
    }

    void Start()
    {
        canSkip = false;
        dialogueText.text = string.Empty;
        canvasGroupObject.SetActive(false);
    }

    void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F)) && canSkip)
        {
            if(dialogueText.text == dialogueString[index])
            {
                NextDialogue();
                //RefreshSkipTimer();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueString[index];
                RefreshSkipTimer();
            }
        }

        if(started)
        {
            if(skipTimer > 0 && !canSkip)
            {
                skipTimer -= Time.unscaledDeltaTime;
            }
            else
            {
                canSkip = true;
            }
        }
        
    }

    public void RefreshSkipTimer()
    {
        skipTimer = skipDuration;
        canSkip = false;
    }

    public void StartDialogue()
    {
        dialogueText.text = string.Empty;
        started = true;
        //gameManager.PauseForDialogue();
        canvasGroupObject.SetActive(true);
        canvasGroup.alpha = 1;
        index = 0;
        StartCoroutine(TypeDialogue());
    }

    public IEnumerator TypeDialogue()
    {
        foreach(char c in dialogueString[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typeInterval);
        }
    }

    public void NextDialogue()
    {
        if(index < dialogueString.Length - 1)
        {
            //gameManager.SoundButton();
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeDialogue());
        }
        else
        {
            //gameManager.SoundButton();
            canvasGroup.alpha = 0;
            canvasGroupObject.SetActive(false);
            //gameManager.ResumeForDialogue();
            ended = true;
            if(callNextPetPosition)
            {
                //gameManager.playerManager.petHandler.NextPetPosition(petPositionIndex);
            }
            gameObject.SetActive(false);
        }
    }
}
