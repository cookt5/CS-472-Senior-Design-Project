using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveStats : IStats
{
    public SaveStats(DateTime time)
    {
        // Taking the long time because it includes the seconds
        timeLastPlayed = time.ToLongTimeString();
        // Taking the short date because we don't need the day of the week
        dateLastPlayed = time.ToShortDateString();
    }

    public DateTime GetDateTime()
    {
        Debug.Log(dateLastPlayed + " " + timeLastPlayed);
        DateTime returnTime = DateTime.Parse(dateLastPlayed + " " + timeLastPlayed);
        Debug.Log(returnTime);
        return returnTime;
    }

    public string dateLastPlayed;
    public string timeLastPlayed;
}
