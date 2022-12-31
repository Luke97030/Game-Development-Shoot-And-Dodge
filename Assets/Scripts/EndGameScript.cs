using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScript : MonoBehaviour
{
    //public static bool isGamePaused = false;
    public GameObject endGameUI;
    public TimerScript timerScript;
    public Text playerITCountTxt;
    public Text enemyITCountTxt;
    public GameManager gameManager;
    public InputField nameInput;

    public Button saveBtn;

    private ScoreManager scoreManager;

    // instance of RecordData for saving data to leaderboard
    private RecordData recordData;
    private bool saveBtnClicked = false;

    // faded restart game page
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    bool gameTimeUp;
    float m_Timer;
    bool imagedFaded = false;


    private void Awake()
    {
        // load the PlayerPrefs
        var json = PlayerPrefs.GetString("itcount", "{}");
        recordData = JsonUtility.FromJson<RecordData>(json);
        scoreManager = new ScoreManager(recordData);
    }
    // Update is called once per frame
    void Update()
    {
        // the times up, the I changed the timeTxt.color to yellow 
        if (timerScript.timerTxt.color == Color.yellow)
        {
            gameTimeUp = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (saveBtnClicked)
        {
            saveBtn.GetComponent<Image>().color = Color.red;
        }

        // run the logic for gracefully restart image
        if (gameTimeUp)
        {
            EndLevel();
        }
        // when the image faded. should the summary UI. In there you can have main menu, restart, quit options
        if (imagedFaded)
        {
            endGameUI.SetActive(true);
            playerITCountTxt.text = gameManager.playerItCountText.text.ToString();
            enemyITCountTxt.text = gameManager.enemyItCountText.text.ToString();
            //Time.timeScale = 0f;
        }
    }

    public void SaveRecord() 
    {
        // need to make sure the record is only be saved once, disable the save functionality after be clicked once 
        if (saveBtnClicked == false)
        {
            // when the save button be clicked
            // save the record to RecordData which is a list of records
            Record tempScore = new Record(nameInput.text.ToString(), gameManager.playerITCount);
            // append the new record to the list record list
            recordData.records.Add(tempScore);         
            var json = JsonUtility.ToJson(recordData);
            // set the PlayerPrefs
            PlayerPrefs.SetString("itcount", json);


            saveBtnClicked = true;
        }

    }

    public void RestartGame() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
        //_pauseScript.Resume();

    }

    public void ExitGame() {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void BacktoMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    void EndLevel()
    {
        m_Timer += Time.deltaTime;
        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;
        if (m_Timer > fadeDuration + displayImageDuration)
        {
            imagedFaded = true;
        }
    }
    //public void OnDestroy()
    //{
    //    PlayerPrefs.DeleteKey("itcount");
    //}

}
