using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastSceneStartDialogue : MonoBehaviour
{
    [Header("Arrastar objeto com diálogo")]
    public FinalDialogue dialogueBoxScript;
    public string nextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        dialogueBoxScript.StartDialogue();
    }

    void Update()
    {
        Debug.Log("koe");
        if(dialogueBoxScript.ended)
        {
            Debug.Log("cabou");
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }
}
