using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

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

#endif // UNITY_EDITOR