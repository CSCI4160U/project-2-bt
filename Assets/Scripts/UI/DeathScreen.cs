using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    public void Show()
    {
        Time.timeScale = 0.0f;
        menu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
