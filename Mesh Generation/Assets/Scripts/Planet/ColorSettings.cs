using UnityEngine;

public class ColorSettings : MonoBehaviour
{
    public Gradient PlanetColors;
    public Material planetMaterial;
    private GradientColorKey[] oldColorKeys;
    private GradientMode oldMode;

    private void Awake()
    {
        oldColorKeys = PlanetColors.colorKeys;
        oldMode = PlanetColors.mode;
    }

    public bool CheckColorUpdate()
    {
        GradientColorKey[] newColorKeys = PlanetColors.colorKeys;
        if (newColorKeys.Length != oldColorKeys.Length || oldMode != PlanetColors.mode)
        {
            resetCheck();
            return true;
        }
        for (int i = 0; i < oldColorKeys.Length; i++)
        {
            if (!oldColorKeys[i].Equals(newColorKeys[i]))
            {
                resetCheck();
                return true;
            }
        }
        return false;
    }

    private void resetCheck()
    {
        oldColorKeys = PlanetColors.colorKeys;
        oldMode = PlanetColors.mode;
    }
}
