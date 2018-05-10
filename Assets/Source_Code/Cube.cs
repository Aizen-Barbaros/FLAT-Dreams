using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The class Cube is the representation of a cube of 1 by 1 by 1 in Unity
// Only the visible faces of the cube will be shown
public class Cube
{
    public enum CubeSides { FRONT, BACK, TOP, BOTTOM, LEFT, RIGHT };

    // Coordinates of all the cubes' face's vertices on the texture atlas
    // TOP TOP means the top face of a cube that is on the surface
    // TOP SIDE means a side face of a cube that is on the surface
    // UNDER SIDE means a side face of a cube that is not on the surface
    // There are no UNDER TOP because this face can never be seen, so it'll
    // never be rendered.
    private Vector2[,] cubeTextureInAtlas = {{new Vector2(0.0f, 0.8f), new Vector2(0.2f, 0.8f), new Vector2(0.0f, 1.0f), new Vector2(0.2f, 1.0f)},              // NORMAL TOP TOP 0
	                                         {new Vector2(0.2f, 0.8f), new Vector2(0.4f, 0.8f), new Vector2(0.2f, 1.0f), new Vector2(0.4f, 1.0f)},              // NORMAL TOP SIDE 1
		                                     {new Vector2(0.4f, 0.8f), new Vector2(0.6f, 0.8f), new Vector2(0.4f, 1.0f), new Vector2(0.6f, 1.0f)},              // NORMAL UNDER SIDE 2
                                             {new Vector2(0.0f, 0.6f), new Vector2(0.2f, 0.6f), new Vector2(0.0f, 0.8f), new Vector2(0.2f, 0.8f)},              // SNOWY TOP TOP 3
	                                         {new Vector2(0.2f, 0.6f), new Vector2(0.4f, 0.6f), new Vector2(0.2f, 0.8f), new Vector2(0.4f, 0.8f)},              // SNOWY TOP SIDE 4
		                                     {new Vector2(0.4f, 0.8f), new Vector2(0.6f, 0.8f), new Vector2(0.4f, 1.0f), new Vector2(0.6f, 1.0f)},              // SNOWY UNDER SIDE 5
                                             {new Vector2(0.0f, 0.2f), new Vector2(0.2f, 0.2f), new Vector2(0.0f, 0.4f), new Vector2(0.2f, 0.4f)},              // HELL TOP TOP 6
                                             {new Vector2(0.2f, 0.2f), new Vector2(0.4f, 0.2f), new Vector2(0.2f, 0.4f), new Vector2(0.4f, 0.4f)},              // HELL TOP SIDE 7
                                             {new Vector2(0.2f, 0.2f), new Vector2(0.4f, 0.2f), new Vector2(0.2f, 0.4f), new Vector2(0.4f, 0.4f)},              // HELL UNDER SIDE 8
                                             {new Vector2(0.0f, 0.4f), new Vector2(0.2f, 0.4f), new Vector2(0.0f, 0.6f), new Vector2(0.2f, 0.6f)},              // DREAMY TOP TOP 9
	                                         {new Vector2(0.2f, 0.4f), new Vector2(0.4f, 0.4f), new Vector2(0.2f, 0.6f), new Vector2(0.4f, 0.6f)},              // DREAMY TOP SIDE 10
		                                     {new Vector2(0.4f, 0.4f), new Vector2(0.6f, 0.4f), new Vector2(0.4f, 0.6f), new Vector2(0.6f, 0.6f)},              // DREAMY UNDER SIDE 11
                                             {new Vector2(0.6f, 0.8f), new Vector2(0.8f, 0.8f), new Vector2(0.6f, 1.0f), new Vector2(0.8f, 1.0f)},              // METAL TOP TOP 12
	                                         {new Vector2(0.6f, 0.8f), new Vector2(0.8f, 0.8f), new Vector2(0.6f, 1.0f), new Vector2(0.8f, 1.0f)},              // METAL TOP SIDE 13
		                                     {new Vector2(0.6f, 0.8f), new Vector2(0.8f, 0.8f), new Vector2(0.6f, 1.0f), new Vector2(0.8f, 1.0f)},              // METAL UNDER SIDE 14
                                             {new Vector2(0.6f, 0.6f), new Vector2(0.8f, 0.6f), new Vector2(0.6f, 0.8f), new Vector2(0.8f, 0.8f)},              // CHEESE TOP TOP 15
	                                         {new Vector2(0.8f, 0.6f), new Vector2(1.0f, 0.6f), new Vector2(0.8f, 0.8f), new Vector2(1.0f, 0.8f)},              // CHEESE TOP SIDE 16
		                                     {new Vector2(0.8f, 0.6f), new Vector2(1.0f, 0.6f), new Vector2(0.8f, 0.8f), new Vector2(1.0f, 0.8f)},              // CHEESE UNDER SIDE 17
                                             {new Vector2(0.6f, 0.4f), new Vector2(0.8f, 0.4f), new Vector2(0.6f, 0.6f), new Vector2(0.8f, 0.6f)},              // AUTUMN TOP TOP 18
	                                         {new Vector2(0.8f, 0.4f), new Vector2(1.0f, 0.4f), new Vector2(0.8f, 0.6f), new Vector2(1.0f, 0.6f)},              // AUTUMN TOP SIDE 19
		                                     {new Vector2(0.4f, 0.8f), new Vector2(0.6f, 0.8f), new Vector2(0.4f, 1.0f), new Vector2(0.6f, 1.0f)},              // AUTUMN UNDER SIDE 20
                                             {new Vector2(0.0f, 0.0f), new Vector2(0.2f, 0.0f), new Vector2(0.0f, 0.2f), new Vector2(0.2f, 0.2f)},              // ROTTING TOP TOP 21
	                                         {new Vector2(0.2f, 0.0f), new Vector2(0.4f, 0.0f), new Vector2(0.2f, 0.2f), new Vector2(0.4f, 0.2f)},              // ROTTING TOP SIDE 22
		                                     {new Vector2(0.4f, 0.0f), new Vector2(0.6f, 0.0f), new Vector2(0.4f, 0.2f), new Vector2(0.6f, 0.2f)},              // ROTTING UNDER SIDE 23
                                             {new Vector2(0.6f, 0.2f), new Vector2(0.8f, 0.2f), new Vector2(0.6f, 0.4f), new Vector2(0.8f, 0.4f)},              // TROPICAL TOP TOP 24
	                                         {new Vector2(0.6f, 0.2f), new Vector2(0.8f, 0.2f), new Vector2(0.6f, 0.4f), new Vector2(0.8f, 0.4f)},              // TROPICAL TOP SIDE 25
		                                     {new Vector2(0.6f, 0.2f), new Vector2(0.8f, 0.2f), new Vector2(0.6f, 0.4f), new Vector2(0.8f, 0.4f)},              // TROPICAL UNDER SIDE 26
                                             {new Vector2(0.6f, 0.0f), new Vector2(0.8f, 0.0f), new Vector2(0.6f, 0.2f), new Vector2(0.8f, 0.2f)},              // MATRIX TOP TOP 27
	                                         {new Vector2(0.8f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(0.8f, 0.2f), new Vector2(1.0f, 0.2f)},              // MATRIX TOP SIDE 28
		                                     {new Vector2(0.8f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(0.8f, 0.2f), new Vector2(1.0f, 0.2f)}};             // MATRIX UNDER SIDE 29

    private GameObject parent;          // The chunk that contains the cube
    private Vector3 position;           // The relative position in the chunk containing the cube
    private Vector3 mapPosition;        // The absolute position in the map
    private Material material;          // The material used to display the cube
    private bool isOnTheSurface;        // A boolean which is true if this cube in on the surface, and which is false otherwise


    public Cube(GameObject parent, Vector3 position, Material material, bool isOnTheSurface)
    {
        this.parent = parent;
        this.position = position;
        this.mapPosition = new Vector3(this.parent.transform.position.x + this.position.x,
                                       this.parent.transform.position.y + this.position.y,
                                       this.parent.transform.position.z + this.position.z);
        this.material = material;
        this.isOnTheSurface = isOnTheSurface;
    }


    // This function displays the cube's faces that are visible
    public void DisplayCube()
    {
        if (!this.CheckNeighbour((int)this.mapPosition.x, (int)this.mapPosition.z + 1))
            this.CreateFace(CubeSides.FRONT);
        if (!this.CheckNeighbour((int)this.mapPosition.x, (int)this.mapPosition.z - 1))
            this.CreateFace(CubeSides.BACK);
        if (!this.CheckNeighbour((int)this.mapPosition.x - 1, (int)this.mapPosition.z))
            this.CreateFace(CubeSides.LEFT);
        if (!this.CheckNeighbour((int)this.mapPosition.x + 1, (int)this.mapPosition.z))
            this.CreateFace(CubeSides.RIGHT);

        // The top face of the cubes that are on the surface is always shown,
        // no matter what
        if (this.isOnTheSurface == true)
            this.CreateFace(CubeSides.TOP);

        // Also, the under face is never shown
    }



    // This function creates a cube's face and apply a texture on it, as well as a mesh filter and renderer
    public void CreateFace(CubeSides face)
    {
        // VERTICES
        // List all the coordinates of the vertices on a cube
        Vector3 vertex1 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 vertex2 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 vertex3 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 vertex4 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 vertex5 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 vertex6 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 vertex7 = new Vector3(-0.5f, 0.5f, -0.5f);
        Vector3 vertex8 = new Vector3(-0.5f, 0.5f, 0.5f);

        // UVs
        // List all the uvs on a cube
        Vector2 uvBottomLeft;
        Vector2 uvBottomRight;
        Vector2 uvTopLeft;
        Vector2 uvTopRight;

        // TEXTURES' INDEX
        // Determine wish texture to apply on each type of face, depending on
        // the world's type
        int topTopIndex = 3 * (int)World.worldType;
        int topSideIndex = topTopIndex + 1;
        int underSideIndex = topTopIndex + 2;

        // SETTING TEXTURES
        // Actually sets the texture on the face, depending on it's type
        if (this.isOnTheSurface && face == CubeSides.TOP)
        {
            uvBottomLeft = this.cubeTextureInAtlas[topTopIndex, 0];
            uvBottomRight = this.cubeTextureInAtlas[topTopIndex, 1];
            uvTopLeft = this.cubeTextureInAtlas[topTopIndex, 2];
            uvTopRight = this.cubeTextureInAtlas[topTopIndex, 3];
        }

        else if (this.isOnTheSurface && face != CubeSides.TOP)
        {
            uvBottomLeft = this.cubeTextureInAtlas[topSideIndex, 0];
            uvBottomRight = this.cubeTextureInAtlas[topSideIndex, 1];
            uvTopLeft = this.cubeTextureInAtlas[topSideIndex, 2];
            uvTopRight = this.cubeTextureInAtlas[topSideIndex, 3];
        }

        else
        {
            uvBottomLeft = this.cubeTextureInAtlas[underSideIndex, 0];
            uvBottomRight = this.cubeTextureInAtlas[underSideIndex, 1];
            uvTopLeft = this.cubeTextureInAtlas[underSideIndex, 2];
            uvTopRight = this.cubeTextureInAtlas[underSideIndex, 3];
        }

        // MESH'S VARIABLES
        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        // SIDES
        // Determine the mesh's variables depending on the face's type
        switch (face)
        {
            case CubeSides.BOTTOM:
                vertices = new Vector3[] { vertex6, vertex2, vertex1, vertex5 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                uvs = new Vector2[] { uvTopRight, uvTopLeft, uvBottomLeft, uvBottomRight };
                break;

            case CubeSides.TOP:
                vertices = new Vector3[] { vertex7, vertex3, vertex4, vertex8 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                uvs = new Vector2[] { uvTopRight, uvTopLeft, uvBottomLeft, uvBottomRight };
                break;

            case CubeSides.LEFT:
                vertices = new Vector3[] { vertex7, vertex8, vertex6, vertex5 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                uvs = new Vector2[] { uvTopRight, uvTopLeft, uvBottomLeft, uvBottomRight };
                break;

            case CubeSides.RIGHT:
                vertices = new Vector3[] { vertex4, vertex3, vertex1, vertex2 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                uvs = new Vector2[] { uvTopRight, uvTopLeft, uvBottomLeft, uvBottomRight };
                break;

            case CubeSides.FRONT:
                vertices = new Vector3[] { vertex8, vertex4, vertex2, vertex6 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                uvs = new Vector2[] { uvTopRight, uvTopLeft, uvBottomLeft, uvBottomRight };
                break;

            case CubeSides.BACK:
                vertices = new Vector3[] { vertex3, vertex7, vertex5, vertex1 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                uvs = new Vector2[] { uvTopRight, uvTopLeft, uvBottomLeft, uvBottomRight };
                break;
        }

        triangles = new int[] { 3, 1, 0, 3, 2, 1 };

        // MESH
        // Create a new mesh and initialize it's variables with the ones which
        // where chosen
        Mesh mesh = new Mesh
        {
            name = "FaceMesh" + face.ToString(),
            vertices = vertices,
            normals = normals,
            uv = uvs,
            triangles = triangles
        };

        mesh.RecalculateBounds();

        // Creates a game object instantiating the face
        GameObject side = new GameObject("Side");
        side.transform.position = this.position;
        side.transform.parent = this.parent.transform;

        // Apply a mesh filter and a mesh renderer on the face
        MeshFilter meshFilter = (MeshFilter)side.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;

        MeshRenderer renderer = side.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = this.material;
    }


    // This function checks if a face has some neighbours that will make this
    // face not visible
    public bool CheckNeighbour(int x, int z)
    {
        if (x >= 0 && x < World.mapSize && z >= 0 && z < World.mapSize) // Verifies if a cube can actually exist at the coordinates we want to check out
        {
            // If > 0, it means that this face is a least one cube above it's neighbours, so it is possibly visible
            if ((World.surfaceHeights[(int)this.mapPosition.x, (int)this.mapPosition.z] - World.surfaceHeights[x, z]) > 0)
                return false;
            else
                return true;
        }

        else
            return true; // This means that the side faces that are on the border of the world will not be rendered
    }
}
