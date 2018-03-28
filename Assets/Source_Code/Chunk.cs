using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    private GameObject chunk;
    private Material textureAtlas;


    public Chunk(Vector3 position, Material textureAtlas)
    {
        this.chunk = new GameObject((int)position.x + "_" + (int)position.y + "_" + (int)position.z);
        this.chunk.transform.position = position;
        //this.chunk.tag = "Ground";
        this.textureAtlas = textureAtlas;

        this.CreateChunk();
        this.CombineCubes();

        MeshCollider meshCollider = this.chunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        meshCollider.sharedMesh = this.chunk.transform.GetComponent<MeshFilter>().mesh;
    }

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
                
                if (yDifferenceWithNeighbours > 1)
                {
                    for (int i = -(--yDifferenceWithNeighbours); i < 0; i++)
                    {
                        Cube underCube = new Cube(this.chunk, new Vector3(x, World.surfaceHeights[mapX, mapZ] + i, z), this.textureAtlas, false);
                        underCube.DisplayCube();
                    }
                }
                
                Cube cube = new Cube(this.chunk, new Vector3(x, World.surfaceHeights[mapX, mapZ], z), this.textureAtlas, true);
                cube.DisplayCube();
            }
        }
    }


    public void CombineCubes()
    {
        MeshFilter[] childrenMeshes = this.chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[childrenMeshes.Length];

        for (int i = 0; i < childrenMeshes.Length; i++)
        {
            combine[i].mesh = childrenMeshes[i].sharedMesh;
            combine[i].transform = childrenMeshes[i].transform.localToWorldMatrix;
        }

        MeshFilter parentMesh = (MeshFilter)this.chunk.gameObject.AddComponent(typeof(MeshFilter));
        parentMesh.mesh = new Mesh();

        parentMesh.mesh.CombineMeshes(combine);

        MeshRenderer parentMeshRenderer = this.chunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        parentMeshRenderer.material = this.textureAtlas;

        foreach (Transform square in this.chunk.transform)
            GameObject.Destroy(square.gameObject);
    }
}
