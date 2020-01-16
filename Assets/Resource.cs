using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    public RESOURCE_TYPE type;
    public int amount;

    public Resource(RESOURCE_TYPE type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }
}
