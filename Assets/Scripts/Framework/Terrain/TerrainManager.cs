using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class TerrainManager : MonoBehaviour
{
	public TerrainSettings terrainSettings;
	public Transform viewer;

	private Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	private List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

#if UNITY_EDITOR
	[HideInInspector]
	private Camera editorCamera;
#endif // UNITY_EDITOR

	public Vector2 viewerPosition
    {
		get
		{
#if UNITY_EDITOR
			if (Application.isEditor)
			{
				return GetEditorViewerPosition();
			}
#endif // UNITY_EDITOR

			return new Vector2(viewer.position.x, viewer.position.z);
		}
	}

    private Vector2 viewerPositionOld;

	private void Start()
	{
		UpdateVisibleChunks();
	}

	private void Update()
	{
		if (viewerPosition != viewerPositionOld)
		{
			foreach (TerrainChunk chunk in visibleTerrainChunks)
			{
				chunk.UpdateCollisionMesh();
			}
		}

		if ((viewerPositionOld - viewerPosition).sqrMagnitude > terrainSettings.sqrViewerMoveThresholdForChunkUpdate)
		{
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks();
		}
	}

	public void UpdateVisibleChunks()
	{
		HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
		for (int i = visibleTerrainChunks.Count - 1; i >= 0; i--)
		{
			TerrainChunk terrainChunk = visibleTerrainChunks[i];

			if (!alreadyUpdatedChunkCoords.Contains(terrainChunk.coords))
			{
				alreadyUpdatedChunkCoords.Add(terrainChunk.coords);
				terrainChunk.UpdateTerrainChunk();
			}
			else
            {
				Debug.LogWarning("Nope");
            }
		}

		float meshWorldSize = terrainSettings.meshWorldSize;
		int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / meshWorldSize);
		int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / meshWorldSize);

		int chunksVisibleInViewDst = terrainSettings.chunksVisibleInViewDst;
		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; ++yOffset)
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; ++xOffset)
			{
				Vector2 viewedChunkCoords = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
				if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoords))
				{
					if (terrainChunkDictionary.ContainsKey(viewedChunkCoords))
					{
						terrainChunkDictionary[viewedChunkCoords].UpdateTerrainChunk();
					}
					else
					{
						TerrainChunk newChunk = new TerrainChunk(viewedChunkCoords, this, transform);
						terrainChunkDictionary.Add(viewedChunkCoords, newChunk);
						newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
						newChunk.Load();
					}
				}
			}
		}
	}

	public int GetChunkCachedCount()
	{
		return terrainChunkDictionary.Count;
	}

	public int GetChunkVisibleCount()
	{
		return visibleTerrainChunks.Count;
	}

	public int GetChunkChildCount()
    {
		return transform.childCount;
    }

	public void OnTerrainChunkVisibilityChanged(TerrainChunk terrainChunk, bool isVisible)
	{
		if (isVisible)
		{
			if (visibleTerrainChunks.Contains(terrainChunk))
            {
				Debug.LogError("Added the same chunk twice !");
			}
			else
			{
				visibleTerrainChunks.Add(terrainChunk);
			}
		}
		else
		{
			visibleTerrainChunks.Remove(terrainChunk);
		}
	}

	public void ClearAllChunks()
	{
		visibleTerrainChunks.Clear();
		terrainChunkDictionary.Clear();

		for (int i = transform.childCount - 1; i >= 0; --i)
        {
			Transform child = transform.GetChild(i);
#if UNITY_EDITOR
			if (Application.isEditor)
			{
				GameObject.DestroyImmediate(child.gameObject);
			}
			else
#endif // UNITY_EDITOR
			{
				GameObject.Destroy(child.gameObject);
			}
		}
	}

#if UNITY_EDITOR
	public void UpdateEditor()
    {
		if ((viewerPositionOld - viewerPosition).sqrMagnitude > terrainSettings.sqrViewerMoveThresholdForChunkUpdate)
		{
			viewerPositionOld = viewerPosition;

			ClearAllChunks();
			UpdateVisibleChunks();

			if (transform.childCount != GetChunkCachedCount())
			{
				Debug.LogError("Error with cached chunks => Leak");
			}
		}
	}

	private Vector2 GetEditorViewerPosition()
    {
		if (editorCamera == null)
		{
			editorCamera = SceneView.lastActiveSceneView.camera;
		}

		Ray ray = editorCamera.ScreenPointToRay(new Vector2((editorCamera.pixelWidth - 1) * 0.5f, (editorCamera.pixelHeight - 1) * 0.5f));
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		float distance = 0;
		Vector3 position = editorCamera.transform.position;
		if (hPlane.Raycast(ray, out distance))
		{
			position = ray.GetPoint(distance);
		}
		return new Vector2(position.x, position.z);
	}
#endif // UNITY_EDITOR
}
