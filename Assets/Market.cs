using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Market
{
    const float MIN_PRICE = 10;
    const float MAX_PRICE = 1000;

    public Dictionary<RESOURCE_TYPE, float> resource_price_dict;

    public Market()
    {
        resource_price_dict = new Dictionary<RESOURCE_TYPE, float>();
        int count = Enum.GetNames(typeof(RESOURCE_TYPE)).Length;
        for (int i = 0; i < count; i++)
        {
            resource_price_dict.Add((RESOURCE_TYPE) i, UnityEngine.Random.Range(MIN_PRICE, MAX_PRICE));
            Debug.Log("type " + (RESOURCE_TYPE)i + "price " + resource_price_dict[(RESOURCE_TYPE)i]);
        }
    }

    public void UpdateMarket()
    {
        for (int i = 0; i < resource_price_dict.Count - 1; i++)
        {
            float change = UnityEngine.Random.Range(-0.5f, 0.5f);
            float temp = resource_price_dict[(RESOURCE_TYPE)i];
            resource_price_dict[(RESOURCE_TYPE)i] += temp * change;
        }
    }
}
