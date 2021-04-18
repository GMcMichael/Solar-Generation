using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoiseFilter
{

    protected Noise noise = new Noise();
    protected NoiseSettings noiseSettings;
    protected NoiseSettings oldNoiseSettings;

    public static NoiseFilter CreateNoiseFilter(NoiseSettings noiseSettings, NoiseSettings oldNoiseSettings)
    {
        switch(noiseSettings.filterType)
        {
            case NoiseSettings.FilterTypes.Simple:
                return new SimpleNoiseFilter(noiseSettings, oldNoiseSettings);
            case NoiseSettings.FilterTypes.Rigid:
                return new RigidNoiseFilter(noiseSettings, oldNoiseSettings);
        }
        Debug.Log("Noise Type not found during creation: " + noiseSettings.filterType);
        return null;
    }

    public abstract float Evaluate(Vector3 point);

    public bool Compare()
    {
        bool x = noiseSettings.Compare(oldNoiseSettings);
        oldNoiseSettings.ReplaceValues(noiseSettings);
        return x;
    }

    public void moveTerrain()
    {
        noiseSettings.moveTerrain();
    }

    public void RandomizeNoise()
    {
        noiseSettings.RandomizeNoise();
    }

    public void RandomizeCenter()
    {
        noiseSettings.RandomizeCenter();
    }
}
