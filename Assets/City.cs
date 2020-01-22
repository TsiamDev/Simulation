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
}
