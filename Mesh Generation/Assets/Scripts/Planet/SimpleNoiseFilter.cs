using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : NoiseFilter
{

    public SimpleNoiseFilter(NoiseSettings noiseSettings, NoiseSettings oldNoiseSettings)
    {
        noiseSettings.getNewValues();
        this.noiseSettings = noiseSettings;
        this.oldNoiseSettings = oldNoiseSettings;
    }

    public override float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = noiseSettings.baseRoughness;
        float amplitude = 1;
        for (int i = 0; i < noiseSettings.numLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + noiseSettings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= noiseSettings.roughness;
            amplitude *= noiseSettings.resistance;
        }
        noiseValue = Mathf.Max(0, noiseValue - noiseSettings.minElevation);
        return noiseValue * noiseSettings.strength;
    }
}
