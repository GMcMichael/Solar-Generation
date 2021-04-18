using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneration : MonoBehaviour
{
    [SerializeField]
    private CelestialBodyMeshGeneration mesh;
    [SerializeField]
    private int objectDensity;
    private GameObject[] objects;
    private Vector3[] ObjectPositions;
    private int[] ObjectTypes;
    private Vector3[] meshVertices;
    [SerializeField]
    private List<GameObject> prefabs;
    //need to also have a way to store object data if I need it
    void Awake()
    {
        mesh = GetComponent<CelestialBodyMeshGeneration>();
    }

    private void CreateArrays()
    {
        ObjectPositions = new Vector3[objectDensity];
        ObjectTypes = new int[objectDensity];
    }

    public void Generate()
    {
        PlanObjects();
        InstantiateObjects();
    }

    public void PlanObjects()
    {
        CreateArrays();
        meshVertices = mesh.GetVertices();
        for (int i = 0; i < objectDensity; i++)
        {
            ObjectPositions[i] = meshVertices[Random.Range(0, meshVertices.Length - 1)];
            ObjectTypes[i] = Random.Range(0, prefabs.Count);
        }
    }

    public void InstantiateObjects()//Need to call this when I get to a planet
    {
        objects = new GameObject[objectDensity];
        for (int i = 0; i < objectDensity; i++)
        {
            objects[i] = SpawnObject(ObjectPositions[i], ObjectTypes[i]);
        }
    }

    private GameObject SpawnObject(Vector3 spawnPosition, int type)
    {
        spawnPosition = mesh.transform.TransformPoint(spawnPosition);
        return Instantiate(prefabs[type], spawnPosition, Quaternion.FromToRotation(transform.up, -(transform.position - spawnPosition)), mesh.transform);
    }

    public void DestroyObjects()//need to call this when I leave a planet
    {
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }

    public void Rebuild()
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
