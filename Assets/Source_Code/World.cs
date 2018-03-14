using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World : MonoBehaviour
{
    // Characters
    public GameObject player;

    // Atlas
    public Material textureAtlas;

    // Skys
    public Material skyNormal;
    public Material skySnowy;
    public Material skyHell;
    public Material skyDreamy;

    // Trees
    public GameObject oakTree;
    public GameObject pineTree;
    public GameObject lollipopTree;

    public enum TerrainTypes { PLAINS, HILLS, MOUNTAINS };
    public enum WorldTypes { NORMAL, SNOWY, HELL, DREAMY};

    public static int mapSize = 320;
    public static int chunkSize = 32;

    public static WorldTypes worldType;
    private TerrainTypes terrainType;

    private int maxHeight;
    private int octaves;
    private float smooth;
    private float persistence;

    public static int[,] surfaceHeights;


    public void Start()
    {
        this.ChooseTerrainAndWorldType();
        this.ChooseTerrainValues();
        this.GenerateSurfaceHeights();
        this.GenerateTerrain();

        player.transform.position = new Vector3(mapSize / 2, surfaceHeights[mapSize / 2, mapSize / 2] + 1, mapSize / 2);
        player.SetActive(true);
    }


    public void Update()
    {
        
    }


    public void GenerateTerrain()
    {
        for (int x = 0; x < mapSize; x += chunkSize)
        {
            for (int z = 0; z < mapSize; z += chunkSize)
            {
                Chunk chunk = new Chunk(new Vector3(x, 0, z), this.textureAtlas, oakTree, pineTree, lollipopTree);
            }
        }
    }


    public void ChooseTerrainAndWorldType()
    {
        int tType = Random.Range(0, 3);
        int wType = Random.Range(0, 4);

        if (tType == 0)
            this.terrainType = TerrainTypes.PLAINS;
        else if (tType == 1)
            this.terrainType = TerrainTypes.HILLS;
        else
            this.terrainType = TerrainTypes.MOUNTAINS;

        if (wType == 0)
        {
            worldType = WorldTypes.NORMAL;

            RenderSettings.skybox = skyNormal;
        }

        else if (wType == 1)
        {
            worldType = WorldTypes.SNOWY;

            RenderSettings.skybox = skySnowy;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = Color.white;
            RenderSettings.fogDensity = 0.05f;

        }

        else if (wType == 2)
        {
            worldType = WorldTypes.HELL;

            RenderSettings.skybox = skyHell;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.03f;
        }

        else
        {
            worldType = WorldTypes.DREAMY;

            RenderSettings.skybox = skyDreamy;
        }
    }


    public void ChooseTerrainValues()
    {
        if (this.terrainType == TerrainTypes.PLAINS)
        {
            this.maxHeight = 50;
            this.smooth = Random.Range(0.007f, 0.009f);
        }

        else if (this.terrainType == TerrainTypes.HILLS)
        {
            this.maxHeight = 100;
            this.smooth = Random.Range(0.009f, 0.011f);
        }

        else
        {
            this.maxHeight = 150;
            this.smooth = Random.Range(0.011f, 0.013f);
        }

        this.octaves = Random.Range(3, 5);
        this.persistence = 0.5f;
    }


    public void GenerateSurfaceHeights()
    {
        surfaceHeights = new int[mapSize, mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
                surfaceHeights[x, z] = this.GenerateHeight(x, z);
        }
    }


    public int GenerateHeight(float x, float z)
    {
        return (int)Mathf.Lerp(0, maxHeight, Mathf.InverseLerp(0, 1, Noise(x * smooth, z * smooth, octaves, persistence)));
    }


    public float Noise(float x, float z, int octaves, float persistence)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        float offset = 32000f;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise((x + offset) * frequency, (z + offset) * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }

        return total / maxValue;
    }
}
