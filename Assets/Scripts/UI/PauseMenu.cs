using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SaveManager saveManager;

    private bool paused = false;

    public bool Paused { get => paused; }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        panel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        panel.SetActive(false);
    }

    public void Save()
    {
        saveManager.SaveAll();
        Resume();
        paused = false;
    }

    public void Load()
    {
        saveManager.LoadAll();
        Resume();
        paused = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
}
