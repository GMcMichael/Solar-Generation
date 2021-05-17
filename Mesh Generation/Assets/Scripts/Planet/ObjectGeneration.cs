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


    private enum ResourceFertility {barren = 1, scarce, normal, rich, plentiful};//How many resource nodes will be in one clump of nodes
    [SerializeField]
    private ResourceFertility resourceFertility;
    [SerializeField]
    private int resourceDensity;
    private GameObject[] resources;
    private Vector3[] resourcePositions;
    private int[] resourceTypes;
    private int minResourceSpacing = 30;
    [SerializeField]
    private int resourceRotation = 2;

    void Awake()
    {
        mesh = GetComponent<CelestialBodyMeshGeneration>();
    }

    private void CreateArrays()
    {
        ObjectPositions = new Vector3[objectDensity];
        ObjectTypes = new int[objectDensity];
        resourcePositions = new Vector3[resourceDensity*(int)resourceFertility];
        resourceTypes = new int[resourceDensity*(int)resourceFertility];
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

    private void PlanResources() {//spawns resource groups on random land vertex
        int index = 0;
        for (int i = 0; i < resourceDensity; i++)
        {
            Vector3 newPos = GetNewResourcePosition(i*(int)resourceFertility);//makes sure the groups are far enough apart
            int resourceType = Random.Range(0, ResourcePrefabs.Count);
            //Vector3[] nextPoints = NearestPoints(newPos);
            for (int j = 0; j < (int)resourceFertility; j++)
            {
                //get direction from planet to newPos
                Vector3 Dir = transform.TransformPoint(newPos) - transform.position;
                //rotate the vector by a random x and y
                float[] angles = GetRotations();
                Vector3 rotatedPos = Quaternion.Euler(angles[0], angles[1], 0) * Dir;
                //add vector to planet pos with an offset
                Vector3 nextPos = transform.position + rotatedPos*1.1f;
                //Raycast to planet to get posititon
                RaycastHit hit;
                Ray ray = new Ray(nextPos, -Dir);
                if(GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) nextPos = transform.InverseTransformPoint(hit.point);
                resourcePositions[index] = nextPos;
                resourceTypes[index] = resourceType;
                index++;
            }
        }
    }

    private float[] GetRotations() {
        float[] angles = new float[2];
        angles[0] = Random.Range(-resourceRotation*1f, resourceRotation*1f);
        angles[1] = Random.Range(-resourceRotation*1f, resourceRotation*1f);
        if(angles[0] == 0 && angles[1] == 0) angles = GetRotations();
        return angles;
    }

    private Vector3 GetNewResourcePosition(int index) {
        Vector3 newPos = meshVertices[Random.Range(0, meshVertices.Length - 1)];
        for (int i = 0; i < index; i++)
        {
            if(Vector3.Distance(newPos, resourcePositions[i]) < minResourceSpacing)
                newPos = GetNewResourcePosition(index);
        }
        return newPos;
    }

    private void InstantiateObjects()//Could call this when I get to a planet
    {
        objects = new GameObject[objectDensity+(resourceDensity*(int)resourceFertility)];
        for (int i = 0; i < objectDensity; i++)
        {
            objects[i] = SpawnObject(0, ObjectPositions[i], ObjectTypes[i]);
        }
        for (int i = 0; i < (resourceDensity*(int)resourceFertility); i++)
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
        spawnPosition = transform.TransformPoint(spawnPosition);
        return Instantiate(prefab, spawnPosition, Quaternion.FromToRotation(transform.up, -(transform.position - spawnPosition)), transform);
    }

    private void DestroyObjects()//Could call this when I leave a planet
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
