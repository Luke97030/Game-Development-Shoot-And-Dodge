using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // this setting let the game wont pause when you click play button on the main menu
        Time.timeScale = 1f;
        //gameSound.Play();
        SceneManager.LoadScene("Level1");
    }

    public void Leaderboard()
    {
        // this setting let the game wont pause when you click play button on the main menu
        Time.timeScale = 1f;
        //gameSound.Play();
        //SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

}
