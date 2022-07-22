using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class LocationOnTheMap
{
    internal float x;
    internal float y;

    internal LocationOnTheMap(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    internal Vector2 ToVector()
    {
        return new Vector2(x, y);
    }

    public static LocationOnTheMap operator +(LocationOnTheMap a, LocationOnTheMap b) => new LocationOnTheMap(a.x + b.x, a.y + b.y);

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return this.x == ((LocationOnTheMap)obj).x && this.y == ((LocationOnTheMap)obj).y;
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }
}
