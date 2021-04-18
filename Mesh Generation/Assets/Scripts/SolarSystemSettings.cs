using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemSettings : MonoBehaviour
{
    static private GameObject planetPrefab;
    static private int minPlanets;
    static private int maxPlanets;
    static private int minDistFromCenter;
    static private int maxDistFromCenter;
    static private int minRadius;
    static private int maxRadius;
    static private int minOrbitSpeed;
    static private int maxOrbitSpeed;

    public GameObject _planetPrefab;
    public int _minPlanets = 4;
    public int _maxPlanets = 7;
    public int _minDistFromCenter = 1000;
    public int _maxDistFromCenter = 10000;
    public int _minRadius = 100;
    public int _maxRadius = 200;
    public int _minOrbitSpeed = 1;
    public int _maxOrbitSpeed = 10;

    public struct Planet
    {
        public GameObject planet;
        public int radius;
        public int speed;
        public int distFromCenter;

        public Planet(GameObject planet, int radius, int speed, int distFromCenter)
        {
            this.planet = planet;
            this.radius = radius;
            this.speed = speed;
            this.distFromCenter = distFromCenter;
        }
    }

    private void Awake()
    {
        planetPrefab = _planetPrefab;
        minPlanets = _minPlanets;
        maxPlanets = _maxPlanets;
        minDistFromCenter = _minDistFromCenter;
        maxDistFromCenter = _maxDistFromCenter;
        minRadius = _minRadius;
        maxRadius = _maxRadius;
        minOrbitSpeed = _minOrbitSpeed;
        maxOrbitSpeed = _maxOrbitSpeed;
    }

    static public int getPlanetNum()
    {
        return Random.Range(minPlanets, maxPlanets);
    }

    static public int getEvenDistance(int planets)
    {
        int dist = maxDistFromCenter - minDistFromCenter;
        return dist/planets;
    }

    static public int getRadius()
    {
        int radius = Random.Range(minRadius, maxRadius);
        return radius;
    }

    static public int getSpeed()
    {
        int speed = Random.Range(minOrbitSpeed, maxOrbitSpeed);
        return speed;
    }

    static public List<GameObject> getPlanets()
    {
        int numPlanets = getPlanetNum();
        int dist = getEvenDistance(numPlanets);
        List<GameObject> planets = new List<GameObject>();
        for(int i = 0; i < numPlanets; i++)
        {
            GameObject newPlanet = Instantiate(planetPrefab);
            newPlanet.GetComponent<CelestialBodyMeshGeneration>().radius = getRadius();
            newPlanet.GetComponent<CelestialBodyMeshGeneration>().Randomize();
            newPlanet.GetComponent<PlanetController>().setSpeed(getSpeed());
            newPlanet.GetComponent<PlanetController>().setDist(dist*(i+1));
            planets.Add(newPlanet);
        }
        return planets;
    }
}
