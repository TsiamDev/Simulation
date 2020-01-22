using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public GameObject go;
    public List<City> cities;
    public Region(GameObject region)
    {
        this.go = region;
        cities = new List<City>();
        foreach (Transform child in go.transform)
        {
            cities.Add(new City(child.gameObject));
        }
    }
}
