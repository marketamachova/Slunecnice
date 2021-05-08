using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainConfig : MonoBehaviour
{
    public float distance;

    void Start()
    {
        Terrain.activeTerrain.treeDistance = distance;
    }
}
