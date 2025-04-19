using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastedSpell 
{
    public enum Type
    {
        WaterBolt,
        FireBolt,
        Undefined
    }

    public Type type;
    public override bool Equals(object obj)
    {
        

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        CastedSpell otherSpell = (CastedSpell)obj;

        if (otherSpell.type == type)
        {
            return true;
        }
        return false;
    }



}
