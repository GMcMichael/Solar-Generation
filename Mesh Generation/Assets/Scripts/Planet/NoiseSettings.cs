using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterTypes
    {
        Simple, Rigid
    }
    public FilterTypes filterType;

    [Range(1, 10)]
    public int numLayers = 1;
    public float strength = 1;
    public float baseRoughness = 1;
    public float roughness = 2;
    public float resistance = 1;
    public float minElevation = 0;
    public Vector3 center = Vector3.zero;

    public float stepSize = 0.01f;
    public float lerpDuration = 3;
    private float timeElapsed;
    private float x;
    private float y;
    private float z;
    private float x2;
    private float y2;
    private float z2;

    //These are the range of the randomization.  THESE NEED TO BE CHANGED AND OPTOMIZED IN THE FUTURE, maybe just change the center and slightly change the other options
    private float minStrength = 0.2f;
    private float maxStrength = 0.8f;
    private float minBaseRoughness = 0.4f;
    private float maxBaseRoughness = 1;
    private float minRoughness = 2.5f;
    private float maxRoughness = 3.5f;
    private float minResistance = 0.4f;
    private float maxResistance = 0.6f;
    private float _minElevation = 0;
    private float maxElevation = 1;
    private float minCenter = -1000;
    private float maxCenter = 1000;

    [Header("Rigid Type Specific")]
    public float weightMultiplier = 0.8f;


    public bool Compare(NoiseSettings noiseSettings)
    {
        return ((numLayers == noiseSettings.numLayers) &&(strength == noiseSettings.strength) && (baseRoughness == noiseSettings.baseRoughness) && (roughness == noiseSettings.roughness) && (resistance == noiseSettings.resistance) && (minElevation == noiseSettings.minElevation) && (center.x == noiseSettings.center.x) && (center.y == noiseSettings.center.y) && (center.z == noiseSettings.center.z));
    }

    public void ReplaceValues(NoiseSettings newNoiseSettings)
    {
        this.numLayers = newNoiseSettings.numLayers;
        this.strength = newNoiseSettings.strength;
        this.baseRoughness = newNoiseSettings.baseRoughness;
        this.roughness = newNoiseSettings.roughness;
        this.resistance = newNoiseSettings.resistance;
        this.minElevation = newNoiseSettings.minElevation;
        this.center = newNoiseSettings.center;
    }

    public void RandomizeNoise()
    {
        numLayers = 5;
        strength = Random.Range(minStrength, maxStrength);
        baseRoughness = Random.Range(minBaseRoughness, maxBaseRoughness);
        roughness = Random.Range(minRoughness, maxRoughness);
        resistance = Random.Range(minResistance, maxResistance);
        minElevation = Random.Range(_minElevation, maxElevation);
        RandomizeCenter();
    }

    public void RandomizeCenter()
    {
        center = new Vector3(Random.Range(minCenter, maxCenter), Random.Range(minCenter, maxCenter), Random.Range(minCenter, maxCenter));
    }

    public void moveTerrain()
    {
        if(timeElapsed < lerpDuration)
        {
            x2 = Mathf.Lerp(0, x, timeElapsed / lerpDuration);
            y2 = Mathf.Lerp(0, y, timeElapsed / lerpDuration);
            z2 = Mathf.Lerp(0, z, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        } else
        {
            x2 = x;
            y2 = y;
            z2 = z;
            getNewValues();
        }
        center.x = x2;
        center.y = y2;
        center.z = z2;
    }

    public void getNewValues()
    {
        x = Random.Range(-stepSize, stepSize);
        y = Random.Range(-stepSize, stepSize);
        z = Random.Range(-stepSize, stepSize);
        timeElapsed = 0;
    }
}
