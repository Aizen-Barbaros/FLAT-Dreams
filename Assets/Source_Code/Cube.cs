﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cube
{
    public enum CubeSides { FRONT, BACK, TOP, BOTTOM, LEFT, RIGHT };

    private Vector2[,] cubeTextureInAtlas = {{new Vector2(0.0f, 0.8f), new Vector2(0.2f, 0.8f), new Vector2(0.0f, 1.0f), new Vector2(0.2f, 1.0f)},              // NORMAL TOP TOP
	                                         {new Vector2(0.2f, 0.8f), new Vector2(0.4f, 0.8f), new Vector2(0.2f, 1.0f), new Vector2(0.4f, 1.0f)},              // NORMAL TOP SIDE
		                                     {new Vector2(0.4f, 0.8f), new Vector2(0.6f, 0.8f), new Vector2(0.4f, 1.0f), new Vector2(0.6f, 1.0f)},              // NORMAL / SNOWY UNDER SIDE
                                             {new Vector2(0.0f, 0.6f), new Vector2(0.2f, 0.6f), new Vector2(0.0f, 0.8f), new Vector2(0.2f, 0.8f)},              // SNOWY TOP TOP
	                                         {new Vector2(0.2f, 0.6f), new Vector2(0.4f, 0.6f), new Vector2(0.2f, 0.8f), new Vector2(0.4f, 0.8f)},              // SNOWY TOP SIDE
                                             {new Vector2(0.0f, 0.2f), new Vector2(0.2f, 0.2f), new Vector2(0.0f, 0.4f), new Vector2(0.2f, 0.4f)},              // HELL TOP / UNDER SIDE
                                             {new Vector2(0.0f, 0.4f), new Vector2(0.2f, 0.4f), new Vector2(0.0f, 0.6f), new Vector2(0.2f, 0.6f)},              // DREAMY TOP TOP
	                                         {new Vector2(0.2f, 0.4f), new Vector2(0.4f, 0.4f), new Vector2(0.2f, 0.6f), new Vector2(0.4f, 0.6f)},              // DREAMY TOP SIDE
		                                     {new Vector2(0.4f, 0.4f), new Vector2(0.6f, 0.4f), new Vector2(0.4f, 0.6f), new Vector2(0.6f, 0.6f)}};             // DREAMY UNDER SIDE

    private GameObject parent;
    private Vector3 position;
    private Vector3 mapPosition;
    private Material material;
    private bool isOnTheSurface;


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

        if (this.isOnTheSurface == true)
            this.CreateFace(CubeSides.TOP);
    }


    public void CreateFace(CubeSides face)
    {
        // VERTICES
        Vector3 vertex1 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 vertex2 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 vertex3 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 vertex4 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 vertex5 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 vertex6 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 vertex7 = new Vector3(-0.5f, 0.5f, -0.5f);
        Vector3 vertex8 = new Vector3(-0.5f, 0.5f, 0.5f);

        // UVs
        Vector2 uvBottomLeft;
        Vector2 uvBottomRight;
        Vector2 uvTopLeft;
        Vector2 uvTopRight;

        // TEXTURES' INDEX
        int topTopIndex;
        int topSideIndex;
        int underSideIndex;

        if (Map.worldType == Map.WorldTypes.NORMAL)
        {
            topTopIndex = 0;
            topSideIndex = 1;
            underSideIndex = 2;
        }

        else if (Map.worldType == Map.WorldTypes.SNOWY)
        {
            topTopIndex = 3;
            topSideIndex = 4;
            underSideIndex = 2;
        }

        else if (Map.worldType == Map.WorldTypes.HELL)
        {
            topTopIndex = 5;
            topSideIndex = 5;
            underSideIndex = 5;
        }

        else
        {
            topTopIndex = 6;
            topSideIndex = 7;
            underSideIndex = 8;
        }

        // SETTING TEXTURES
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
        Mesh mesh = new Mesh
        {
            name = "FaceMesh" + face.ToString(),
            vertices = vertices,
            normals = normals,
            uv = uvs,
            triangles = triangles
        };

        mesh.RecalculateBounds();

        GameObject side = new GameObject("Side");
        side.transform.position = this.position;
        side.transform.parent = this.parent.transform;

        MeshFilter meshFilter = (MeshFilter)side.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;

        MeshRenderer renderer = side.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = this.material;
    }


    public bool CheckNeighbour(int x, int z)
    {
        if (x >= 0 && x < Map.mapSize && z >= 0 && z < Map.mapSize)
        {
            if ((Map.surfaceHeights[(int)this.mapPosition.x, (int)this.mapPosition.z] - Map.surfaceHeights[x, z]) > 0)
                return false;
            else
                return true;
        }

        else
            return true;
    }
}
