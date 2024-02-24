using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScreen");
    }

    public void ReturnMenu()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("MenuScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
