using System.Collections.Generic;
using UnityEngine;

//Creates normalized layers of icospheres so I can pull the layers that are generated once instead of generating them every time
public class IcosphereCreator : MonoBehaviour
{
    private static List<List<Vector3>> VertexLayers = new List<List<Vector3>>();
    private static List<List<int>> TriangleLayers = new List<List<int>>();
    private static List<Vector3> newVertices = new List<Vector3>();
    private static List<int> newTriangles = new List<int>();
    private static List<List<Triangle>> refiningTriangleLayers = new List<List<Triangle>>();
    private static Dictionary<string, int> trianglePoints = new Dictionary<string, int>(); //vector3 hashcode, vertices location for tirangle array
    private static List<Chunk> Chunks;
    private static int refineCount = 5;
    [SerializeField]
    [Range(1, 10)]
    private int RefineCount = 5;

    public static List<Vector3> getVertexLayer(int layer)
    {
        return VertexLayers[layer];
    }
    public static List<int> getTriangleLayer(int layer)
    {
        return TriangleLayers[layer];
    }

    public static List<List<Vector3>> getVertexLayers()
    {
        return VertexLayers;
    }
    public static List<List<int>> getTriangleLayers()
    {
        return TriangleLayers;
    }

    private struct Triangle
    {
        public Vector3[] vertices;
        public Triangle(Vector3[] vs)
        {
            vertices = vs;
        }
    }

    private struct Chunk
    {
        private List<List<Triangle>> chunkTriangleLayers;

        public Chunk(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            List<Vector3> ownVertices = new List<Vector3>()
            {
                v1, v2, v3
            };
            //fix the vertices position by noramilzing them
            for (int i = 0; i < ownVertices.Count; i++)
            {
                ownVertices[i] = ownVertices[i].normalized;
            }
            List<int> ownTriangles = new List<int>()
            {
                0, 1, 2
            };
            chunkTriangleLayers = new List<List<Triangle>>();
            chunkTriangleLayers.Add(new List<Triangle>() { new Triangle(ownVertices.ToArray()) });
        }

        public void RefineChunk()
        {
            List<Triangle> newStructs = new List<Triangle>();
            //set old lists to last of chunk layer
            List<Triangle> oldStructs = chunkTriangleLayers[chunkTriangleLayers.Count - 1];
            for (int i = 0; i < oldStructs.Count; i++)
            {
                Vector3[] vs = oldStructs[i].vertices;
                //create a vector3 List and integer array of length 6 to hold all the vertices
                List<Vector3> tempVertices = new List<Vector3>();
                List<Vector3> addingVertices = new List<Vector3>();
                int[] verticesIndex = new int[6];
                //Get the original 3 vertices and normalize them, check each one for if they already exist
                for (int j = 0; j < 3; j++)
                {
                    tempVertices.Add(vs[j].normalized);
                    int temp;
                    if (trianglePoints.TryGetValue(GetHashCode(tempVertices[j]), out temp))
                    {
                        //already exists, get index
                        verticesIndex[j] = temp;
                    }
                    else
                    {
                        //doesnt exist, create it and add index
                        addingVertices.Add(tempVertices[j]);
                        verticesIndex[j] = (IcosphereCreator.newVertices.Count - 1) + addingVertices.Count;
                        trianglePoints.Add(GetHashCode(tempVertices[j]), verticesIndex[j]);
                    }
                }
                //get the middle points of the originals
                tempVertices.Add(getMiddlePoint(tempVertices[0], tempVertices[1]).normalized);
                tempVertices.Add(getMiddlePoint(tempVertices[1], tempVertices[2]).normalized);
                tempVertices.Add(getMiddlePoint(tempVertices[2], tempVertices[0]).normalized);
                //check if they exist
                for (int j = 3; j < 6; j++)
                {
                    int temp;
                    if (trianglePoints.TryGetValue(GetHashCode(tempVertices[j]), out temp))
                    {
                        //they exist, add index
                        verticesIndex[j] = temp;
                    }
                    else
                    {
                        //doesnt exist, create it and add index
                        addingVertices.Add(tempVertices[j]);
                        verticesIndex[j] = (IcosphereCreator.newVertices.Count - 1) + addingVertices.Count;
                        trianglePoints.Add(GetHashCode(tempVertices[j]), verticesIndex[j]);
                    }
                }
                //triangle int array of the four new triangles
                int[] addingTriangles = new int[]
                {
                    verticesIndex[0], verticesIndex[3], verticesIndex[5],
                    verticesIndex[3], verticesIndex[1], verticesIndex[4],
                    verticesIndex[5], verticesIndex[4], verticesIndex[2],
                    verticesIndex[3], verticesIndex[4], verticesIndex[5]
                };
                IcosphereCreator.newVertices.AddRange(addingVertices);
                IcosphereCreator.newTriangles.AddRange(addingTriangles);
                newStructs.AddRange(new Triangle[]
                {
                    new Triangle(new Vector3[] {tempVertices[0], tempVertices[3], tempVertices[5]}),
                    new Triangle(new Vector3[] { tempVertices[3], tempVertices[1], tempVertices[4]}),
                    new Triangle(new Vector3[] { tempVertices[5], tempVertices[4], tempVertices[2]}),
                    new Triangle(new Vector3[] { tempVertices[3], tempVertices[4], tempVertices[5]})
                });
            }
            //save the new chunk vertices and triangles to the layers list
            chunkTriangleLayers.Add(newStructs);
        }

        string GetHashCode(Vector3 v)
        {
            string hash = "" + v.x + "," + v.y + "," + v.z;
            return hash;
        }

        Vector3 getMiddlePoint(Vector3 v1, Vector3 v2)
        {
            return new Vector3((v1.x + v2.x) / 2, (v1.y + v2.y) / 2, (v1.z + v2.z) / 2);
        }
    }

    void Awake()
    {
        refineCount = RefineCount;
        GenerateMesh();
    }

    private static void GenerateMesh()
    {
        CreateIcosahedron();
        clearLists();
        for (int i = 0; i < refineCount; i++)
        {
            foreach (Chunk chunk in Chunks)
            {
                chunk.RefineChunk();
            }
            //save the planet layers
            List<Vector3> temp = new List<Vector3>();
            List<int> temp2 = new List<int>();
            temp.AddRange(newVertices);
            temp2.AddRange(newTriangles);
            VertexLayers.Add(temp);
            TriangleLayers.Add(temp2);
            clearLists();
        }
    }

    private static void clearLists()
    {
        newVertices.Clear();
        newTriangles.Clear();
        trianglePoints.Clear();
    }

    private static void CreateIcosahedron()//Golden radtio is 1 : (1+sqrt(5))/2
    {
        int radius = 10;
        double halfWidth = radius / 2;
        double halfLength = (radius + 2.2360679774) / 4;//2.2360679774 is the square root of 5, divide by 4 instead of 2 cause to get half I have to divide by 2 again
        List<Vector3> vertices = new List<Vector3>()
        {
            //top layer
            new Vector3 (0,(float)halfWidth,(float)-halfLength),
            new Vector3 (0,(float)halfWidth,(float)halfLength),
            //middle top layer
            new Vector3 ((float)halfWidth,(float)halfLength,0),
            new Vector3 ((float)-halfWidth,(float)halfLength,0),
            //middle layer
            new Vector3 ((float)halfLength,0,(float)-halfWidth),
            new Vector3 ((float)-halfLength,0,(float)-halfWidth),
            new Vector3 ((float)-halfLength,0,(float)halfWidth),
            new Vector3 ((float)halfLength,0,(float)halfWidth),
            //middle bottom layer
            new Vector3 ((float)halfWidth,(float)-halfLength,0),
            new Vector3 ((float)-halfWidth,(float)-halfLength,0),
            //bottom layer
            new Vector3 (0,(float)-halfWidth,(float)-halfLength),
            new Vector3 (0,(float)-halfWidth,(float)halfLength)
        };
        //fix the vertices position by noramilzing them
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = vertices[i].normalized;
        }
        //create the triangle array
        List<int> triangles = new List<int>()
        {
            0, 1, 2,//0
            0, 3, 1,//1
            0, 2, 4,//2
            0, 4, 5,//3
            3, 0, 5,//4
            3, 5, 9,//5
            3, 9, 6,//6
            1, 3, 6,//7
            1, 6, 7,//8
            1, 7, 2,//9
            2, 7, 8,//10
            2, 8, 4,//11
            4, 8, 10,//12
            4, 10, 5,//13
            5, 10, 9,//14
            6, 9, 11,//15
            6, 11, 7,//16
            7, 11, 8,//17
            8, 11, 10,//18
            9, 10, 11//19
        };
        //save the layers
        VertexLayers.Add(vertices);
        TriangleLayers.Add(triangles);
        //create the chunk array
        Chunks = new List<Chunk>();
        for (int i = 0; i < triangles.Count / 3; i++)
        {
            Chunks.Add(new Chunk(vertices[triangles[i * 3]], vertices[triangles[(i * 3) + 1]], vertices[triangles[(i * 3) + 2]]));
        }
        /*List<Triangle> RefiningTriangles = new List<Triangle>();
        for(int i = 0; i < (triangles.Count/3); i++)
        {
            Vector3[] vs = new Vector3[] { vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]] };
            RefiningTriangles.Add(new Triangle(vs));
        }
        refiningTriangleLayers.Add(RefiningTriangles);*/
    }

    /*void MapUvs()//I think Uvs are still broken
    {
        Mesh uvMesh;
        List<Vector2> uvTemp;
        //loop through each mesh and generate a uv layer
        for (int i = 0; i < VertexLayers.Count; i++)
        {
            uvMesh = new Mesh();
            uvTemp = new List<Vector2>();
            int[] uvMeshTriangles = TriangleLayers[i].ToArray();
            uvMesh.vertices = VertexLayers[i].ToArray();
            uvMesh.triangles = uvMeshTriangles;
            uvTemp.AddRange(UnityEditor.Unwrapping.GeneratePerTriangleUV(uvMesh));
            Vector2[] addingUvs = new Vector2[uvMesh.vertices.Length];
            for (int j = 0; j < uvMesh.triangles.Length; j++)
            {
                addingUvs[uvMeshTriangles[i]] = uvTemp[i];
            }
            uvTemp.Clear();
            uvTemp.AddRange(addingUvs);
            UVLayers.Add(uvTemp);
        }
    }*/
}
