using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class World : MonoBehaviour
{
    // CANVAS
    public Canvas escapeMenu;

    // GO
    public GameObject loading;

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

    // HEART
    public GameObject heart;

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
    private int numberOfMonsters = 50;
    private int numberOfTrees;

    // MODELS
    private GameObject treeModel;
    private GameObject monsterModel;

    // TERRAIN OBJECTS
    private Chunk[,] chunks;
    private GameObject[] keys;
    private GameObject[] monsters;
    private GameObject[] trees;
    private GameObject life;

    // LEVEL
    private int level;
    private float normalSpeed;

    // NAME OF THE FILE CONTAINING THE PATH TO THE SAVING FILE
    private string fileName;

    // VERIFIES IF THE PLAYER PAUSED THE GAME OR NOT
    private bool playerIsFrozen;


    public void Start()
    {
        Cursor.visible = false;
        escapeMenu.enabled = false;
        loading.SetActive(false);

        try
        {
            using (StreamReader reader = new StreamReader("fileName.txt"))
                this.fileName = reader.ReadLine();
        }

        catch (System.Exception exception)
        {
            Debug.Log(exception.ToString());
        }

        this.level = 1;
        this.normalSpeed = 5.0f;
        this.playerIsFrozen = false;

        // If the file already existed, it means that the program as the generate a save world
        if (File.Exists(this.fileName))
            this.GenerateSavedWorld(this.fileName);

        // Else it has to create a new one
        else
            this.GenerateWorld();
    }


    public void Update()
    {
        if (player.activeSelf == true) // Only check that if the player is active
        {
            // Update the level, lives, keys and spell values in the HUD
            GameObject.Find("Level").GetComponent<Text>().text = this.level.ToString();
            GameObject.Find("Lives").GetComponent<Text>().text = player.GetComponentInChildren<Player>().GetCurrentLives().ToString();
            GameObject.Find("Keys").GetComponent<Text>().text = player.GetComponentInChildren<Player>().GetKeyCaught().ToString();

            GameObject.Find("SpeedBoost").GetComponent<Text>().text = (player.GetComponentInChildren<Player>().GetSpeedBoostTimeBeforeNext() == 0) ? " " : Mathf.Ceil(player.GetComponentInChildren<Player>().GetSpeedBoostTimeBeforeNext()).ToString();
            GameObject.Find("Dash").GetComponent<Text>().text = (player.GetComponentInChildren<Player>().GetDashTimeBeforeNext() == 0) ? " " : Mathf.Ceil(player.GetComponentInChildren<Player>().GetDashTimeBeforeNext()).ToString();
            GameObject.Find("Rocket").GetComponent<Text>().text = (player.GetComponentInChildren<Player>().GetRocketTimeBeforeNext() == 0) ? " " : Mathf.Ceil(player.GetComponentInChildren<Player>().GetRocketTimeBeforeNext()).ToString();
            GameObject.Find("Fog").GetComponent<Text>().text = (player.GetComponentInChildren<Player>().GetFogTimeBeforeNext() == 0) ? " " : Mathf.Ceil(player.GetComponentInChildren<Player>().GetFogTimeBeforeNext()).ToString();
            GameObject.Find("Stun").GetComponent<Text>().text = (player.GetComponentInChildren<Player>().GetStunTimeBeforeNext() == 0) ? " " : Mathf.Ceil(player.GetComponentInChildren<Player>().GetStunTimeBeforeNext()).ToString();

            // If the player paused the game
            if (player.GetComponentInChildren<Player>().GetIsFrozen() == true)
            {
                Cursor.visible = true;
                this.playerIsFrozen = true;

                for (int i = 0; i < this.numberOfMonsters; i++)
                    this.monsters[i].GetComponentInChildren<Enemy>().SetIsFrozen(true);

                escapeMenu.enabled = true;
            }

            // If the player unpaused the game but the actual monsters and stuff haven't regain mobility yet
            else if (player.GetComponentInChildren<Player>().GetIsFrozen() == false && this.playerIsFrozen == true)
            {
                Cursor.visible = false;
                this.playerIsFrozen = false;

                for (int i = 0; i < this.numberOfMonsters; i++)
                    this.monsters[i].GetComponentInChildren<Enemy>().SetIsFrozen(false);

                escapeMenu.enabled = false;
            }

            // Verifies firt if the loading screen is active
            if (loading.activeSelf)
            {
                // If the player got all 3 keys
                if (player.GetComponentInChildren<Player>().GetKeyCaught() == this.numberOfKeys)
                {
                    player.GetComponentInChildren<Player>().SetKeyCaught(0);    // Set the number of key caught to zero
                    this.level++;                                               // Increment the level
                    this.normalSpeed += 0.75f;                                  // Increase the speed of the characters

                    this.DeleteWorld();                                         // Delete the current world
                    this.GenerateWorld();                                       // Generate a new one

                    loading.SetActive(false);                                   // Remove the loading screen
                }

                // If the player got caught but still has at least a life
                else if ((player.GetComponentInChildren<Player>().GetCaught() == true || player.transform.position.y < -100) && player.GetComponentInChildren<Player>().GetCurrentLives() - 1 > 0)
                {
                    player.GetComponentInChildren<Player>().SetCaught(false);   // Set the player as uncaught
                    player.GetComponentInChildren<Player>().SetKeyCaught(0);    // Set the number of key caught to zero
                    player.GetComponentInChildren<Player>().SetCurrentLives(player.GetComponentInChildren<Player>().GetCurrentLives() - 1);     // Get one life off of the player

                    this.DeleteWorld();                                         // Delete the current world
                    this.GenerateWorld();                                       // Generate a new one

                    loading.SetActive(false);
                }

                // Game over
                else
                {
                    this.DeleteWorld();                                         // Delete the current world
                    File.Delete(this.fileName);                                 // Since he definitively lost, delete the save file
                    SceneManager.LoadScene("Menu");                             // Go back to the main menu
                }
            }

            // Else if the player got all 3 keys, died or when down to 0 life, the loading screen will appear
            else if (!loading.activeSelf && (player.GetComponentInChildren<Player>().GetKeyCaught() == this.numberOfKeys || player.GetComponentInChildren<Player>().GetCurrentLives() == 0 || player.GetComponentInChildren<Player>().GetCaught() == true || player.transform.position.y < -100))
                loading.SetActive(true);
        }
    }


    // Generate a new random world
    public void GenerateWorld()
    {
        // CHUNKS
        this.chunks = new Chunk[mapSize / chunkSize, mapSize / chunkSize];

        this.GenerateWorldValues(Random.Range(0, 10));

        this.GenerateTerrainValues(Random.Range(0, 3));
        this.GenerateSurfaceHeights();
        this.GenerateTerrain();

        //PLAYER
        player.transform.position = this.GenerateRandomVector(1);

        // KEYS
        this.keys = new GameObject[this.numberOfKeys];
        this.GenerateKeys();

        // LIFE
        this.GenerateLife();

        // MONSTERS
        this.monsters = new GameObject[this.numberOfMonsters];
        this.GenerateMonsters();

        // TREES
        this.trees = new GameObject[this.numberOfTrees];
        this.GenerateTrees();

        // SPEEDS
        player.GetComponentInChildren<Player>().SetNormalSpeed(this.normalSpeed + 3.0f);

        for (int i = 0; i < this.numberOfMonsters; i++)
            this.monsters[i].GetComponentInChildren<Enemy>().SetNormalSpeed(this.normalSpeed);

        // PLAYER
        player.SetActive(true);
    }


    // Generate a world based on a file save
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
                this.level = int.Parse(reader.ReadLine());
                this.maxHeight = int.Parse(reader.ReadLine());
                this.octaves = int.Parse(reader.ReadLine());
                this.smooth = float.Parse(reader.ReadLine());
                this.persistence = float.Parse(reader.ReadLine());

                int wType;

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
                    this.terrainType = TerrainTypes.PLAINS;
                else if (terrainTypeRead == "HILLS")
                    this.terrainType = TerrainTypes.HILLS;
                else
                    this.terrainType = TerrainTypes.MOUNTAINS;

                // GENERATE TERRAIN
                this.GenerateWorldValues(wType);
                this.GenerateSurfaceHeights();
                this.GenerateTerrain();

                reader.ReadLine();

                // PLAYER
                // Put the player at the position where he was when he saved
                string text = reader.ReadLine();
                string[] coordinates = text.Split(' ');
                player.transform.position = new Vector3(float.Parse(coordinates[0]), float.Parse(coordinates[1]), float.Parse(coordinates[2]));
                player.GetComponentInChildren<Player>().SetCurrentLives(int.Parse(reader.ReadLine()));
                player.GetComponentInChildren<Player>().SetKeyCaught(int.Parse(reader.ReadLine()));

                reader.ReadLine();

                // KEYS
                // Put the keys at the position where they where when the world was saved
                for (int i = 0; i < this.keys.Length - player.GetComponentInChildren<Player>().GetKeyCaught(); i++)
                {
                    text = reader.ReadLine();
                    coordinates = text.Split(' ');
                    this.keys[i] = Instantiate(this.key, new Vector3(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2])), Quaternion.identity) as GameObject;
                }

                reader.ReadLine();

                // LIFE
                // Put the life at the position where they where when the world was saved
                text = reader.ReadLine();
                coordinates = text.Split(' ');
                this.life = Instantiate(heart, new Vector3(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2])), Quaternion.identity) as GameObject;

                reader.ReadLine();

                // MONSTERS
                // Put the monsters at the position where they where when the world was saved
                for (int i = 0; i < this.monsters.Length; i++)
                {
                    text = reader.ReadLine();
                    coordinates = text.Split(' ');
                    this.monsters[i] = Instantiate(this.monsterModel, new Vector3(float.Parse(coordinates[0]), float.Parse(coordinates[1]), float.Parse(coordinates[2])), Quaternion.identity) as GameObject;
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

                // SPEEDS
                // Adjusts the speed of the player and of the monsters depending on the level
                this.normalSpeed += (this.level - 1) * 0.75f;

                player.GetComponentInChildren<Player>().SetNormalSpeed(this.normalSpeed + 3.0f);

                for (int i = 0; i < this.numberOfMonsters; i++)
                    this.monsters[i].GetComponentInChildren<Enemy>().SetNormalSpeed(this.normalSpeed);

                // PLAYER
                player.SetActive(true);
            }
        }

        catch (System.Exception exception)
        {
            Debug.Log(exception.ToString());
        }
    }


    // Save the all the world important value
    public void SaveWorld()
    {
        try
        {
            if(File.Exists(this.fileName))
                File.Delete(this.fileName);

            using (StreamWriter writer = new StreamWriter(this.fileName))
            {
                // TERRAIN VALUES
                writer.WriteLine(worldType);
                writer.WriteLine(this.terrainType);
                writer.WriteLine(this.level);
                writer.WriteLine(this.maxHeight);
                writer.WriteLine(this.octaves);
                writer.WriteLine(this.smooth);
                writer.WriteLine(this.persistence);

                writer.WriteLine();

                // PLAYER
                writer.WriteLine(player.transform.position.x + " " + player.transform.position.y + " " + player.transform.position.z);
                writer.WriteLine(player.GetComponentInChildren<Player>().GetCurrentLives());
                writer.WriteLine(player.GetComponentInChildren<Player>().GetKeyCaught());

                writer.WriteLine();

                // KEYS
                for (int i = 0; i < this.keys.Length; i++)
                {
                    if (this.keys[i])
                        writer.WriteLine(this.keys[i].transform.position.x + " " + this.keys[i].transform.position.y + " " + this.keys[i].transform.position.z);
                }

                writer.WriteLine();

                // LIFE
                writer.WriteLine(this.life.transform.position.x + " " + this.life.transform.position.y + " " + this.life.transform.position.z);

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


    // Delete all the game objects of the current world
    public void DeleteWorld()
    {
        // PLAYER
        player.SetActive(false);

        for (int i = 0; i < this.numberOfKeys; i++)
            GameObject.Destroy(this.keys[i]);

        GameObject.Destroy(this.life);

        for (int i = 0; i < this.numberOfMonsters; i++)
            GameObject.Destroy(this.monsters[i]);

        for (int i = 0; i < this.numberOfTrees; i++)
            GameObject.Destroy(this.trees[i]);

        for (int x = 0; x < this.chunks.GetLength(0); x++)
        {
            for (int z = 0; z < chunks.GetLength(1); z++)
                this.chunks[x, z].DeleteChunk();
        }
    }


    // Generate the world values depending on the world type
    // World values include : the monster model, the number of trees, the tree mode, the sky and the fog
    public void GenerateWorldValues(int wType)
    {
        if (wType == 0)
        {
            worldType = WorldTypes.NORMAL;

            this.monsterModel = littleMonster;
            this.numberOfTrees = 1200;
            this.treeModel = this.oakTree;

            RenderSettings.skybox = skyNormal;
            RenderSettings.fog = false;
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
            RenderSettings.fogDensity = 0.03f;
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
            RenderSettings.fog = false;
        }

        else if (wType == 4)
        {
            worldType = WorldTypes.METAL;

            this.monsterModel = this.ghost;
            this.numberOfTrees = 0;
            this.treeModel = null;

            RenderSettings.skybox = skyMetal;
            RenderSettings.fog = false;
        }

        else if (wType == 5)
        {
            worldType = WorldTypes.CHEESE;

            this.monsterModel = this.gnome;
            this.numberOfTrees = 0;
            this.treeModel = null;

            RenderSettings.skybox = skyCheese;
            RenderSettings.fog = false;
        }

        else if (wType == 6)
        {
            worldType = WorldTypes.AUTUMN;

            this.monsterModel = this.yeti;
            this.numberOfTrees = 1000;
            this.treeModel = this.autumnTree;

            RenderSettings.skybox = skyAutumn;
            RenderSettings.fog = false;
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
            RenderSettings.fog = false;
        }

        else
        {
            worldType = WorldTypes.MATRIX;

            this.monsterModel = this.agentSmith;
            this.numberOfTrees = 0;
            this.treeModel = null;

            RenderSettings.skybox = skyMatrix;
            RenderSettings.fog = false;
        }
    }


    // Generate terrain values depending on the terrain type
    // Terrain values include : the maximal height of the terrain, and the smoothness of it
    public void GenerateTerrainValues(int tType)
    {
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


    // Generate the terrain based on the world and terrain variables
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


    // Determines the coordinates of all the surface cubes of the world
    public void GenerateSurfaceHeights()
    {
        surfaceHeights = new int[mapSize, mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
                surfaceHeights[x, z] = (int)Mathf.Lerp(0, this.maxHeight, Mathf.InverseLerp(0, 1, Noise(x * this.smooth, z * this.smooth)));
        }
    }


    // Determines the y position of a surface cube

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


    // Generate the key's game objects
    public void GenerateKeys()
    {
        for(int i = 0; i < this.keys.Length; i++)
        {
            this.keys[i] = Instantiate(this.key, this.GenerateRandomVector(75, 0), Quaternion.identity) as GameObject;
        }
    }


    // Generate the life game object
    public void GenerateLife()
    {
        this.life = Instantiate(heart, this.GenerateRandomVector(0, 1), Quaternion.identity) as GameObject;
    }


    // Generate the monster's game object
    public void GenerateMonsters()
    {
        for (int i = 0; i < this.monsters.Length; i++)
        {
            this.monsters[i] = Instantiate(this.monsterModel, this.GenerateRandomVector(30, 0), Quaternion.identity) as GameObject;
        }
    }


    // Generate the tree's game object
    public void GenerateTrees()
    {
        for (int i = 0; i < this.trees.Length; i++)
        {
            this.trees[i] = Instantiate(this.treeModel, this.GenerateRandomVector(15, 0), Quaternion.identity) as GameObject;
        }
    }


    // Generate a random vector with an offset on the y axis
    public Vector3 GenerateRandomVector(int offset)
    {
        int x = Random.Range(0, mapSize);
        int z = Random.Range(0, mapSize);

        return new Vector3(x, surfaceHeights[x, z] + offset, z);
    }


    // Generate a random vector with an offset on the y axis
    // The vector need to be at as specific distance from the spawn position of the player
    public Vector3 GenerateRandomVector(int distanceFromSpawn, int offset)
    {
        int x, z;

        do
        {
            x = Random.Range(0, mapSize);
            z = Random.Range(0, mapSize);
        } while (Mathf.Abs(player.transform.position.x - x) < distanceFromSpawn && Mathf.Abs(player.transform.position.z - z) < distanceFromSpawn);

        return new Vector3(x, surfaceHeights[x, z] + offset, z);
    }
}
