using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGeneration
{//If I cant see the planet materal in the game view, go to Window->Rendering->Lighting Settings->Environment and make sure "Profile" is selected and "Static Lighting Sky" is "HDRISky"
    ColorSettings colorSettings;
    Texture2D texture;
    const int textureResolution = 50;
    private MaterialPropertyBlock _propBlock;
    private MeshRenderer meshRenderer;

    public ColorGeneration(ColorSettings colorSettings, MeshRenderer meshRenderer)
    {
        this.colorSettings = colorSettings;
        texture = new Texture2D(textureResolution, 1);
        this.meshRenderer = meshRenderer;
        _propBlock = new MaterialPropertyBlock();
    }

    public void UpdateElevation(ElevationMinMax elevationMinMax)
    {
        meshRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetVector("elevationMinMax", new Vector4(elevationMinMax.getMin(), elevationMinMax.getMax()));
        meshRenderer.SetPropertyBlock(_propBlock);
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            colors[i] = colorSettings.PlanetColors.Evaluate(i/(textureResolution-1f));
        }
        texture.SetPixels(colors);
        texture.Apply();
        colorSettings.planetMaterial.SetTexture("planetTexture", texture);
    }

    public void CheckColorUpdate()
    {
        if (colorSettings.CheckColorUpdate()) UpdateColors();
    }
}
