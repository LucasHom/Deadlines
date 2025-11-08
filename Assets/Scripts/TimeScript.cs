using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    public Text timeText;
    private float fiveMinuteLength = 3.125f; // 3.125 means that it's a five minute day
    private float totalSeconds;
    private float fiveSecondTimer;
    private bool isPM;
    private bool pastTwelve;
    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        totalSeconds = 9 * 60.0f; // 9 AM
        isPM = false;
        pastTwelve = false;
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        // update both the time values
        if (isRunning)
        {
            fiveSecondTimer += Time.deltaTime;
        }
        if (fiveSecondTimer > fiveMinuteLength)
        {
            totalSeconds += 5.0f;
            fiveSecondTimer = 0.0f;
        }

        // if it's 12 o'clock, make it PM
        if (totalSeconds >= 12 * 60.0f)
        {
            isPM = true;
        }

        // if it's 1 PM, make it adapt to 12 hour reading
        if (totalSeconds >= 13 * 60.0f)
        {
            totalSeconds = 60.0f;
            pastTwelve = true;
        }

        // If it's 5PM, end the game
        if (totalSeconds >= 5 * 60.0f && pastTwelve && isRunning)
        {
            Debug.Log("The day is over!");
            isRunning = false;
        }

        System.TimeSpan time = System.TimeSpan.FromSeconds(totalSeconds);

        // If it's in the last 10 minutes, have the timer flash red and white
        if (totalSeconds % 60 >= 50)
        {
            timeText.color = Color.red;
        }
        else
        {
            timeText.color = Color.white;
        }
        
        // formatting for the UI display
        string formattedTime = string.Format("{0:D1}:{1:D2}",
            time.Minutes, time.Seconds);

        if (isPM)
        {
            timeText.text = formattedTime + "PM";
        }
        else
        {
            timeText.text = formattedTime + "AM";
        }
    }
}
