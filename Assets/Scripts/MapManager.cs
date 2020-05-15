using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    public Texture2D heightMap;

    public GameObject forestTile;
    public GameObject sandTile;
    public GameObject mountainTile;
    public GameObject waterTile;
    public GameObject grassTile;
    public GameObject stoneTile;

    private List<GameObject> tiles;

    private float tileWidth = 0;
    private float tileDepth = 0;

    //The factor applied to the grayscale to set the y value of each tile
    const int heightFactor = 20;



    // Start is called before the first frame update
    void Start()
    {
        tiles = new List<GameObject>() { forestTile, sandTile, mountainTile, waterTile, grassTile, stoneTile };
        LoadTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void LoadTiles()
    {
        Debug.Log("Heightmap dimensions: "+heightMap.width + "x" + heightMap.height);

        //Remove the showcasing tiles
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject prefab in prefabs)
        {
            Destroy(prefab);
        }

        //Calculate the bounds of the tiles and ensure all are of equal size
        foreach (GameObject o in tiles)
        {

            Vector3 collider = o.GetComponent<Collider>().bounds.size;
            Vector3 renderer = o.GetComponent<Renderer>().bounds.size;

            float maxWidth = Math.Max(collider.z, renderer.z);
            float maxDepth = Math.Max(collider.x, renderer.x);

            if (tileWidth == 0) tileWidth = maxWidth;
            else if (tileWidth != maxWidth) throw new Exception("The tiles are not of equal width. " + o.name + " is " + maxWidth + " while previously iterated tiles where " + tileWidth + " wide.");

            if (tileDepth == 0) tileDepth = maxDepth;
            else if (tileDepth != maxDepth) throw new Exception("The tiles are not of equal depth. " + o.name + " is " + maxDepth + " while previously iterated tiles where " + tileDepth + " deep.");

        }

        Color current;

        //Shift by half a row width
        float shiftZBy = (heightMap.width * tileWidth / 2f);

        //Shift by half the map width times the added tileDepth per row;
        float shiftXBy = heightMap.height * tileDepth * 0.75f / 2f;

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
                

                Instantiate(toLoad, new Vector3(x * tileDepth * 0.75f - shiftXBy, current.grayscale * heightFactor, z * tileWidth + x % 2 * tileWidth / 2f - shiftZBy), Quaternion.identity);
            }
        }


    }


    public float GetMaxX()
    {
        return 10f;
    }

    public float GetMaxZ()
    {
        return 10f;
    }

    public float GetMinX()
    {
        return -10f;
    }

    public float GetMinZ()
    {
        return -10f;
    }

}
