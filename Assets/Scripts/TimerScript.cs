using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Text timerTxt;
    private float initialTime;
    private bool isTimeUp = false;

    private string minutes;
    private string seconds;
    // Start is called before the first frame update
    void Start()
    {
        initialTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeUp)
        { 
            return;
        }
        else
        {
            // 3 minutes, (int)(Time.time - initialTime)/180 == 1
            if ((int)(Time.time - initialTime) / 180 == 1)
            {
                Finish();
            }
            minutes = ((int)(Time.time - initialTime) / 60).ToString();
            seconds = ((Time.time - initialTime) % 60).ToString("f2");
            timerTxt.text = "Time: " + minutes + ":" + seconds;
            
        }

        
    }

    public void Finish()
    {
        isTimeUp = true;
        timerTxt.color = Color.yellow;
    }
}
