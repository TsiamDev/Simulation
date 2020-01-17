﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    int land;
    int water;
    public int money;
    public List<Property> properties;
    public List<Job> jobs;
    public List<Worker> workers;
    public List<Storage> storages;

    public Player(int land, int water,int money)
    {
        this.money = money;
        this.land = land;
        this.water = water;
        properties = new List<Property>();
        //properties.Add(new Property("apple_orchard", PROPERTY_TYPE.orchard, 5));
        jobs = new List<Job>();
        workers = new List<Worker>();
        storages = new List<Storage>();
    }
    
}
