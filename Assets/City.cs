using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City
{
    public GameObject go;
    public int population;
    public PROSPERITY prosperity;
    public Market market;
    public string name;

    static int num = 0;

    public City(GameObject city)
    {
        this.name = "City " + num;
        num++;
        this.go = city;
        this.population = Random.Range(1, 500);
        this.prosperity = PROSPERITY.stagnating;
        this.market = new Market(); //TODO make market not have all the items 
    }

    public void CheckProsperity()
    {
        CheckMarket();
        if(this.prosperity == PROSPERITY.starving)
        {
            this.population -= Random.Range(1, 20);
        }else if (this.prosperity == PROSPERITY.stagnating)
        {
            this.population = this.population;
        }
        else if (this.prosperity == PROSPERITY.growing)
        {
            this.population += Random.Range(1, 20);
        }
    }

    private void CheckMarket()
    {
        int cnt = 0;
        foreach (Resource res in this.market.resourcesList)
        {
            if( res.amount > (this.population /100))
            {
                cnt++;
            }
        }
        if(cnt > 10)
        {
            this.prosperity = PROSPERITY.growing;
        }else if((5 < cnt )&&(cnt < 7))
        {
            this.prosperity = PROSPERITY.stagnating;
        }
        else
        {
            this.prosperity = PROSPERITY.starving;
        }
    }

    public void ConsumeResources()
    {
        int consumed_amount = this.population / 100;
        if(consumed_amount == 0)
        {
            consumed_amount = 1;
        }
        foreach (Resource res in this.market.resourcesList)
        {
            res.amount -= consumed_amount;
        }
    }
}
