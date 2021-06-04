using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemSettings : MonoBehaviour
{
    static private string seed;
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
    public string _seed;
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
        GenerateSeed();
        _seed = seed;
    }

    private void GenerateSeed() {//for seep copy the getPlanets script, then have get planets just decrypt the seed
        //seed = the number of planets, the dist, the speed, then recursevly for each planet: the radius, speed and dist
        int numPlanets = getPlanetNum();
        int[] seedData = {
            numPlanets,
            getEvenDistance(numPlanets),
            getSpeed(numPlanets)
        };
        //reset the seed
        seed = "";
        //add main data to seed as a string
        foreach (int data in seedData)
        {
            seed += data + ",";
        }
        //add the data for each planet (radius, speed, dist)
        for (int i = 0; i < numPlanets; i++)
        {
            seed += getRadius() + "," + (minOrbitSpeed + (seedData[2]*(numPlanets-i))) + "," + (minDistFromCenter + (seedData[1]*(i+1)));
        }
    }
    static public List<GameObject> getPlanets()
    {
        //split seed into array at each comma and convert to int
        int[] seedData = System.Array.ConvertAll<string, int>(seed.Split(','), new System.Converter<string, int>(StringToInt));
        int numPlanets = seedData[0];
        int dist = seedData[1];
        int speed = seedData[2];
        List<GameObject> planets = new List<GameObject>();
        for(int i = 0; i < numPlanets; i++)
        {
            GameObject newPlanet = Instantiate(planetPrefab);
            newPlanet.GetComponent<CelestialBodyMeshGeneration>().setRadius(seedData[3*(i+1)]);
            newPlanet.GetComponent<CelestialBodyMeshGeneration>().Randomize();
            newPlanet.GetComponent<PlanetController>().setSpeed(seedData[4*(i+1)]);
            newPlanet.GetComponent<PlanetController>().setDist(seedData[5*(i+1)]);
            planets.Add(newPlanet);
        }
        return planets;
    }

    private static int StringToInt(string s) {
        return int.Parse(s);
    }

    public string GetSeed() {
        return seed;
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

    static public int getSpeed(int planets)
    {
        int speed = maxOrbitSpeed - minOrbitSpeed;//Random.Range(minOrbitSpeed, maxOrbitSpeed);
        return speed/planets;
    }
}
