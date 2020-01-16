﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job
{
    public PROPERTY_TYPE type;
    public JOB_SUB_TYPE sub_type;
    public int work;
    public string job_name;
    public float time_left;
    public bool is_displayed;

    static int num = 0;

    //Constants
    //Orchards
    const int PRUNING_WORK = 10;
    const int FERTILIZING_WORK = 15;
    const int SPRAYING_WORK = 15;
    //Livestock
    const int WRANGLING_WORK = 10;
    const int CLEANING_WORK = 10;   //for aquaculture as well
    const int FEEDING_WORK = 15;    //for aquaculture as well
    const int DOCTORING_WORK = 10;
    //Aquaculture
    const int MAINTENANCE_WORK = 15;

    public Job(string p_type, string job_type, string name)
    {
        is_displayed = false;
        num++;
        SetType(p_type);
        SetSubType(job_type);
        this.job_name = name + " " + num;
        this.time_left = this.work;
    }
 
    public void SetType(string type)
    {
        ConvertType(type);
    }

    public void SetSubType(string sub_type)
    {
        ConvertSubType(sub_type);
    }

    void ConvertType(string type)
    {
        if(type.Contains("Orchard"))
        {
            //this.name = "orchard ";
            this.type = PROPERTY_TYPE.orchard;
        }else if (type.Contains("Livestock"))
        {
            //this.name = "livestock ";
            this.type = PROPERTY_TYPE.livestock;
        }else if (type.Contains("Aquaculture"))
        {
            //this.name = "aquaculture ";
            this.type = PROPERTY_TYPE.aquaculture;
        }else if (type.Contains("Electricity"))
        {
            //this.name = "electricity ";
            this.type = PROPERTY_TYPE.electricity;
        }
    }

    void ConvertSubType(string sub_type)
    {
        //Orchards
        if(sub_type == "pruning")
        {
            //this.name += "pruning " + num;
            this.sub_type = JOB_SUB_TYPE.pruning;
            this.work = PRUNING_WORK;
        }else if(sub_type == "fertilizing")
        {
            //this.name += "fertilizing " + num;
            this.sub_type = JOB_SUB_TYPE.fertilizing;
            this.work = FERTILIZING_WORK;
        }else if(sub_type == "spraying")
        {
            //this.name += "spraying " + num;
            this.sub_type = JOB_SUB_TYPE.spraying;
            this.work = SPRAYING_WORK;
        }//Livestock
        else if (sub_type == "wrangling")
        {
            //this.name += "wrangling " + num;
            this.sub_type = JOB_SUB_TYPE.wrangling;
            this.work = WRANGLING_WORK;
        }else if (sub_type == "cleaning")
        {
            //this.name += "cleaning " + num;
            this.sub_type = JOB_SUB_TYPE.cleaning;
            this.work = CLEANING_WORK;
        }else if (sub_type == "feeding")
        {
            //this.name += "feeding " + num;
            this.sub_type = JOB_SUB_TYPE.feeding;
            this.work = FEEDING_WORK;
        }else if (sub_type == "doctoring")
        {
            //this.name += "doctoring " + num;
            this.sub_type = JOB_SUB_TYPE.doctoring;
            this.work = DOCTORING_WORK;
        }//Aquaculture
        else if (sub_type == "maintenance")
        {
            //this.name += "maintenance " + num;
            this.sub_type = JOB_SUB_TYPE.maintenance;
            this.work = MAINTENANCE_WORK;
        }
    }

    public int GetWork()
    {
        return this.work;
    }
}
