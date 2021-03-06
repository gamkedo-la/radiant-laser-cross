﻿using UnityEngine;
using System.Collections;

public class WobbleVerts : MonoBehaviour
{
    public float scale = 0.1f;
    public float speed = 1.0f;
    public float noiseStrength = 1f;
    public float noiseWalk = 1f;
    public float z_strength = 0f;

    private Vector3[] baseHeight;
    private Mesh mesh;

    void Update()
    {
        if (mesh == null)
            mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = mesh.vertices;

        Vector3[] vertices = new Vector3[baseHeight.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.x += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
            vertex.x += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
            vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale/2;
            vertex.y += Mathf.PerlinNoise(baseHeight[i].x, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
            if (z_strength>0f)
            {
                vertex.y += Mathf.Sin(1234f + Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale / 2;
                vertex.y += Mathf.PerlinNoise(baseHeight[i].x, baseHeight[i].y + Mathf.Sin(1234f + Time.time * 0.1f)) * z_strength;
            }
            vertices[i] = vertex;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}