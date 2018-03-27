using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class World : MonoBehaviour
{
    // CHARACTER
    public GameObject player;

    // ATLAS
    public Material textureAtlas;

    // SKYS
    public Material skyNormal;
    public Material skySnowy;
    public Material skyHell;
    public Material skyDreamy;

    // TREES
    public GameObject oakTree;
    public GameObject pineTree;
    public GameObject lollipopTree;
    public GameObject hellTree;

    // KEY
    public GameObject key;

    // MONSTERS
    public GameObject zombie;
    public GameObject littleMonster;/*
    public GameObject reaper;
    public GameObject ghost;
    public GameObject bear;
    public GameObject gnome;
    public GameObject snowMan;
    public GameObject yeti;*/

    // TYPES
    public enum TerrainTypes { PLAINS, HILLS, MOUNTAINS };
    public enum WorldTypes { NORMAL, SNOWY, HELL, DREAMY};

    // SIZES
    public static int mapSize = 320;
    public static int chunkSize = 32;

    // CURRENT TYPES
    public static WorldTypes worldType;
    private TerrainTypes terrainType;

    // TERRAIN VARIABLES
    private int maxHeight;
    private int octaves;
    private float smooth;
    private float persistence;

    // TERRAIN HEIGHTS
    public static int[,] surfaceHeights;

    // NUMBERS OF OBJECTS
    private int numberOfKeys = 3;
    private int numberOfMonsters = 30;
    private int numberOfTrees;

    // TREE MODEL
    private GameObject treeModel;

    // TERRAIN OBJECTS
    private Chunk[,] chunks;
    private GameObject[] keys;
    private GameObject[] monsters;
    private GameObject[] trees;


    public void Start()
    {
        player.SetActive(false);

        this.GenerateWorld();
        //this.SaveWorld("C:\\Users\\Arthur\\Desktop\\world.txt");
        //this.GenerateSavedWorld("C:\\Users\\Arthur\\Desktop\\world.txt");
    }


    public void Update()
    {
        
    }


    public void GenerateWorld()
    {
        // CHUNKS
        this.chunks = new Chunk[mapSize, mapSize];

        this.ChooseTerrainAndWorldType(Random.Range(0, 4), Random.Range(0, 3));
        this.ChooseTerrainValues();
        this.GenerateSurfaceHeights();
        this.GenerateTerrain();
        
        // KEYS
        this.keys = new GameObject[3];
        this.GenerateKeys();

        // MONSTERS
        this.monsters = new GameObject[30];
        this.GenerateMonsters();

        // TREES
        this.GenerateNumberOfTrees();
        this.GenerateTreeModel();
        this.trees = new GameObject[this.numberOfTrees];
        this.GenerateTrees();

        // PLAYER
        player.transform.position = new Vector3(mapSize / 2, surfaceHeights[mapSize / 2, mapSize / 2] + 1, mapSize / 2);
        player.SetActive(true);
    }


    public void GenerateSavedWorld(string fileName)
    {
        // CHUNKS
        this.chunks = new Chunk[mapSize, mapSize];

        // KEYS
        this.keys = new GameObject[this.numberOfKeys];

        // MONSTERS
        this.monsters = new GameObject[this.numberOfMonsters];

        // TREES
        this.GenerateNumberOfTrees();
        this.GenerateTreeModel();
        this.trees = new GameObject[this.numberOfTrees];

        try
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                // TERRAIN VALUES
                string worldTypeRead = reader.ReadLine();
                string terrainTypeRead = reader.ReadLine();
                this.maxHeight = int.Parse(reader.ReadLine());
                this.octaves = int.Parse(reader.ReadLine());
                this.smooth = float.Parse(reader.ReadLine());
                this.persistence = float.Parse(reader.ReadLine());

                int wType;
                int tType;

                // WORLD TYPE
                if (worldTypeRead == "NORMAL")
                    wType = 0;
                else if (worldTypeRead == "SNOWY")
                    wType = 1;
                else if (worldTypeRead == "HELL")
                    wType = 2;
                else
                    wType = 3;

                // TERRAIN TYPE
                if (terrainTypeRead == "PLAINS")
                    tType = 0;
                else if (terrainTypeRead == "HILLS")
                    tType = 1;
                else
                    tType = 2;

                this.ChooseTerrainAndWorldType(wType, tType);

                // GENERATE TERRAIN
                this.GenerateSurfaceHeights();
                this.GenerateTerrain();
                
                reader.ReadLine();

                // PLAYER
                string text = reader.ReadLine();
                string[] bits = text.Split(' ');
                player.transform.position = new Vector3(int.Parse(bits[0]), int.Parse(bits[1]), int.Parse(bits[2]));

                reader.ReadLine();

                // KEYS
                for(int i = 0; i < this.keys.Length; i++)
                {
                    text = reader.ReadLine();
                    bits = text.Split(' ');
                    this.keys[i] = Instantiate(this.key, new Vector3(int.Parse(bits[0]), int.Parse(bits[1]), int.Parse(bits[2])), Quaternion.identity) as GameObject;
                }

                reader.ReadLine();

                // MONSTERS
                for (int i = 0; i < this.monsters.Length; i++)
                {
                    text = reader.ReadLine();
                    bits = text.Split(' ');
                    this.monsters[i] = Instantiate(this.littleMonster, new Vector3(int.Parse(bits[0]), int.Parse(bits[1]), int.Parse(bits[2])), Quaternion.identity) as GameObject;
                }

                reader.ReadLine();

                // TREES
                for (int i = 0; i < this.trees.Length; i++)
                {
                    text = reader.ReadLine();
                    bits = text.Split(' ');
                    this.trees[i] = Instantiate(this.treeModel, new Vector3(int.Parse(bits[0]), int.Parse(bits[1]), int.Parse(bits[2])), Quaternion.identity) as GameObject;
                }

                player.SetActive(true);
            }
        }

        catch (System.Exception exception)
        {
            Debug.Log(exception.ToString());
        }
    }

    public void SaveWorld(string fileName)
    {
        try
        {
            if(File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // TERRAIN VALUES
                writer.WriteLine(worldType);
                writer.WriteLine(terrainType);
                writer.WriteLine(maxHeight);
                writer.WriteLine(octaves);
                writer.WriteLine(smooth);
                writer.WriteLine(persistence);

                writer.WriteLine();

                // PLAYER
                writer.WriteLine(player.transform.position.x + " " + player.transform.position.y + " " + player.transform.position.z);

                writer.WriteLine();

                // KEYS
                for (int i = 0; i < this.keys.Length; i++)
                    writer.WriteLine(this.keys[i].transform.position.x + " " + this.keys[i].transform.position.y + " " + this.keys[i].transform.position.z);

                writer.WriteLine();

                // MONSTERS
                for (int i = 0; i < this.monsters.Length; i++)
                    writer.WriteLine(this.monsters[i].transform.position.x + " " + this.monsters[i].transform.position.y + " " + this.monsters[i].transform.position.z);

                writer.WriteLine();

                // TREES
                for (int i = 0; i < this.trees.Length; i++)
                    writer.WriteLine(this.trees[i].transform.position.x + " " + this.trees[i].transform.position.y + " " + this.trees[i].transform.position.z);
            }
        }

        catch (System.Exception exception)
        {
            Debug.Log(exception.ToString());
        }
    }


    public void GenerateTerrain()
    {
        for (int x = 0; x < mapSize; x += chunkSize)
        {
            for (int z = 0; z < mapSize; z += chunkSize)
            {
                this.chunks[x, z] = new Chunk(new Vector3(x, 0, z), this.textureAtlas);
            }
        }
    }


    public void ChooseTerrainAndWorldType(int wType, int tType)
    {
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
            RenderSettings.fogDensity = 0.02f;
        }

        else if (wType == 2)
        {
            worldType = WorldTypes.HELL;

            RenderSettings.skybox = skyHell;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.01f;
        }

        else
        {
            worldType = WorldTypes.DREAMY;

            RenderSettings.skybox = skyDreamy;
        }

        if (tType == 0)
            this.terrainType = TerrainTypes.PLAINS;
        else if (tType == 1)
            this.terrainType = TerrainTypes.HILLS;
        else
            this.terrainType = TerrainTypes.MOUNTAINS;
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


    public Vector3 GenerateRandomVector(int distanceFromSpawn)
    {
        int x, z;

        do
        {
            x = Random.Range(0, mapSize);
            z = Random.Range(0, mapSize);
        } while (Mathf.Abs(mapSize/2 - x) < distanceFromSpawn && Mathf.Abs(mapSize/2 - z) < distanceFromSpawn);

        return new Vector3(x, surfaceHeights[x, z], z);
    }


    public void GenerateKeys()
    {
        for(int i = 0; i < this.keys.Length; i++)
        {
            this.keys[i] = Instantiate(this.key, this.GenerateRandomVector(75), Quaternion.identity) as GameObject;
        }
    }


    public void GenerateMonsters()
    {
        for (int i = 0; i < this.monsters.Length; i++)
        {
            this.monsters[i] = Instantiate(this.littleMonster, this.GenerateRandomVector(30), Quaternion.identity) as GameObject;
        }
    }


    public void GenerateNumberOfTrees()
    {
        if (World.worldType == World.WorldTypes.NORMAL)
            this.numberOfTrees = 1000;
        else if (World.worldType == World.WorldTypes.SNOWY)
            this.numberOfTrees = 1000;
        else if (World.worldType == World.WorldTypes.HELL)
            this.numberOfTrees = 300;
        else
            this.numberOfTrees = 500;
    }


    public void GenerateTreeModel()
    {
        if (World.worldType == World.WorldTypes.NORMAL)
            this.treeModel = this.oakTree;
        else if (World.worldType == World.WorldTypes.SNOWY)
            this.treeModel = this.pineTree;
        else if (World.worldType == World.WorldTypes.HELL)
            this.treeModel = this.hellTree;
        else
            this.treeModel = this.lollipopTree;
    }


    public void GenerateTrees()
    {
        for (int i = 0; i < this.trees.Length; i++)
        {
            this.trees[i] = Instantiate(this.treeModel, this.GenerateRandomVector(15), Quaternion.identity) as GameObject;
        }
    }
}
