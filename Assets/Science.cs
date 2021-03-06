﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Science
{
    public List<SCIENCE> prerequisites;
    public List<SCIENCE> unlockedNodes;
    public SCIENCE type;

    public Science(SCIENCE type)
    {
        this.prerequisites = new List<SCIENCE>();
        this.unlockedNodes = new List<SCIENCE>();
        this.type = type;
        AddPrerequisites();
        AddUnlockedNodes();
    }

    private void AddUnlockedNodes()
    {
        if (type == SCIENCE.orchards)
        {
            unlockedNodes.Add(SCIENCE.electricity);
        }else if (type == SCIENCE.electricity)
        {
            unlockedNodes.Add(SCIENCE.aquaculture);
        }else if (type == SCIENCE.aquaculture)
        {
            unlockedNodes.Add(SCIENCE.livestock);
        }else if (type == SCIENCE.fertilizing)
        {
            unlockedNodes.Add(SCIENCE.spraying);
        }else if (type == SCIENCE.spraying)
        {
            unlockedNodes.Add(SCIENCE.doctoring);
        }
    }

    private void AddPrerequisites()
    {
        if (type == SCIENCE.electricity)
        {
            prerequisites.Add(SCIENCE.orchards);
        }else if (type == SCIENCE.livestock)
        {
            prerequisites.Add(SCIENCE.aquaculture);
        }else if (type == SCIENCE.aquaculture)
        {
            prerequisites.Add(SCIENCE.electricity);
        }else if (type == SCIENCE.spraying)
        {
            prerequisites.Add(SCIENCE.fertilizing);
        }else if (type == SCIENCE.doctoring)
        {
            prerequisites.Add(SCIENCE.spraying);
        }
    }
}
