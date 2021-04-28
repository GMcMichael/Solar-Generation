using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneration : MonoBehaviour
{
    [SerializeField]
    private CelestialBodyMeshGeneration mesh;
    private Vector3[] meshVertices;
    [SerializeField]
    private List<GameObject> TerrainPrefabs;
    [SerializeField]
    private List<GameObject> ResourcePrefabs;

    //need to also have a way to store object data if I need it
    [SerializeField]
    private int objectDensity;
    private GameObject[] objects;
    private Vector3[] ObjectPositions;
    private int[] ObjectTypes;


    [SerializeField]
    private int resourceDensity;
    private GameObject[] resources;
    private Vector3[] resourcePositions;
    private int[] resourceTypes;

    void Awake()
    {
        mesh = GetComponent<CelestialBodyMeshGeneration>();
    }

    private void CreateArrays()
    {
        ObjectPositions = new Vector3[objectDensity];
        ObjectTypes = new int[objectDensity];
        resourcePositions = new Vector3[resourceDensity];
        resourceTypes = new int[resourceDensity];
    }

    public void Generate()
    {
        PlanObjects();
        InstantiateObjects();
    }

    private void PlanObjects()
    {
        CreateArrays();
        meshVertices = mesh.GetVertices();
        PlanTerrain();
        PlanResources();
    }

    private void PlanTerrain() {
        for (int i = 0; i < objectDensity; i++)
        {
            ObjectPositions[i] = meshVertices[Random.Range(0, meshVertices.Length - 1)];
            ObjectTypes[i] = Random.Range(0, TerrainPrefabs.Count);
        }
    }

    private void PlanResources() {//spawns resources on random land vertex
        for (int i = 0; i < resourceDensity; i++)
        {
            resourcePositions[i] = meshVertices[Random.Range(0, meshVertices.Length - 1)];
            resourceTypes[i] = Random.Range(0, ResourcePrefabs.Count);
        }
    }

    private void InstantiateObjects()//Need to call this when I get to a planet
    {
        objects = new GameObject[objectDensity+resourceDensity];
        for (int i = 0; i < objectDensity; i++)
        {
            objects[i] = SpawnObject(0, ObjectPositions[i], ObjectTypes[i]);
        }
        for (int i = 0; i < resourceDensity; i++)
        {
            objects[objectDensity+i] = SpawnObject(1, resourcePositions[i], resourceTypes[i]);
        }
    }

    private GameObject SpawnObject( int group, Vector3 spawnPosition, int type)
    {
        GameObject prefab = null;
        switch(group) {
            default:
                prefab = TerrainPrefabs[type];
                break;
            case 1:
                prefab = ResourcePrefabs[type];
                break;
        }
        spawnPosition = mesh.transform.TransformPoint(spawnPosition);
        return Instantiate(prefab, spawnPosition, Quaternion.FromToRotation(transform.up, -(transform.position - spawnPosition)), mesh.transform);
    }

    private void DestroyObjects()//need to call this when I leave a planet
    {
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }

    private void Rebuild()
    {
        if (objects.Length > 0) DestroyObjects();
        InstantiateObjects();
    }

    public void Randomized()
    {
        PlanObjects();
        Rebuild();
    }
}
