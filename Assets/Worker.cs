using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker
{
    public List<Bionic_Slot> slots;
    public bool is_working;
    public string name;
    public int id;
    public bool is_hired;
    public string job_name;

    static int num = 0;

    public Worker()
    {
        //TODO randomly generate names
        this.name = "Worker " + num;
        this.id = num;
        num++;

        this.job_name = "";
        this.is_hired = false;
        this.is_working = false;
        slots = new List<Bionic_Slot>();
        slots.Add(new Bionic_Slot(BIONIC_TYPE.head));
        slots.Add(new Bionic_Slot(BIONIC_TYPE.eyes));
        slots.Add(new Bionic_Slot(BIONIC_TYPE.torso));
        slots.Add(new Bionic_Slot(BIONIC_TYPE.left_foot));
        slots.Add(new Bionic_Slot(BIONIC_TYPE.right_foot));
        slots.Add(new Bionic_Slot(BIONIC_TYPE.left_hand));
        slots.Add(new Bionic_Slot(BIONIC_TYPE.right_hand));
    }
}
