using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Science
{
    public List<Science> prerequisites;
    public List<Science> unlockedNodes;
    public SCIENCE type;

    public Science(SCIENCE type)
    {
        this.prerequisites = new List<Science>();
        this.unlockedNodes = new List<Science>();
        this.type = type;
        AddPrerequisites();
        AddUnlockedNodes();
    }
    
    private void AddUnlockedNodes()
    {
        if(type == SCIENCE.orchards)
        {
            unlockedNodes.Add(new Science(SCIENCE.electricity));
        }
        else if (type == SCIENCE.electricity)
        {
            unlockedNodes.Add(new Science(SCIENCE.aquaculture));
        }
        else if (type == SCIENCE.aquaculture)
        {
            unlockedNodes.Add(new Science(SCIENCE.livestock));
        }
    }

    private void AddPrerequisites()
    {
        if (type == SCIENCE.electricity)
        {
            prerequisites.Add(new Science(SCIENCE.orchards));
        }
        else if (type == SCIENCE.livestock)
        {
            prerequisites.Add(new Science(SCIENCE.aquaculture));
        }
        else if (type == SCIENCE.aquaculture)
        {
            prerequisites.Add(new Science(SCIENCE.electricity));
        }
    }
}
