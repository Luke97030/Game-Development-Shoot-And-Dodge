using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;
    public ScoreManager scoreManager;

    private void Start()
    {
        //scoreManager.addScore(new Score("test", 5));
        //scoreManager.addScore(new Score("eran", 1));
        //scoreManager.addScore(new Score("Luke", 2));

        var scores = scoreManager.sortedScores().ToArray();

        // only showing the first 9 persons
        //for (int i = 0; i < scores.Length; ++i)
        for (int i = 0; i < 9; ++i)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.Name.text = scores[i].name;
            row.iTCount.text = scores[i].ITCountInt.ToString();
            row.rank.text = (i + 1).ToString();

        }
    }

}
