using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public int amount;
    public RESOURCE_TYPE rt;
    public int size;
    //public int cost;

    const int SIZE = 1000;
    //const int COST = 1000;

    public Storage(int amount, RESOURCE_TYPE rt)
    {
        this.amount = amount;
        this.rt = rt;
        this.size = SIZE;
    }
}
