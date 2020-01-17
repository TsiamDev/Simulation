using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IF YOU ADD HERE also add at Property.cs at the convert_type & convert_sub_type
//and also in Game.cs in OnValueChanged
public enum PROPERTY_TYPE : int
{
    orchard,
    livestock,
    aquaculture,
    electricity
}

public enum PROPERTY_SUB_TYPE : int
{
    //Orchards
    pear,
    apple,
    plum,
    citrus,
    olive,
    date,
    fig,
    orange,
    pecan,
    cashews,
    almonds,
    //Electricity
    wind,
    solar,
    //Livestock
    cattle,
    goat,
    sheep,
    swine,
    poultry,
    //Aquaculture
    carp,
    salmon,
    tilapia,
    catfish,
    microalgea,
    shrimp,
    prawn,
    oyster,
    mussel
}

public enum JOB_SUB_TYPE : int
{
    harvesting, //this is for orchards, livestock and aquaculture
    //Orchards
    pruning,
    fertilizing,
    spraying,
    //Livestock
    wrangling,
    cleaning,   //this is for aquaculture as well
    feeding,    //this is for aquaculture as well
    doctoring,
    //Aquaculture
    maintenance
}    

public enum RESOURCE_TYPE : int
{
    //Orchards
    pear,
    apple,
    plum,
    citrus,
    olive,
    date,
    fig,
    orange,
    pecan,
    cashews,
    almonds,
    //Livestock
    cattle_meat,
    goat_meat,
    sheep_meat,
    swine_meat,
    poultry_meat,
    //Aquaculture
    carp,
    salmon,
    tilapia,
    catfish,
    microalgea,
    shrimp,
    prawn,
    oyster,
    mussel,
    //Electricity
    electricity
}

public enum BIONIC_TYPE : int
{
    head,
    eyes,
    mouth,
    torso,
    left_foot,
    right_foot,
    left_hand,
    right_hand
}

public enum SEASONS : int
{
    winter,
    spring,
    summer,
    autumn
}

public enum HARVESTING_PERIOD : int
{
    winter,
    spring,
    summer,
    autumn,
    year_round
}