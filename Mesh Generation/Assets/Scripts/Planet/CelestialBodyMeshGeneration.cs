using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(ColorSettings))]
public class CelestialBodyMeshGeneration : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshCollider;
    MeshRenderer meshRenderer;
    private int radius;
    private List<List<int>> TriangleLayers = new List<List<int>>();
    private List<List<Vector3>> VertexLayers = new List<List<Vector3>>();

    [Range(0, 5)]
    public int layer;
    private int oldLayer;

    private ColorSettings colorSettings;
    private ColorGeneration colorGeneration;
    [SerializeField]
    private NoiseLayer[] noiseLayers;
    private NoiseFilter[] noiseFilters;
    private NoiseFilter[] oldNoiseFilters;
    public ElevationMinMax elevationMinMax;
    [SerializeField]
    private bool moveTerrain;
    [SerializeField]
    private bool RanomizeTerrain;
    [SerializeField]
    private bool RanomizeCenter;

    private ObjectGeneration objectGeneration;
    [SerializeField]
    private float LandRequirement = 5;
    private List<Vector3> LandVertices;

    [System.Serializable]
    private struct NoiseLayer
    {
        public bool enabled;
        public bool useFistAsMask;
        public NoiseSettings noiseSettings;
        public NoiseSettings oldNoiseSettings;
    }

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        objectGeneration = GetComponent<ObjectGeneration>();
        colorSettings = GetComponent<ColorSettings>();
        elevationMinMax = new ElevationMinMax();
        colorGeneration = new ColorGeneration(colorSettings, meshRenderer);
        noiseFilters = new NoiseFilter[noiseLayers.Length];
        for(int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilter.CreateNoiseFilter(noiseLayers[i].noiseSettings, noiseLayers[i].oldNoiseSettings);
        }
        meshRenderer.sharedMaterial = colorSettings.planetMaterial;
    }

    void Start()
    {
        GetLayers();
        GenerateMesh();
        GenerateColors();
        objectGeneration.Generate();
    }

    void Update()
    {//All of these are temporary and need to be removed because the player will never update the planets, just comment them out or remove them
        bool x = true;
        foreach(NoiseFilter nf in noiseFilters)
        {
            if(moveTerrain)
            {
                nf.moveTerrain();
            }
            if(RanomizeTerrain)
            {
                RanomizeTerrain = false;
                nf.RandomizeNoise();
            }
            if(RanomizeCenter)
            {
                RanomizeCenter = false;
                nf.RandomizeCenter();
            }
            if(!nf.Compare()) x = false;
        }
        if (!x)
        {
            GenerateMesh();
            objectGeneration.Randomized();
        }
        if (layer != oldLayer)
        {
            GenerateMesh();
            oldLayer = layer;
        }
        colorGeneration.CheckColorUpdate();
    }

    void GetLayers()
    {
        VertexLayers = IcosphereCreator.getVertexLayers();
        TriangleLayers = IcosphereCreator.getTriangleLayers();
    }

    void GenerateMesh()
    {
        elevationMinMax.Reset();
        LandVertices = new List<Vector3>();
        foreach (List<Vector3> vertices in VertexLayers)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = addNoise(vertices[i]);
            }
        }
        UpdatePlanetMesh(layer);
    }
    private Vector3 addNoise(Vector3 point)
    {
        point.Normalize();
        float firstLayer = 0;
        float elevation = 0;
        if(noiseFilters.Length > 0)
        {
            firstLayer = noiseFilters[0].Evaluate(point);
            if (noiseLayers[0].enabled) elevation = firstLayer;
        }
        for(int i = 1; i < noiseFilters.Length; i++)
        {
            if (noiseLayers[i].enabled)
            {
                float mask = (noiseLayers[i].useFistAsMask) ? firstLayer : 1;
                elevation += noiseFilters[i].Evaluate(point) * mask;
            }
        }
        elevation = radius * (1 + elevation);
        elevationMinMax.CheckElevation(elevation);
        Vector3 vertex = point * elevation;
        if ((elevation-radius) >= LandRequirement) LandVertices.Add(vertex);
        return vertex;
    }

    void GenerateColors()
    {
        colorGeneration.UpdateColors();
    }

    void UpdatePlanetMesh(int layer)
    {
        mesh.Clear();

        mesh.vertices = VertexLayers[layer].ToArray();
        mesh.triangles = TriangleLayers[layer].ToArray();
        
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
        colorGeneration.UpdateElevation(elevationMinMax);
    }

    public void Randomize()
    {
        //RanomizeTerrain = true;
        RanomizeCenter = true;
    }

    public Vector3[] GetVertices()
    {
        return LandVertices.ToArray();
    }

    public void setRadius(int _radius) {
        radius = _radius;
    }
}