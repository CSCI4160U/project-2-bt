using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    public void Show()
    {
		Time.timeScale = 0.0f;
		menu.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
	}

    public void StartAgain()
    {
		SceneManager.LoadScene(0);
		Time.timeScale = 1.0f;
		Cursor.lockState = CursorLockMode.Locked;
	}

    public void Exit()
    {
        Application.Quit();
    }
}
