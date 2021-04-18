using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : NoiseFilter
{

    public RigidNoiseFilter(NoiseSettings noiseSettings, NoiseSettings oldNoiseSettings)
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
        float weight = 1;
        for (int i = 0; i < noiseSettings.numLayers; i++)
        {
            float v = 1-Mathf.Abs(noise.Evaluate(point * frequency + noiseSettings.center));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * noiseSettings.weightMultiplier);
            noiseValue += v * amplitude;
            frequency *= noiseSettings.roughness;
            amplitude *= noiseSettings.resistance;
        }
        noiseValue = Mathf.Max(0, noiseValue - noiseSettings.minElevation);
        return noiseValue * noiseSettings.strength;
    }
}
