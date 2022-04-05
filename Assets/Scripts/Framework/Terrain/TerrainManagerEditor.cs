using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainManager terrainManager = (TerrainManager)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Recompute"))
        {
            terrainManager.ClearAllChunks();
            terrainManager.UpdateVisibleChunks();
        }

        GUILayout.Space(20);

        GUILayout.Label("ChunksCached: " + terrainManager.GetChunkCachedCount());
        GUILayout.Label("ChunksVisibles: " + terrainManager.GetChunkVisibleCount());
    }
}