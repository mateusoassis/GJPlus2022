using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [Header("Referências")]
    //private CanvasGroup canvasGroup;
    //private GameObject canvasGroupObject;
    [SerializeField] private TextMeshProUGUI dialogueText;
    //public GameManager gameManager;

    [Header("Botão para skipar")]
    public KeyCode skipButton1;
    public KeyCode skipButton2;

    [Header("Texto")]
    public string[] dialogueString;
    public float typeInterval;
    public float skipDuration;
    private float skipTimer;
    [SerializeField] private bool disableAfterEnd;

    [Header("Ignore abaixo")]
    private int index;
    private bool started;
    public bool ended;
    private bool canSkip;
    
    void Awake()
    {
        //canvasGroupObject = GameObject.Find("TutorialWindowContainer");
        // = canvasGroupObject.GetComponent<CanvasGroup>();
        //dialogueText = GameObject.Find("DialogoTexto").GetComponent<TextMeshProUGUI>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        skipTimer = skipDuration;
        canSkip = false;
        dialogueText.text = string.Empty;
    }

    void Start()
    {
        
        //canvasGroupObject.SetActive(false);
    }

    void Update()
    {
        if((Input.GetKeyDown(skipButton1) || Input.GetKeyDown(skipButton2) && canSkip))
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
        //canvasGroupObject.SetActive(true);
        //canvasGroup.alpha = 1;
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
            //canvasGroup.alpha = 0;
            //canvasGroupObject.SetActive(false);
            //gameManager.ResumeForDialogue();
            ended = true;
            //if(callNextPetPosition)
            //{
                //gameManager.playerManager.petHandler.NextPetPosition(petPositionIndex);
            //}
            if(disableAfterEnd)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
