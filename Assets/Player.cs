using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Enums
{
    int land;
    int water;
    public int money;
    public List<Property> properties;

    public Player(int land, int water,int money)
    {
        this.money = money;
        this.land = land;
        this.water = water;
        properties = new List<Property>();
        //properties.Add(new Property("apple_orchard", PROPERTY_TYPE.orchard, 5));
    }
    
}
