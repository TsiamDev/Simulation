﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property
{
    public string name;
    public int value;
    public PROPERTY_TYPE type;
    public PROPERTY_SUB_TYPE sub_type;
    public int size;
    public HARVESTING_PERIOD harvestingPeriod;

    public static int num = 0;

    //constants
    const int ORCHARD_VALUE = 1000;
    const int LIVESTOCK_VALUE = 1500;
    const int ELECTRICITY_VALUE = 2000;
    const int AQUACULTURE_VALUE = 5000;

    public Property(string prop_type, string prop_sub_type, int size)
    {
        num++;
        this.name = prop_type + "(" + prop_sub_type + ") " + num;
        this.size = size;
        Convert_Type(prop_type);    //also defines value
        Convert_Sub_Type(prop_sub_type);
        //Enums.PROPERTY_TYPE.Parse(this.type, prop_type);
    }

    void Convert_Type(string prop_type)
    {
        if (prop_type.Equals("Orchard"))
        {
            this.type = PROPERTY_TYPE.orchard;
            this.value = size * ORCHARD_VALUE;
        }else if (prop_type.Equals("Livestock"))
        {
            this.type = PROPERTY_TYPE.livestock;
            this.value = size * LIVESTOCK_VALUE;
            this.harvestingPeriod = HARVESTING_PERIOD.year_round;
        }
        else if (prop_type.Equals("Electricity"))
        {
            this.type = PROPERTY_TYPE.electricity;
            this.value = size * ELECTRICITY_VALUE;
            this.harvestingPeriod = HARVESTING_PERIOD.year_round;
        }
        else if (prop_type.Equals("Aquaculture"))
        {
            this.type = PROPERTY_TYPE.aquaculture;
            this.value = size * AQUACULTURE_VALUE;
            this.harvestingPeriod = HARVESTING_PERIOD.year_round;
        }
    }

    void Convert_Sub_Type(string prop_sub_type)
    {
        //Orchards
        if (prop_sub_type.Equals("pear"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.pear;
            this.harvestingPeriod = HARVESTING_PERIOD.autumn;
        }
        else if (prop_sub_type.Equals("apple"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.apple;
            this.harvestingPeriod = HARVESTING_PERIOD.autumn;
        }
        else if (prop_sub_type.Equals("plum"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.plum;
            this.harvestingPeriod = HARVESTING_PERIOD.spring;
        }
        else if (prop_sub_type.Equals("citrus"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.citrus;
            this.harvestingPeriod = HARVESTING_PERIOD.year_round;
        }
        else if (prop_sub_type.Equals("olive"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.olive;
            this.harvestingPeriod = HARVESTING_PERIOD.autumn;
        }
        else if (prop_sub_type.Equals("date"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.date;
            this.harvestingPeriod = HARVESTING_PERIOD.autumn;
        }
        else if (prop_sub_type.Equals("fig"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.fig;
            this.harvestingPeriod = HARVESTING_PERIOD.summer;
        }
        else if (prop_sub_type.Equals("orange"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.orange;
            this.harvestingPeriod = HARVESTING_PERIOD.year_round;
        }
        else if (prop_sub_type.Equals("pecan"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.pecan;
            this.harvestingPeriod = HARVESTING_PERIOD.autumn;
        }
        else if (prop_sub_type.Equals("cashews"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.cashews;
            this.harvestingPeriod = HARVESTING_PERIOD.winter;
        }
        else if (prop_sub_type.Equals("almonds"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.almonds;
            this.harvestingPeriod = HARVESTING_PERIOD.summer;
        }
        //Electricity
        else if (prop_sub_type.Equals("wind"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.wind;
        }
        else if (prop_sub_type.Equals("solar"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.solar;
        }
        //Livestock
        else if (prop_sub_type.Equals("cattle"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.cattle;
        }
        else if (prop_sub_type.Equals("goat"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.goat;
        }
        else if (prop_sub_type.Equals("sheep"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.sheep;
        }
        else if (prop_sub_type.Equals("swine"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.swine;
        }
        else if (prop_sub_type.Equals("poultry"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.poultry;
        }
        //Aquaponics
        else if (prop_sub_type.Equals("carp"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.carp;
        }
        else if (prop_sub_type.Equals("salmon"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.salmon;
        }
        else if (prop_sub_type.Equals("tilapia"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.tilapia;
        }
        else if (prop_sub_type.Equals("catfish"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.catfish;
        }
        else if (prop_sub_type.Equals("microalgea"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.microalgea;
        }
        else if (prop_sub_type.Equals("shrimp"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.shrimp;
        }
        else if (prop_sub_type.Equals("prawn"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.prawn;
        }
        else if (prop_sub_type.Equals("oyster"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.oyster;
        }
        else if (prop_sub_type.Equals("mussel"))
        {
            this.sub_type = PROPERTY_SUB_TYPE.mussel;
        }
    }
}