using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
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
}
