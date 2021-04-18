using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemGenerator : MonoBehaviour
{
    private List<GameObject> planets;
    [SerializeField]
    private float angleStep = 0.0001f;
    [SerializeField]
    private float rotateStep = 1;
    [SerializeField]
    private bool disableOrbits = false;

    // Start is called before the first frame update
    void Start()
    {
        planets = SolarSystemSettings.getPlanets();
        GameObject.Find("Poppins").GetComponent<ShipManager>().AddWarpObjects(planets.ToArray());
        MovePlanets();
        RotatePlanets();
    }

    private void FixedUpdate()
    {
        if(disableOrbits) return;
        MovePlanets();
        RotatePlanets();
    }

    private void MovePlanets()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            PlanetController planetController = planets[i].GetComponent<PlanetController>();
            planetController.addAngle(angleStep);
            float[] coords = findCoords(planetController.getAngle(), planetController.getDist());
            planets[i].transform.position = new Vector3(coords[0], planets[i].transform.position.y,  coords[1]);
        }
    }

    private void RotatePlanets()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            planets[i].transform.Rotate(new Vector3(0, rotateStep, 0));
        }
    }

    private float[] findCoords(float angle, int dist)
    {
        float[] coords = new float[2];
        coords[0] = Mathf.Cos(angle)*dist;//x
        coords[1] = Mathf.Sin(angle)*dist;//z
        return coords;
    }

    public int numPlanets()
    {
        return planets.Count;
    }

    public GameObject[] getPlanets()
    {
        return planets.ToArray();
    }
}
