using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PathMarker
{
    internal LocationOnTheMap location;
    internal float G;
    internal float H;
    internal float F;

    internal GameObject marker;
    internal PathMarker parent;

    internal PathMarker(LocationOnTheMap location, float g, float h, float f, GameObject marker, PathMarker parent)
    {
        this.location = location;
        this.G = g;
        this.H = h;
        this.F = f;
        this.marker = marker;
        this.parent = parent;
    }

    public override bool Equals(object other)
    {
        if ((other == null) || !this.GetType().Equals(other.GetType()))
        {
            return false;
        }
        else
        {
            return location.Equals(((PathMarker)other).location);
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }
}
