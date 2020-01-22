using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Market
{
    //Price per unit
    const float MIN_PRICE = 1;
    const float MAX_PRICE = 50;

    public Dictionary<RESOURCE_TYPE, float> resource_price_dict;
    public float[,] pastValues;
    public List<Resource> resourcesList;

    public Market()
    {
        resourcesList = new List<Resource>();
        int count = Enum.GetNames(typeof(RESOURCE_TYPE)).Length;
        pastValues = new float[count, 10];
        resource_price_dict = new Dictionary<RESOURCE_TYPE, float>();
        for (int i = 0; i < count; i++)
        {
            resource_price_dict.Add((RESOURCE_TYPE) i, UnityEngine.Random.Range(MIN_PRICE, MAX_PRICE));
            //Debug.Log("type " + (RESOURCE_TYPE)i + "price " + resource_price_dict[(RESOURCE_TYPE)i]);
            pastValues[i, 9] = resource_price_dict[(RESOURCE_TYPE)i];
            resourcesList.Add(new Resource((RESOURCE_TYPE)i, UnityEngine.Random.Range(0, 10)));
        }
    }

    public void UpdateMarket()
    {
        for (int i = 0; i < resource_price_dict.Count; i++)
        {
            float change = UnityEngine.Random.Range(-0.5f, 0.5f);
            float temp = resource_price_dict[(RESOURCE_TYPE)i];
            resource_price_dict[(RESOURCE_TYPE)i] += temp * change;
        }
        for (int i = 0; i < resource_price_dict.Count; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                //Shift previous values
                pastValues[i,j] = pastValues[i,j + 1];
            }
            //Store new value at the end
            pastValues[i, 9] = resource_price_dict[(RESOURCE_TYPE)i];
        }
    }
}
