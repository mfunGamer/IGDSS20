using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public Texture2D heightMap;

    public GameObject forestTile;
    public GameObject sandTile;
    public GameObject mountainTile;
    public GameObject waterTile;
    public GameObject grassTile;
    public GameObject stoneTile;

    const int tileWidth = 10;

    // Start is called before the first frame update
    void Start()
    {
        loadTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void loadTiles()
    {
        Debug.Log("Heightmap dimensions: "+heightMap.width + "x" + heightMap.height);

        //Remove the showcasing tiles
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Tile");
        foreach(GameObject prefab in prefabs)
        {
            Destroy(prefab);
        }

        Color current;

        float shiftZBy = heightMap.width * tileWidth / 2f;

        //Only every second row is added to the total x;
        float shiftXBy = heightMap.height * tileWidth / 3f + tileWidth;

        for (int x = 0; x < heightMap.height; x++)
        {
            for(int z = 0; z < heightMap.width; z++)
            {
                current = heightMap.GetPixel(x, z);

                float max = current.maxColorComponent;

                GameObject toLoad;
                if (max == 0) toLoad = waterTile;
                else if (max <= 0.2) toLoad = sandTile;
                else if (max <= 0.4) toLoad = grassTile;
                else if (max <= 0.6) toLoad = forestTile;
                else if (max <= 0.8) toLoad = stoneTile;
                else if (max <= 1) toLoad = mountainTile;
                else throw new Exception("Error in parsing height map. Recieved max rgb value >1: "+max+". On tile: "+x+", "+z);
                

                Instantiate(toLoad, new Vector3(x * tileWidth * 0.75f - shiftXBy, current.grayscale * tileWidth * 2, z * tileWidth + x % 2 * tileWidth / 2f - shiftZBy), Quaternion.identity);
            }
        }


    }


    public float getMaxX()
    {
        return 10f;
    }

    public float getMaxZ()
    {
        return 10f;
    }

    public float getMinX()
    {
        return -10f;
    }

    public float getMinZ()
    {
        return -10f;
    }

}
