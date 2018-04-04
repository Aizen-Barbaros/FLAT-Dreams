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
    public Material skyMetal;
    public Material skyCheese;
    public Material skyAutumn;
    public Material skyTropical;
    public Material skyRotting;
    public Material skyMatrix;

    // TREES
    public GameObject oakTree;
    public GameObject pineTree;
    public GameObject lollipopTree;
    public GameObject hellTree;
    public GameObject autumnTree;
    public GameObject rottingTree;
    public GameObject palmtree;

    // KEY
    public GameObject key;

    // MONSTERS
    public GameObject littleMonster;
    public GameObject snowMan;
    public GameObject reaper;
    public GameObject bear;
    public GameObject ghost;
    public GameObject gnome;
    public GameObject yeti;
    public GameObject zombie;
    public GameObject agentSmith;

    // TYPES
    public enum TerrainTypes { PLAINS, HILLS, MOUNTAINS };
    public enum WorldTypes { NORMAL, SNOWY, HELL, DREAMY, METAL, CHEESE, AUTUMN, ROTTING, TROPICAL, MATRIX };

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

    // MODELS
    private GameObject treeModel;
    private GameObject monsterModel;

    // TERRAIN OBJECTS
    private Chunk[,] chunks;
    private GameObject[] keys;
    private GameObject[] monsters;
    private GameObject[] trees;


    public void Start()
    {   
        this.GenerateWorld();
        //this.SaveWorld("C:\\Users\\Arthur\\Desktop\\world.txt");
        //this.GenerateSavedWorld("C:\\Users\\Arthur\\Desktop\\world.txt");
        //this.DeleteWorld();
    }


    public void FixedUpdate()
    {
        /*
        if (player.gameObject.GetComponent<Player>().GetCurrentLives() == 0)
        {
            player.SetActive(false);
            Debug.Log("Dead");
        }
        */
    }


    public void GenerateWorld()
    {
        // CHUNKS
        this.chunks = new Chunk[mapSize / chunkSize, mapSize / chunkSize];

        this.GenerateWorldValues(Random.Range(0, 10), Random.Range(0, 3));
        this.GenerateSurfaceHeights();
        this.GenerateTerrain();
        
        // KEYS
        this.keys = new GameObject[3];
        this.GenerateKeys();

        // MONSTERS
        this.monsters = new GameObject[30];
        this.GenerateMonsters();

        // TREES
        this.trees = new GameObject[this.numberOfTrees];
        this.GenerateTrees();

        // PLAYER
        player.transform.position = this.GenerateRandomVector(0, 1);
        player.SetActive(true);
    }


    public void GenerateSavedWorld(string fileName)
    {
        // CHUNKS
        this.chunks = new Chunk[mapSize / chunkSize, mapSize / chunkSize];

        // KEYS
        this.keys = new GameObject[this.numberOfKeys];

        // MONSTERS
        this.monsters = new GameObject[this.numberOfMonsters];

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
                else if (worldTypeRead == "DREAMY")
                    wType = 3;
                else if (worldTypeRead == "METAL")
                    wType = 4;
                else if (worldTypeRead == "CHEESE")
                    wType = 5;
                else if (worldTypeRead == "AUTUMN")
                    wType = 6;
                else if (worldTypeRead == "ROTTING")
                    wType = 7;
                else if (worldTypeRead == "TROPICAL")
                    wType = 8;
                else
                    wType = 9;

                // TERRAIN TYPE
                if (terrainTypeRead == "PLAINS")
                    tType = 0;
                else if (terrainTypeRead == "HILLS")
                    tType = 1;
                else
                    tType = 2;

                // GENERATE TERRAIN
                this.GenerateWorldValues(wType, tType);
                this.GenerateSurfaceHeights();
                this.GenerateTerrain();
                
                reader.ReadLine();

                // PLAYER
                string text = reader.ReadLine();
                string[] coordinates = text.Split(' ');
                player.transform.position = new Vector3(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2]));

                reader.ReadLine();

                // KEYS
                for(int i = 0; i < this.keys.Length; i++)
                {
                    text = reader.ReadLine();
                    coordinates = text.Split(' ');
                    this.keys[i] = Instantiate(this.key, new Vector3(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2])), Quaternion.identity) as GameObject;
                }

                reader.ReadLine();

                // MONSTERS
                for (int i = 0; i < this.monsters.Length; i++)
                {
                    text = reader.ReadLine();
                    coordinates = text.Split(' ');
                    this.monsters[i] = Instantiate(this.littleMonster, new Vector3(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2])), Quaternion.identity) as GameObject;
                }

                reader.ReadLine();

                // TREES
                if (this.numberOfTrees > 0)
                {
                    this.trees = new GameObject[this.numberOfTrees];

                    for (int i = 0; i < this.trees.Length; i++)
                    {
                        text = reader.ReadLine();
                        coordinates = text.Split(' ');
                        this.trees[i] = Instantiate(this.treeModel, new Vector3(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2])), Quaternion.identity) as GameObject;
                    }
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
                if (this.numberOfTrees > 0)
                {
                    for (int i = 0; i < this.trees.Length; i++)
                        writer.WriteLine(this.trees[i].transform.position.x + " " + this.trees[i].transform.position.y + " " + this.trees[i].transform.position.z);
                }
            }
        }

        catch (System.Exception exception)
        {
            Debug.Log(exception.ToString());
        }
    }

    public void DeleteWorld()
    {
        player.SetActive(false);

        for (int i = 0; i < this.keys.Length; i++)
            GameObject.Destroy(this.keys[i]);

        for (int i = 0; i < this.monsters.Length; i++)
            GameObject.Destroy(this.monsters[i]);

        for (int i = 0; i < this.trees.Length; i++)
            GameObject.Destroy(this.trees[i]);

        for (int x = 0; x < this.chunks.GetLength(0); x += 1)
        {
            for (int z = 0; z < chunks.GetLength(1); z += 1)
                this.chunks[x, z].DeleteChunk();
        }
    }


    public void GenerateWorldValues(int wType, int tType)
    {
        if (wType == 0)
        {
            worldType = WorldTypes.NORMAL;

            this.monsterModel = littleMonster;
            this.numberOfTrees = 1200;
            this.treeModel = this.oakTree;

            RenderSettings.skybox = skyNormal;
        }

        else if (wType == 1)
        {
            worldType = WorldTypes.SNOWY;

            this.monsterModel = this.snowMan;
            this.numberOfTrees = 1200;
            this.treeModel = this.pineTree;

            RenderSettings.skybox = skySnowy;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = Color.white;
            RenderSettings.fogDensity = 0.02f;
        }

        else if (wType == 2)
        {
            worldType = WorldTypes.HELL;

            this.monsterModel = this.reaper;
            this.numberOfTrees = 300;
            this.treeModel = this.hellTree;

            RenderSettings.skybox = skyHell;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.01f;
        }

        else if (wType == 3)
        {
            worldType = WorldTypes.DREAMY;

            this.monsterModel = this.bear;
            this.numberOfTrees = 500;
            this.treeModel = this.lollipopTree;

            RenderSettings.skybox = skyDreamy;
        }

        else if (wType == 4)
        {
            worldType = WorldTypes.METAL;

            this.monsterModel = this.ghost;
            this.numberOfTrees = 0;
            this.treeModel = null;

            RenderSettings.skybox = skyMetal;
        }

        else if (wType == 5)
        {
            worldType = WorldTypes.CHEESE;

            this.monsterModel = this.gnome;
            this.numberOfTrees = 0;
            this.treeModel = null;

            RenderSettings.skybox = skyCheese;
        }

        else if (wType == 6)
        {
            worldType = WorldTypes.AUTUMN;

            this.monsterModel = this.yeti;
            this.numberOfTrees = 1000;
            this.treeModel = this.autumnTree;

            RenderSettings.skybox = skyAutumn;
        }

        else if (wType == 7)
        {
            worldType = WorldTypes.ROTTING;

            this.monsterModel = this.zombie;
            this.numberOfTrees = 300;
            this.treeModel = this.rottingTree;

            RenderSettings.skybox = skyRotting;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Exponential;
            RenderSettings.fogColor = Color.green;
            RenderSettings.fogDensity = 0.1f;
        }

        else if (wType == 8)
        {
            worldType = WorldTypes.TROPICAL;

            this.monsterModel = this.littleMonster;
            this.numberOfTrees = 500;
            this.treeModel = this.palmtree;

            RenderSettings.skybox = skyTropical;
        }

        else
        {
            worldType = WorldTypes.MATRIX;

            this.monsterModel = this.agentSmith;
            this.numberOfTrees = 0;
            this.treeModel = null;

            RenderSettings.skybox = skyMatrix;
        }

        if (tType == 0)
        {
            this.terrainType = TerrainTypes.PLAINS;
            this.maxHeight = 50;
            this.smooth = Random.Range(0.007f, 0.009f);
        }

        else if (tType == 1)
        {
            this.terrainType = TerrainTypes.HILLS;
            this.maxHeight = 100;
            this.smooth = Random.Range(0.009f, 0.011f);
        }

        else
        {
            this.terrainType = TerrainTypes.MOUNTAINS;
            this.maxHeight = 150;
            this.smooth = Random.Range(0.011f, 0.013f);
        }

        this.octaves = Random.Range(3, 5);
        this.persistence = 0.5f;
    }


    public void GenerateTerrain()
    {
        for (int x = 0; x < this.chunks.GetLength(0); x += 1)
        {
            for (int z = 0; z < this.chunks.GetLength(1); z += 1)
            {
                this.chunks[x, z] = new Chunk(new Vector3(x, 0, z), this.textureAtlas);
            }
        }
    }


    public void GenerateSurfaceHeights()
    {
        surfaceHeights = new int[mapSize, mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
                surfaceHeights[x, z] = (int)Mathf.Lerp(0, this.maxHeight, Mathf.InverseLerp(0, 1, Noise(x * this.smooth, z * this.smooth)));
        }
    }


    public float Noise(float x, float z)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        float offset = 32000f;

        for (int i = 0; i < this.octaves; i++)
        {
            total += Mathf.PerlinNoise((x + offset) * frequency, (z + offset) * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= this.persistence;
            frequency *= 2;
        }

        return total / maxValue;
    }


    public void GenerateKeys()
    {
        for(int i = 0; i < this.keys.Length; i++)
        {
            this.keys[i] = Instantiate(this.key, this.GenerateRandomVector(75, 0), Quaternion.identity) as GameObject;
        }
    }


    public void GenerateMonsters()
    {
        for (int i = 0; i < this.monsters.Length; i++)
        {
            this.monsters[i] = Instantiate(this.monsterModel, this.GenerateRandomVector(30, 1), Quaternion.identity) as GameObject;
        }
    }


    public void GenerateTrees()
    {
        for (int i = 0; i < this.trees.Length; i++)
        {
            this.trees[i] = Instantiate(this.treeModel, this.GenerateRandomVector(15, 0), Quaternion.identity) as GameObject;
        }
    }


    public Vector3 GenerateRandomVector(int distanceFromSpawn, int offset)
    {
        int x, z;

        do
        {
            x = Random.Range(0, mapSize);
            z = Random.Range(0, mapSize);
        } while (Mathf.Abs(mapSize / 2 - x) < distanceFromSpawn && Mathf.Abs(mapSize / 2 - z) < distanceFromSpawn);

        return new Vector3(x, surfaceHeights[x, z] + offset, z);
    }
}
