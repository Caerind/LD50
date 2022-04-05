using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainSettings))]
public class TerrainSettingsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		TerrainSettings terrainSettings = (TerrainSettings)target;

		DrawDefaultInspector();

		GUILayout.Space(20);

		GUILayout.Label("ChunkSize: " + TerrainSettings.supportedChunkSizes[terrainSettings.chunkSizeIndex]);
		GUILayout.Label("VerticesPerLine: " + terrainSettings.numVertsPerLine);
		GUILayout.Label("MeshWorldSize: " + terrainSettings.meshWorldSize);
		GUILayout.Label("MaxViewDistance: " + terrainSettings.maxViewDistance);
		GUILayout.Label("ChunksVisibleInViewDst: " + terrainSettings.chunksVisibleInViewDst);
	}
}