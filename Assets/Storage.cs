using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public int amount;  //how many storage units of this type exist
    public RESOURCE_TYPE rt;
    public int size;
    //public int cost;
    public float held_resource_amount;

    const int SIZE = 1000;
    //const int COST = 1000;

    public Storage(int amount, RESOURCE_TYPE rt)
    {
        this.amount = amount;
        this.rt = rt;
        this.size = SIZE;
        this.held_resource_amount = 0f;
    }
}
