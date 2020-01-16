using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bionic_Slot
{
    const int HEAD_WEIGHT = 10;
    const int EYES_WEIGHT = 5;
    const int MOUTH_WEIGHT = 5;
    const int TORSO_WEIGHT = 20;
    const int LEFT_FOOT_WEIGHT = 20;
    const int RIGHT_FOOT_WEIGHT = 20;
    const int LEFT_HAND_WEIGHT = 20;
    const int RIGHT_HAND_WEIGHT = 20;

    public BIONIC_TYPE type;
    public int weight_limit;

    public Bionic_Slot(BIONIC_TYPE t)
    {
        this.type = t;
        if(t == BIONIC_TYPE.head)
        {
            this.weight_limit = HEAD_WEIGHT;
        }else if (t == BIONIC_TYPE.eyes)
        {
            this.weight_limit = EYES_WEIGHT;
        }
        else if (t == BIONIC_TYPE.mouth)
        {
            this.weight_limit = MOUTH_WEIGHT;
        }
        else if (t == BIONIC_TYPE.torso)
        {
            this.weight_limit = TORSO_WEIGHT;
        }
        else if (t == BIONIC_TYPE.left_foot)
        {
            this.weight_limit = LEFT_FOOT_WEIGHT;
        }
        else if (t == BIONIC_TYPE.right_foot)
        {
            this.weight_limit = RIGHT_FOOT_WEIGHT;
        }
        else if (t == BIONIC_TYPE.left_hand)
        {
            this.weight_limit = LEFT_HAND_WEIGHT;
        }
        else if (t == BIONIC_TYPE.right_hand)
        {
            this.weight_limit = RIGHT_HAND_WEIGHT;
        }
    }
}
