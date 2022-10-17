using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image image;

    [Header("Botão para skipar")]
    public KeyCode skipButton1;
    public KeyCode skipButton2;

    [Header("Portrait and Order")]
    public Sprite[] sprites;
    public int[] dialogueSprite;

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
        skipTimer = skipDuration;
        canSkip = false;
        dialogueText.text = string.Empty;
    }

    void Update()
    {
        if(((Input.GetKeyDown(skipButton1) || Input.GetKeyDown(skipButton2)) && canSkip))
        {
            if(dialogueText.text == dialogueString[index])
            {
                NextDialogue();
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
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeDialogue());
        }
        else
        {
            ended = true;

            if(disableAfterEnd)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
