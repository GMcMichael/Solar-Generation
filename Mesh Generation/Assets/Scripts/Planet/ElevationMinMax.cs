using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevationMinMax
{
    private float Min;
    private float Max;

    public ElevationMinMax()
    {
        Reset();
    }

    public void CheckElevation(float elevation)
    {
        if (elevation > Max) Max = elevation;
        if (elevation < Min) Min = elevation;
    }

    public void Reset()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public float getMin()
    {
        return Min;
    }

    public float getMax()
    {
        return Max;
    }
}
