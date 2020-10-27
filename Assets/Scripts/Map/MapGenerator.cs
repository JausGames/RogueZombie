using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] List<GameObject> tilesObj;
    [SerializeField] List<MapTile> tiles;
    [SerializeField] Transform map;
    [SerializeField] int size = 4;
    [SerializeField] MapTile[,] tileMatrix = new MapTile[4, 4];
    [SerializeField] float[,] rotMatrix = new float[4, 4];
    [SerializeField] float tileSize = 80f;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject tile in tilesObj)
        {
            tiles.Add(tile.GetComponent<MapTile>());
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var rnd = Random.Range(0, tilesObj.Count);
                if (j == 0 && i == 0)
                {
                    var inst = Instantiate(tilesObj[2], new Vector3(tileSize * i, 0f, tileSize * j), Quaternion.AngleAxis(-180f, Vector3.up), map);
                    tileMatrix[0, 0] = inst.GetComponent<MapTile>();
                    tileMatrix[0, 0].RotateConnectors();
                    tileMatrix[0, 0].RotateConnectors();

                    rotMatrix[i, j] = 0f;
                }
                else if (j == 0 && i != 0)
                {
                    var inst = Instantiate(tilesObj[rnd], new Vector3(tileSize * i, 0f, tileSize * j), Quaternion.identity, map);
                    tileMatrix[i, j] = inst.GetComponent<MapTile>();
                    rotMatrix[i, j] = 0f;
                }
                else
                {
                    var done = false;
                    Debug.Log("MapGenerator, Awake : i = " + i + ", j = " + j);
                    Debug.Log("MapGenerator, Awake : tileMatrix = " + tileMatrix[i, j - 1]);
                    var conectors = tileMatrix[i, j - 1].GetConnectors();
                    for (int k = 0; k < 4; k++)
                    {
                        if (tiles[rnd].GetConnectors()[1] == conectors[3] && !done)
                        {
                            done = true;
                            var inst = Instantiate(tilesObj[rnd], new Vector3(tileSize * i, 0f, tileSize * j), Quaternion.AngleAxis(-90f * (float)k, Vector3.up), map);
                            tileMatrix[i, j] = inst.GetComponent<MapTile>();
                            rotMatrix[i, j] = -90f * (float)k;
                        }
                        else if (!done)
                        { 
                            tiles[rnd].RotateConnectors();
                            if (k == 3)
                            {
                                Debug.Log("MapGenerator, Awake : last chance for " + i + ", j = " + j);
                                var inst = Instantiate(tilesObj[rnd], new Vector3(tileSize * i, 0f, tileSize * j), Quaternion.AngleAxis(-90f * (float)k, Vector3.up), map);
                                tileMatrix[i, j] = inst.GetComponent<MapTile>();
                                rotMatrix[i, j] = -90f * (float)k;
                            }
                        }

                    }
                }

            }
        }
        surface.BuildNavMesh();
    }
}
