using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public RecordData scoresData;

    public ScoreManager(RecordData scoresData)
    {
        this.scoresData = scoresData;
    }

    private void Awake()
    {
        var json = PlayerPrefs.GetString("itcount", "{}");
        scoresData = JsonUtility.FromJson<RecordData>(json);
    }


    // the higest score should basedo n the PlayITCount, if the playItCount is 0, it means it shoot the enemy and starting to dodge 
    // and the enemy did not hit him not even once in 3 minutes
    public IEnumerable<Record> sortedScores()
    {
        // the OrderBy is ascending order?
        return scoresData.records.OrderBy(x => x.ITCountInt);
    }

    public void addScore(Record score)
    {
        scoresData.records.Add(score);
    }

    private void OnDestroy()
    {
        SaveScore();
    }
    public void SaveScore()
    {
        var json = JsonUtility.ToJson(scoresData);
        PlayerPrefs.SetString("itcount", json);
    }

    public void backToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

}

[Serializable]
public class RecordData
{
    public List<Record> records;

    public RecordData() {
        records = new List<Record>();
    }

}

[Serializable]
public class Record 
{
    public string name;
    public int ITCountInt;

    public Record(string name, int ITCountInt)
    {
        this.name = name;
        this.ITCountInt = ITCountInt;
    } 
}


