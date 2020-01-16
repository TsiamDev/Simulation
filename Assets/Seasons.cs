using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Seasons
{
    private const int CHANGE_SEASON = 10;   //In Days

    private SEASONS curSeason;
    private int season_count;

    public Seasons(GameObject text)
    {
        curSeason = SEASONS.winter;
        season_count = 0;
        text.GetComponent<Text>().text = "Season: " + Enum.GetName(curSeason.GetType(), curSeason);
    }

    public void CheckSeason(GameObject text)
    {
        if(season_count >= CHANGE_SEASON)
        {
            season_count = 0;
            curSeason++;
            if((int)curSeason >= 4)
            {
                curSeason = SEASONS.winter;
            }
            text.GetComponent<Text>().text = "Season: " + Enum.GetName(curSeason.GetType(), curSeason);
        }
    }
}
