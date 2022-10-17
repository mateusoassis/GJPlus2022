using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Disable : MonoBehaviour
{
    [SerializeField] private PlayerInfo playerInfo;
    public void DisableThis()
    {
        gameObject.SetActive(false);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
