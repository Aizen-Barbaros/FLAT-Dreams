using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A chunk is a set of cubes that are combined togethers to create a single entity
public class Chunk
{
    private GameObject chunk;           // The actual game object that will reprenset the chunk in the game
    private Material textureAtlas;      // The texture atlas used on this chunk


    public Chunk(Vector3 position, Material textureAtlas)
    {
        this.chunk = new GameObject((int)position.x * World.chunkSize + "_" + (int)position.y * World.chunkSize + "_" + (int)position.z * World.chunkSize);
        this.chunk.transform.position = position * World.chunkSize;
        this.chunk.tag = "Ground";              // Put a tag Ground so that the player can actually only jump if he touches the ground
        this.textureAtlas = textureAtlas;

        this.CreateChunk();         // Creates the chunk
        this.CombineCubes();        // Combines all the chunk's cubes to create a single entity

        MeshCollider meshCollider = this.chunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;       // Put a mesh collider on it
        meshCollider.sharedMesh = this.chunk.transform.GetComponent<MeshFilter>().mesh;                             // Apply a mesh filter on it
    }


    // Create all the needed cubes to properly get this chunk
    public void CreateChunk()
    {
        for (int x = 0; x < World.chunkSize; x++)
        {
            for (int z = 0; z < World.chunkSize; z++)
            {
                int yDifferenceWithNeighbours = 0;
                int mapX = (int)this.chunk.transform.position.x + x;
                int mapZ = (int)this.chunk.transform.position.z + z;

                // x neighbours check
                for (int i = -1; i <= 1; i++)
                {
                    if (mapX + i >= 0 && mapX + i < World.mapSize)
                    {
                        if (World.surfaceHeights[mapX, mapZ] - World.surfaceHeights[mapX + i, mapZ] > yDifferenceWithNeighbours)
                            yDifferenceWithNeighbours = World.surfaceHeights[mapX, mapZ] - World.surfaceHeights[mapX + i, mapZ];
                    }
                }

                // z neighbours check
                for (int i = -1; i <= 1; i++)
                {
                    if (mapZ + i >= 0 && mapZ + i < World.mapSize)
                    {
                        if (World.surfaceHeights[mapX, mapZ] - World.surfaceHeights[mapX, mapZ + i] > yDifferenceWithNeighbours)
                            yDifferenceWithNeighbours = World.surfaceHeights[mapX, mapZ] - World.surfaceHeights[mapX, mapZ + i];
                    }
                }

                // This indicates and creates the right number of cube that have to be created under each top cubes, so that there are no holes on the terrain
                if (yDifferenceWithNeighbours > 1)
                {
                    for (int i = -(--yDifferenceWithNeighbours); i < 0; i++)
                    {
                        Cube underCube = new Cube(this.chunk, new Vector3(x, World.surfaceHeights[mapX, mapZ] + i, z), this.textureAtlas, false);
                        underCube.DisplayCube();
                    }
                }

                // Creates the top cube
                Cube cube = new Cube(this.chunk, new Vector3(x, World.surfaceHeights[mapX, mapZ], z), this.textureAtlas, true);
                cube.DisplayCube();
            }
        }
    }


    // This function is used to destroy all the game objects that are used to display a chunk
    public void DeleteChunk()
    {
        MonoBehaviour.Destroy(this.chunk.GetComponent<MeshCollider>());
        MonoBehaviour.Destroy(this.chunk.GetComponent<MeshFilter>());
        MonoBehaviour.Destroy(this.chunk.GetComponent<MeshRenderer>());
        MonoBehaviour.Destroy(this.chunk);
    }


    // This function uses all the cubes mesh filters, and combine them into a single one, so that there will be less game object on the map
    // So in fact, the cubes are just put together and then become a whole
    public void CombineCubes()
    {
        MeshFilter[] childrenMeshes = this.chunk.GetComponentsInChildren<MeshFilter>();     // Get all the chunk's cubes' faces
        CombineInstance[] combine = new CombineInstance[childrenMeshes.Length];

        for (int i = 0; i < childrenMeshes.Length; i++)
        {
            combine[i].mesh = childrenMeshes[i].sharedMesh;
            combine[i].transform = childrenMeshes[i].transform.localToWorldMatrix;
        }

        MeshFilter parentMesh = (MeshFilter)this.chunk.gameObject.AddComponent(typeof(MeshFilter));     // Combine all the chunk's cubes' faces
        parentMesh.mesh = new Mesh();

        parentMesh.mesh.CombineMeshes(combine);

        MeshRenderer parentMeshRenderer = this.chunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        parentMeshRenderer.material = this.textureAtlas;

        foreach (Transform square in this.chunk.transform)      // Destroy each faces entity because we don't need them as they already exists in the form of a chunk
            GameObject.Destroy(square.gameObject);
    }
}
