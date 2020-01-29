using System.Collections;
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
    public bool is_harvesting_job;
    public Property property;

    static int num = 0;

    //Constants
    const int SCIENCE_WORK = 10;
    const int HARVESTING_WORK = 10; //for orchards,livestock and aquaculture
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

    public Job(string p_type, string job_type, string name, Property property)
    {
        this.is_harvesting_job = false;
        this.is_displayed = false;
        num++;
        SetType(p_type);
        SetSubType(job_type);
        this.job_name = name + " " + num;
        this.time_left = this.work;
        this.property = property;
    }
 
    public void CalcWorkDone()
    {
        float percentage = 0f;   //THe denominator is the totla number of jobs that have to occur before harvesting
        if(this.type == PROPERTY_TYPE.orchard)
        {
            percentage = (float)(1f / 3f); 
        }else if (this.type == PROPERTY_TYPE.livestock)
        {
            percentage = (float)(1f / 4f);
        }else if (this.type == PROPERTY_TYPE.aquaculture)
        {
            percentage = (float)(1f / 3f);
        }

        foreach (JOB_SUB_TYPE job in this.property.work_done.Keys)
        {
            if(this.property.work_done[job] == true)
            {
                this.property.work_completed += percentage;
            }
        }
    }

    public void WorkDone()
    {
        if (this.sub_type != JOB_SUB_TYPE.harvesting)
        {
            if(this.property.work_done.ContainsKey(this.sub_type) == true)
            {
                this.property.work_done[this.sub_type] = true;
            }
            else
            {
                this.property.work_done.Add(this.sub_type, true);
            }
            
        }
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
        }else if (type.Contains("Science"))
        {
            this.type = PROPERTY_TYPE.science;
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
        }else if (sub_type == "harvesting")
        {
            this.sub_type = JOB_SUB_TYPE.harvesting;
            this.work = HARVESTING_WORK;
            this.is_harvesting_job = true;
        }else if (sub_type.Contains("science"))
        {
            this.sub_type = JOB_SUB_TYPE.science;
            this.work = SCIENCE_WORK;
        }
    }

    public int GetWork()
    {
        return this.work;
    }
}
