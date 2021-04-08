using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainConfig : MonoBehaviour
{
    public float distance;

    void Start()
    {
        //Terrain terrain = (Terrain) GameObject.FindObjectOfType(typeof(Terrain));
        //terrain.treeDistance = distance;

        //var terrainScripts = GameObject.FindSceneObjectsOfType(typeof(Terrain));

        //foreach (var terrain in terrainScripts)
        //{
        //    (terrain as Terrain).treeDistance = 2000;
        //}

        Terrain.activeTerrain.treeDistance = distance;

    }


}
